using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Events.RegisterParticipant;
using PassIn.Application.UseCases.Participants.GetAllByEventsId;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        [HttpPost]
        [Route("{eventId}/registerParticipant")]
        [ProducesResponseType(typeof(ResponseRegisteredEventJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public IActionResult RegisterParticipant([FromBody] RequestRegisterEventJson request, [FromRoute] Guid eventId)
        {
            var useCase = new RegisterParticipantEventUseCase();
            var response = useCase.Execute(request, eventId);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{eventID}")]
        [ProducesResponseType(typeof(ResponseAllAttendeesjson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public IActionResult GetAllParticipansEvent([FromRoute] Guid eventID)
        {
            var useCase = new GetAllParticipantsByEventIdUseCase();
            var response = useCase.Execute(eventID);

            return Ok(response);
        }
    }
}
