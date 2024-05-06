using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Net.Mime;
using System.Reflection.PortableExecutable;


namespace _62413___Project
{
    public class GptPrompt
    {
        private readonly HttpClient? client;

        /// <summary>
        /// HttpClient for making requests to the GPT-3 API.
        /// </summary>
        public GptPrompt()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// Makes a request to the GPT-3 API to generate a response to a prompt.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public async Task<string> GptRapidApiAsync(string prompt)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://simple-chatgpt-api.p.rapidapi.com/ask"),
                Headers =
                {
                    { "X-RapidAPI-Key", "2eced3f581mshfa96752f794ad99p121183jsn338232762482" },
                    { "X-RapidAPI-Host", "simple-chatgpt-api.p.rapidapi.com" },
                },
                Content = new StringContent(JsonSerializer.Serialize(new { question = prompt }), Encoding.UTF8, "application/json")
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
                if (result != null && result.ContainsKey("answer"))
                {
                    return result["answer"];
                }
                else
                {
                    return "Could not get a valid response.";
                }
            }
        }
    }    
}
