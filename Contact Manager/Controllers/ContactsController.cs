using Contact_Manager.Models;
using ContactManager.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Contact_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactDbContext _dbContext;

        public ContactsController(ContactDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAll()
        {
            var contacts = await _dbContext.Contacts.ToListAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetById(int id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult<Contact>> Create([FromBody] Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Salutation) || contact.Salutation.Length < 2)
            {
                return BadRequest("Salutation must not be empty and must be at least 2 characters long.");
            }
            if (string.IsNullOrEmpty(contact.FirstName) || contact.FirstName.Length < 2)
            {
                return BadRequest("First name must not be empty and must be at least 2 characters long.");
            }
            if (string.IsNullOrEmpty(contact.LastName) || contact.LastName.Length < 2)
            {
                return BadRequest("Last name must not be empty and must be at least 2 characters long.");
            }

            if (string.IsNullOrEmpty(contact.DisplayName))
            {
                contact.DisplayName = $"{contact.Salutation} {contact.FirstName} {contact.LastName}";
            }

            _dbContext.Contacts.Add(contact);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Contact>> Update(int id, Contact updatedContact)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updatedContact.Salutation) && updatedContact.Salutation.Length < 2)
            {
                return BadRequest("Salutation must be at least 2 characters long.");
            }
            if (!string.IsNullOrEmpty(updatedContact.FirstName) && updatedContact.FirstName.Length < 2)
            {
                return BadRequest("First name must be at least 2 characters long.");
            }
            if (!string.IsNullOrEmpty(updatedContact.LastName) && updatedContact.LastName.Length < 2)
            {
                return BadRequest("Last name must be at least 2 characters long.");
            }

            if (!string.IsNullOrEmpty(updatedContact.Salutation))
            {
                contact.Salutation = updatedContact.Salutation;
            }
            if (!string.IsNullOrEmpty(updatedContact.FirstName))
            {
                contact.FirstName = updatedContact.FirstName;
            }
            if (!string.IsNullOrEmpty(updatedContact.LastName))
            {
                contact.LastName = updatedContact.LastName;
            }
            if (!string.IsNullOrEmpty(updatedContact.DisplayName))
            {
                contact.DisplayName = updatedContact.DisplayName;
            }
            if (updatedContact.BirthDate.HasValue)
            {
                contact.BirthDate = updatedContact.BirthDate;
            }
            contact.LastChangeTimestamp = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _dbContext.Contacts.Remove(contact);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
