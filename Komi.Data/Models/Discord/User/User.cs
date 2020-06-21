using System.ComponentModel.DataAnnotations.Schema;

namespace Komi.Data.Models.Discord.User
{
    public class User : IUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        public string? Description { get; set; }
    }
}