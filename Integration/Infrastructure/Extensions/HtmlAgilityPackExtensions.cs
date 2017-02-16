using System;
using HtmlAgilityPack;
using MMU.BoerseDownloader.Integration.Model.Enumerations;

namespace MMU.BoerseDownloader.Integration.Infrastructure.Extensions
{
    internal static class HtmlAgilityPackExtensions
    {
        internal static string GetElementClassName(this HtmlNode htmlNode)
        {
            var classAttr = htmlNode.Attributes["class"];
            if (classAttr == null)
            {
                return string.Empty;
            }

            return classAttr.Value;
        }

        internal static HtmlNode NavigateToElement(this HtmlNode htmlNode, Func<HtmlNode, bool> predicate, HtmlNavigationType htmlNavigationType)
        {
            var result = NavigateToElement(htmlNode, htmlNavigationType);
            while (result != null && !predicate(result))
            {
                result = NavigateToElement(result, predicate, htmlNavigationType);
            }

            return result;
        }

        internal static HtmlNode NavigateToElementOfType(this HtmlNode htmlNode, string typeName, HtmlNavigationType htmlNavigationType)
        {
            var result = NavigateToElement(htmlNode, htmlNavigationType);
            while (result != null && result.Name != typeName)
            {
                result = NavigateToElement(result, htmlNavigationType);
            }

            return result;
        }

        private static HtmlNode NavigateToElement(HtmlNode htmlNode, HtmlNavigationType htmlNavigationType)
        {
            switch (htmlNavigationType)
            {
                case HtmlNavigationType.PreviousSibling:
                {
                    return htmlNode.PreviousSibling;
                }

                case HtmlNavigationType.NextSibling:
                {
                    return htmlNode.NextSibling;
                }

                case HtmlNavigationType.Parent:
                {
                    return htmlNode.ParentNode;
                }

                case HtmlNavigationType.FirstChild:
                {
                    return htmlNode.FirstChild;
                }

                default:
                    throw new NotImplementedException(htmlNavigationType.ToString());
            }
        }
    }
}