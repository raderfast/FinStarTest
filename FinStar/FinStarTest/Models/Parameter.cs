using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Parameter
    {
        /// <summary>
        /// Название параметра
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; } = String.Empty;
    }
}
