using System.Net.WebSockets;
using System.Text;
using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Services.Schedule.Common
{
    /// <summary>
    /// Сервис по работе с шаблонами
    /// </summary>
    public sealed class TemplateService : ITemplateService
    {
        private readonly ITemplatesRepository _templatesRepo;
        private readonly IShiftSequencesRepository _shiftsSequencesRepo;
        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _updateUtility;

        public TemplateService(
            ITemplatesRepository templatesRepo,
            IShiftSequencesRepository shiftsSequencesRepo,
            IMapper mapper,
            IEntityUpdateUtility updateUtility
        )
        {
            _templatesRepo = templatesRepo;
            _shiftsSequencesRepo = shiftsSequencesRepo;
            _mapper = mapper;
            _updateUtility = updateUtility;
        }

        public async Task<ServiceResult> CreateAsync(
            TemplateDto dto,
            CancellationToken cancellation = default
        )
        {
            var request = new EntityRequest<TemplateEntity>(dto.Id);
            var existed = await _templatesRepo.GetAsync(request, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            if (existed != null)
            {
                return ServiceResult.Error($"Такой шаблон (ID: {dto.Id}) уже существует");
            }

            var entity = _mapper.Map<TemplateEntity>(dto);

            var result = await _templatesRepo.CreateAsync(entity, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }
            if (result.IsSuccess)
            {
                await _templatesRepo.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Шаблон (ID: {entity.Id}) успешно создан!");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<ServiceResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var result = await _templatesRepo.DeleteAsync(id, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            if (result.IsSuccess)
            {
                await _templatesRepo.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Шаблон (ID: {id}) был успешно удалён!");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<TemplateDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var request = new EntityRequest<TemplateEntity>(id);
            var entity = await _templatesRepo.GetAsync(request, cancellation);

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<TemplateDto>(entity);
            return dto;
        }

        public async Task<TemplateDto?> GetByScheduleIdAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new EntityRequest<TemplateEntity>()
            {
                Predicate = (x) => x.ScheduleId == scheduleId
            };

            var entity = await _templatesRepo.GetAsync(request, cancellation);

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<TemplateDto>(entity);
            return dto;
        }

        public async Task<ServiceResult> UpdateAsync(
            TemplateDto dto,
            CancellationToken cancellation = default
        )
        {
            var source = _mapper.Map<TemplateEntity>(dto);

            var request = new EntityRequest<TemplateEntity>(source.Id);
            var target = await _templatesRepo.GetAsync(request, cancellation);

            if (target == null)
            {
                return ServiceResult.Error($"Обновляемый шаблон (ID: {source.Id}) не найден");
            }

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            _updateUtility.Update(target, source);

            _templatesRepo.Update(target);

            await _templatesRepo.SaveChangesAsync(cancellation);

            return ServiceResult.Ok($"Шаблон (ID: {target.Id}) был успешно обновлён");
        }

        public async Task<ServiceResult> UpdateSequenceAsync(
            ShiftSequencesModel model,
            CancellationToken cancellation = default
        )
        {
            var request = new EntityRequest<TemplateEntity>(model.TemplateId);
            var template = await _templatesRepo.GetAsync(request, cancellation);

            if (template == null)
            {
                return ServiceResult.Error($"Не найден шаблон(ID: {model.TemplateId})");
            }

            await _shiftsSequencesRepo.DeleteByConditionAsync(
                (x) => x.TemplateId == template.Id,
                cancellation
            );

            var sequence = _mapper.Map<List<ShiftSequenceEntity>>(model.Sequences);

            var result = await _shiftsSequencesRepo.CreateManyAsync(sequence, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            if (result.IsSuccess)
            {
                await _shiftsSequencesRepo.SaveChangesAsync(cancellation);
                sequence = sequence.OrderBy(x => x.Position).ToList();
                var message = new StringBuilder(
                    $"Последовательность для шаблона (ID: {template.Id}) была успешно обновлена!"
                );

                return ServiceResult.Ok(message.ToString());
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }
    }
}
