using System;

namespace MarketProject.Models;

public class Product
{
    public string Id { get; set; } // ID do produto
    public string ProdName { get; set; }
    public int ProdQtd { get; set; }
    public string SupplyName { get; set; } // Nome do fornecedor
    public string ProdStatus { get; set; } // Categoria
    public int ProdMin { get; set; }
    public int ProdMax { get; set; }

    public Product(string prdName, int prdQtd, string supply, string prodStatus, int min, int max)
    {
        Id = Guid.NewGuid().ToString(); // Gera um Id único
        ProdName = prdName;
        ProdQtd = prdQtd;
        SupplyName = supply;
        ProdStatus = prodStatus;
        ProdMin = min;
        ProdMax = max;
    }

}