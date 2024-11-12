using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Shifts.Basic
{
    /// <summary>
    /// API контроллер для работы с "персональными" сменами
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/schedule/{scheduleId}/shifts/personal")]
    [Route("api/shifts/personal")]
    public class PersonalShiftsController : Controller
    {
        private readonly IPersonalShiftsService _personalShiftsService;

        public PersonalShiftsController(IPersonalShiftsService personalShiftsService)
        {
            _personalShiftsService = personalShiftsService;
        }

        /// <summary>
        /// Возвращает "персональную" смену
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
            var result = await _personalShiftsService.GetAsync(id, cancellation);

            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Json(result);
            }
        }

        /// <summary>
        /// Возвращает "персональные" смены для указанного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="request">Пагинированный запрос</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetForScheduleAsync(
            [FromRoute] [FromQuery] string scheduleId,
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var result = await _personalShiftsService.GetForScheduleAsync(
                scheduleId,
                request,
                cancellation
            );

            return Json(result);
        }

        /// <summary>
        /// Создаёт смену для указанного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="shift">DTO создаваемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            [FromRoute] [FromQuery] string scheduleId,
            [FromBody] PersonalShiftDto shift,
            CancellationToken cancellation = default
        )
        {
            var result = await _personalShiftsService.CreateAsync(
                new PersonalShiftModel { ScheduleId = scheduleId, Shift = shift },
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
        /// Обновляет смену
        /// </summary>
        /// <param name="id">Идентификатор обновляемой смены</param>
        /// <param name="shift">DTO обновляемой смены</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] string id,
            [FromBody] PersonalShiftDto shift,
            CancellationToken cancellation = default
        )
        {
            shift.Id = id;
            var result = await _personalShiftsService.UpdateAsync(shift, cancellation);

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
        /// Удаляет смену
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
            var result = await _personalShiftsService.DeleteAsync(id, cancellation);

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
