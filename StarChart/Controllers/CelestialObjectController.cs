using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (celestialObject == null) return NotFound();

            celestialObject.Satellites = _context.CelestialObjects
                .Where(
                    satellite => satellite.OrbitedObjectId == id)
                .ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => string.Equals(x.Name, name)).ToList();

            if (celestialObjects == null || celestialObjects.Count() == 0) return NotFound();

            celestialObjects.ForEach(celestialObject =>
                celestialObject.Satellites = _context.CelestialObjects
                    .Where(
                        satellite => satellite.OrbitedObjectId == celestialObject.Id)
                    .ToList());

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            if (celestialObjects == null || celestialObjects.Count() == 0) return NotFound();

            celestialObjects.ForEach(celestialObject =>
                celestialObject.Satellites = _context.CelestialObjects
                    .Where(
                        satellite => satellite.OrbitedObjectId == celestialObject.Id)
                    .ToList());

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var dbCelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (dbCelestialObject == null) return NotFound();

            dbCelestialObject.Name = celestialObject.Name;
            dbCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            dbCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(dbCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var dbCelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (dbCelestialObject == null) return NotFound();

            dbCelestialObject.Name = name;

            _context.CelestialObjects.Update(dbCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dbCelestialObjects = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id);

            if (dbCelestialObjects == null || dbCelestialObjects.Count() == 0) return NotFound();

            _context.CelestialObjects.RemoveRange(dbCelestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
