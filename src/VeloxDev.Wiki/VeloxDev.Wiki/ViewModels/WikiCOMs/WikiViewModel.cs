using System.Collections.ObjectModel;
using VeloxDev.Core.MVVM;

namespace VeloxDev.Wiki.ViewModels.WikiCOMs
{
    public partial class WikiViewModel
    {
        [VeloxProperty] private string _title = string.Empty;
        [VeloxProperty] private string _version = string.Empty;
        [VeloxProperty] private ObservableCollection<NavigatorViewModel> _navigators = [];
    }
}
