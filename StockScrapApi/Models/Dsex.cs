using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockScrapApi.Models
{
    public class Dsex
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string TradingCode { get; set; } = null!;
        public double? Ltp { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public double? Closep { get; set; }
        public double? Ycp { get; set; }
        public double? Change { get; set; }
        public long? Trade { get; set; }
        public double? Value { get; set; }
        public long? Volume { get; set; }
        public DateTime? InfoTime { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}