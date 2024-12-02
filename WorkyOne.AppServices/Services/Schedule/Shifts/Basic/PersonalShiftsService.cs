﻿using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Context;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Shifts.Basic
{
    /// <summary>
    /// Сервис по работе со сменами в расписании
    /// </summary>
    public class PersonalShiftsService : IPersonalShiftsService
    {
        private readonly IPersonalShiftRepository _shiftRepository;
        private readonly ISchedulesRepository _schedulesRepository;

        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _entityUpdater;
        private readonly IAccessFiltersStore _accessFiltersStore;

        private readonly IApplicationContextService _contextService;

        private AccessFilter<PersonalShiftEntity> _shiftAccessFilter =>
            _accessFiltersStore.GetFilter<PersonalShiftEntity>();
        private AccessFilter<ScheduleEntity> _scheduleAccessFilter =>
            _accessFiltersStore.GetFilter<ScheduleEntity>();

        public PersonalShiftsService(
            IUserAccessInfoProvider accessInfoProvider,
            IPersonalShiftRepository shiftRepository,
            ISchedulesRepository schedulesRepository,
            IMapper mapper,
            IEntityUpdateUtility entityUpdater,
            IAccessFiltersStore accessFiltersStore,
            IApplicationContextService contextService
        )
        {
            _shiftRepository = shiftRepository;
            _schedulesRepository = schedulesRepository;

            _mapper = mapper;
            _entityUpdater = entityUpdater;
            _accessFiltersStore = accessFiltersStore;
            _contextService = contextService;
        }

        public async Task<RepositoryResult> CreateAsync(
            PersonalShiftModel model,
            CancellationToken cancellation = default
        )
        {
            await _contextService.CreateTransactionAsync(cancellation);

            var schedule = await _schedulesRepository.GetAsync(
                new ScheduleRequest(
                    new EntityIdFilter<ScheduleEntity>(model.ScheduleId).And(_scheduleAccessFilter)
                ),
                cancellation
            );

            if (schedule == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    model.ScheduleId,
                    nameof(ScheduleEntity)
                );
            }

            var entity = _mapper.Map<PersonalShiftEntity>(model.Shift);
            entity.Schedule = schedule;

            var result = await _shiftRepository.CreateAsync(entity, cancellation);

            if (result.IsSucceed)
            {
                schedule.IsGraphicUpdateRequired = true;
                _schedulesRepository.Update(schedule);

                await _contextService.SaveChangesAsync(cancellation);
                await _contextService.CommitTransactionAsync(cancellation);
            }
            else
            {
                await _contextService.RollbackTransactionAsync(cancellation);
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            await _contextService.CreateTransactionAsync(cancellation);

            var entity = await _shiftRepository.GetAsync(
                new EntityRequest<PersonalShiftEntity>(
                    new EntityIdFilter<PersonalShiftEntity>(id).And(_shiftAccessFilter)
                ),
                cancellation
            );

            if (entity == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    id,
                    typeof(PersonalShiftEntity).Name
                );
            }

            var schedule = await _schedulesRepository.GetAsync(
                new ScheduleRequest(new EntityIdFilter<ScheduleEntity>(entity.ScheduleId)),
                cancellation
            );

            var result = _shiftRepository.Delete(entity);

            if (result.IsSucceed)
            {
                schedule.IsGraphicUpdateRequired = true;
                _schedulesRepository.Update(schedule);

                await _contextService.SaveChangesAsync(cancellation);
                await _contextService.CommitTransactionAsync(cancellation);
            }
            else
            {
                await _contextService.RollbackTransactionAsync(cancellation);
            }

            return result;
        }

        public async Task<PersonalShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var entity = await _shiftRepository.GetAsync(
                new EntityRequest<PersonalShiftEntity>(
                    new EntityIdFilter<PersonalShiftEntity>(id).And(_shiftAccessFilter)
                ),
                cancellation
            );

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<PersonalShiftDto>(entity);
        }

        public async Task<List<PersonalShiftDto>> GetForScheduleAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var entities = await _shiftRepository.GetManyAsync(
                new PaginatedRequest<PersonalShiftEntity>(
                    new Specification<PersonalShiftEntity>(x => x.ScheduleId == scheduleId).And(
                        _shiftAccessFilter
                    ),
                    request.PageIndex,
                    request.Amount
                ),
                cancellation
            );

            return _mapper.Map<List<PersonalShiftDto>>(entities);
        }

        public async Task<RepositoryResult> UpdateAsync(
            PersonalShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            await _contextService.CreateTransactionAsync(cancellation);

            var target = await _shiftRepository.GetAsync(
                new EntityRequest<PersonalShiftEntity>(
                    new EntityIdFilter<PersonalShiftEntity>(dto.Id).And(_shiftAccessFilter)
                ),
                cancellation
            );

            if (target == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    dto.Id,
                    nameof(PersonalShiftEntity)
                );
            }

            var source = _mapper.Map<PersonalShiftEntity>(dto);

            _entityUpdater.Update(target, source);

            var result = _shiftRepository.Update(target);

            var schedule = await _schedulesRepository.GetAsync(
                new ScheduleRequest(new EntityIdFilter<ScheduleEntity>(target.ScheduleId)),
                cancellation
            );

            if (result.IsSucceed)
            {
                schedule.IsGraphicUpdateRequired = true;
                _schedulesRepository.Update(schedule);

                await _contextService.SaveChangesAsync(cancellation);
                await _contextService.CommitTransactionAsync(cancellation);
            }
            else
            {
                await _contextService.RollbackTransactionAsync(cancellation);
            }

            return result;
        }
    }
}
