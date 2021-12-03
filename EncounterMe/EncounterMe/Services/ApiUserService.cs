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
    //Klasė kuri sieja telefoną su db lentele Users kur saugom visus savo userius
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

        //Userio pridėjimas, bool - dėl registracijos
        public static async Task<bool> AddUser(User user)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var myStringContent = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");

           
            var response = await s_httpClient.PostAsync("Users", myStringContent);
 
            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                return true;
            }
            else
                return false;
                
            
        }

        //Userio gavimas pagal id
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

        //grąžina visą sąrašą userių
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

        //Updatinam userį db
        public static async Task UpdateUser(User user)
        {
            var json = JsonSerializer.Serialize<User>(user);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await s_httpClient.PutAsync($"Users/{user.Id}", stringContent);
            response.EnsureSuccessStatusCode();

        }

        //ištrinam userį iš db
        public static async Task DeleteUser(User user)
        {
           var response = await s_httpClient.DeleteAsync($"Users/{user.Id}");

           response.EnsureSuccessStatusCode();

        }
    }
}

