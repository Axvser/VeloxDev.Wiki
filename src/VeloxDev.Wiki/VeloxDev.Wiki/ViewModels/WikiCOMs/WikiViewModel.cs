using VeloxDev.Core.MVVM;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public partial class WikiViewModel
    {
        [VeloxProperty] private string _title = string.Empty;
        [VeloxProperty] private NavigatorViewModel _rootNavigator = new();
        [VeloxProperty] private WikiPageViewModel _currentPage = new();
    }
}
