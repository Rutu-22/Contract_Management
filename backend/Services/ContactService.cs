
using ContactsApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ContactsApi.Services
{
    public class ContactService : IContactService
    {
        private readonly string _filePath = "Data/contacts.json";
        private readonly object _lock = new();

        public async Task<ListResponseModel<Contact>> GetAllAsync()
        {
            var res = await ReadContactsAsync();
            return new ListResponseModel<Contact>()
            {
                data = ReadContactsAsync().Result.ToList(),
                code = "200",
                msg = "Contact retrived successfully"
            };
        }

        public async Task<ResponseModel<Contact>> GetByIdAsync(int id) => new ResponseModel<Contact>()
        {
            data = (await ReadContactsAsync()).ToList().SingleOrDefault(c => c.Id == id),
            code = "200",
            msg = "Contact retrived successfully"
        };

        public async Task<ResponseModel<Contact>> CreateAsync(Contact contact)
        {
            var contacts = (await ReadContactsAsync()).ToList();

            if (CheckEmailExist(contact.Id, contact, contacts))
            {
                throw new Exception("A contact with the same email already exists.");
            }

            contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
            contacts.Add(contact);
            await WriteContactsAsync(contacts);

            return new ResponseModel<Contact>
            {
                data = contact,
                code = "200",
                msg = "Contact created successfully"
            };
        }



        public async Task<string> UpdateAsync(int id, Contact contact)
        {
            var contacts = await ReadContactsAsync();

            var existingContact = contacts.FirstOrDefault(x => x.Id == id);

            if (existingContact == null)
                return "NotFound";

            // Ensure that if the email is being updated, it doesn't already exist in another contact
            if (existingContact.Email != contact.Email && contacts.Any(c => c.Email == contact.Email))
            {
                return "EmailExists";
            }

            // Only allow updating the Status field
            existingContact.Status = contact.Status;

            await WriteContactsAsync(contacts);
            return "Success";
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var contacts = (await ReadContactsAsync()).ToList().Where(x => x.Id != id);
            if (contacts == null)
                return false;
            await WriteContactsAsync(contacts);
            return true;
        }


        #region Lookup Method

        private async Task<IEnumerable<Contact>> ReadContactsAsync()
        {
            if (!File.Exists(_filePath))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                await File.WriteAllTextAsync(_filePath, "[]");
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<IEnumerable<Contact>>(json);
        }

        private async Task WriteContactsAsync(IEnumerable<Contact> contacts)
        {
            lock (_lock)
            {
                var json = JsonConvert.SerializeObject(contacts, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
        }

        private static bool CheckEmailExist(int id, Contact contact, IEnumerable<Contact> contacts)
        {
            if (contacts.Any(c => c.Email.ToLower() == contact.Email.ToLower() && c.Id != id))
            {
                return true;
            }
            else
                return false;
        }

        #endregion
    }
}
