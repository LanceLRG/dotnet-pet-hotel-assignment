using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetsController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult MakePet([FromBody] Pet pet)
        {
            PetOwner petOwner = _context.PetOwners.Find(pet.petOwnerid);
            if (petOwner == null)
            {
                ModelState.AddModelError("petOwnerid", "Invalid Owner ID");
                return ValidationProblem(ModelState);
            }

            _context.Add(pet);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = pet.id }, pet);
        }

        [HttpGet]
        public IEnumerable<Pet> GetPets()
        {
            return _context.Pets.Include(p => p.PetOwner).OrderBy(p => p.name);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Pet pet = _context.Pets.Include(p => p.PetOwner).SingleOrDefault(p => p.id == id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            Pet pet = _context.Pets.Include(p => p.PetOwner).SingleOrDefault(p => p.id == id);
            if (pet == null)
            {
                return NotFound();
            }
            _context.Pets.Remove(pet);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/checkin")]
        public IActionResult CheckinById(int id)
        {
            Pet pet = _context.Pets.Include(p => p.PetOwner).SingleOrDefault(p => p.id == id);
            if (pet == null)
            {
                return NotFound();
            }
            pet.checkin();
            _context.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }

        [HttpPut("{id}")]
        public IActionResult updatePet(int id, [FromBody] Pet pet)
        {
            if (!_context.Pets.Any(p => pet.id == id)) return NotFound();
            _context.Pets.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }

        [HttpPut("{id}/checkout")]
        public IActionResult CheckoutById(int id)
        {
            Pet pet = _context.Pets.Include(p => p.PetOwner).SingleOrDefault(p => p.id == id);
            if (pet == null)
            {
                return NotFound();
            }
            pet.checkout();
            _context.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }

        // [HttpGet]
        // [Route("test")]
        // public IEnumerable<Pet> GetPets() {
        //     PetOwner blaine = new PetOwner{
        //         name = "Blaine"
        //     };

        //     Pet newPet1 = new Pet {
        //         name = "Big Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Black,
        //         breed = PetBreedType.Poodle,
        //     };

        //     Pet newPet2 = new Pet {
        //         name = "Little Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Golden,
        //         breed = PetBreedType.Labrador,
        //     };

        //     return new List<Pet>{ newPet1, newPet2};
        // }
    }
}
