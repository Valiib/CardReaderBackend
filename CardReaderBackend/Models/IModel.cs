using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardReaderBackend.Models
{
    interface IModel
    {
        public long id { get; set; }
        public string Name { get; set; } 
        public DateTime Created { get; set; }

    }
}
