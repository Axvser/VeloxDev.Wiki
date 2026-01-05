using VeloxDev.Core.MVVM;
using VeloxDev.Wiki.ViewModels.WikiCOMs.Interfaces;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public partial class TextViewModel : IWikiPageElement
    {
        [VeloxProperty] private WikiPageViewModel? _parent = null;
        [VeloxProperty] private int _index = -1;
        [VeloxProperty] private WikiPageElementType _type = WikiPageElementType.Text;

        [VeloxProperty] private string _text = string.Empty;
    }
}