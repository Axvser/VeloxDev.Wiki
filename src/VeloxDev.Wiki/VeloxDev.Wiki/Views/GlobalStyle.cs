using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using VeloxDev.Avalonia.PlatformAdapters;
using VeloxDev.Core.DynamicTheme;

namespace VeloxDev.Wiki.Views
{
    [ThemeConfig<ObjectConverter, Dark, Light>(nameof(Background), ["#1e1e1e"], ["#ffffff"])]
    [ThemeConfig<ObjectConverter, Dark, Light>(nameof(Foreground), ["#ffffff"], ["#1e1e1e"])]
    [ThemeConfig<ObjectConverter, Dark, Light>(nameof(SplitterBrush), ["#6a6a6a"], ["#6a1e1e1e"])]
    public partial class GlobalStyle : Control
    {
        public GlobalStyle() => InitializeTheme();

        public static readonly StyledProperty<IBrush> BackgroundProperty =
            AvaloniaProperty.Register<GlobalStyle, IBrush>(nameof(Background), Brushes.Transparent);

        public IBrush Background
        {
            get => this.GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> ForegroundProperty =
            AvaloniaProperty.Register<GlobalStyle, IBrush>(nameof(Foreground), Brushes.Transparent);

        public IBrush Foreground
        {
            get => this.GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> SplitterBrushProperty =
            AvaloniaProperty.Register<GlobalStyle, IBrush>(nameof(SplitterBrush), Brushes.Transparent);

        public IBrush SplitterBrush
        {
            get => this.GetValue(SplitterBrushProperty);
            set => SetValue(SplitterBrushProperty, value);
        }
    }
}
