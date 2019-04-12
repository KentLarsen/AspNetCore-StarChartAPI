using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var co = _context.CelestialObjects.Find(id);
            if (co == null) return NotFound();
            co.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();
            return Ok(co);
        }

        [HttpGet("{name:string}")]
        public IActionResult GetByName(string name)
        {
            var co = _context.CelestialObjects.Where(o => o.Name == name).ToList();
            if (co == null) return NotFound();
            co.ForEach(e => e.Satellites = _context.CelestialObjects.Where(o => o.Id == e.Id).ToList());
            return Ok(co);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var co = _context.CelestialObjects.ToList();
            if (co == null) return NotFound();
            co.ForEach(e => e.Satellites = _context.CelestialObjects.Where(o => o.Id == e.Id).ToList());
            return Ok(co);
        }
    }
}
