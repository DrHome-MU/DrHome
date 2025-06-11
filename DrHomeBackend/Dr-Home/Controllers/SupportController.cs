using Dr_Home.DTOs.SupportDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController(ISupportHelper supportHelper) : ControllerBase
    {
        private readonly ISupportHelper _supportHelper = supportHelper;
        /// <summary>
        /// send message by user (can be not signed in)
        /// </summary>
        /// <param name="request">
        ///  <br/> 
        ///  all fields must be not null or empty ,
        ///  <br/>
        ///  Phone number must be valid egyptian number 
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Contact/Messages")]

        public async Task<IActionResult> RecieveMessageAsync([FromBody] MessageRequest request , CancellationToken cancellationToken)
        {
            var result = await _supportHelper.RecieveMessageAsync(request , cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        /// <summary>
        /// Get all the messages in admin dashboard (can be empty)
        /// </summary>
        /// <returns></returns>

        [HttpGet("Contact/Messages")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<MessageResponse>) , StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMessages()
        {
            var result = await _supportHelper.GetAllMessagesAsync();
            return Ok(result.Value);
        }
        /// <summary>
        /// This Endpoint enables to response to message via gmail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "200">sended successfully</response>
        /// <response code = "404">Message does not exist</response>
        /// <response code = "500"> فس مشكلة في السيرفر والرساله متبعتتش حاول في وقت تاني</response>
        [HttpPost("Contact/Messages/{id}/response")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResponseToUserMessage([FromRoute] Guid id , [FromBody] ResponseToMessageRequest request , CancellationToken cancellationToken)
        {
            var result = await _supportHelper.ResponseToUserMessageByAdminAsync(id , request , cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        /// <summary>
        /// delete message by admin using its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpDelete("Contact/Messages/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteMessage([FromRoute] Guid id , CancellationToken cancellationToken)
        {
            var result = await _supportHelper.DeleteMessageAsync(id , cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem(); 
        }
    }
}
