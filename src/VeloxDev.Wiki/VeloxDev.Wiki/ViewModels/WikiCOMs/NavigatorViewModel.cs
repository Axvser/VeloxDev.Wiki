using System.Collections.ObjectModel;
using VeloxDev.Core.MVVM;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public partial class NavigatorViewModel
    {
        [VeloxProperty] private NavigatorViewModel? _parent = null;
        [VeloxProperty] private ObservableCollection<NavigatorViewModel> _children = [];
        [VeloxProperty] private ObservableCollection<WikiPageViewModel> _pages = [];
    }
}
