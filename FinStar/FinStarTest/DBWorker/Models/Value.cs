using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorker.Models
{
    public class Body
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
    }
}
