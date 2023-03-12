using Contact_Manager.Model;
using Contact_Manager.Models;
using ContactManager.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactDbContext _dbContext;
        private readonly DbHelper _dbHelper;

        public ContactsController(ContactDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbHelper = new DbHelper(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contacts = await _dbHelper.GetContactsAsync();
                var response = ResponseHandler.GetAppResponse(ResponseType.Success, contacts);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ResponseHandler.GetExceptionResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var contact = await _dbContext.Contacts.FindAsync(id);
                if (contact == null)
                {
                    var responseBadRequest = ResponseHandler.GetAppResponse(ResponseType.NotFound, null);
                    return NotFound(responseBadRequest);
                }
                var response = ResponseHandler.GetAppResponse(ResponseType.Success, contact);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ResponseHandler.GetExceptionResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contact contact)
        {
            // Check if the received Contact object is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with error details if the Contact object is not valid
                return BadRequest(ModelState);
            }
            try
            {
                if (string.IsNullOrEmpty(contact.Salutation) || contact.Salutation.Length < 2)
                {
                    var responseBadRequest = ResponseHandler.GetAppResponse(ResponseType.Error, "Salutation must not be empty and must be at least 2 characters long.");
                    return BadRequest(responseBadRequest);
                }
                if (string.IsNullOrEmpty(contact.FirstName) || contact.FirstName.Length < 2)
                {
                    var responseBadRequest = ResponseHandler.GetAppResponse(ResponseType.Error, "First name must not be empty and must be at least 2 characters long.");
                    return BadRequest(responseBadRequest);
                }
                if (string.IsNullOrEmpty(contact.LastName) || contact.LastName.Length < 2)
                {
                    var responseBadRequest = ResponseHandler.GetAppResponse(ResponseType.Error, "Last name must not be empty and must be at least 2 characters long.");
                    return BadRequest(responseBadRequest);
                }

                if (string.IsNullOrEmpty(contact.DisplayName))
                {
                    contact.DisplayName = $"{contact.Salutation} {contact.FirstName} {contact.LastName}";
                }
                if (contact.BirthDate.HasValue)
                {
                    // Convert the BirthDate to UTC
                    contact.BirthDate = contact.BirthDate.Value.ToUniversalTime();
                }

                if (contact.CreationTimestamp == default(DateTime))
                {
                    // Set the CreationTimestamp to the current UTC time
                    contact.CreationTimestamp = DateTime.UtcNow;
                }

                if (contact.LastChangeTimestamp == default(DateTime))
                {
                    // Set the LastChangeTimestamp to the current UTC time
                    contact.LastChangeTimestamp = DateTime.UtcNow;
                }
                if (contact.BirthDate.HasValue)
                {
                    // Set the NotifyHasBirthdaySoon property based on the BirthDate
                    contact.NotifyHasBirthdaySoon = contact.BirthDate.HasValue &&
                                        contact.BirthDate.Value.Month == DateTime.Today.Month &&
                                        contact.BirthDate.Value.Day <= DateTime.Today.AddDays(14).Day;
                }


                _dbContext.Contacts.Add(contact);
                await _dbContext.SaveChangesAsync();
                var response = ResponseHandler.GetAppResponse(ResponseType.Success, contact);
                return CreatedAtAction(nameof(GetById), new { id = contact.Id }, response);
            }
            catch (Exception ex)
            {
                var response = ResponseHandler.GetExceptionResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Contact updatedContact)
        {
            // Check if the received Contact object is valid
            if (!ModelState.IsValid)
            {
                // Return a BadRequest response with error details if the Contact object is not valid
                return BadRequest(ModelState);
            }
            try
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
                    contact.BirthDate = DateTime.SpecifyKind(updatedContact.BirthDate.Value, DateTimeKind.Utc);
                }
                if (updatedContact.BirthDate.HasValue)
                {
                    // Set the NotifyHasBirthdaySoon property based on the BirthDate
                    updatedContact.NotifyHasBirthdaySoon = updatedContact.BirthDate.HasValue &&
                                        updatedContact.BirthDate.Value.Month == DateTime.Today.Month &&
                                       // updatedContact.BirthDate.Value.Day >= DateTime.Today.Day &&
                                        updatedContact.BirthDate.Value.Day <= DateTime.Today.AddDays(14).Day;
                }

                contact.LastChangeTimestamp = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHandler.GetExceptionResponse(ex));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
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
            catch (Exception ex)
            {
                var response = ResponseHandler.GetExceptionResponse(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
