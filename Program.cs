using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Bem-vindo ao Conversor de Moedas!");

        Console.Write("Digite a moeda de origem (ex: USD): ");
        string moedaOrigem = Console.ReadLine().ToUpper();

        Console.Write("Digite a moeda de destino (ex: BRL): ");
        string moedaDestino = Console.ReadLine().ToUpper();

        Console.Write("Digite o valor a ser convertido: ");
        decimal valor = decimal.Parse(Console.ReadLine());

        decimal taxaDeCambio = await ObterTaxaDeCambio(moedaOrigem, moedaDestino);
        decimal valorConvertido = valor * taxaDeCambio;

        Console.WriteLine($"\n{valor} {moedaOrigem} equivalem a {valorConvertido:F2} {moedaDestino}.");
    }

    static async Task<decimal> ObterTaxaDeCambio(string moedaOrigem, string moedaDestino)
    {
        string apiUrl = $"https://api.exchangerate-api.com/v4/latest/{moedaOrigem}";
        using HttpClient client = new HttpClient();

        HttpResponseMessage response = await client.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        var dados = System.Text.Json.JsonSerializer.Deserialize<ExchangeRateResponse>(responseBody);

        return dados.rates[moedaDestino];
    }

    public class ExchangeRateResponse
    {
        public string base_code { get; set; }
        public Dictionary<string, decimal> rates { get; set; }
    }
}
