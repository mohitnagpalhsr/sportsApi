using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace SportsEventProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly SportsEventManagementContext db;
        public EventController(SportsEventManagementContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public async Task<ActionResult> GetEvents()
        {
            
            return Ok(await db.Events.ToListAsync());

        }

        [HttpPost]
        public async Task<ActionResult> AddEvent(Event e)
        {
            db.Events.Add(e);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvent(int id, Event e)
        {
            db.Entry(e).State= EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok();
        }

        // [HttpDelete]
        // public async Task<ActionResult> DeleteEvent(int id)
        // {
        //     Event e = db.Events.Find(id);
        //     db.Events.Remove(e);
        //     await db.SaveChangesAsync();
        //     return Ok();
        // }
        [HttpGet]
        [Route("EventById")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            Event e = await db.Events.SingleOrDefaultAsync(x=>x.EventId==id);
            
            if(e is null)
            return NotFound();
            else
            return Ok(e);
        }

        [HttpGet]
        [Route("EventByName")]
        public async Task<ActionResult<Event>> GetEventByName(string name)
        {
            Event e = await db.Events.SingleOrDefaultAsync(x=>x.EventName==name);
            
            if(e is null)
            return NotFound();
            else
            return Ok(e);
        }

        [HttpGet]
        [Route("EventsByName")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByName(string name)
        {
            //Event e = await db.Events.Where(x=>x.EventName==name);

            List<Event> e=await db.Events.Where(item => item.EventName == name)
                       .ToListAsync();
            
            if(!e.Any())
            return NotFound();
            else
            return Ok(e);
        }

        [HttpGet]
        [Route("EventsHavingSport")]
        public async Task<ActionResult<IEnumerable<Event>>> EventsHavingSport(string name)
        {
            //Event e = await db.Events.Where(x=>x.EventName==name);

            List<Event> e=await db.Events.Where(item => (item.SportsName == name) && item.Status == "scheduled")
                       .ToListAsync();
            
            if(!e.Any())
            return NotFound();
            else
            return Ok(e);
        }
    }
}