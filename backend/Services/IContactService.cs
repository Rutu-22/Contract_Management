using ContactsApi.Models;

namespace ContactsApi.Services
{
    public interface IContactService
    {
        Task<ListResponseModel<Contact>> GetAllAsync();
        Task<ResponseModel<Contact>> GetByIdAsync(int id);
        Task<ResponseModel<Contact>> CreateAsync(Contact contact);
        Task<String> UpdateAsync(int id, Contact contact);
        Task<bool> DeleteAsync(int id);
    }
}
