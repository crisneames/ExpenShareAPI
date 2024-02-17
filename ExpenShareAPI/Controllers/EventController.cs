using ExpenShareAPI.Repositories;
using ExpenShareAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpenShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase    
    {
        private readonly IEventRepository _eventRepository;
        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: api/<EventController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_eventRepository.GetEvents());
        }

        // GET api/<EventController>/5
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    var gig = _eventRepository.GetEventById(id);
        //    if (gig == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(gig);
        //}

        // GET api/<EventController>/5
        [HttpGet("{EventId}")]
        public IActionResult Get(int EventId)
        {
            var gig = _eventRepository.GetEventWithUsers(EventId);
            if (gig == null)
            {
                return NotFound();
            }
            return Ok(gig);
        }

        // POST api/<EventController>
        [HttpPost]
        public IActionResult Post(Event gig)
        {
            _eventRepository.AddNewEvent(gig);
            return CreatedAtAction("Get", new { id = gig.Id }, gig);
        }

        // PUT api/<EventController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Event gig)
        {
            if (id != gig.Id)
            {
                return BadRequest();
            }

            _eventRepository.UpdateEvent(gig);
            return NoContent();
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _eventRepository.DeleteEvent(id);
            return NoContent();
        }
    }
}
