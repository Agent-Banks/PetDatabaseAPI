using System;

namespace PetDatabaseAPI
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; } = DateTime.Today;
        public int HungerLevel { get; set; } = 0;
        public int HappinessLevel { get; set; } = 0;
    }
}