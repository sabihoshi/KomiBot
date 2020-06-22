using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Komi.Data;
using Komi.Data.Models.Discord.Guild;
using Komi.Data.Models.Moderation;
using Komi.Data.Models.Settings;
using Komi.Data.Models.Tracking;
using Komi.Data.Models.Tracking.Scanlation;

namespace Komi.Bot.Services.Tracking
{
    public class GroupService
    {
        private readonly KomiContext _context;

        public GroupService(KomiContext context) => _context = context;

        public bool GroupExists(ulong guildId)
        {
            return _context.Groups
                      .SingleOrDefault(g => g.GroupId == guildId)
                != null;
        }

        public bool TryGetGroup(ulong guildId, [MaybeNullWhen(false)] out Group? group)
        {
            group = _context.Groups
               .SingleOrDefault(g => g.GroupId == guildId);

            return group != null;
        }

        public bool HasGroup(IUser user)
        {
            throw new NotImplementedException();
        }

        public bool IsMemberOf(IUser user, ulong guildId)
        {
            return _context
                   .GroupMembers
                   .SingleOrDefault(g =>
                        g.GroupId == guildId
                     && g.Id == user.Id)
                != null;
        }

        public async Task CreateGroupAsync(
            ulong guildId,
            string name,
            IEnumerable<WorkType>? types,
            ulong trackingChannel)
        {
            var settings = new GroupSetting
            {
                DefaultWorkTypes = (types ?? WorkTypeExtensions.Default)
                   .Select(x => new WorkTypeSetting(x)).ToList(),
                TrackingChannel = trackingChannel
            };

            var group = new Group
            {
                GroupId = guildId,
                GroupSettings = settings,
                Members = new List<GroupMember>(),
                ModerationSettings = new ModerationSetting(),
                ModerationData = new ModerationData(),
                Projects = new List<Series>()
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

    }
}