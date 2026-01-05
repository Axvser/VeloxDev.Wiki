namespace VeloxDev.Wiki.ViewModels.WikiCOMs.Interfaces
{
    public enum WikiPageElementType
    {
        None,
        Text,
        Code,
        Image,
        Link
    }

    public interface IWikiPageElement
    {
        public WikiPageViewModel? Parent { get; set; }
        public int Index { get; set; }
        public WikiPageElementType Type { get; set; }
    }
}