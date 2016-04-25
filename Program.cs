using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace Flurl_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainAsync = MainAsync(args);
            mainAsync.Wait();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("=> Url building stuff:\n");

            var myCoolUrl1 = "http://localhost/"
                .AppendPathSegment("sales")
                .AppendPathSegment("Q1");
            Console.WriteLine(myCoolUrl1);

            var myCoolUrl2 = "http://localhost/"
                .AppendPathSegment("sales")
                .AppendPathSegment("Q1")
                .AppendPathSegment("max");
            Console.WriteLine(myCoolUrl2);

            // From an array
            var urlParts = new string[] { "liga_mx", "results", "america" };
            var builtUrl = new Url("http://api.soccer-data.com");
            foreach (var urlPart in urlParts)
            {
                builtUrl.AppendPathSegment(urlPart);
            }
            Console.WriteLine(builtUrl);

            // Single method
            var builtUrl2 = new Url("http://api.soccer-data.com");
            builtUrl2.AppendPathSegments(urlParts);
            Console.WriteLine(builtUrl2);

            // Add query string parameters
            var endDate = DateTime.Today;
            var startDate = endDate.AddDays(-30);
            var competition = "concacaf champions league";
            builtUrl.SetQueryParams(new { endDate, startDate, c = competition });
            Console.WriteLine(builtUrl);

            // Remove query string params
            builtUrl.RemoveQueryParams("c", "startDate");
            Console.WriteLine(builtUrl);


            Console.WriteLine("\n=> Http client stuff:\n");
            var client = myCoolUrl1
                .WithBasicAuth("antonio", "secretPass")
                .WithHeader("User-Agent", "Flurl-Sample");

            var httpClient = client.HttpClient;
            Console.WriteLine(httpClient.DefaultRequestHeaders);

            var client2 = builtUrl
                .WithOAuthBearerToken("t0k3n")
                .WithHeader("Accept-Language", "it")
                .WithHeader("User-Agent", "Flurl-Sample")
                .WithHeaders(new
                {
                    CustomHeader = "Another value",
                    Accept = "text/json"
                });
            var httpClient1 = client2.HttpClient;
            Console.WriteLine(httpClient1.DefaultRequestHeaders);


            Console.WriteLine("\n=> Http requests stuff:\n");

            // "Real" example:

            var pokemonId = 25;
            var url1 = "http://pokeapi.co/api/v2/"
                .AppendPathSegment("pokemon")
                .AppendPathSegment(pokemonId.ToString());

            Console.WriteLine("Consultando " + url1);

            // Realiza la petición HTTP:
            dynamic pkmn1 = await url1.GetJsonAsync(); // pkmn1 
            Console.WriteLine(pkmn1.name + " " + "\nH:" + pkmn1.height + "\nW:" + pkmn1.weight + "\n");

            // Cambia el nombre por otro pokemon para obtener un error
            var pokemonName = "charmander";
            try
            {
                // Construcción de la URL
                var url2 = "http://pokeapi.co/api/v2/"
                    .AppendPathSegments("pokemon", pokemonName);

                Console.WriteLine("Consultando " + url2);

                // Realiza la petición HTTP:
                var pokemon = await url2.GetJsonAsync<Pokemon>();
                Console.WriteLine(pokemon.Name + "\nH:" + pokemon.Height + "\nW:" + pokemon.Weight + "\n");

            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.Response != null
                    && ex.Call.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("No existe un pokemon llamado " + pokemonName);
                }
            }
            Console.Read();
        }

    }
}
