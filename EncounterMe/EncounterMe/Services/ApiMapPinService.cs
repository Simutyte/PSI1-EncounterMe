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
        public static async Task AddMapPin(MapPin MapPin)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var myStringContent = new StringContent(JsonSerializer.Serialize(MapPin, options), Encoding.UTF8, "application/json");

            Console.WriteLine(myStringContent);
            var response = await s_httpClient.PostAsync("MapPins", myStringContent);

            //var response = await s_httpClient.PostAsync("MapPins",
               //new StringContent(JsonSerializer.Serialize(MapPin), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }
        public static async Task DeleteMapPin(MapPin MapPin)
        {
            var response = await s_httpClient.DeleteAsync($"MapPins/{MapPin.Id}");

            response.EnsureSuccessStatusCode();

        }
        public static async Task<MapPin> GetMapPin(int id)
        {
           
            var response = await s_httpClient.GetAsync($"MapPins/{id}");

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MapPin>(responseAsString);
        }
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

        /* var response = await s_httpClient.GetAsync("MapPins");

            Console.WriteLine(response);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseJson);
            List<MapPin> myMapPins = JsonConvert.DeserializeObject<List<MapPin>>(responseJson);

            //var responseAsString = await response.Content.ReadAsStringAsync();
            return myMapPins;*/
    }
}
