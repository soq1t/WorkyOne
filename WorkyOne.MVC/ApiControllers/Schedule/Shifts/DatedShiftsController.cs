using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.MVC.ViewModels.Api.Schedule.Shifts;

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

        /// <summary>
        /// Возвращает "датированную" смену
        /// </summary>
        /// <param name="id">Идентификатор смены</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDatedShiftAsync(
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
        /// <param name="viewModel">Вьюмодель создаваемой "датированной" смены</param>
        /// <param name="cancellation">Токен отмены задания</param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateDatedShiftAsync(
            [FromBody] DatedShiftViewModel viewModel,
            CancellationToken cancellation = default
        )
        {
            var result = await _datedShiftsService.CreateAsync(
                viewModel.DatedShift,
                viewModel.ScheduleId,
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

        /// <summary>
        /// Удаляет "датированну" смену
        /// </summary>
        /// <param name="id">Идентификатор удаляемой смены</param>
        /// <param name="cancellationToken">Токен отмены задания</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDatedShiftAsync(
            [FromRoute] string id,
            CancellationToken cancellationToken
        )
        {
            var result = await _datedShiftsService.DeleteAsync(id, cancellationToken);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Обновляет "датированную" смену
        /// </summary>
        /// <param name="dto">DTO обновляемой смены</param>
        /// <param name="cancellationToken">Токен отмены задания</param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateDatedShiftAsync(
            [FromBody] DatedShiftDto dto,
            CancellationToken cancellationToken
        )
        {
            var result = await _datedShiftsService.UpdateAsync(dto, cancellationToken);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
