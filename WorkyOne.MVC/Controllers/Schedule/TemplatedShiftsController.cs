using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Controllers.Schedule
{
    /// <summary>
    /// Контроллер по работе с "шаблонными" сменами
    /// </summary>
    [Authorize]
    [Route("shifts/templated")]
    public class TemplatedShiftsController : Controller
    {
        private readonly IPersonalShiftsService _personalShiftsService;
        private readonly ISharedShiftsService _sharedShiftsService;

        public TemplatedShiftsController(
            IPersonalShiftsService personalShiftsService,
            ISharedShiftsService sharedShiftsService
        )
        {
            _personalShiftsService = personalShiftsService;
            _sharedShiftsService = sharedShiftsService;
        }

        /// <summary>
        /// Создаёт частичное представление для последующего использования в таблице "шаблонных" смен
        /// </summary>
        /// <param name="shiftId">Идентификатор смены, на которую ссылается "шаблонная" смена</param>
        /// <param name="position">Позиция шаблонной смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPost]
        [Route("partial")]
        public async Task<IActionResult> CreatePartialAsync(
            string shiftId,
            int position,
            CancellationToken cancellation = default
        )
        {
            ShiftDtoBase? shift = await _personalShiftsService.GetAsync(shiftId, cancellation);

            if (shift == null)
            {
                shift = await _sharedShiftsService.GetAsync(shiftId, cancellation);

                if (shift == null)
                {
                    return BadRequest();
                }
            }

            var model = new TemplatedShiftViewModel() { ReferenceShift = shift };

            model.TemplatedShift = new TemplatedShiftDto
            {
                Position = position,
                ShiftId = shift.Id
            };
            model.Index = position - 1;

            return PartialView("Views/Shared/Schedules/_TemplatedShiftPartial.cshtml", model);
        }
    }
}
