using System.Collections.ObjectModel;
using VeloxDev.Core.MVVM;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public partial class NavigatorViewModel
    {
        [VeloxProperty] private ObservableCollection<NavigatorViewModel> _children = [];
    }
}
