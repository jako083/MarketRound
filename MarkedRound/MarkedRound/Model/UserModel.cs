using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkedRound.Model
{
    public class UserModel
    {
        public UserModel(ObjectId id, string username, string password, string salt, string firstName, string lastName, int phoneNumber, string country, string city, string address, int?[] ongoingSales, int?[] salesHistory, int?[] reviews)
        {
            _id = id;
            Username = username;
            Password = password;
            Salt = salt;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Country = country;
            City = city;
            Address = address;
            OngoingSales = ongoingSales;
            SalesHistory = salesHistory;
            Reviews = reviews;
        }

        public ObjectId _id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int?[] OngoingSales { get; set; }
        public int?[] SalesHistory { get; set; }
        public int?[] Reviews { get; set; }
    }
}
