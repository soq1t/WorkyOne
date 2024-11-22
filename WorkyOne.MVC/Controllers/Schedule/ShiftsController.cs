using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Special;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Controllers.Schedule
{
    /// <summary>
    /// Контроллер по работе со сменами
    /// </summary>
    [Authorize]
    [Route("schedules/{scheduleId}")]
    [Route("shifts")]
    public class ShiftsController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly ITemplatedShiftService _templatedShiftService;

        public ShiftsController(
            IScheduleService scheduleService,
            ITemplatedShiftService templatedShiftService
        )
        {
            _scheduleService = scheduleService;
            _templatedShiftService = templatedShiftService;
        }

        [HttpGet]
        [Route("templated/partial")]
        public async Task<IActionResult> GetTemplatedShiftsPartialAsync(
            [FromRoute] string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var schedule = await _scheduleService.GetAsync(scheduleId, cancellation);

            if (schedule == null)
            {
                return BadRequest("Расписание не найдено");
            }

            var model = new TemplatedShiftsViewModel
            {
                Shifts = schedule.Template?.Shifts ?? [],
                PersonalShifts = schedule.PersonalShifts,
                SharedShifts = schedule.SharedShifts
            };

            return PartialView("Schedules/_NewSchedulePartial", model);
        }

        [HttpGet]
        [Route("templated/{shiftId}/move")]
        public async Task<IActionResult> ChangeTemplatedPositionAsync(
            [FromRoute] string shiftId,
            [FromQuery] int steps,
            CancellationToken cancellation = default
        )
        {
            var result = await _templatedShiftService.ChangePositionAsync(
                shiftId,
                steps,
                cancellation
            );

            if (result.IsSucceed)
            {
                return Ok(result.SucceedMessage);
            }
            else
            {
                return BadRequest(result.GetErrors());
            }
        }
    }
}
