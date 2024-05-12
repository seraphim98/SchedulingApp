using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Scheduler.Database;
using Scheduler.Controllers;
using Scheduler.Models;

namespace Scheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;
        public EventsController(DatabaseContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetEvents()
        {
            return await _context.Events.Select(@event => _mapper.Map<EventModel>(@event)).ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventModel>> GetEvent(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return _mapper.Map<EventModel>(@event);
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(Guid id, EventUpdateModel @eventModel)
        {
            var @event = _mapper.Map<Event>(@eventModel);
            if (id != @eventModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;
            var databasePersonEvents = _context.PersonEvents.Where(e => e.EventId == id).ToList();
            var peopleToAdd = @event.People.Where(p => !databasePersonEvents.Select(pe => pe.PersonId).Contains(p.Id));
            var personEventsToRemove = databasePersonEvents.Where(pe => !@event.People.Select(p => p.Id).Contains(pe.PersonId));

            try
            {
                foreach (var person in peopleToAdd) 
                {
                    _context.PersonEvents.Add(new PersonEvent(Guid.NewGuid(), person.Id, id, @event.Name));
                }
                foreach (var personEvent in personEventsToRemove) 
                {
                    _context.PersonEvents.Remove(personEvent);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventCreateModel>> PostEvent(EventCreateModel @eventModel)
        {
            var @event = _mapper.Map<Event>(@eventModel);
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
