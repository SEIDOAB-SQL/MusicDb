using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public enum enAnimal { Dog, Cat, Rabbit, Fish, Bird };
    public class csPet : ISeed<csPet>
	{
        [Key]
        public Guid PetId {get; set;}

        public enAnimal AnimalKind { get; set; }
		public string Name { get; set; }

        public override string ToString() => $"{Name} the {AnimalKind}";


        //Navigation properties that EFC will use to build relations
        public csFriend Owner{ get; set; } = null;


        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public csPet Seed(csSeedGenerator _seeder)
        {
            var country = _seeder.Country;
            return new csPet
            {
                PetId = Guid.NewGuid(),
                
                Name = _seeder.PetName,
                AnimalKind = _seeder.FromEnum<enAnimal>(),
                Seeded = true
            };
        }
        #endregion
    }
}

