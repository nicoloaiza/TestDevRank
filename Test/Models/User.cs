using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    [Table("Users")]
    public class User : AbstractEntity<int>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
