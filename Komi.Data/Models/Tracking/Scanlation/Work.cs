using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Komi.Data.Models.Discord.Guild;
using Komi.Data.Models.Discord.User;

namespace Komi.Data.Models.Tracking.Scanlation
{
    public class Work
    {
        public long WorkId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Volume { get; set; }

        public int? Chapter { get; set; }

        public Group Group { get; set; }

        [NotMapped]
        public IEnumerable<IUser> Collaborators =>
            Jobs
               .SelectMany(x => x.Workers)
               .Where(x => !x.Groups.Contains(Group));

        public Status? OverridenStatus { get; set; }

        [NotMapped]
        public Status Status => OverridenStatus ?? Jobs.Select(x => x.Status).GetStatus();

        public List<Job> Jobs { get; set; }
    }
}