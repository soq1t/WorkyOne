using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Common
{
    /// <summary>
    /// Контроллер для работы с очерёдностью смен в шаблоне
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/schedule/{scheduleId}/template/sequences")]
    [Route("api/template/{templateId}/sequences")]
    public class ShiftSequencesController : Controller
    {
        private readonly ITemplateService _templateService;

        public ShiftSequencesController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        /// <summary>
        /// Возвращает последовательность смен в шаблоне
        /// </summary>
        /// <param name="templateId">Идентификатор шаблона</param>
        /// <param name="request">Пагинированный запрос</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] [FromQuery] string templateId,
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var result = await _templateService.GetSequencesAsync(
                templateId,
                request,
                cancellation
            );

            return Json(result);
        }

        /// <summary>
        /// Обновляет последовательность смен в шаблоне
        /// </summary>
        /// <param name="model">Модель с данными</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] [FromQuery] string templateId,
            [FromBody] ShiftSequencesModel model,
            CancellationToken cancellation = default
        )
        {
            model.TemplateId = templateId;
            var result = await _templateService.UpdateSequenceAsync(model, cancellation);

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
