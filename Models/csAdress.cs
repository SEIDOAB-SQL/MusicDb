using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Models
{
	public class csAdress : ISeed<csAdress>
	{   
        [Key]
        public Guid AdressId {get; set;}

        public string StreetAdress { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public override string ToString() => $"{StreetAdress}, {ZipCode} {City}, {Country}";

        //Navigation properties that EFC will use to build relations
        public List<csFriend> Residents{ get; set; } = null;

        #region Random Seeding
        public bool Seeded { get; set; } = false;

        public csAdress Seed(csSeedGenerator _seeder)
        {
            var country = _seeder.Country;
            return new csAdress
            {
                AdressId = Guid.NewGuid(),
                
                StreetAdress = _seeder.StreetAddress(country),
                ZipCode = _seeder.ZipCode,
                City = _seeder.City(country),
                Country = country,
                Seeded = true
            };
        }
        #endregion
    }
}

