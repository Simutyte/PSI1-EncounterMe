// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EncounterMe.Pins;

namespace EncounterMe.Services
{
    //Klasė kuri sieja telefoną su db lentele FavouriteMapPins kur saugom ryšį tarp objekto ir vartotojo
    public static class APIFavouriteMapPinService
    {
        static HttpClient s_httpClient;
        static string BaseUrl = "http://10.0.2.2:54134/api/";

        static APIFavouriteMapPinService()
        {
            try
            {
                s_httpClient = new HttpClient
                {
                    BaseAddress = new Uri(BaseUrl)
                };
                s_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            }
            catch
            {
                Console.WriteLine("Bandem sukurt httpclient ir nepavyko");
            }
        }

        //Pridėjimas
        public static async Task AddFavouriteMapPin(FavouriteMapPin MapPin)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var myStringContent = new StringContent(JsonSerializer.Serialize(MapPin, options), Encoding.UTF8, "application/json");

            Console.WriteLine(myStringContent);
            var response = await s_httpClient.PostAsync("FavouriteMapPins", myStringContent);

            response.EnsureSuccessStatusCode();
        }

        //Gaunam sąrašą visų objektų id pagal userio id. Čia gausim visus objekt'ų id, kuriuos yra pamėgęs vartotojas, kurio id paduodam
        public static async Task<IEnumerable<FavouriteMapPin>> GetFavouriteMapPins(int id1) //čia id1 - vartotojo id
        {

            var response = await s_httpClient.GetAsync($"FavouriteMapPins/{id1}");      

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<IEnumerable<FavouriteMapPin>>(responseAsString, options);
        }

        //grąžina visą sąrašą visų ryšių (nelabai gal reikalingas galim būtų ištrinti)
        public static async Task<IEnumerable<FavouriteMapPin>> GetFavouriteMapPin()
        {
            Console.WriteLine("Pateko i API");

            var response = await s_httpClient.GetAsync("FavouriteMapPins");

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();

            //nustatom kad nekreiptų dėmesio į raidžių skirtumus
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            return JsonSerializer.Deserialize<IEnumerable<FavouriteMapPin>>(responseAsString, options);
        }

        //ištrinam ryšį iš db
        public static async Task DeleteFavouriteMapPin(int id1, int id2)
        {
            var response = await s_httpClient.DeleteAsync($"FavouriteMapPins/{id1},{id2}");

            response.EnsureSuccessStatusCode();

        }
    }
}
