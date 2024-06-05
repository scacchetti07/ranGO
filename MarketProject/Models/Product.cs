using System;

namespace MarketProject.Models;

public class Product
{
    public string Id { get; set; } // ID do produto
    public string ProdName { get; set; }
    public int ProdQtd { get; set; }
    public string SupName { get; set; } // Nome do fornecedor
    public string ProdCat { get; set; } // Categoria
    public int ProdMin { get; set; }
    public int ProdMax { get; set; }

    public Product(string prdName, int prdQtd, string supply, string prdcat, int min, int max)
    {
        Id = Guid.NewGuid().ToString();
        ProdName = prdName;
        ProdQtd = prdQtd;
        SupName = supply;
        ProdCat = prdcat;
        ProdMin = min;
        ProdMax = max;
    }

}