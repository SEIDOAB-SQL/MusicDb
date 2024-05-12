﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Data.Common;

using Configuration;
using Models;
using DbContext;

namespace AppConsole
{
    static class MyLinqExtensions
    {
        public static void Print<T>(this IEnumerable<T> collection)
        {
            collection.ToList().ForEach(item => Console.WriteLine(item));
        }
    }


    class Program
    {
        const int nrItemsSeed = 50;
        static void Main(string[] args)
        {
            #region run below to test the model only

            Console.WriteLine($"\nSeeding the Model...");
            var _modelList = SeedModel(nrItemsSeed);

            Console.WriteLine($"\nTesting Model...");
            WriteModel(_modelList);
            #endregion

            #region  run below only when Database i created
            Console.WriteLine($"\nConnecting to database...");
            Console.WriteLine($"Database type: {csAppConfig.DbSetActive.DbServer}");
            Console.WriteLine($"Connection used: {csAppConfig.DbSetActive.DbConnection}");
  
            Console.WriteLine($"\nSeeding database...");
            try
            {
                SeedDataBase(_modelList).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: Database could not be seeded. Ensure the database is correctly created");
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine($"\nError: {ex.InnerException.Message}");
                return;
            }

            Console.WriteLine("\nQuery database...");
            QueryDatabaseAsync().Wait();
            #endregion
        }


        #region Replaced by new model methods
        private static void WriteModel(List<csFriend> _modelList)
        {
            /*
            foreach (var friend in _modelList)
            {
                Console.WriteLine(friend);
            }
            */

            Console.WriteLine($"NrOfFriends: {_modelList.Count()}");
            Console.WriteLine($"NrOfFriends without any pets: {_modelList.Count(
                f => f.Pets == null || f.Pets?.Count == 0)}");
            Console.WriteLine($"NrOfFriends without an adress: {_modelList.Count(
                f => f.Adress == null)}");
               
            Console.WriteLine($"First Friend: {_modelList.First()}");
            Console.WriteLine($"Last Friend: {_modelList.Last()}");
        }

        private static List<csFriend> SeedModel(int nrItems)
        {
            var _seeder = new csSeedGenerator();
            
            //Create a list of friends, adresses and pets
            var _goodfriends = _seeder.ItemsToList<csFriend>(nrItems);
            var _adresses = _seeder.ItemsToList<csAdress>(nrItems);

            //Assign adress and pet to friends
            for (int i = 0; i < nrItems; i++)
            {
                //assign an address randomly
                _goodfriends[i].Adress = (_seeder.Bool) ? _seeder.FromList(_adresses) :null;

                //Create between 0 and 3 pets
                var _pets = new List<csPet>();
                for (int c = 0; c < _seeder.Next(0,4); c++)
                {
                    _pets.Add(new csPet().Seed(_seeder)); 
                }
                _goodfriends[i].Pets = (_pets.Count > 0) ? _pets : null;
            }
            return _goodfriends;
        }
        #endregion

        #region Update to reflect you new Model
        private static async Task SeedDataBase(List<csFriend> _modelList)
        {
            using (var db = csMainDbContext.DbContext())
            {
                #region move the seeded model into the database using EFC
                foreach (var _friend in _modelList)
                {
                    db.Friends.Add(_friend);
                }
                #endregion

                await db.SaveChangesAsync();
            }
        }

        private static async Task QueryDatabaseAsync()
        {
            Console.WriteLine("--------------");
            using (var db = csMainDbContext.DbContext())
            {
                #region Reading the database using EFC
                var _modelList = await db.Friends
                    .Include(f => f.Adress)
                    .Include(f => f.Pets)
                    .ToListAsync();

                //read any table and include Navigation properties
                var _pets = await db.Pets
                    .Include(p => p.Owner) 
                    .ToListAsync(); 

                var _adresses = await db.Adress
                    .Include(p => p.Residents)
                    .ToListAsync();
                
                #endregion

                WriteModel(_modelList);
            }
        }
        #endregion
    }
}
