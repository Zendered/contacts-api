using ContactsApi.Dtos;
using ContactsApi.Exceptions;
using ContactsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApi.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsService contactsService;

        public ContactsController(IContactsService contactsService)
        {
            this.contactsService = contactsService;
        }

        [HttpGet]
        public ActionResult<List<GetContactsDto>> GetContacts()
        {
            var contacts = contactsService.GetAllContactsAsync();
            return Ok(contacts.Result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetContactsDto> GetContactsById(Guid id)
        {
            var contacts = contactsService.GetContactsByIdAsync(id).Result;
            return contacts is not null
                ? Ok(contacts)
                : NotFound(new NotFoundException());
        }

        [HttpPost]
        public async Task<ActionResult<GetContactsDto>> AddContact(AddContactsDto newContact)
        {
            await contactsService.AddContactAsync(newContact);
            return NoContent();
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<GetContactsDto>> UptadeContact([FromRoute] Guid id, UpdateContactsDto newContact)
        {
            var contact = await contactsService.UpdateContactAsync(id, newContact);
            return contact is not null
                ? CreatedAtAction("GetContactsById", new { id }, newContact)
                : NotFound(new NotFoundException());
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult<GetContactsDto>> DeleteContact([FromRoute] Guid id)
        {
            await contactsService.RemoveContactAsync(id);
            return NoContent();
        }
    }
}
