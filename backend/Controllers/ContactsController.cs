
using ContactsApi.Models;
using ContactsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _service;

        public ContactsController(IContactService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) =>
    (await _service.GetByIdAsync(id)) is ResponseModel<Contact> contact ? Ok(contact) : NotFound(new { Message = $"Contact with ID {id} not found." });


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contact contact)
        {
            if (!ModelState.IsValid || (contact.Status != "Approved" && contact.Status != "Rejected"))
            {
                ModelState.AddModelError("Status", "Status must be either 'Approved' or 'Rejected'.");
                return BadRequest(ModelState);
            }

            var createdContact = await _service.CreateAsync(contact);
            return createdContact is ResponseModel<Contact> response
                ? CreatedAtAction(
                      nameof(GetById),
                      new { id = response.data.Id },
                      new ResponseModel<Contact>
                      {
                          data = response.data,
                          code = "201",
                          msg = "Created successfully"
                      })
                : BadRequest("Unable to create the contact.");
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateResult = await _service.UpdateAsync(id, contact);

            return updateResult switch
            {
                "NotFound" => NotFound(new { Message = $"Contact with ID {id} not found." }),
                "EmailExists" => BadRequest(new { Message = "A contact with this email already exists. Email cannot be changed." }),
                "Success" => NoContent(),
                _ => BadRequest("Unable to update the contact.")
            };
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => (await _service.DeleteAsync(id)) is false ? NotFound(new { Message = $"Contact with ID {id} not found." }) : NoContent();

    }
}
