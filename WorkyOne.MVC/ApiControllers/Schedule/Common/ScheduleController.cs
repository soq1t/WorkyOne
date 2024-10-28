using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Common
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDatedShiftsService _datedShiftsService;
        private readonly IWorkGraphicService _workGraphicService;

        public ScheduleController(
            IScheduleService scheduleService,
            IDatedShiftsService datedShiftsService,
            IWorkGraphicService workGraphicService
        )
        {
            _scheduleService = scheduleService;
            _datedShiftsService = datedShiftsService;
            _workGraphicService = workGraphicService;
        }

        /// <summary>
        /// Возвращает множество расписаний согласно заданным условиям
        /// </summary>
        /// <param name="request">Запрос на получение расписания</param>
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
        /// <param name="dto">DTO создавемого расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] ScheduleDto dto,
            CancellationToken cancellation
        )
        {
            var result = await _scheduleService.CreateScheduleAsync(dto, cancellation);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Удаляет расписание
        /// </summary>
        /// <param name="id">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var result = await _scheduleService.DeleteAsync(id, cancellation);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Обновляет расписание
        /// </summary>
        /// <param name="id">Идентификатор расписания</param>
        /// <param name="dto">DTO расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] string id,
            [FromBody] ScheduleDto dto,
            CancellationToken cancellation = default
        )
        {
            dto.Id = id;
            var result = await _scheduleService.UpdateScheduleAsync(dto, cancellation);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        #region Work Graphic
        [HttpPost]
        [Route("{id}/graphic")]
        public async Task<IActionResult> CreateGraphicAsync(
            [FromRoute] string id,
            [FromBody] WorkGraphicModel model,
            CancellationToken cancellation = default
        )
        {
            model.ScheduleId = id;
            var result = await _workGraphicService.CreateAsync(model, cancellation);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("{id}/graphic")]
        public async Task<IActionResult> GetGraphicAsync(
            [FromRoute] string id,
            [FromQuery] PaginatedWorkGraphicRequest request,
            CancellationToken cancellation = default
        )
        {
            request.ScheduleId = id;

            var result = await _workGraphicService.GetGraphicAsync(request, cancellation);

            return Json(result);
        }
        #endregion
    }
}
