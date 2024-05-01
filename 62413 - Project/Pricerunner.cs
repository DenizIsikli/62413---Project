using System;
using System.IO;
using System.Text;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace _62413___Project
{
    internal class Pricerunner
    {
        /// <summary>
        /// Web scraper to fetch the top 10 products from Pricerunner
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<string> GetTopTenProducts(string searchQuery)
        {
            // Construct the URL to fetch from Pricerunner
            var url = $"https://www.pricerunner.dk/search?q={Uri.EscapeDataString(searchQuery)}";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var pageContent = await response.Content.ReadAsStringAsync();

            // Load the HTML content into an HtmlDocument object
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(pageContent);

            // Define the XPath query to extract product names and links
            var productNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'pr-eh3e2')]");

            var products = new StringBuilder();
            int maxProductCount = 10;

            for (int i = 0; i < productNodes.Count; i++) {
                if (i >= maxProductCount)
                {
                    break;
                }

                var nameNode = productNodes[i].SelectSingleNode(".//h3[contains(@class, 'pr-c6rk6p')]");
                var priceNode = productNodes[i].SelectSingleNode(".//span[contains(@class, 'pr-yp1q6p')]");
                var linkNode = productNodes[i].SelectSingleNode(".//a");

                string? name = nameNode?.InnerText.Trim();
                string? price = priceNode?.InnerText.Trim();
                string? link = linkNode?.Attributes["href"]?.Value;

                products.AppendLine($"- {name} - {price}: ");
                products.AppendLine($"  https://www.pricerunner.dk{link}");
                products.AppendLine();
            }

            return products.ToString();
        }
    }
}
