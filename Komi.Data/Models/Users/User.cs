using System.ComponentModel.DataAnnotations.Schema;

namespace Komi.Data.Models.Users
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        public string? Description { get; set; }
    }
}