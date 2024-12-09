using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.MVC.Models.Admin;

namespace WorkyOne.MVC.Controllers.Admin
{
    /// <summary>
    /// Контроллер по работе со сменами приложения
    /// </summary>
    [Authorize(Roles = "Admin,God")]
    [Route("admin/shifts")]
    public class ShiftsController : Controller
    {
        private readonly ISharedShiftsService _shiftsService;

        public ShiftsController(ISharedShiftsService shiftsService)
        {
            _shiftsService = shiftsService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> IndexAsync(CancellationToken cancellation = default)
        {
            var shifts = await _shiftsService.GetManyAsync(
                new PaginatedRequest() { Amount = 100, PageIndex = 1 },
                cancellation
            );

            var model = new ShiftsViewModel { Shifts = shifts };

            return View("/Views/Admin/Shifts.cshtml", model);
        }

        [HttpGet]
        [Route("partial")]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string? id,
            CancellationToken cancellation = default
        )
        {
            SharedShiftDto? shift;

            if (id == null)
            {
                shift = new SharedShiftDto();
            }
            else
            {
                shift = await _shiftsService.GetAsync(id, cancellation);
            }

            if (shift != null)
            {
                return PartialView("/Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", shift);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            SharedShiftDto shift,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return PartialView("/Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", shift);
            }

            await _shiftsService.CreateAsync(shift, cancellation);

            return Ok();
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(
            SharedShiftDto shift,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return PartialView("/Views/Shared/Schedules/Shifts/_ShiftPartial.cshtml", shift);
            }

            await _shiftsService.UpdateAsync(shift, cancellation);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            await _shiftsService.DeleteAsync(id, cancellation);

            return Ok();
        }
    }
}
