// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EncounterMe.Services
{
    //Klasė kuri sieja telefoną su db lentele MapPins kur saugom objektus (visus)
    public static class ApiMapPinService
    {
        static HttpClient s_httpClient;
        static string BaseUrl = "http://10.0.2.2:54134/api/";

        static ApiMapPinService()
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

        //Pridedam objektą į db
        public static async Task AddMapPin(MapPin MapPin)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var myStringContent = new StringContent(JsonSerializer.Serialize(MapPin, options), Encoding.UTF8, "application/json");

            Console.WriteLine(myStringContent);
            var response = await s_httpClient.PostAsync("MapPins", myStringContent);

            response.EnsureSuccessStatusCode();
        }

        //Gaunam 1 objektą pagal id
        public static async Task<MapPin> GetMapPin(int id)
        {
           
            var response = await s_httpClient.GetAsync($"MapPins/{id}");

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<MapPin>(responseAsString, options);
        }

        //grąžina visą sąrašą objektų
        public static async Task<IEnumerable<MapPin>> GetMapPins()
        {
            Console.WriteLine("Pateko i API");

            var response = await s_httpClient.GetAsync("MapPins");              

            response.EnsureSuccessStatusCode();
           
            var responseAsString = await response.Content.ReadAsStringAsync();
    
            //nustatom kad nekreiptų dėmesio į raidžių skirtumus
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            
            return JsonSerializer.Deserialize<IEnumerable<MapPin>>(responseAsString, options);
        }


        //ištrinam objektą iš db
        public static async Task DeleteMapPin(MapPin MapPin)
        {
           var response = await s_httpClient.DeleteAsync($"MapPins/{MapPin.Id}");
            Console.WriteLine(response);
           response.EnsureSuccessStatusCode();

        }
    }
}
