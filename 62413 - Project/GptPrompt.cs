using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;


namespace _62413___Project
{
    public class GptPrompt
    {
        private readonly HttpClient? client;

        public GptPrompt()
        {
            client = new HttpClient();
        }

        public async Task<string> GptRapidApiAsync(string prompt)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://simple-chatgpt-api.p.rapidapi.com/ask"),
                Headers =
                {
                    { "X-RapidAPI-Key", "f4093ae77fmsh3b5b518992b9c97p136562jsn8eb575f33b51" },
                    { "X-RapidAPI-Host", "simple-chatgpt-api.p.rapidapi.com" },
                },
                Content = new StringContent($"{{\"question\": \"{prompt}\"}}", Encoding.UTF8, "application/json")
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);

                var json = JsonDocument.Parse(body);
                var answer = json.RootElement.GetProperty("answer").GetString();

                return answer ?? "Sorry, I couldn't get the response.";
            }
        }
    }
}
