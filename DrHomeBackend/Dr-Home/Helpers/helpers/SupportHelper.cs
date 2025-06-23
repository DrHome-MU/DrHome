using Dr_Home.Data.Models;
using Dr_Home.DTOs.EmailSender;
using Dr_Home.DTOs.SupportDtos;
using Dr_Home.Email_Sender;
using Mapster;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Dr_Home.Helpers.helpers
{
    public class SupportHelper(AppDbContext db , IEmailSender emailSender) : ISupportHelper
    {
        private readonly AppDbContext _db = db;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task<Result<MessageResponse>> RecieveMessageAsync(MessageRequest request, CancellationToken cancellationToken = default)
        {
            var duplicationIsExists = await _db.Set<Message>().AnyAsync(m => m.SenderName == request.SenderName && 
            m.SenderPhoneNumber == request.SenderPhoneNumber 
            && m.SenderEmail == request.SenderEmail 
            && m.Content == request.Content , cancellationToken);

            if(duplicationIsExists)
                return Result.Failure<MessageResponse>(MessagesErrors.DuplicateMessage);

            var message = request.Adapt<Message>();

            await _db.Set<Message>().AddAsync(message, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);  

            return Result.Success(message.Adapt<MessageResponse>());

        }

        public async Task<Result<IEnumerable<MessageResponse>>> GetAllMessagesAsync()
        {
            var messages = await _db.Set<Message>()
                .ProjectToType<MessageResponse>()
                .ToListAsync();

            return Result.Success<IEnumerable<MessageResponse>>(messages);
        }

        public async Task<Result> DeleteMessageAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var message = await _db.Set<Message>()
                .FirstOrDefaultAsync(m => m.Id == id , cancellationToken);

            if(message == null)
                return Result.Failure(MessagesErrors.MessageNotFound);

             _db.Remove(message);

            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public Task<Result> ResponseToUserMessageByAdminAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Result> ResponseToUserMessageByAdminAsync(Guid id , ResponseToMessageRequest request, CancellationToken cancellationToken = default)
        {
            var message = await _db.Set<Message>().FindAsync(id ,cancellationToken);

            if (message == null)
                return Result.Failure(MessagesErrors.MessageNotFound);

            var emailBody = EmailBodyBuilder.GenerateEmailBody("AdminResponse",
            templateModel: new Dictionary<string, string>
            {
                  { "{{Name}}" , $"{message.SenderName}" },
                   {"{{reply}}" , $"{request.ResponseBody}" }

            });

            var sendDto = new SendEmailRegisterDto
            {
                subject = "Response from Dr-Home Team",
                message = emailBody,
                toEmail = message.SenderEmail
            };

            await _emailSender.SendRegisterEmailAsync(sendDto);

            return Result.Success();
        }
    }
}
