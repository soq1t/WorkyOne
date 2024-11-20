using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Services.Requests;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Controllers.Home
{
    [Route("calendar")]
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet]
        [Route("graphic")]
        public async Task<IActionResult> GetGraphicAsync(
            [FromQuery] MonthGraphicRequest request,
            CancellationToken cancellation = default
        )
        {
            var monthGraphic = await _calendarService.GetMonthGraphicAsync(
                request,
                request.ScheduleId,
                cancellation
            );
            var model = new CalendarViewModel
            {
                Info = monthGraphic.CalendarInfo,
                Legend = monthGraphic.Legend,
                ScheduleDto = monthGraphic.Schedule,
                WorkGraphic = monthGraphic.Graphic
            };
            return PartialView("Calendar/_CalendarInfoPartial", model);
        }

        [HttpGet]
        [Route("info")]
        public IActionResult GetCalendarInfo(
            [FromQuery] CalendarInfoRequest request,
            CancellationToken cancellation = default
        )
        {
            var calendarInfo = _calendarService.GetCalendarInfo(request);

            return Json(calendarInfo);
        }

        [HttpGet]
        [Route("graphic/legend")]
        public async Task<IActionResult> GetGraphicLegendAsync(
            [FromQuery] MonthGraphicRequest request,
            CancellationToken cancellation = default
        )
        {
            var monthGraphic = await _calendarService.GetMonthGraphicAsync(
                request,
                request.ScheduleId,
                cancellation
            );

            var model = new CalendarLegendViewModel { Legend = monthGraphic.Legend };

            return PartialView("Calendar/_CalendarLegendPartial", model);
        }
    }
}
