using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById
{
    public class GetEventByIdUseCase
    {
        public ResponseEventJson Execute(Guid id)
        {
            var dbContext = new PassInDBContext();

            var entity = dbContext.Events.Include(evento => evento.Participants).FirstOrDefault(evento => evento.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"Evento com id: [{id}], não encontrado.");
            }

            return new ResponseEventJson
            {
                Id = entity.Id,
                Title = entity.Title,
                Details = entity.Details,
                MaximumAttendees = entity.Maximum_Attendees,
                AttendeesAmount = entity.Participants.Count,
            };
        }
    }
}
