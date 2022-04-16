using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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
    }
}
