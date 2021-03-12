using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetOwnersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetOwnersController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            PetOwner petOwner = _context.PetOwners.Include(p => p.pets).SingleOrDefault(p => p.id == id);
            if (petOwner == null) {
                return NotFound();
            }
            return Ok(petOwner);
        }

        [HttpGet]
        public IEnumerable<PetOwner> GetPetOwners() {
            return _context.PetOwners.Include(p => p.pets).OrderBy(p => p.name);
        }

        [HttpPost]
        public IActionResult CreatePetOwner([FromBody] PetOwner petOwner) {
            _context.Add(petOwner);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = petOwner.id}, petOwner);

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id) {
            PetOwner petOwner = _context.PetOwners.Find(id);
            if (petOwner == null) {
                return NotFound();
            }

            _context.PetOwners.Remove(petOwner);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult updatePetOwner(int id, [FromBody] PetOwner petOwner) {
            if (!_context.PetOwners.Any(p => petOwner.id == id)) return NotFound();
            _context.PetOwners.Update(petOwner);
            _context.SaveChanges();
            return Ok(petOwner);
        }

    }
}
