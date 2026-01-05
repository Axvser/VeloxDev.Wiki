using VeloxDev.Core.MVVM;
using VeloxDev.Wiki.ViewModels.WikiCOMs.Interfaces;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public enum CodeLanguage
    {
        None,
        CSharp,
        XML
    }

    public partial class CodeViewModel : IWikiPageElement
    {
        [VeloxProperty] private WikiPageViewModel? _parent = null;
        [VeloxProperty] private int _index = -1;
        [VeloxProperty] private WikiPageElementType _type = WikiPageElementType.Code;
        
        [VeloxProperty] private string _code = string.Empty;
        [VeloxProperty] private CodeLanguage _language = CodeLanguage.None;
    }
}