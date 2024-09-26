using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MarketProject.Models;

public class Supply
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; private set; }
    public string? Name { get; set; }
    public string Cnpj { get; private set; }
    public List<Product> Products { get; private set; }
    public int DayLimit { get; set; }
    public string Cep { get; set; }
    public string? Adress { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    
    public Supply(string name, string cnpj, List<Product> products, int dayLimit, string cep, string adress, string phone,
        string email)
    {
        Name = name;
        Cnpj = cnpj;
        DayLimit = dayLimit;
        Products = products;
        Cep = cep;
        Adress = adress;
        Phone = phone;
        Email = email;
    }
    public Supply()
    { }
}