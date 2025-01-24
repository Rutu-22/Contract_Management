using Assiginment.Model;

namespace Assiginment.Services
{
    public interface IEmailService
    {
        Task SendApprovalEmailAsync(int userId, string email);
        Task UpdateStatusAsync(StatusUpdateRequest request);
    }

}
