using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace MarketProject.Models;

public class Database
{
    public List<Product> Products { get; private set; } = new(); // lista que armazena os dados de produtos registrados
    // getter é público para ser possível acionar ele na tela do Storage View. e em outro setores
    // setter é privado somente para o BD, assegurando que alguma modificação possa ser feita em meio a execução do programa
    
    // Adiciona o produto da lista do banco pelo parâmetro recebido
    public void AddProduct(Product product)
    {
        Products.Add(product); // adiciona o produto na lista
    }

    // Remove o produto da lista do banco pelo parâmetro recebido
    public void RemoveProduct(Product product)
    {
        Products.Remove(product); // adiciona o produto na lista
    }

    // Documentação do Newtonsoft.json => nuget.org/packages/newtonsoft.json
    public void Serialize() // Adiciona os produtos registrados no arquivo .json
    {
        // Convertendo os dados armazenados na lista de produtos do tipo .NET para o tipo JSON
        string json = JsonConvert.SerializeObject(Products); // recebe a conversão dos itens da lista
        // Método que cria um arquivo .json caso não exista e permitindo que seja possível escrever nele.
        using StreamWriter sw = new StreamWriter(File.Open(@".\teste.json",FileMode.Create)); 
        sw.WriteLine(json); // escreve no arquivo os dados armazenados da lista.
    }

    public void Deserialize() // Remove os produtos registrados no arquivo .json
    {
        string path = @".\teste.json";
        if (!File.Exists(path)) // verifica se o arquivo .json criado existe no sistema
            return;
        using StreamReader sr = new StreamReader(path); // abre uma instância da váriavel "sr" para ler o arquivo .json com os dados
        string json = sr.ReadToEnd();
        Products = JsonConvert.DeserializeObject<List<Product>>(json)!; // Remove do arquivo os dados armazenados da lista e json.
    }
}