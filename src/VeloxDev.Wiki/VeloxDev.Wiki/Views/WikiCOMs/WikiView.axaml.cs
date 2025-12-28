using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace VeloxDev.Wiki;

public partial class WikiView : UserControl
{
    public WikiView()
    {
        InitializeComponent();
    }
}