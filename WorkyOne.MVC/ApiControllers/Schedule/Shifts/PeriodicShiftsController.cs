using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkyOne.MVC.ApiControllers.Schedule.Shifts
{
    [ApiController]
    [Authorize]
    [Route("api/shifts/periodic")]
    [Route("api/schedule/{scheduleId}/shifts/periodic")]
    public class PeriodicShiftsController : Controller
    {
        //public private readonly Task GetManyAsync(
        //    [FromRoute] string? scheduleId,
        //    CancellationToken cancellation = default
        //) { }
    }
}
