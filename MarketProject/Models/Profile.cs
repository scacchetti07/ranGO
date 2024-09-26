using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MarketProject.Models;

public class Profile
{
    public Profile(string fullName, DateTime birthYear, string username, string password, JobRoles role)
    {
        FullName = fullName;
        Age = birthYear.Year;
        Username = username;
        Password = password;
        Role = role;
    }
    
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string FullName { get; set; }
    
    private int _age;
    public int Age
    {
        get => _age;
        private set
        {
            if (value == 0 || value > DateTime.Now.Year) return;
            _age = DateTime.Now.Year - value;
        }
    }
    
    public string Username { get; set; }
    public string Password { get; set; }
    public JobRoles Role { get; set; }
    
}

public enum JobRoles
{
    Stocker,
    Waiter,
    Admin,
}