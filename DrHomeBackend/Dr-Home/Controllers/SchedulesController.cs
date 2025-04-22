

namespace Dr_Home.Controllers
{
    [Route("api/Doctors/{DoctorId}/Clinics/{ClinicId}/[controller]")]
    [ApiController]
    public class SchedulesController(IScheduleHelper scheduleHelper , ILogger<SchedulesController> logger) : ControllerBase
    {
        private readonly IScheduleHelper _scheduleHelper = scheduleHelper;
        private readonly ILogger _logger = logger;
        /// <summary>
        /// Get All Clinic Scheduels
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="ClinicId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ///<response code = "200">Data Loaded Successfully , ValidTimesForBooking (if the value is true then this time Is Empty to book)
        ///</response>
        ///<response code = "404">Clinic Not Found </response>

        [HttpGet("")]
        [Authorize]

        public async Task<IActionResult> GetAllClinicSchedules([FromRoute] Guid DoctorId , [FromRoute] Guid ClinicId , CancellationToken cancellationToken)
        {
            var result = await _scheduleHelper.GetSchedulesAsync(DoctorId, ClinicId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        /// <summary>
        /// Get Schedule By Id
        /// </summary>
        /// <param name="ClinicId"></param>
        /// <param name="ScheduleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{ScheduleId}")]

        public async Task<IActionResult> GetSchedule([FromRoute] Guid ClinicId , [FromRoute] Guid ScheduleId , CancellationToken cancellationToken)
        {
            var result = await _scheduleHelper.GetSchedueleAsync(ClinicId, ScheduleId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        /// <summary>
        /// Add Schedule By Doctor 
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="ClinicId">
        ///
        /// </param>
        /// <param name="request">
        /// <br/>
        /// -<b>WorkDay</b>: it must be Greater than or equal today`s date <br/>
        /// -<b>StartTime</b> its value between 00:00:00 and 23:59:59 , it must be less than EndTime<br/>
        /// -<b>EndTime</b> its value between 00:00:00 and 23:59:59 <br/>
        /// -<b>Fee</b>: greater than or equal to  0 <br/>
        /// -<b>AppointmentDuration</b>: at least 5 mins<br/>
        /// -<b>*Note*</b> if StartTime + appointment duration time > EndTime = (Not Valid)<br/>
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "200">Added Successfully + the schedule Data will return</response>
        /// <response code = "404"> Clinic doesn`t exist </response>
        /// <response code = "409">This Schedule Conflicts another schedule Added Before</response>
        /// <response code = "401">Unauthorized</response>

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult>AddSchedule([FromRoute] Guid DoctorId , [FromRoute] Guid ClinicId ,  ScheduleRequest request , CancellationToken cancellationToken)
        {
            var result = await _scheduleHelper.AddScheduleAsync(DoctorId , ClinicId , request , cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        /// <summary>
        /// Update Schedule By Doctor
        /// </summary>
        /// <param name="ScheduleId"></param>
        /// <param name="request">
        /// <br/> 
        /// -<b>Note</b>: The Same Validation Of Add Schedule Endpoint
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "204">Updated Successfully</response>
        /// <response code="409">This Schedule Conflicts another schedule Added Before</response>
        /// <response code="404">Schedule Doesn`t Exist</response>
        [HttpPut("{ScheduleId}")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(typeof(ScheduleResponse), 200)]   
        public async Task<IActionResult> UpdateSchedule([FromRoute] Guid ScheduleId , ScheduleRequest request , CancellationToken cancellationToken)
        {
            var result = await _scheduleHelper.UpdateAsync(ScheduleId , request , cancellationToken);   

            return result.IsSuccess ? NoContent() : result.ToProblem(); 
        }
        /// <summary>
        /// Delete Schedule By Id By Doctor
        /// </summary>
        /// <param name="ScheduleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "404">Schedule Doesn`t Exist</response>
        /// <Rsesponse code= "204">Deleted Successfully</Rsesponse>

        [HttpDelete("{ScheduleId}")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> DeleteSchedule([FromRoute] Guid ScheduleId , CancellationToken cancellationToken)
        {
            var result  = await _scheduleHelper.DeleteAsync(ScheduleId , cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem(); 
        }
    }
}
