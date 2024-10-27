using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Common
{
    /// <summary>
    /// API для работы с шаблонами
    /// </summary>
    [ApiController]
    [Route("api/schedule/{scheduleId}/template")]
    [Route("api/template")]
    public sealed class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        /// <summary>
        /// Возвращает шаблон по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор шаблона</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var result = await _templateService.GetAsync(id, cancellation);

            return Json(result);
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
        /// <param name="model">Модель, содержащая информацию о создаваемом шаблоне</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            [FromRoute] [FromQuery] string? scheduleId,
            [FromBody] TemplateModel model,
            CancellationToken cancellation = default
        )
        {
            model.ScheduleId = scheduleId;

            if (model.ScheduleId == null)
            {
                return BadRequest($"{nameof(model)}.{nameof(model.ScheduleId)} is required");
            }

            var result = await _templateService.CreateAsync(model, cancellation);

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
