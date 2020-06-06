using System.Collections.Generic;
using System.Linq;
using PetDatabaseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

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

        [HttpPost]
        public ActionResult<Pet> Create(Pet petToCreate)
        {
            if (petToCreate.HungerLevel != 0)
            {
                var errorMessage = new
                {
                    message = $"You requested the hunger level to be {petToCreate.HungerLevel} but it needs to start at 0."
                };

                return UnprocessableEntity(errorMessage);
            }

            if (petToCreate.HappinessLevel != 0)
            {
                var errorMessage = new
                {
                    message = $"You requested the happiness level to be {petToCreate.HappinessLevel} but it needs to start at 0."
                };

                return UnprocessableEntity(errorMessage);
            }

            if (petToCreate.Birthday != DateTime.Today)
            {
                var errorMessage = new
                {
                    message = $"You requested the birthday to be {petToCreate.Birthday} but it needs to be todays date."
                };

                return UnprocessableEntity(errorMessage);
            }

            _context.Pets.Add(petToCreate);
            _context.SaveChanges();

            return CreatedAtAction(null, null, petToCreate);
        }

        [HttpPost("{id}/playtimes")]
        public ActionResult<Pet> playTimeUpdate(int id, string playtimes)
        {
            var petThatIsLiveInTheDatabase = _context.Pets.FirstOrDefault(pet => pet.Id == id);

            if (petThatIsLiveInTheDatabase == null)
            {
                return NotFound();
            }

            petThatIsLiveInTheDatabase.HappinessLevel += 5;
            petThatIsLiveInTheDatabase.HungerLevel += 3;

            _context.Entry(petThatIsLiveInTheDatabase).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(petThatIsLiveInTheDatabase);
        }

        [HttpPost("{id}/feedings")]
        public ActionResult<Pet> feedingsUpdate(int id, string feedings)
        {
            var petThatIsLiveInTheDatabase = _context.Pets.FirstOrDefault(pet => pet.Id == id);

            if (petThatIsLiveInTheDatabase == null)
            {
                return NotFound();
            }

            if (petThatIsLiveInTheDatabase.HappinessLevel < 3)
            {
                petThatIsLiveInTheDatabase.HappinessLevel = 0;
            }

            else
            {
                petThatIsLiveInTheDatabase.HappinessLevel -= 3;
            }

            if (petThatIsLiveInTheDatabase.HungerLevel < 5)
            {
                petThatIsLiveInTheDatabase.HappinessLevel = 0;
            }

            else
            {
                petThatIsLiveInTheDatabase.HungerLevel -= 5;
            }

            _context.Entry(petThatIsLiveInTheDatabase).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(petThatIsLiveInTheDatabase);
        }

        [HttpPost("{id}/scoldings")]
        public ActionResult<Pet> scoldingsUpdate(int id, string scoldings)
        {
            var petThatIsLiveInTheDatabase = _context.Pets.FirstOrDefault(pet => pet.Id == id);

            if (petThatIsLiveInTheDatabase == null)
            {
                return NotFound();
            }

            if (petThatIsLiveInTheDatabase.HappinessLevel < 5)
            {
                petThatIsLiveInTheDatabase.HappinessLevel = 0;
            }

            else
            {
                petThatIsLiveInTheDatabase.HappinessLevel -= 5;
            }

            _context.Entry(petThatIsLiveInTheDatabase).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(petThatIsLiveInTheDatabase);
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