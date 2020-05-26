using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Discord;
using Discord.Commands;
using MangaDexApi;
using MangaDexApi.Serialization;

namespace Komi.Bot.Modules
{
    [Group("mangadex")]
    [Alias("md")]
    [Summary("Commands related to querying MangaDex")]
    public class MangaDexModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMangaDexApi _api;

        private readonly IEnumerable<(string bbTag, char discord)> _bbTagEquivalents = new[]
        {
            ("b", '*'),
            ("u", '_'),
            ("spoiler", '|'),
            ("hr", '\n'),
        };

        private readonly Regex _decodeLinkRegex = new Regex(
            @"\[url=(?<url>[^\]]+?)\](?<text>[^\]]+?)\[\/url\]",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly Regex _encodeLinkRegex = new Regex(
            @"\[(?<text>[^\]]+?)\]\((?<url>[^\]]+?)\)",
            RegexOptions.Compiled);

        public MangaDexModule(IMangaDexApi api) => _api = api;

        [Command("title", RunMode = RunMode.Async)]
        public async Task TitleInfoAsync(int id)
        {
            var title = await _api.GetTitle(id);
            var embed = new EmbedBuilder()
               .WithAuthor(title.Manga.Author)
               .WithTitle(title.Manga.Title)
               .WithDescription(DecodeBbTag(title.Manga.Description))
               .WithThumbnailUrl($"https://mangadex.org{title.Manga.CoverUrl}");

            foreach (Source.SourceType sourceType in Enum.GetValues(typeof(Source.SourceType)))
            {
                var links = title.Manga.Links
                   .Where(l => l.Type == sourceType)
                   .ToArray();

                if (links.Length > 0)
                {
                    var fieldValue = links
                       .Select(l => $"[{l.Emoji}]({l.Get()})");

                    embed.AddField(builder =>
                        builder
                           .WithName(sourceType.ToString())
                           .WithValue(string.Join(" ", fieldValue))
                           .WithIsInline(false));
                }
            }

            await ReplyAsync(embed: embed.Build());
        }

        private string DecodeBbTag(string text)
        {
            text = _decodeLinkRegex
               .Replace(text, m =>
                    $"[{m.Groups["text"]}]({m.Groups["url"]})");

            foreach ((string bbTag, char discord) in _bbTagEquivalents)
            {
                text = Regex.Replace(text,
                    $@"\[/?{bbTag}\]",
                    new string(discord, 2),
                    RegexOptions.IgnoreCase);
            }

            return HttpUtility.HtmlDecode(text);
        }

        private string EncodeBbTag(string text)
        {
            text = _encodeLinkRegex
               .Replace(text, m =>
                    $"[url={m.Groups["url"]}]{m.Groups["text"]}[/url]");

            foreach ((string bbTag, char discord) in _bbTagEquivalents)
            {
                text = Regex.Replace(text,
                    new string(discord, 2),
                    m => $@"[{bbTag}]{m.Value}[/{bbTag}]");
            }

            return HttpUtility.HtmlEncode(text);
        }
    }
}