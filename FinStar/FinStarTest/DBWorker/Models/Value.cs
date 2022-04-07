
using Newtonsoft.Json;

namespace DBWorker.Models
{
    public class Value
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        [JsonProperty("orderNumber", NullValueHandling = NullValueHandling.Ignore)]
        public int OrderNumber { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string? ValueString { get; set; }
    }
}
