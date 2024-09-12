using System.Collections.Generic;

namespace MarketProject.Models;

public class Supply
{
    public Supply(string name, int cnpj, List<Product> products, int dayLimit, int cep, string adress, string phone,
        string email)
    {
        Name = name;
        Cnpj = cnpj;
        Products = products;
        DayLimit = dayLimit;
        Cep = cep;
        Adress = adress;
        Phone = phone;
        Email = email;
    }

    public Supply()
    { }


    public string? Name { get; set; }
    public int Cnpj { get; private set; }
    public List<Product> Products { get; set; }
    public int DayLimit { get; set; }
    public int Cep { get; private set; }
    public string? Adress { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
}