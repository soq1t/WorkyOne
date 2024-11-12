using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Common
{
    /// <summary>
    /// API для работы с шаблонами
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/schedule/{scheduleId}/template")]
    public sealed class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        /// <summary>
        /// Возвращает шаблон по идентификатору расписания, к которому он относится
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetByScheduleAsync(
            [FromRoute] string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var result = await _templateService.GetByScheduleIdAsync(scheduleId, cancellation);

            return Json(result);
        }

        /// <summary>
        /// Создаёт шаблон в базе данных
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания, для которого создаётся шаблон</param>
        /// <param name="dto">DTO создаваемого шаблона</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            [FromRoute] [FromQuery] string scheduleId,
            [FromBody] TemplateDto dto,
            CancellationToken cancellation = default
        )
        {
            var result = await _templateService.CreateAsync(
                new TemplateModel { ScheduleId = scheduleId, Template = dto },
                cancellation
            );

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(
            [FromBody] TemplateDto dto,
            CancellationToken cancellation = default
        )
        {
            var result = await _templateService.UpdateAsync(dto, cancellation);

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var result = await _templateService.DeleteAsync(id, cancellation);

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
