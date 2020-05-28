using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly Regex _mangaDexLinkRegex = new Regex(
            @"^https?://mangadex\.org/(?<type>chapter|title)/(?<id>[0-9]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly Regex _chapterRegex = new Regex(
            @"^https?://mangadex\.org/chapter/(?<id>[0-9]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly Regex _titleRegex = new Regex(
            @"^https?://mangadex\.org/title/(?<id>[0-9]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public MangaDexModule(IMangaDexApi api) => _api = api;

        [Command("", RunMode = RunMode.Async)]
        [Summary("Requests info for a MangaDex chapter/title based on the link.")]
        [Alias("info")]
        [Priority(-1)]
        public async Task InfoAsync(string url)
        {
            var link = _mangaDexLinkRegex.Match(url);
            if (link.Success)
            {
                var id = int.Parse(link.Groups["id"].Value);
                switch (link.Groups["type"].Value)
                {
                    case "chapter": await ChapterInfoAsync(id);
                        break;
                    case "title": await TitleInfoAsync(id);
                        break;
                }
            }
        }

        [Command("chapter", RunMode = RunMode.Async)]
        [Summary("Requests info for a specific chapter.")]
        public async Task ChapterInfoAsync(string url)
        {
            var link = _chapterRegex.Match(url);
            if (link.Success)
            {
                var id = int.Parse(link.Groups["id"].Value);
                await ChapterInfoAsync(id);
            }
        }

        [Command("title", RunMode = RunMode.Async)]
        [Summary("Requests info for a specific title.")]
        public async Task TitleInfoAsync(string url)
        {
            var link = _titleRegex.Match(url);
            if (link.Success)
            {
                var id = int.Parse(link.Groups["id"].Value);
                await TitleInfoAsync(id);
            }
        }

        [Command("title", RunMode = RunMode.Async)]
        [Priority(1)]
        public async Task TitleInfoAsync(int id)
        {
            var title = await _api.GetTitle(id);
            if (title.Status != "OK")
                return;

            var embed = new EmbedBuilder()
               .WithAuthor(title.Manga.Author)
               .WithTitle($"{title.Manga.Title} :flag_{title.Manga.LangFlag}:")
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

        [Command("title", RunMode = RunMode.Async)]
        [Priority(1)]
        public async Task ChapterInfoAsync(int id)
        {
            var chapter = await _api.GetChapter(id);
            if (chapter.Status != "OK")
                return;

            var title = await _api.GetTitle((int)chapter.MangaId!);

            var description = new StringBuilder();
            if (chapter.Volume != null)
                description.AppendLine($"Volume: {chapter.Volume}");

            description.AppendLine($"Chapter: {chapter.ChapterNumber}");

            var embed = new EmbedBuilder()
               .WithAuthor(title.Manga.Author)
               .WithTitle($"{chapter.Title ?? title.Manga.Title} :flag_{title.Manga.LangFlag}: → :flag_{chapter.LangCode}:")
               .WithDescription(description.ToString())
               .WithThumbnailUrl($"https://mangadex.org{title.Manga.CoverUrl}");

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