using Microsoft.EntityFrameworkCore;
using ContactManager.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contact_Manager.Model
{
    public class DbHelper
    {
        private readonly DbContext _context;

        public DbHelper(DbContext context)
        {
            _context = context;
        }
        public async Task<List<ContactModel>> GetContactsAsync()
        {
            return await _context.Set<ContactModel>().ToListAsync();
        }
        public List<ContactModel> GetContacts()
        {
            return _context.Set<ContactModel>().ToList();
        }

        public ContactModel GetContact(int id)
        {
            return _context.Set<ContactModel>().FirstOrDefault(c => c.Id == id);
        }

        public void CreateContact(ContactModel contact)
        {
            contact.CreationTimestamp = DateTime.Now;
            contact.LastChangeTimestamp = DateTime.Now;

            _context.Set<ContactModel>().Add(contact);
            _context.SaveChanges();
        }

        public void UpdateContact(ContactModel contact)
        {
            var existingContact = _context.Set<ContactModel>().FirstOrDefault(c => c.Id == contact.Id);

            if (existingContact == null)
            {
                throw new InvalidOperationException($"Contact with id {contact.Id} does not exist");
            }

            existingContact.Salutation = contact.Salutation;
            existingContact.FirstName = contact.FirstName;
            existingContact.LastName = contact.LastName;
            existingContact.DisplayName = contact.DisplayName;
            existingContact.BirthDate = contact.BirthDate;
            existingContact.LastChangeTimestamp = DateTime.Now;
            existingContact.NotifyHasBirthdaySoon = contact.NotifyHasBirthdaySoon;
            existingContact.Email = contact.Email;
            existingContact.PhoneNumber = contact.PhoneNumber;

            _context.SaveChanges();
        }

        public void DeleteContact(int id)
        {
            var existingContact = _context.Set<ContactModel>().FirstOrDefault(c => c.Id == id);

            if (existingContact == null)
            {
                throw new InvalidOperationException($"Contact with id {id} does not exist");
            }

            _context.Set<ContactModel>().Remove(existingContact);
            _context.SaveChanges();
        }
    }
}
