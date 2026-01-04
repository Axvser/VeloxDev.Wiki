using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using VeloxDev.Wiki.ViewModels.WikiCOMs;
using VeloxDev.Wiki.Views.WikiCOMs.Tools.SyntaxAnalyzer;

namespace VeloxDev.Wiki;

public partial class CodeView : UserControl
{
    public CodeView()
    {
        InitializeComponent();
    }

    private static Dictionary<CodeLanguage,ISyntaxHighlighter> _highlighters = new()
    {
        { CodeLanguage.CSharp, new CSharpHighlighter() },
        { CodeLanguage.XML, new XmlHighlighter() }
    };

    public static readonly StyledProperty<string> CodeProperty =
        AvaloniaProperty.Register<CodeView, string>(nameof(Code), string.Empty);

    public static readonly StyledProperty<CodeLanguage> LanguageProperty =
        AvaloniaProperty.Register<CodeView, CodeLanguage>(nameof(Language), CodeLanguage.None);

    public string Code
    {
        get => this.GetValue(CodeProperty);
        set
        {
            SetValue(CodeProperty, value);
            UpdateHighlightedCode();
        }
    }

    public CodeLanguage Language
    {
       get => this.GetValue(LanguageProperty);
       set
       {
            SetValue(LanguageProperty, value);
            UpdateHighlightedCode();
       }
    }

    private void UpdateHighlightedCode()
    {
        CodeRender.Inlines?.Clear();
        if(_highlighters.TryGetValue(Language, out var highlighter))
        {
            CodeRender.Inlines?.AddRange(highlighter.Highlight(Code));
        }
    }

    private void CopyEventHandler(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(CodeRender);
        topLevel?.Clipboard?.SetTextAsync(Code);
    }
}