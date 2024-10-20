using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.MVC.ViewModels.Api.Schedule.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Common
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDatedShiftsService _datedShiftsService;

        public ScheduleController(
            IScheduleService scheduleService,
            IDatedShiftsService datedShiftsService
        )
        {
            _scheduleService = scheduleService;
            _datedShiftsService = datedShiftsService;
        }

        /// <summary>
        /// Возвращает расписание по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string id,
            CancellationToken cancellation,
            [FromQuery] bool includeFullData = false
        )
        {
            var request = new ScheduleRequest { Id = id };
            if (includeFullData)
            {
                request.IncludePeriodicShifts = true;
                request.IncludeDatedShifts = true;
                request.IncludeTemplate = true;
            }
            var item = await _scheduleService.GetAsync(request, cancellation);

            if (item == null)
            {
                return BadRequest($"Не найдено расписание с указанным ID");
            }
            else
            {
                return Json(item);
            }
        }

        /// <summary>
        /// Создаёт расписание
        /// </summary>
        /// <param name="model">Вьюмодель расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] ScheduleViewModel model,
            CancellationToken cancellation
        )
        {
            var result = await _scheduleService.CreateScheduleAsync(
                model.ScheduleName,
                model.UserDataId,
                cancellation
            );

            if (string.IsNullOrEmpty(result))
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        #region Dated Shifts
        /// <summary>
        /// Возвращает список "датированных" смен для указанного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpGet]
        [Route("{scheduleId}/shifts/dated")]
        public async Task<IActionResult> GetDatedShiftsAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var shifts = await _datedShiftsService.GetForScheduleAsync(scheduleId, cancellation);
            return Json(shifts);
        }

        /// <summary>
        /// Удаляет все "датированные" смены для указанного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpDelete]
        [Route("{scheduleId}/shifts/dated")]
        public async Task<IActionResult> ClearDatedShiftsAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            bool result = await _datedShiftsService.DeleteForScheduleAsync(
                scheduleId,
                cancellation
            );

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
