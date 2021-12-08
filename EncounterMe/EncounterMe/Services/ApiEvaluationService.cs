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
    public static class ApiEvaluationService
    {
        static HttpClient s_httpClient;
        static string BaseUrl = "http://10.0.2.2:54134/api/";

        static ApiEvaluationService()
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
        public static async Task AddEvaluation(Evaluation evaluation)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var myStringContent = new StringContent(JsonSerializer.Serialize(evaluation, options), Encoding.UTF8, "application/json");

            Console.WriteLine(myStringContent);
            var response = await s_httpClient.PostAsync("Evaluations", myStringContent);

            response.EnsureSuccessStatusCode();
        }

        //Gaunam sąrašą visų įvertinimų pagal mapPinId
        public static async Task<IEnumerable<Evaluation>> GetEvaluations(int id1) //čia id1 - mapPinId
        {

            var response = await s_httpClient.GetAsync($"Evaluations/{id1}");

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (string.IsNullOrWhiteSpace(responseAsString))
                return null;

            return JsonSerializer.Deserialize<IEnumerable<Evaluation>>(responseAsString, options);
        }

        //grąžina 1
        public static async Task<Evaluation> GetEvaluation(int id1, int id2)
        {
            Console.WriteLine("Pateko i API");

            var response = await s_httpClient.GetAsync($"Evaluations/{id1},{id2}");
            Console.WriteLine(response);
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();

            //nustatom kad nekreiptų dėmesio į raidžių skirtumus
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (string.IsNullOrWhiteSpace(responseAsString))
                return null;

            return JsonSerializer.Deserialize<Evaluation>(responseAsString, options);
        }

        //ištrinam įvertinimą iš db
        public static async Task DeleteEvaluation(int id1, int id2)
        {
            var response = await s_httpClient.DeleteAsync($"Evaluations/{id1},{id2}");

            response.EnsureSuccessStatusCode();

        }

        public static async Task UpdateEvaluation(Evaluation evaluation)
        {
            var json = JsonSerializer.Serialize<Evaluation>(evaluation);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await s_httpClient.PutAsync($"Evaluations/{evaluation.UserId},{evaluation.MapPinId}", stringContent);
            response.EnsureSuccessStatusCode();

        }
    }
}
