namespace Komi.Data.Models.Tracking
{
    public enum WorkType
    {
        None = 1 << 0,
        RawProvider = 1 << 1,
        Translator = 1 << 2,
        Proofreading = 1 << 3,
        Cleaning = 1 << 4,
        Redrawing = 1 << 5,
        Typesetter = 1 << 6,
        QualityChecker = 1 << 7,
        Uploader = 1 << 8
    }
}