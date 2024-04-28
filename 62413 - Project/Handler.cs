using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _62413___Project
{
    public class Handler
    {
        private readonly GptPrompt gptPrompt = new();
        public Dictionary<string, Func<string, Task<string>>> botCommands;

        public Handler()
        {
            botCommands = new Dictionary<string, Func<string, Task<string>>>()
        {
            { "gpt", async (prompt) => await gptPrompt.GptRapidApiAsync(prompt) }
        };
        }

        public static string Name { get; set; }
        public static string Password { get; set; }
        
    }
}
