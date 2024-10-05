using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MarketProject.Models;

public class Profile
{
    public Profile(string fullName, string cpf, DateTime birthYear, string username, string password, JobFunction function, string role)
    {
        FullName = fullName;
        Cpf = cpf;
        Age = birthYear.Year;
        Username = username;
        Password = password;
        RoleFunction = function;
        Role = role;
    }
    
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
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
    public string Cpf { get; set; }
    public string Password { get; set; }
    public JobFunction RoleFunction { get; set; }
    public string Role { get; set; }
    
}

public enum JobFunction
{
    Stocker,
    Waiter,
    Admin,
}