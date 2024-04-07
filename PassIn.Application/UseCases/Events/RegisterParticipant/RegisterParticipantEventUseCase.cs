using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterParticipant
{
    public class RegisterParticipantEventUseCase
    {
        private readonly PassInDBContext _dbContext;

        public RegisterParticipantEventUseCase()
        {
            _dbContext = new PassInDBContext();
        }

        public ResponseRegisteredEventJson Execute(RequestRegisterEventJson request, Guid eventId)
        {
            Validate(eventId, request);

            var entity = new Participant
            {
                Name = request.Name,
                Email = request.Email,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow,
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredEventJson
            {
                Id = entity.Id,
            };
        }

        public void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var entityEvent = _dbContext.Events.Find(eventId);
            if (entityEvent is null)
            {
                throw new NotFoundException($"O evento com o id: [{eventId}] não existe.");
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("Preencha o campo nome, não pode ser vazio!");
            }
            if (!EmailIsValid(request.Email))
            {
                throw new ErrorOnValidationException("O e-mail inserido não é válido.");
            }

            var ParticipantAlreadyRegistered = _dbContext.Attendees.Any(participant => participant.Email.Equals(request.Email) && participant.Event_Id == eventId);

            if (ParticipantAlreadyRegistered)
            {
                throw new ErrorOnValidationException("O participante não pode se registrar 2 vezes no mesmo evento.");
            }

            var count_Participants = _dbContext.Attendees.Count(participant => participant.Event_Id == eventId);

            if (count_Participants > entityEvent.Maximum_Attendees)
            {
                throw new ErrorOnValidationException("Número máximo de participantes atingido.");
            }
        }

        private bool EmailIsValid(string email)
        {
            try
            {
                //Classe para verificar se o email é válido:
                new MailAddress(email);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
