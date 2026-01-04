using System.Collections.Generic;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeloxDev.Wiki.Views.WikiCOMs.Tools.SyntaxAnalyzer
{
    public class CSharpHighlighter : ISyntaxHighlighter
    {
        public IEnumerable<Inline> Highlight(string code)
        {
            return TokenizeAndColor(code);
        }

        private static IEnumerable<Inline> TokenizeAndColor(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            foreach (var token in root.DescendantTokens())
            {
                foreach (var trivia in token.LeadingTrivia)
                {
                    yield return CreateRunForTrivia(trivia);
                }

                if (!string.IsNullOrEmpty(token.Text))
                {
                    IBrush brush = GetTokenBrush(token);
                    yield return new Run(token.Text) { Foreground = brush };
                }

                foreach (var trivia in token.TrailingTrivia)
                {
                    yield return CreateRunForTrivia(trivia);
                }
            }
        }

        private static Run CreateRunForTrivia(SyntaxTrivia trivia)
        {
            string text = trivia.ToString();
            if (string.IsNullOrEmpty(text))
                return new Run("");

            IBrush brush = trivia.Kind() switch
            {
                // 注释 - 绿色
                SyntaxKind.SingleLineCommentTrivia or
                SyntaxKind.MultiLineCommentTrivia
                    => new SolidColorBrush(Color.FromRgb(0x57, 0xA6, 0x4A)),

                // 文档注释
                SyntaxKind.SingleLineDocumentationCommentTrivia or
                SyntaxKind.MultiLineDocumentationCommentTrivia or
                SyntaxKind.DocumentationCommentExteriorTrivia
                    => new SolidColorBrush(Color.FromRgb(0x57, 0xA6, 0x4A)),

                // 预处理指令 - 紫色
                SyntaxKind.IfDirectiveTrivia or
                SyntaxKind.ElifDirectiveTrivia or
                SyntaxKind.ElseDirectiveTrivia or
                SyntaxKind.EndIfDirectiveTrivia or
                SyntaxKind.RegionDirectiveTrivia or
                SyntaxKind.EndRegionDirectiveTrivia or
                SyntaxKind.DefineDirectiveTrivia or
                SyntaxKind.UndefDirectiveTrivia or
                SyntaxKind.ErrorDirectiveTrivia or
                SyntaxKind.WarningDirectiveTrivia or
                SyntaxKind.LineDirectiveTrivia or
                SyntaxKind.PragmaWarningDirectiveTrivia or
                SyntaxKind.PragmaChecksumDirectiveTrivia or
                SyntaxKind.ReferenceDirectiveTrivia or
                SyntaxKind.LoadDirectiveTrivia or
                SyntaxKind.NullableDirectiveTrivia
                    => new SolidColorBrush(Color.FromRgb(0xC5, 0x86, 0xC0)),

                // 禁用的代码
                SyntaxKind.DisabledTextTrivia
                    => new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80)),

                // 预处理消息
                SyntaxKind.PreprocessingMessageTrivia
                    => new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80)),

                // 空白字符 - 透明
                SyntaxKind.WhitespaceTrivia or
                SyntaxKind.EndOfLineTrivia
                    => Brushes.Transparent,

                // 其他 Trivia
                _ => Brushes.Gray
            };

            return new Run(text) { Foreground = brush };
        }

        private static IBrush GetTokenBrush(SyntaxToken token)
        {
            return token.Kind() switch
            {
                // 关键字 - 蓝色 (#569CD6)
                SyntaxKind.IntKeyword or
                SyntaxKind.StringKeyword or
                SyntaxKind.ClassKeyword or
                SyntaxKind.PublicKeyword or
                SyntaxKind.PrivateKeyword or
                SyntaxKind.ProtectedKeyword or
                SyntaxKind.InternalKeyword or
                SyntaxKind.PartialKeyword or
                SyntaxKind.StaticKeyword or
                SyntaxKind.VoidKeyword or
                SyntaxKind.NewKeyword or
                SyntaxKind.ThisKeyword or
                SyntaxKind.TrueKeyword or
                SyntaxKind.FalseKeyword or
                SyntaxKind.NullKeyword or
                SyntaxKind.BoolKeyword or
                SyntaxKind.ByteKeyword or
                SyntaxKind.SByteKeyword or
                SyntaxKind.ShortKeyword or
                SyntaxKind.UShortKeyword or
                SyntaxKind.UIntKeyword or
                SyntaxKind.LongKeyword or
                SyntaxKind.ULongKeyword or
                SyntaxKind.DoubleKeyword or
                SyntaxKind.FloatKeyword or
                SyntaxKind.DecimalKeyword or
                SyntaxKind.ObjectKeyword or
                SyntaxKind.CharKeyword or
                SyntaxKind.StructKeyword or
                SyntaxKind.InterfaceKeyword or
                SyntaxKind.EnumKeyword or
                SyntaxKind.DelegateKeyword or
                SyntaxKind.NamespaceKeyword or
                SyntaxKind.UsingKeyword or
                SyntaxKind.BaseKeyword or
                SyntaxKind.ConstKeyword or
                SyntaxKind.ReadOnlyKeyword or
                SyntaxKind.SealedKeyword or
                SyntaxKind.AbstractKeyword or
                SyntaxKind.VirtualKeyword or
                SyntaxKind.OverrideKeyword or
                SyntaxKind.ExternKeyword or
                SyntaxKind.RefKeyword or
                SyntaxKind.OutKeyword or
                SyntaxKind.InKeyword or
                SyntaxKind.IsKeyword or
                SyntaxKind.AsKeyword or
                SyntaxKind.ParamsKeyword or
                SyntaxKind.TypeOfKeyword or
                SyntaxKind.SizeOfKeyword or
                SyntaxKind.CheckedKeyword or
                SyntaxKind.UncheckedKeyword or
                SyntaxKind.UnsafeKeyword or
                SyntaxKind.OperatorKeyword or
                SyntaxKind.ExplicitKeyword or
                SyntaxKind.ImplicitKeyword or
                SyntaxKind.FixedKeyword or
                SyntaxKind.StackAllocKeyword or
                SyntaxKind.VolatileKeyword or
                SyntaxKind.EventKeyword or
                SyntaxKind.InternalKeyword
                    => new SolidColorBrush(Color.FromRgb(0x56, 0x9C, 0xD6)),

                // 控制流关键字 - 紫色 (#C6A0DF)
                SyntaxKind.IfKeyword or
                SyntaxKind.ElseKeyword or
                SyntaxKind.WhileKeyword or
                SyntaxKind.ReturnKeyword or
                SyntaxKind.ForKeyword or
                SyntaxKind.ForEachKeyword or
                SyntaxKind.DoKeyword or
                SyntaxKind.SwitchKeyword or
                SyntaxKind.CaseKeyword or
                SyntaxKind.DefaultKeyword or
                SyntaxKind.TryKeyword or
                SyntaxKind.CatchKeyword or
                SyntaxKind.FinallyKeyword or
                SyntaxKind.ThrowKeyword or
                SyntaxKind.BreakKeyword or
                SyntaxKind.ContinueKeyword or
                SyntaxKind.GotoKeyword or
                SyntaxKind.LockKeyword
                    => new SolidColorBrush(Color.FromRgb(0xC6, 0xA0, 0xDF)),

                // 上下文关键字 - 青色 (#4EC9B0)
                SyntaxKind.YieldKeyword or
                SyntaxKind.AliasKeyword or
                SyntaxKind.GlobalKeyword or
                SyntaxKind.GetKeyword or
                SyntaxKind.SetKeyword or
                SyntaxKind.AddKeyword or
                SyntaxKind.RemoveKeyword or
                SyntaxKind.WhereKeyword or
                SyntaxKind.FromKeyword or
                SyntaxKind.GroupKeyword or
                SyntaxKind.JoinKeyword or
                SyntaxKind.IntoKeyword or
                SyntaxKind.LetKeyword or
                SyntaxKind.ByKeyword or
                SyntaxKind.SelectKeyword or
                SyntaxKind.OrderByKeyword or
                SyntaxKind.OnKeyword or
                SyntaxKind.EqualsKeyword or
                SyntaxKind.AscendingKeyword or
                SyntaxKind.DescendingKeyword or
                SyntaxKind.NameOfKeyword or
                SyntaxKind.AsyncKeyword or
                SyntaxKind.AwaitKeyword or
                SyntaxKind.WhenKeyword or
                SyntaxKind.OrKeyword or
                SyntaxKind.AndKeyword or
                SyntaxKind.NotKeyword or
                SyntaxKind.WithKeyword or
                SyntaxKind.InitKeyword or
                SyntaxKind.RecordKeyword or
                SyntaxKind.ManagedKeyword or
                SyntaxKind.UnmanagedKeyword or
                SyntaxKind.RequiredKeyword or
                SyntaxKind.ScopedKeyword or
                SyntaxKind.FileKeyword or
                SyntaxKind.VarKeyword
                    => new SolidColorBrush(Color.FromRgb(0x4E, 0xC9, 0xB0)),

                // 字符串和字符字面量 - 橙色 (#D69D85)
                SyntaxKind.StringLiteralToken or
                SyntaxKind.CharacterLiteralToken or
                SyntaxKind.InterpolatedStringStartToken or
                SyntaxKind.InterpolatedStringEndToken or
                SyntaxKind.InterpolatedStringTextToken or
                SyntaxKind.SingleLineRawStringLiteralToken or
                SyntaxKind.MultiLineRawStringLiteralToken or
                SyntaxKind.Utf8StringLiteralToken or
                SyntaxKind.Utf8SingleLineRawStringLiteralToken or
                SyntaxKind.Utf8MultiLineRawStringLiteralToken or
                SyntaxKind.InterpolatedSingleLineRawStringStartToken or
                SyntaxKind.InterpolatedMultiLineRawStringStartToken or
                SyntaxKind.InterpolatedRawStringEndToken
                    => new SolidColorBrush(Color.FromRgb(0xD6, 0x9D, 0x85)),

                // 数字字面量 - 绿色 (#B5CEA8)
                SyntaxKind.NumericLiteralToken
                    => new SolidColorBrush(Color.FromRgb(0xB5, 0xCE, 0xA8)),

                // 运算符和标点符号 - 浅灰色 (#D4D4D4)
                SyntaxKind.PlusToken or
                SyntaxKind.MinusToken or
                SyntaxKind.AsteriskToken or
                SyntaxKind.SlashToken or
                SyntaxKind.PercentToken or
                SyntaxKind.AmpersandToken or
                SyntaxKind.BarToken or
                SyntaxKind.CaretToken or
                SyntaxKind.TildeToken or
                SyntaxKind.ExclamationToken or
                SyntaxKind.EqualsToken or
                SyntaxKind.LessThanToken or
                SyntaxKind.GreaterThanToken or
                SyntaxKind.DotToken or
                SyntaxKind.CommaToken or
                SyntaxKind.ColonToken or
                SyntaxKind.SemicolonToken or
                SyntaxKind.QuestionToken or
                SyntaxKind.DollarToken or
                SyntaxKind.HashToken or
                SyntaxKind.DotDotToken or
                SyntaxKind.PlusPlusToken or
                SyntaxKind.MinusMinusToken or
                SyntaxKind.AmpersandAmpersandToken or
                SyntaxKind.BarBarToken or
                SyntaxKind.QuestionQuestionToken or
                SyntaxKind.LessThanEqualsToken or
                SyntaxKind.LessThanLessThanToken or
                SyntaxKind.GreaterThanEqualsToken or
                SyntaxKind.GreaterThanGreaterThanToken or
                SyntaxKind.EqualsEqualsToken or
                SyntaxKind.ExclamationEqualsToken or
                SyntaxKind.PlusEqualsToken or
                SyntaxKind.MinusEqualsToken or
                SyntaxKind.AsteriskEqualsToken or
                SyntaxKind.SlashEqualsToken or
                SyntaxKind.PercentEqualsToken or
                SyntaxKind.AmpersandEqualsToken or
                SyntaxKind.BarEqualsToken or
                SyntaxKind.CaretEqualsToken or
                SyntaxKind.LessThanLessThanEqualsToken or
                SyntaxKind.GreaterThanGreaterThanEqualsToken or
                SyntaxKind.QuestionQuestionEqualsToken or
                SyntaxKind.ColonColonToken or
                SyntaxKind.MinusGreaterThanToken or
                SyntaxKind.EqualsGreaterThanToken or
                SyntaxKind.GreaterThanGreaterThanGreaterThanToken or
                SyntaxKind.GreaterThanGreaterThanGreaterThanEqualsToken
                    => new SolidColorBrush(Color.FromRgb(0xD4, 0xD4, 0xD4)),

                // 括号 - 黄色 (#FFD700)
                SyntaxKind.OpenParenToken or
                SyntaxKind.CloseParenToken or
                SyntaxKind.OpenBraceToken or
                SyntaxKind.CloseBraceToken or
                SyntaxKind.OpenBracketToken or
                SyntaxKind.CloseBracketToken
                    => new SolidColorBrush(Color.FromRgb(0xFF, 0xD7, 0x00)),

                // XML相关token
                SyntaxKind.XmlCommentStartToken or
                SyntaxKind.XmlCommentEndToken or
                SyntaxKind.XmlCDataStartToken or
                SyntaxKind.XmlCDataEndToken or
                SyntaxKind.XmlProcessingInstructionStartToken or
                SyntaxKind.XmlProcessingInstructionEndToken or
                SyntaxKind.LessThanSlashToken or
                SyntaxKind.SlashGreaterThanToken or
                SyntaxKind.XmlEntityLiteralToken or
                SyntaxKind.XmlTextLiteralToken or
                SyntaxKind.XmlTextLiteralNewLineToken
                    => new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80)),

                // 其他特殊token
                SyntaxKind.UnderscoreToken or
                SyntaxKind.OmittedTypeArgumentToken or
                SyntaxKind.OmittedArraySizeExpressionToken or
                SyntaxKind.EndOfDirectiveToken or
                SyntaxKind.EndOfDocumentationCommentToken or
                SyntaxKind.EndOfFileToken
                    => Brushes.Gray,

                // 标识符
                SyntaxKind.IdentifierToken when IsTypeName(token)
                    => new SolidColorBrush(Color.FromRgb(0x4E, 0xC9, 0xB0)),

                SyntaxKind.IdentifierToken
                    => new SolidColorBrush(Color.FromRgb(0x8B, 0xDC, 0xFD)),

                _ => Brushes.White
            };
        }

        private static bool IsTypeName(SyntaxToken token)
        {
            string text = token.Text;
            return !string.IsNullOrEmpty(text) && char.IsUpper(text[0]);
        }

    }
}