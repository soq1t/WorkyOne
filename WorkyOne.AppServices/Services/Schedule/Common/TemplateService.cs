using System.Text;
using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Users;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Common
{
    /// <summary>
    /// Сервис по работе с шаблонами
    /// </summary>
    public sealed class TemplateService : ITemplateService
    {
        private readonly ITemplatesRepository _templatesRepo;
        private readonly ISchedulesRepository _schedulesRepo;
        private readonly IShiftSequencesRepository _shiftsSequencesRepo;
        private readonly ITemplatedShiftsRepository _templatedShiftsRepo;

        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _updateUtility;
        private readonly IUserAccessInfoProvider _accessInfoProvider;

        private TemplateAccessFilter _templateFilter;
        private ScheduleAccessFilter _scheduleFilter;
        private ShiftSequenceAccessFilter _sequenceFilter;
        private TemplatedShiftAccessFilter _shiftFilter;

        public TemplateService(
            ITemplatesRepository templatesRepo,
            IShiftSequencesRepository shiftsSequencesRepo,
            IMapper mapper,
            IEntityUpdateUtility updateUtility,
            ISchedulesRepository schedulesRepo,
            IUserAccessInfoProvider accessInfoProvider,
            ITemplatedShiftsRepository templatedShiftsRepo
        )
        {
            _templatesRepo = templatesRepo;
            _shiftsSequencesRepo = shiftsSequencesRepo;
            _mapper = mapper;
            _updateUtility = updateUtility;
            _schedulesRepo = schedulesRepo;
            _accessInfoProvider = accessInfoProvider;

            InitFiltersAsync().Wait();
            _templatedShiftsRepo = templatedShiftsRepo;
        }

        public async Task<ServiceResult> CreateAsync(
            TemplateModel model,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(model.ScheduleId).And(_scheduleFilter);

            var request = new ScheduleRequest(filter) { IncludeTemplate = true };

            var schedule = await _schedulesRepo.GetAsync(request, cancellation);

            if (schedule == null)
            {
                return ServiceResult.Error(
                    $"Указанное расписание (ID: {model.ScheduleId}) не найдено"
                );
            }

            if (schedule.Template != null)
            {
                return ServiceResult.Error(
                    $"У указанного расписания (ID: {schedule.Id}) уже есть шаблон (ID: {schedule.Template.Id})"
                );
            }

            var entity = _mapper.Map<TemplateEntity>(model.Template);
            entity.Id = Guid.NewGuid().ToString();

            entity.ScheduleId = schedule.Id;

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
            var filter = new EntityIdFilter<TemplateEntity>(id).And(_templateFilter);

            var request = new EntityRequest<TemplateEntity>(filter);
            var deleted = await _templatesRepo.GetAsync(request, cancellation);

            if (deleted == null)
            {
                return ServiceResult.Error($"Сущность (ID: {id}) не найдена, либо доступ запрещён");
            }

            var result = _templatesRepo.Delete(deleted);

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
            var filter = new EntityIdFilter<TemplateEntity>(id).And(_templateFilter);

            var entity = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(filter),
                cancellation
            );
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
            var filter = new Specification<TemplateEntity>(x => x.ScheduleId == scheduleId).And(
                _templateFilter
            );

            var entity = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(filter),
                cancellation
            );

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<TemplateDto>(entity);
            return dto;
        }

        public async Task<List<ShiftSequenceDto>> GetSequencesAsync(
            string templateId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<ShiftSequenceEntity>(x =>
                x.TemplateId == templateId
            ).And(_sequenceFilter);

            var entities = await _shiftsSequencesRepo.GetManyAsync(
                new PaginatedRequest<ShiftSequenceEntity>(
                    filter,
                    request.PageIndex,
                    request.Amount
                ),
                cancellation
            );

            var dtos = _mapper.Map<List<ShiftSequenceDto>>(entities);

            dtos = dtos.OrderBy(x => x.Position).ToList();

            return dtos;
        }

        public async Task<ServiceResult> UpdateAsync(
            TemplateDto dto,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplateEntity>(dto.Id).And(_templateFilter);

            var target = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(filter),
                cancellation
            );

            if (target == null)
            {
                return ServiceResult.Error(
                    $"Обновляемый шаблон (ID: {dto.Id}) не найден либо доступ запрещён"
                );
            }

            var source = _mapper.Map<TemplateEntity>(dto);

            _updateUtility.Update(target, source);

            _templatesRepo.Update(target);

            await _templatesRepo.SaveChangesAsync(cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            return ServiceResult.Ok($"Шаблон (ID: {target.Id}) был успешно обновлён");
        }

        public async Task<ServiceResult> UpdateSequenceAsync(
            ShiftSequencesModel model,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplateEntity>(model.TemplateId).And(_templateFilter);

            var template = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(filter),
                cancellation
            );

            if (template == null)
            {
                return ServiceResult.Error(
                    $"Не найден шаблон(ID: {model.TemplateId}), либо доступ запрещён"
                );
            }

            model.Sequences = model.Sequences.OrderBy(x => x.Position).ToList();

            if (!CheckSequence(model.Sequences))
            {
                return ServiceResult.Error(
                    $"Значение Position должно быть уникально для каждой сущности"
                );
            }

            if (!model.Sequences.Any())
            {
                await _shiftsSequencesRepo.DeleteByConditionAsync(
                    new Specification<ShiftSequenceEntity>(x => x.TemplateId == template.Id),
                    cancellation
                );

                return ServiceResult.Ok(
                    $"Последовательность для шаблона (ID: {template.Id}) была успешно очищена!"
                );
            }

            var sequence = _mapper.Map<List<ShiftSequenceEntity>>(model.Sequences);
            var shifts = await GetTemplatedShiftAsync(template.Id, cancellation);

            foreach (var item in sequence)
            {
                item.Template = template;

                var shift = shifts.First(x => x.Id == item.ShiftId);
                if (shift == null)
                {
                    return ServiceResult.Error(
                        $"Не удалось найти \"шаблонную\" смену (ID: {item.ShiftId})"
                    );
                }
                item.Shift = shift;
            }
            sequence = sequence.OrderBy(x => x.Position).ToList();

            await _shiftsSequencesRepo.DeleteByConditionAsync(
                new Specification<ShiftSequenceEntity>(x => x.TemplateId == template.Id),
                cancellation
            );
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

        private async Task InitFiltersAsync()
        {
            if (_sequenceFilter == null)
            {
                var accessInfo = await _accessInfoProvider.GetCurrentAsync();

                _sequenceFilter = new ShiftSequenceAccessFilter(accessInfo);
                _templateFilter = new TemplateAccessFilter(accessInfo);
                _scheduleFilter = new ScheduleAccessFilter(accessInfo);
                _shiftFilter = new TemplatedShiftAccessFilter(accessInfo);
            }
        }

        private bool CheckSequence(IEnumerable<ShiftSequenceDto> sequence)
        {
            var i = 0;
            foreach (var item in sequence)
            {
                if (item.Position == i)
                {
                    return false;
                }

                item.Position = ++i;
            }

            return true;
        }

        private Task<List<TemplatedShiftEntity>> GetTemplatedShiftAsync(
            string templateId,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<TemplatedShiftEntity>(x =>
                x.TemplateId == templateId
            ).And(_shiftFilter);

            return _templatedShiftsRepo.GetManyAsync(
                new PaginatedRequest<TemplatedShiftEntity>(filter, 1, 100)
            );
        }
    }
}
