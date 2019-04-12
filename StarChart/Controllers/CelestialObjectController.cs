using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var co = _context.CelestialObjects.Where(o => o.Name == name).ToList();
            if (!co.Any()) return NotFound();
            co.ForEach(e => e.Satellites = _context.CelestialObjects.Where(o => o.Id == e.Id).ToList());
            return Ok(co);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var co = _context.CelestialObjects.ToList();
            if (!co.Any()) return NotFound();
            co.ForEach(e => e.Satellites = _context.CelestialObjects.Where(o => o.Id == e.Id).ToList());
            return Ok(co);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject co)
        {
            _context.CelestialObjects.Add(co);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = co.Id }, co);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject co)
        {
            var existing = _context.CelestialObjects.Find(id);
            if (existing == null) return NotFound();
            existing.Name = co.Name;
            existing.OrbitalPeriod = co.OrbitalPeriod;
            existing.OrbitedObjectId = co.OrbitedObjectId;
            _context.Update(existing);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existing = _context.CelestialObjects.Find(id);
            if (existing == null) return NotFound();
            existing.Name = name;
            _context.Update(existing);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.CelestialObjects.Find(id);
            if (existing == null) return NotFound();
            _context.Remove(existing);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
