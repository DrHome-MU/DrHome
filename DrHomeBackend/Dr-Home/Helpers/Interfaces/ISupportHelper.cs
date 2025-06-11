using Dr_Home.DTOs.SupportDtos;

namespace Dr_Home.Helpers.Interfaces
{
    public interface ISupportHelper
    {
        Task<Result<MessageResponse>> RecieveMessageAsync(MessageRequest request , CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<MessageResponse>>> GetAllMessagesAsync(); 

        Task<Result> DeleteMessageAsync(Guid id , CancellationToken cancellationToken = default);

        Task<Result> ResponseToUserMessageByAdminAsync(Guid id , ResponseToMessageRequest request , CancellationToken cancellationToken = default);
    }
}
