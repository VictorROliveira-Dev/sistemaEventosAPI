using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Participants.GetAllByEventsId
{
    public class GetAllParticipantsByEventIdUseCase
    {
        private readonly PassInDBContext _dbContext;

        public GetAllParticipantsByEventIdUseCase()
        {
            _dbContext = new PassInDBContext();
        }

        public ResponseAllAttendeesjson Execute(Guid eventId)
        {
            var entity = _dbContext.Events.Include(evento => evento.Participants).ThenInclude(participant => participant.CheckIn).FirstOrDefault(evento => evento.Id == eventId);
            
            if (entity is null)
            {
                throw new NotFoundException($"Evento com id: [{eventId}], não encontrado.");
            }

            return new ResponseAllAttendeesjson
            {
                Attendees = entity.Participants.Select(participant => new ResponseAttendeeJson
                {
                    Id = participant.Id,
                    Name = participant.Name,
                    Email = participant.Email,
                    CreatedAt = participant.Created_At,
                    CheckedInAt = participant.CheckIn?.Created_At
                }).ToList(),
            };
        }
    }
}
