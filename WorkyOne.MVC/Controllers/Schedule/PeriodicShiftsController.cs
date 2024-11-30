using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Special;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.MVC.Models.Schedule.Shifts;

namespace WorkyOne.MVC.Controllers.Schedule
{
    /// <summary>
    /// Котроллер для работы с "периодическими" сменами
    /// </summary>
    [Authorize]
    [Route("shifts/periodic")]
    public class PeriodicShiftsController : Controller
    {
        private readonly IPeriodicShiftsService _periodicShiftsService;
        private readonly IPersonalShiftsService _personalShiftsService;
        private readonly ISharedShiftsService _sharedShiftsService;

        public PeriodicShiftsController(
            IPeriodicShiftsService periodicShiftsService,
            IPersonalShiftsService personalShiftsService,
            ISharedShiftsService sharedShiftsService
        )
        {
            _periodicShiftsService = periodicShiftsService;
            _personalShiftsService = personalShiftsService;
            _sharedShiftsService = sharedShiftsService;
        }

        /// <summary>
        /// Возвращает представление для создания смены
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        [HttpPost]
        [Route("partial")]
        public async Task<IActionResult> CreatePartialAsync(
            [FromQuery] string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var shift = new PeriodicShiftDto { ScheduleId = scheduleId };
            var model = new PeriodicShiftViewModel { Shift = shift };

            await AddReferencedShiftsAsync(model, scheduleId, cancellation);

            return PartialView("Views/Shared/Schedules/Shifts/_PeriodicShiftPartial.cshtml", model);
        }

        /// <summary>
        /// Возвращает частичное представление для смены
        /// </summary>
        /// <param name="id">Идентификатор смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("partial")]
        public async Task<IActionResult> GetPartialAsync(
            [FromQuery] string id,
            CancellationToken cancellation = default
        )
        {
            var shift = await _periodicShiftsService.GetAsync(id, cancellation);

            if (shift == null)
            {
                return BadRequest();
            }

            var model = new PeriodicShiftViewModel { Shift = shift };

            await AddReferencedShiftsAsync(model, shift.ScheduleId, cancellation);

            return PartialView("Views/Shared/Schedules/Shifts/_PeriodicShiftPartial.cshtml", model);
        }

        /// <summary>
        /// Создаёт смену
        /// </summary>
        /// <param name="model">Модель с данными создаваемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            PeriodicShiftViewModel model,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "Views/Shared/Schedules/Shifts/_PeriodicShiftPartial.cshtml",
                    model
                );
            }
            await _periodicShiftsService.CreateAsync(
                new ShiftReferenceModel<PeriodicShiftDto>
                {
                    Shift = model.Shift,
                    ParentId = model.Shift.ScheduleId
                },
                cancellation
            );

            return Ok();
        }

        /// <summary>
        /// Обновляет смену
        /// </summary>
        /// <param name="model">Модель с данными обновляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(
            PeriodicShiftViewModel model,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "Views/Shared/Schedules/Shifts/_PeriodicShiftPartial.cshtml",
                    model
                );
            }

            await _periodicShiftsService.UpdateAsync(model.Shift, cancellation);

            return Ok();
        }

        /// <summary>
        /// Удаляет смену
        /// </summary>
        /// <param name="id">Идентификатор смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteAsync(
            [FromQuery] string id,
            CancellationToken cancellation = default
        )
        {
            await _periodicShiftsService.DeleteAsync(id, cancellation);

            return Ok();
        }

        private async Task AddReferencedShiftsAsync(
            PeriodicShiftViewModel model,
            string scheduleId,
            CancellationToken cancellation
        )
        {
            var request = new PaginatedRequest() { Amount = 100, PageIndex = 1 };

            model.PersonalShifts = await _personalShiftsService.GetForScheduleAsync(
                scheduleId,
                request,
                cancellation
            );
            model.SharedShifts = await _sharedShiftsService.GetManyAsync(request, cancellation);
        }
    }
}
