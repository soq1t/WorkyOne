using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Services.GetRequests.Common;

namespace WorkyOne.MVC.ApiControllers.Schedule.Shifts.Basic
{
    [ApiController]
    [Authorize]
    [Route("api/shifts/shared")]
    public class SharedShiftsController : Controller
    {
        private readonly ISharedShiftsService _sharedShiftsService;

        public SharedShiftsController(ISharedShiftsService sharedShiftsService)
        {
            _sharedShiftsService = sharedShiftsService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var result = await _sharedShiftsService.GetAsync(id, cancellation);

            if (result is null)
            {
                return BadRequest();
            }
            else
            {
                return Json(result);
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetManyAsync(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var result = await _sharedShiftsService.GetManyAsync(request, cancellation);

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, God")]
        [Route("")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] SharedShiftDto shift,
            CancellationToken cancellation = default
        )
        {
            var result = await _sharedShiftsService.CreateAsync(shift, cancellation);

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
        [Authorize(Roles = "Admin, God")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] string id,
            [FromBody] SharedShiftDto shift,
            CancellationToken cancellation = default
        )
        {
            shift.Id = id;
            var result = await _sharedShiftsService.UpdateAsync(shift, cancellation);

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
        [Authorize(Roles = "Admin, God")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var result = await _sharedShiftsService.DeleteAsync(id, cancellation);

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
