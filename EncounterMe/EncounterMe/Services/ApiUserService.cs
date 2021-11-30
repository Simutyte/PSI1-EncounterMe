// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EncounterMe.Users;

namespace EncounterMe.Services
{
    public static class ApiUserService
    {
        static HttpClient s_httpClient;
        static string BaseUrl = "http://10.0.2.2:54134/api/";

        static ApiUserService()
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
        public static async Task<bool> AddUser(User user)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var myStringContent = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");

            Console.WriteLine(myStringContent);
            var response = await s_httpClient.PostAsync("Users", myStringContent);
            Console.WriteLine("RESPONSE: "+response);
            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                return true;
            }
            else
                return false;
                
            
        }

        public static async Task<User> GetUser(int id)
        {

            var response = await s_httpClient.GetAsync($"Users/{id}");

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response as string get user "+responseAsString);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<User>(responseAsString, options);
        }

        //grąžina visą sąrašą mapPin
        public static async Task<IEnumerable<User>> GetUsers()
        {
            Console.WriteLine("Pateko i API");

            var response = await s_httpClient.GetAsync("Users");

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();

            //nustatom kad nekreiptų dėmesio į raidžių skirtumus
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            return JsonSerializer.Deserialize<IEnumerable<User>>(responseAsString, options);
        }

        public static async Task UpdateUser(User user)
        {
            //var response = await s_httpClient.PutAsync($"Users/{user.Id}");

            //response.EnsureSuccessStatusCode();

        }

        public static async Task DeleteUser(User user)
       {
           var response = await s_httpClient.DeleteAsync($"Users/{user.Id}");

           response.EnsureSuccessStatusCode();

       }
    }
}

