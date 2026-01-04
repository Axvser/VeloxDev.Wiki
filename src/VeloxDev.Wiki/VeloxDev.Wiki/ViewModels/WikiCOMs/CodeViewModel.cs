using VeloxDev.Core.MVVM;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public enum CodeLanguage
    {
        None,
        CSharp,
        XML
    }

    public partial class CodeViewModel
    {
        [VeloxProperty] private string _code = string.Empty;
        [VeloxProperty] private CodeLanguage _language = CodeLanguage.None;
    }
}