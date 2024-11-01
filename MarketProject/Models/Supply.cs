using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarketProject.ViewModels;
using MarketProject.Views;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using ReactiveUI;

namespace MarketProject.Models;

public class Supply
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; internal set; }
    public string Name { get; set; }
    public string Cnpj { get; private set; }
    public List<string> Products { get; private set; }
    public int DayLimit { get; set; }
    public string Cep { get; set; }
    public string? Adress { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public bool InDeliver { get; set; }

    [JsonConstructor]
    public Supply(string id, string name, string cnpj, List<string> products, int dayLimit, string cep, string adress,
        string phone,
        string email)
    {
        Id = id;
        Name = name;
        Cnpj = cnpj;
        DayLimit = dayLimit;
        Products = products;
        Cep = cep;
        Adress = adress;
        Phone = phone;
        Email = email;
    }
    public Supply(string name, string cnpj, List<string> products, int dayLimit, string cep, string adress,
        string phone,
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

    
    public static async Task<dynamic> ValidarCEP(string cep)
    {
        // Remove o hífen do CEP, se existir
        cep = cep.Replace("-", "");

        string url = $"https://viacep.com.br/ws/{cep}/json/";
        using HttpClient client = new HttpClient();
        var response = await client.GetAsync(url).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonDefinition = new
        {
            logradouro = "",
            localidade = ""
        };
        var json = JsonConvert.DeserializeAnonymousType(content, jsonDefinition);
        return json;
    }

    public static async Task<dynamic> ConsultaCNPJ(string cnpj)
    {
        var newCnpj = cnpj.Replace("-", "").Replace("/", "").Replace(".", "").Replace(",", "");
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://receitaws.com.br/v1/cnpj/{newCnpj}"),
            Headers =
            {
                { "Accept", "application/json" },
            },
        };
        using var response = await client.SendAsync(request);
        if (response.StatusCode == (HttpStatusCode)429)
            throw new Exception("A consulta de CNPJ ultrapassou de 3 vezes, aguarde 1 minuto...");

        try
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            if (content.Contains("ERROR"))
                throw new Exception("Não foi possível ver os dados do CNPJ informado");

            var jsonDefinition = new
            {
                fantasia = "",
                logradouro = "",
                uf = "",
                cep = "",
                email = "",
                telefone = ""
            };
            return JsonConvert.DeserializeAnonymousType(content, jsonDefinition);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
}