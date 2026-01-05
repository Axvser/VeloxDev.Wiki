using VeloxDev.Core.MVVM;
using VeloxDev.Wiki.ViewModels.WikiCOMs.Interfaces;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public partial class ImageViewModel : IWikiPageElement
    {
        [VeloxProperty] private WikiPageViewModel? _parent = null;
        [VeloxProperty] private int _index = -1;
        [VeloxProperty] private WikiPageElementType _type = WikiPageElementType.Image;
        
        [VeloxProperty] private string _url = string.Empty;
    }
}