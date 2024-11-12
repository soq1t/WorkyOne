using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Shifts
{
    /// <summary>
    /// API для взаимодействия с "шаблонными" сменами
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/shifts/templated")]
    [Route("api/template/{templateId}/shifts")]
    [Route("api/schedule/{scheduleId}/template/shifts")]
    public class TemplatedShiftsController : Controller
    {
        private readonly ITemplatedShiftService _shiftsService;

        public TemplatedShiftsController(ITemplatedShiftService shiftsService)
        {
            _shiftsService = shiftsService;
        }

        /// <summary>
        /// Возвращает смену по её идентификатору
        /// </summary>
        /// <param name="id">Идентификатор смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var result = await _shiftsService.GetAsync(id, cancellation);
            return Json(result);
        }

        /// <summary>
        /// Возвращает множество "шаблонных" смен
        /// </summary>
        /// <param name="templateId">Идентификатор шаблона, для которого возвращаются смены</param>
        /// <param name="scheduleId">Идентификатор расписания, для которого возвращаются смены</param>
        /// <param name="request">Пагинированный запрос на получение смен</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetManyAsync(
            [FromRoute] string? templateId,
            [FromRoute] string? scheduleId,
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            List<TemplatedShiftDto> result;

            if (scheduleId != null)
            {
                result = await _shiftsService.GetByScheduleIdAsync(
                    scheduleId,
                    request,
                    cancellation
                );
            }
            else if (templateId != null)
            {
                result = await _shiftsService.GetByTemplateIdAsync(
                    templateId,
                    request,
                    cancellation
                );
            }
            else
            {
                result = await _shiftsService.GetManyAsync(request, cancellation);
            }

            return Json(result);
        }

        /// <summary>
        /// Создаёт "шаблонную" смену
        /// </summary>
        /// <param name="model">Модель создаваемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            [FromRoute] [FromQuery] string templateId,
            [FromBody] ShiftModel<TemplatedShiftDto> model,
            CancellationToken cancellation = default
        )
        {
            model.ParentId = templateId;
            var result = await _shiftsService.CreateAsync(model, cancellation);

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
        /// Обновляет указанную смену
        /// </summary>
        /// <param name="id">Идентификатор обновляемой смены</param>
        /// <param name="dto">DTO обновляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] string id,
            [FromBody] TemplatedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            dto.Id = id;

            var result = await _shiftsService.UpdateAsync(dto, cancellation);
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
        /// Удаляет указанную смену
        /// </summary>
        /// <param name="id">Идентификатор удаляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var result = await _shiftsService.DeleteAsync(id, cancellation);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
