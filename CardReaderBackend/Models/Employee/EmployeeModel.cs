using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardReaderBackend.Models
{
    public class EmployeeModel: IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Created { get; set; }

        public WorkRecords workRecords { get; set; }
    }
}
