using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.MVC.Models.Schedule.Shifts;

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
        /// <summary>
        /// Возвращает частичное представление для создания новой "персональной" смены
        /// </summary>
        [HttpPost]
        [Route("partial")]
        public IActionResult CreatePartial([FromQuery] [FromRoute] string scheduleId)
        {
            ShiftDtoBase shift = new PersonalShiftDto() { ScheduleId = scheduleId };
            return PartialView("Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", shift);
        }

        /// <summary>
        /// Проверяет валидность данных в модели представления
        /// </summary>
        /// <param name="model">Модель с данными</param>
        [HttpPost]
        [Route("partial/check")]
        public IActionResult CheckPartial(PersonalShiftDto model)
        {
            if (ModelState.IsValid)
            {
                return Json(model);
            }
            else
            {
                return PartialView("Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", model);
            }
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
    }
}
