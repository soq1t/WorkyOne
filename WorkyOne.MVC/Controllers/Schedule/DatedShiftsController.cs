using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Special;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.MVC.Models.Schedule.Shifts;

namespace WorkyOne.MVC.Controllers.Schedule
{
    /// <summary>
    /// Контроллер для работы с "датированными" сменами
    /// </summary>
    [Authorize]
    [Route("shifts/dated")]
    public class DatedShiftsController : Controller
    {
        private readonly IPersonalShiftsService _personalShiftsService;
        private readonly ISharedShiftsService _sharedShiftsService;
        private readonly IDatedShiftsService _datedShiftsService;

        public DatedShiftsController(
            IPersonalShiftsService personalShiftsService,
            ISharedShiftsService sharedShiftsService,
            IDatedShiftsService datedShiftsService
        )
        {
            _personalShiftsService = personalShiftsService;
            _sharedShiftsService = sharedShiftsService;
            _datedShiftsService = datedShiftsService;
        }

        /// <summary>
        /// Возвращает представление для создания смены
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPost]
        [Route("partial")]
        public async Task<IActionResult> CreateAsync(
            [FromQuery] string scheduleId,
            CancellationToken cancellation = default
        )
        {
            if (scheduleId == null)
            {
                return BadRequest("Schedule id is null");
            }

            var model = new DatedShiftViewModel()
            {
                Shift = new DatedShiftDto()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    ScheduleId = scheduleId
                }
            };

            await AddReferencedShiftsAsync(model, scheduleId, cancellation);

            return PartialView("Views/Shared/Schedules/Shifts/_DatedShiftPartial.cshtml", model);
        }

        /// <summary>
        /// Создаёт датированную смену
        /// </summary>
        /// <param name="model">Модель создаваемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            DatedShiftViewModel model,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "Views/Shared/Schedules/Shifts/_DatedShiftPartial.cshtml",
                    model
                );
            }

            await _datedShiftsService.CreateAsync(
                model.Shift,
                model.Shift.ScheduleId,
                cancellation
            );

            return Ok();
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
            var shift = await _datedShiftsService.GetAsync(id, cancellation);

            if (shift == null)
            {
                return BadRequest();
            }

            var model = new DatedShiftViewModel { Shift = shift };

            await AddReferencedShiftsAsync(model, shift.ScheduleId, cancellation);

            return PartialView("Views/Shared/Schedules/Shifts/_DatedShiftPartial.cshtml", model);
        }

        /// <summary>
        /// Обновляет смену
        /// </summary>
        /// <param name="model">Модель обновляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdatePartialAsync(
            DatedShiftViewModel model,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "Views/Shared/Schedules/Shifts/_DatedShiftPartial.cshtml",
                    model
                );
            }

            await _datedShiftsService.UpdateAsync(model.Shift, cancellation);

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
            await _datedShiftsService.DeleteAsync(id, cancellation);

            return Ok();
        }

        private async Task AddReferencedShiftsAsync(
            DatedShiftViewModel model,
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new PaginatedRequest { PageIndex = 1, Amount = 100 };
            model.SharedShifts = await _sharedShiftsService.GetManyAsync(request, cancellation);
            model.PersonalShifts = await _personalShiftsService.GetForScheduleAsync(
                scheduleId,
                request,
                cancellation
            );
        }
    }
}
