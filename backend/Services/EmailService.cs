using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Assiginment.Model;
using System.Collections.Generic;
using System.Linq;
using Assiginment.Services;
using System.Net.Mail;
using ContactsApi.Models;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _jsonFilePath = "D:\\Assiginment11\\Assiginment\\Assiginment\\Data\\contacts.json\r\n"; // Replace with actual path
    public EmailService(SmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
    }

    public async Task SendApprovalEmailAsync(int userId, string email)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress("demo@gmail.com"),
            Subject = "Approval Request",
            Body = GenerateEmailBody(userId),
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await _smtpClient.SendMailAsync(mailMessage);
    }

    public async Task UpdateStatusAsync(StatusUpdateRequest request)
    {
        
        var users = await LoadUsersAsync();

        
        var user = users.FirstOrDefault(u => u.Id == request.UserId);
        if (user != null)
        {
            user.Status = request.Status;
            user.Comment = request.Comment;

            // Save the updated list back to the JSON file
            await SaveUsersAsync(users);
        }
        else
        {
            throw new Exception("User not found.");
        }
    }

    private string GenerateEmailBody(int userId)
    {
        return $@"
            <div style='padding: 20px;'>
                <h3>Please Accept the Request</h3>
                <textarea id='comment' placeholder='Enter your comment here' required></textarea><br/><br/>
                <button onclick='approveRequest({userId})' id='approveBtn' style='background-color: blue; color: white; padding: 10px;'>Approve</button>
                <button onclick='rejectRequest({userId})' id='rejectBtn' style='background-color: red; color: white; padding: 10px;'>Reject</button>
                <script>
                    function approveRequest(userId) {{
                        var comment = document.getElementById('comment').value;
                        if (!comment) {{
                            alert('Please enter a comment before approving.');
                            return;
                        }}
                        fetch('https://your-backend-url/api/Email/UpdateStatus', {{
                            method: 'POST',
                            headers: {{ 'Content-Type': 'application/json' }},
                            body: JSON.stringify({{ userId: userId, status: 'Approved', comment: comment }})
                        }})
                        .then(response => response.json())
                        .then(data => {{
                            document.getElementById('approveBtn').disabled = true;
                            document.getElementById('rejectBtn').disabled = true;
                        }});
                    }}
                    function rejectRequest(userId) {{
                        var comment = document.getElementById('comment').value;
                        fetch('https://your-backend-url/api/Email/UpdateStatus', {{
                            method: 'POST',
                            headers: {{ 'Content-Type': 'application/json' }},
                            body: JSON.stringify({{ userId: userId, status: 'Rejected', comment: comment }})
                        }})
                        .then(response => response.json())
                        .then(data => {{
                            document.getElementById('approveBtn').disabled = true;
                            document.getElementById('rejectBtn').disabled = true;
                        }});
                    }}
                </script>
            </div>";
    }

    private async Task<List<Contact>> LoadUsersAsync()
    {
        using var stream = new FileStream(_jsonFilePath, FileMode.Open, FileAccess.Read);
        return await JsonSerializer.DeserializeAsync<List<Contact>>(stream);
    }

    private async Task SaveUsersAsync(List<Contact> users)
    {
        using var stream = new FileStream(_jsonFilePath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(stream, users);
    }
}
