using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Shifts
{
    [ApiController]
    [Route("api/shifts/dated")]
    [Route("api/schedule/{scheduleId}/shifts/dated")]
    public class DatedShiftsController : Controller
    {
        private readonly IDatedShiftsService _datedShiftsService;

        public DatedShiftsController(IDatedShiftsService datedShiftsService)
        {
            _datedShiftsService = datedShiftsService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetManyAsync(
            [FromQuery] PaginatedRequest request,
            [FromRoute] string? scheduleId,
            CancellationToken cancellation = default
        )
        {
            List<DatedShiftDto> result;

            if (scheduleId != null)
            {
                result = await _datedShiftsService.GetForScheduleAsync(
                    scheduleId,
                    request,
                    cancellation
                );
            }
            else
            {
                result = await _datedShiftsService.GetManyAsync(request, cancellation);
            }

            return Json(result);
        }

        /// <summary>
        /// Возвращает "датированную" смену
        /// </summary>
        /// <param name="id">Идентификатор смены</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var item = await _datedShiftsService.GetAsync(id, cancellation);

            if (item == null)
            {
                return BadRequest($"Не найдена смена с указанным ID");
            }
            else
            {
                return Json(item);
            }
        }

        /// <summary>
        /// Создаёт "датированную" смену для указанного расписания
        /// </summary>
        /// <param name="model">Модель, содержащая информацию о создаваемой смене</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            [FromRoute] [FromQuery] string scheduleId,
            [FromBody] ShiftModel<DatedShiftDto> model,
            CancellationToken cancellation = default
        )
        {
            model.ParentId = model.ParentId ?? scheduleId;

            if (model.ParentId == null)
            {
                return BadRequest($"{nameof(model)}.{nameof(model.ParentId)} is required");
            }

            var result = await _datedShiftsService.CreateAsync(
                model.Shift,
                model.ParentId,
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

        /// <summary>
        /// Удаляет "датированну" смену
        /// </summary>
        /// <param name="id">Идентификатор удаляемой смены</param>
        /// <param name="cancellationToken">Токен отмены задания</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id,
            CancellationToken cancellationToken
        )
        {
            var result = await _datedShiftsService.DeleteAsync(id, cancellationToken);
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
        /// Обновляет "датированную" смену
        /// </summary>
        /// <param name="id">Идентификатор обновляемой смены</param>
        /// <param name="dto">DTO обновляемой смены</param>
        /// <param name="cancellationToken">Токен отмены задания</param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] string id,
            [FromBody] DatedShiftDto dto,
            CancellationToken cancellationToken
        )
        {
            dto.Id = id;
            var result = await _datedShiftsService.UpdateAsync(dto, cancellationToken);

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
