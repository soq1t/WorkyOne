using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;

namespace WorkyOne.MVC.Controllers.Schedule
{
    /// <summary>
    /// Контроллер по работе с "персональными" сменами
    /// </summary>
    [Authorize]
    [Route("schedules/{scheduleId}/shifts/personal")]
    [Route("shifts/personal")]
    public class PersonalShiftsController : Controller
    {
        private readonly IPersonalShiftsService _personalShiftsService;

        public PersonalShiftsController(IPersonalShiftsService personalShiftsService)
        {
            _personalShiftsService = personalShiftsService;
        }

        /// <summary>
        /// Возвращает частичное представление для создания новой "персональной" смены
        /// </summary>
        [HttpGet]
        [Route("")]
        public IActionResult CreatePartial([FromQuery] [FromRoute] string scheduleId)
        {
            ShiftDtoBase shift = new PersonalShiftDto() { ScheduleId = scheduleId };
            return PartialView("Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", shift);
        }

        /// <summary>
        /// Создаёт смену
        /// </summary>
        /// <param name="model">Создаваемая смена</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            PersonalShiftDto model,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", model);
            }

            await _personalShiftsService.CreateAsync(
                new PersonalShiftModel() { Shift = model, ScheduleId = model.ScheduleId }
            );

            return Ok();
        }

        /// <summary>
        /// Вовращает частичное представление для указанной модели
        /// </summary>
        /// <param name="model">МОдель с данными</param>

        [HttpGet]
        [Route("partial")]
        public IActionResult GetPartial(PersonalShiftDto model)
        {
            if (ModelState.IsValid)
            {
                return PartialView("Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", model);
            }
            else
            {
                return BadRequest("Invalid model");
            }
        }

        /// <summary>
        /// Обновляет смену
        /// </summary>
        /// <param name="model">Модель обновляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(
            PersonalShiftDto model,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", model);
            }

            await _personalShiftsService.UpdateAsync(model, cancellation);

            return Ok();
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteAsync(
            [FromQuery] string id,
            CancellationToken cancellation = default
        )
        {
            await _personalShiftsService.DeleteAsync(id, cancellation);

            return Ok();
        }
    }
}
