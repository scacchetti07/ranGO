using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MarketProject.Models;

public class Supply
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; internal set; }
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
    
    public static async Task<dynamic> ValidarCEP(string cep)
    {
        // Remove o hífen do CEP, se existir
        cep = cep.Replace("-", "");
        
        string url = $"https://viacep.com.br/ws/{cep}/json/";
        using HttpClient client = new HttpClient();
            var response = await client.GetAsync(url).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new Exception("CEP Inválido");
                
            var conteudo = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var jsonDefinition = new
            {
                cep = "",
                logradouro = "",
                localidade = ""
            };
            var json = JsonConvert.DeserializeAnonymousType(conteudo, jsonDefinition);
            // Verifica se o conteúdo contém a chave "erro", que indica que o CEP não foi encontrado
            return json;
    }
}