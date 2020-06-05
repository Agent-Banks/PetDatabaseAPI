using System.Collections.Generic;
using System.Linq;
using PetDatabaseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PetDatabaseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        //Constructor
        public PetsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Pet>> GetAll()
        {
            var allThePets = _context.Pets;

            return Ok(allThePets);
        }

        [HttpGet("{id}")]
        public ActionResult<Pet> GetByID(int id)
        {
            var pet = _context.Pets.FirstOrDefault(pet => pet.Id == id);

            if (pet == null)
            {
                // Give back a 404 error
                return NotFound();
            }

            return Ok(pet);
        }

        [HttpDelete("{id}")]
        public ActionResult<Pet> Delete(int id)
        {
            var foundPet = _context.Pets.FirstOrDefault(pet => pet.Id == id);

            if (foundPet == null)
            {
                return NotFound();
            }

            _context.Pets.Remove(foundPet);
            _context.SaveChanges();

            return Ok(foundPet);
        }
    }
}