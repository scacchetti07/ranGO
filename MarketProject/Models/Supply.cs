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
    public string Name { get; set; }
    public string Cnpj { get; private set; }
    public List<string> Products { get; private set; }
    public int DayLimit { get; set; }
    public string Cep { get; set; }
    public string? Adress { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    
    public Supply(string name, string cnpj, List<string> products, int dayLimit, string cep, string adress, string phone,
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
    
    public static async Task<bool> ValidarCEP(string cep)
    {
        // Remove o hífen do CEP, se existir
        cep = cep.Replace("-", "");
        
        string url = $"https://viacep.com.br/ws/{cep}/json/";

        using HttpClient client = new HttpClient();
        try
        {
            var response = await client.GetAsync(url).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return false;
                
            var conteudo = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            // Verifica se o conteúdo contém a chave "erro", que indica que o CEP não foi encontrado
            return !conteudo.Contains("\"erro\"");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao consultar a API ViaCEP: {ex.Message}");
            return false;
        }
    }
}