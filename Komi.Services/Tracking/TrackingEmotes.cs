using System.Collections.Generic;
using System.Linq;
using Discord;
using Komi.Data.Models.Tracking;

namespace Komi.Bot.Services.Tracking
{
    public static class TrackingEmotes
    {
        public static IEnumerable<WorkStatusEmote> Emotes = new[]
        {
            new WorkStatusEmote(WorkType.Uploader,
                "<:UPlightgrey:717670620967469066>",
                "<:UPyellow:717670621131178004>",
                "<:UPgreen:717670621135241346>",
                "<:UPred:717670620976119899>"),

            new WorkStatusEmote(WorkType.Typesetter,
                "<:TSlightgrey:717670620556689419>",
                "<:TSyellow:717670620892102708>",
                "<:TSgreen:717670621034578020>",
                "<:TSred:717670620988571708>"),

            new WorkStatusEmote(WorkType.Translator,
                "<:TLlightgrey:717670620917137448>",
                "<:TLyellow:717670620950822932>",
                "<:TLgreen:717670621210869801>",
                "<:TLred:717670621013737492>"),

            new WorkStatusEmote(WorkType.RawProvider,
                "<:RPlightgrey:717670620577398866>",
                "<:RPyellow:717670620695101481>",
                "<:RPgreen:717670620543975446>",
                "<:RPred:717670620887777370>"),

            new WorkStatusEmote(WorkType.Redrawing,
                "<:RDlightgrey:717670620669804555>",
                "<:RDyellow:717670620862873620>",
                "<:RDgreen:717670620862873609>",
                "<:RDred:717670621068263485>"),

            new WorkStatusEmote(WorkType.QualityChecker,
                "<:QClightgrey:717670620795502652>",
                "<:QCyellow:717670620925788220>",
                "<:QCgreen:717670620690645064>",
                "<:QCred:717670620556427296>"),

            new WorkStatusEmote(WorkType.Proofreading,
                "<:PRlightgrey:717670620485255220>",
                "<:PRyellow:717670620783181864>",
                "<:PRgreen:717670620770598922>",
                "<:PRred:717670620577661049>"),

            new WorkStatusEmote(WorkType.Cleaning,
                "<:CLlightgrey:717670620799827999>",
                "<:CLyellow:717670620636250113>",
                "<:CLgreen:717670620422209588>",
                "<:CLred:717670620569272351>")
        };

        public static Dictionary<WorkType, WorkStatusEmote> EmoteDictionary =>
            Emotes.ToDictionary(x => x.Type, x => x);

        public static Emote GetEmote(WorkType type, WorkStatusEmote.StatusColor color) =>
            EmoteDictionary[type].Emotes[color];
    }
}