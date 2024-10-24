using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.Repositories.Requests.Common;
using WorkyOne.Contracts.Repositories.Requests.Schedule.Common;
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
        /// Возвращает множество расписаний согласно заданным условиям
        /// </summary>
        /// <param name="model">Условия получения расписани</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetManyAsync(
            [FromQuery] PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            var result = await _scheduleService.GetManyAsync(request, cancellation);

            return Json(result);
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
            CancellationToken cancellation
        )
        {
            var item = await _scheduleService.GetAsync(id, cancellation);

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
            [FromBody] CreateScheduleViewModel model,
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
            [FromRoute] string scheduleId,
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var shifts = await _datedShiftsService.GetForScheduleAsync(
                scheduleId,
                request,
                cancellation
            );
            return Json(shifts);
        }
        #endregion
    }
}
