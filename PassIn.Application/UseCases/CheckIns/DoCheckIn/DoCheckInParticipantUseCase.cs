using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.CheckIns.DoCheckIn
{
    public class DoCheckInParticipantUseCase
    {
        private readonly PassInDBContext _dbContext;

        public DoCheckInParticipantUseCase()
        {
            _dbContext = new PassInDBContext();
        }

        public ResponseRegisteredEventJson Execute(Guid participantId)
        {
            Validate(participantId);
            
            var entity = new CheckIn
            {
                Attendee_Id = participantId,
                Created_At = DateTime.UtcNow,
            };

            _dbContext.CheckIns.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredEventJson 
            { 
                Id = entity.Id,
            };
        }

        private void Validate(Guid participantId)
        {
            var existParticipant = _dbContext.Attendees.Any(participant => participant.Id == participantId);

            if (!existParticipant)
            {
                throw new NotFoundException($"O participante de id: [{participantId}], não foi encontrado.");
            }

            var existCheckIn = _dbContext.CheckIns.Any(checkIn => checkIn.Attendee_Id == participantId);

            if (existCheckIn)
            {
                throw new ErrorOnValidationException("O participante não pode realizar o check-in no mesmo evento 2 vezes");
            }
        }
    }
}
