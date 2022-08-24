using ContactsApi.Dtos;

namespace ContactsApi.Services
{
    public interface IContactsService
    {
        public Task<List<GetContactsDto>> GetAllContactsAsync();

        public Task<GetContactsDto> GetContactsByIdAsync(Guid id);

        public Task AddContactAsync(AddContactsDto newContact);

        public Task RemoveContactAsync(Guid id);

        public Task<GetContactsDto> UpdateContactAsync(Guid id, UpdateContactsDto updatedContact);
    }
}
