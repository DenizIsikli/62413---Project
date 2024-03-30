using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _62413___Project
{
    public class Generator
    {
        public static string GenerateName()
        {
            return "Client " + new Random().Next(1000, 9999);
        }
    }
}
