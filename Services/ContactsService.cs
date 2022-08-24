using AutoMapper;
using ContactsApi.Data;
using ContactsApi.Dtos;
using ContactsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ContactsApi.Services
{
    public class ContactsService : IContactsService
    {
        private readonly ContactsContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;

        public ContactsService(ContactsContext context, IMapper mapper, IHttpContextAccessor httpContext)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContext = httpContext;
        }

        private Guid GetUserId() => Guid.Parse(httpContext.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task AddContactAsync(AddContactsDto newContact)
        {
            var contact = new Contact { Email = newContact.Email, FullName = newContact.FullName, Phone = newContact.Phone };
            contact.User = await context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            await context.Contacts.AddAsync(contact);
            await context.SaveChangesAsync();
        }

        public async Task<List<GetContactsDto>> GetAllContactsAsync()
        {
            var contacts = await context.Contacts
                .Where(contacts => contacts.User.Id == GetUserId())
                .ToListAsync();
            var res = mapper.Map<List<GetContactsDto>>(contacts);
            return res;
        }

        public async Task<GetContactsDto> GetContactsByIdAsync(Guid id)
        {
            var contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            var res = mapper.Map<GetContactsDto>(contact);
            return res;
        }

        public async Task RemoveContactAsync(Guid id)
        {
            var contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            if (contact == null) return;

            context.Contacts.Remove(contact);
            await context.SaveChangesAsync(); ;
        }

        public async Task<GetContactsDto> UpdateContactAsync(Guid id, UpdateContactsDto updatedContact)
        {
            var contact = await context.Contacts
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null) return null;

            if (contact.User.Id == GetUserId())
            {
                contact.Email = updatedContact.Email;
                contact.FullName = updatedContact.FullName;
                contact.Phone = updatedContact.Phone;

                await context.SaveChangesAsync();
            }
            var res = mapper.Map<GetContactsDto>(contact);
            return res;
        }
    }
}
