using System;

namespace CardReaderBackend.Models
{
    public class WorkRecords : IModel
    {
        public long id { get ; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }
}