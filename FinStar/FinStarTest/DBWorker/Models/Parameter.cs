using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DBWorker.Models
{
    public class Parameter
    {
        /// <summary>
        /// Название параметра
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }
}
