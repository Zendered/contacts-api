using AutoMapper;
using ContactsApi.Dtos;
using ContactsApi.Models;

namespace ContactsApi.Profiles
{
    public class ContactsProfiles : Profile
    {
        public ContactsProfiles()
        {
            CreateMap<Contact, GetContactsDto>();

            CreateMap<AddContactsDto, Contact>();

            CreateMap<UpdateContactsDto, Contact>();
        }
    }
}
