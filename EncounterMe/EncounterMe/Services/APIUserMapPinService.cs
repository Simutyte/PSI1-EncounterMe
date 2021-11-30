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
    public static class APIUserMapPinService
    {
        static HttpClient s_httpClient;
        static string BaseUrl = "http://10.0.2.2:54134/api/";

        static APIUserMapPinService()
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
       /* public static async Task AddUserMapPin(UserMapPin MapPin)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var myStringContent = new StringContent(JsonSerializer.Serialize(MapPin, options), Encoding.UTF8, "application/json");

            Console.WriteLine(myStringContent);
            var response = await s_httpClient.PostAsync("UserMapPins", myStringContent);

            response.EnsureSuccessStatusCode();
        }

        public static async Task<UserMapPin> GetUserMapPins(int id1, int id2)
        {

            var response = await s_httpClient.GetAsync($"UserMapPins/{id1},{id2}");

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<UserMapPin>(responseAsString, options);
        }

        //grąžina visą sąrašą mapPin
        public static async Task<IEnumerable<UserMapPin>> GetUserMapPin()
        {
            Console.WriteLine("Pateko i API");

            var response = await s_httpClient.GetAsync("UserMapPins");

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();

            //nustatom kad nekreiptų dėmesio į raidžių skirtumus
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            return JsonSerializer.Deserialize<IEnumerable<UserMapPin>>(responseAsString, options);
        }*/
    }
}
