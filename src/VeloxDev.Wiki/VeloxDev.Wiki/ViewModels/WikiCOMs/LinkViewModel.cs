using VeloxDev.Core.MVVM;
using VeloxDev.Wiki.ViewModels.WikiCOMs.Interfaces;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public partial class LinkViewModel : IWikiPageElement
    {
        [VeloxProperty] private WikiPageViewModel? _parent = null;
        [VeloxProperty] private int _index = -1;
        [VeloxProperty] private WikiPageElementType _type = WikiPageElementType.Link;
        
        [VeloxProperty] private string _text = string.Empty;
        [VeloxProperty] private string _url = string.Empty;
    }
}