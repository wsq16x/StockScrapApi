using HtmlAgilityPack;

namespace StockScrapApi.Types
{
    public class TableWithHead
    {
        public string Head { get; set; }
        public HtmlNodeCollection Nodes { get; set; }
    }
}