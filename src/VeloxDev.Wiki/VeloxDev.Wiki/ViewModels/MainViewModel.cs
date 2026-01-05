using VeloxDev.Core.MVVM;
using VeloxDev.Wiki.ViewModels.WikiCOMs;

namespace VeloxDev.Wiki.ViewModels;

public partial class MainViewModel
{
    [VeloxProperty] private WikiViewModel? _wiki = null;
}
