using System;

namespace FirsatBul.Entities
{
    public class Firsatlar
    {
        public int Id { get; set; }
        public string FirsatName { get; set; } 
        public DateTime FirsatExpirationDate { get; set; } 
        public bool Deleted { get; set; } 
        public string CreatedBy { get; set; } 
    }
}
