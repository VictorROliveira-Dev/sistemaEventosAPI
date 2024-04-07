using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public ResponseRegisteredEventJson Execute(RequestEventJson request)
        {
            Validate(request);
            var dbContext = new PassInDBContext();

            var entity = new Event
            {
                Title = request.Title,
                Details = request.Details,
                Maximum_Attendees = request.MaximumAttendees,
                Slug = request.Title.ToLower().Replace(" ", "-")
            };

            dbContext.Events.Add(entity);
            dbContext.SaveChanges();

            return new ResponseRegisteredEventJson
            {
                 Id = entity.Id
            };
        }

        public void Validate(RequestEventJson request) 
        { 
            if (request.MaximumAttendees <= 0)
            {
                throw new ErrorOnValidationException("Número máximo de participantes é inválido.");
            }
            
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ErrorOnValidationException("Preencha o campo, não pode ser vazio!");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ErrorOnValidationException("Preencha o campo, não pode ser vazio!");
            }
        }
    }
}
