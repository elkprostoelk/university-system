using System.Collections.Generic;

namespace UniversitySystem.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string FullName { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}