using System.Globalization;
using System.Xml;
using UserPanel.Models.Product;
using UserPanel.Types;

namespace UserPanel.Services
{
    public static class FeedParser
    {

        private static Dictionary<FeedTypes, Func<XmlDocument, List<Product>>> strategy = new Dictionary<FeedTypes, Func<XmlDocument, List<Product>>>()
        {
            { FeedTypes.Ceneo, ParsCeneo }
        };

        public static List<Product> ParseFeed(FeedTypes feedType, XmlDocument document)
        {
            try
            {
                return strategy[feedType].Invoke(document);

            }catch (Exception) {

                throw new ArgumentException("The type of Feed doesnt exists in service");
            }
        }

        private static List<Product> ParsCeneo(XmlDocument document)
        {
            List<Product> products = new List<Product>();

            try
            {
                XmlNodeList offerNodes = document.SelectNodes("//o");

                foreach (XmlNode offerNode in offerNodes)
                {
                    var product = new Product
                    {
                        Id = offerNode.Attributes["id"]?.InnerText,
                        Url = offerNode.Attributes["url"]?.InnerText,
                        Price = ParsePrice(offerNode.Attributes["price"]?.InnerText),
                        InStock = offerNode.Attributes["avail"]?.InnerText == "1",
                        Category = offerNode.SelectSingleNode("cat")?.InnerText.Trim(),
                        Name = offerNode.SelectSingleNode("name")?.InnerText.Trim(),
                        ImageUrl = offerNode.SelectSingleNode("imgs/main")?.Attributes["url"]?.InnerText
                    };

                    products.Add(product);
                }
                return products;

            }catch (Exception)
            {
                throw new FormatException("Bad format of feed for Ceneo");
            }

        }

        private static decimal ParsePrice(string priceString)
        {
            if (decimal.TryParse(priceString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price))
            {
                return price;
            }
            else
            {
                throw new FormatException($"The input string '{priceString}' was not in a correct format.");
            }
        }
    }
}
