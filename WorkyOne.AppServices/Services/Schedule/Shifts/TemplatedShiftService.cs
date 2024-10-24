using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Shifts;

namespace WorkyOne.AppServices.Services.Schedule.Shifts
{
    /// <summary>
    /// Сервис по работе с "шаблонными" сменамиы
    /// </summary>
    public class TemplatedShiftService : ITemplatedShiftService
    {
        private readonly ITemplatedShiftsRepository _shiftsRepo;
        private readonly IMapper _mapper;
        private readonly ITemplatesRepository _templatesRepo;
        private readonly IEntityUpdateUtility _updateUtility;

        public TemplatedShiftService(
            ITemplatedShiftsRepository shiftsRepo,
            IMapper mapper,
            ITemplatesRepository templatesRepo,
            IEntityUpdateUtility updateUtility
        )
        {
            _shiftsRepo = shiftsRepo;
            _mapper = mapper;
            _templatesRepo = templatesRepo;
            _updateUtility = updateUtility;
        }

        public async Task<ServiceResult> CreateAsync(
            ShiftModel<TemplatedShiftDto> model,
            CancellationToken cancellation = default
        )
        {
            var entity = _mapper.Map<TemplatedShiftEntity>(model.Shift);
            var template = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(model.ParentId),
                cancellation
            );

            if (template == null)
            {
                return ServiceResult.Error($"Указанный шаблон (ID: {model.ParentId}) не найден");
            }

            entity.TemplateId = template.Id;

            var result = await _shiftsRepo.CreateAsync(entity, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            if (result.IsSuccess)
            {
                await _shiftsRepo.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Шаблонная смена (ID: {entity.Id}) успешно создана!");
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
            var result = await _shiftsRepo.DeleteAsync(id, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }
            if (result.IsSuccess)
            {
                await _shiftsRepo.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Шаблонная смена (ID: {id}) успешно удалена");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<TemplatedShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var entity = await _shiftsRepo.GetAsync(
                new EntityRequest<TemplatedShiftEntity>(id),
                cancellation
            );

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<TemplatedShiftDto>(entity);
            return dto;
        }

        public async Task<List<TemplatedShiftDto>> GetByTemplateIdAsync(
            string templateId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var repoRequest = new PaginatedTemplatedShiftRequest(templateId)
            {
                PageIndex = request.PageIndex.Value,
                Amount = request.Amount.Value,
            };
            var entities = await _shiftsRepo.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<TemplatedShiftDto>>(entities);
            return dtos;
        }

        public async Task<List<TemplatedShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var repoRequest = new PaginatedTemplatedShiftRequest()
            {
                PageIndex = request.PageIndex.Value,
                Amount = request.Amount.Value,
            };

            var entities = await _shiftsRepo.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<TemplatedShiftDto>>(entities);
            return dtos;
        }

        public async Task<ServiceResult> UpdateAsync(
            TemplatedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            var source = _mapper.Map<TemplatedShiftEntity>(dto);

            var target = await _shiftsRepo.GetAsync(
                new EntityRequest<TemplatedShiftEntity>(source.Id),
                cancellation
            );

            if (target == null)
            {
                return ServiceResult.Error($"Не найден указанный шаблон (ID: {source.Id})");
            }

            _updateUtility.Update(target, source);

            var result = _shiftsRepo.Update(target);

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            if (result.IsSuccess)
            {
                await _shiftsRepo.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Шаблонная смена (ID: {target.Id}) успешно обновлена!");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }
    }
}
