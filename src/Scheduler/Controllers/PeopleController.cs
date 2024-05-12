using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Scheduler;
using Scheduler.Database;
using Scheduler.Models;
using System.Configuration;

namespace Scheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController(IDatabaseContext context, IMapper mapper) : ControllerBase
    {
        private readonly IDatabaseContext _context = context;
        private readonly IMapper _mapper = mapper;
            

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonModel>>> GetPeople()
        {
            var people = await _context.People.Select(person => _mapper.Map<PersonModel>(person)).ToListAsync();
            return people;
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonModel>> GetPerson(Guid id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }
            return _mapper.Map<PersonModel>(person);
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonCreateModel>> PostPerson(PersonCreateModel personCreateModel)
        {
            var person = _mapper.Map<Person>(personCreateModel);
            _context.People.Add(person);
            await _context.SaveChangesAsynchronous();

            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsynchronous();

            return NoContent();
        }
        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(Guid id, PersonUpdateModel personModel)
        {
            var person = _mapper.Map<Person>(personModel);
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.MarkAsModified(person);

            try
            {
                await _context.SaveChangesAsynchronous();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool PersonExists(Guid id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
