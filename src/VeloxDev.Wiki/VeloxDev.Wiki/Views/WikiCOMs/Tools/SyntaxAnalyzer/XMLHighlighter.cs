using System.Collections.Generic;
using Avalonia.Controls.Documents;
using Avalonia.Media;

namespace VeloxDev.Wiki.Views.WikiCOMs.Tools.SyntaxAnalyzer
{
    public class XmlHighlighter : ISyntaxHighlighter
    {
        public IEnumerable<Inline> Highlight(string code)
        {
            return ParseXmlWithHighlighting(code);
        }

        private static IEnumerable<Inline> ParseXmlWithHighlighting(string xmlCode)
        {
            int position = 0;
            int length = xmlCode.Length;
            
            while (position < length)
            {
                // 检查当前位置的字符
                if (position + 4 <= length && xmlCode.Substring(position, 4) == "<!--")
                {
                    // 注释
                    var (text, endPos) = ParseUntil(xmlCode, position, "-->");
                    if (endPos > position)
                    {
                        yield return CreateRun(text, XmlTokenType.Comment);
                        position = endPos;
                    }
                    else
                    {
                        yield return CreateRun(xmlCode.Substring(position), XmlTokenType.Error);
                        position = length;
                    }
                }
                else if (position + 9 <= length && xmlCode.Substring(position, 9) == "<![CDATA[")
                {
                    // CDATA
                    var (text, endPos) = ParseUntil(xmlCode, position, "]]>");
                    if (endPos > position)
                    {
                        yield return CreateRun(text, XmlTokenType.CData);
                        position = endPos;
                    }
                    else
                    {
                        yield return CreateRun(xmlCode.Substring(position), XmlTokenType.Error);
                        position = length;
                    }
                }
                else if (position + 2 <= length && xmlCode[position] == '<' && xmlCode[position + 1] == '?')
                {
                    // 处理指令
                    var (text, endPos) = ParseUntil(xmlCode, position, "?>");
                    if (endPos > position)
                    {
                        yield return CreateRun(text, XmlTokenType.ProcessingInstruction);
                        position = endPos;
                    }
                    else
                    {
                        yield return CreateRun(xmlCode.Substring(position), XmlTokenType.Error);
                        position = length;
                    }
                }
                else if (position + 2 <= length && xmlCode[position] == '<' && xmlCode[position + 1] == '!')
                {
                    // 声明
                    var (text, endPos) = ParseUntilChar(xmlCode, position, '>');
                    if (endPos > position)
                    {
                        yield return CreateRun(text, XmlTokenType.Declaration);
                        position = endPos;
                    }
                    else
                    {
                        yield return CreateRun(xmlCode.Substring(position), XmlTokenType.Error);
                        position = length;
                    }
                }
                else if (xmlCode[position] == '<')
                {
                    // 标签
                    var tagResult = ParseTag(xmlCode, position);
                    foreach (var inline in tagResult.Inlines)
                    {
                        yield return inline;
                    }
                    position = tagResult.EndPosition;
                }
                else
                {
                    // 普通文本
                    int textStart = position;
                    while (position < length && xmlCode[position] != '<')
                    {
                        position++;
                    }
                    
                    if (position > textStart)
                    {
                        string text = xmlCode.Substring(textStart, position - textStart);
                        yield return CreateRun(text, XmlTokenType.Text);
                    }
                }
            }
        }

        private static (List<Inline> Inlines, int EndPosition) ParseTag(string xmlCode, int startPosition)
        {
            var inlines = new List<Inline>();
            int position = startPosition;
            int length = xmlCode.Length;
            
            if (position >= length || xmlCode[position] != '<')
            {
                return (inlines, position);
            }
            
            // 标签开始
            inlines.Add(CreateRun("<", XmlTokenType.TagSymbol));
            position++;
            if (position < length && xmlCode[position] == '/')
            {
                // 检查是否是结束标签
                inlines.Add(CreateRun("/", XmlTokenType.TagSymbol));
                position++;
            }
            
            // 跳过标签前的空白
            SkipWhitespace(xmlCode, ref position);
            
            // 解析标签名
            int tagNameStart = position;
            while (position < length && 
                   !char.IsWhiteSpace(xmlCode[position]) && 
                   xmlCode[position] != '>' && 
                   xmlCode[position] != '/')
            {
                position++;
            }
            
            if (tagNameStart < position)
            {
                string tagName = xmlCode.Substring(tagNameStart, position - tagNameStart);
                inlines.Add(CreateRun(tagName, XmlTokenType.TagName));
            }
            
            // 解析属性和标签内容
            bool insideTag = true;
            while (insideTag && position < length)
            {
                char currentChar = xmlCode[position];
                
                if (char.IsWhiteSpace(currentChar))
                {
                    // 添加空白字符
                    int wsStart = position;
                    while (position < length && char.IsWhiteSpace(xmlCode[position]))
                    {
                        position++;
                    }
                    if (wsStart < position)
                    {
                        string whitespace = xmlCode.Substring(wsStart, position - wsStart);
                        inlines.Add(CreateRun(whitespace, XmlTokenType.Text));
                    }
                }
                else if (currentChar == '>')
                {
                    // 标签结束
                    inlines.Add(CreateRun(">", XmlTokenType.TagSymbol));
                    position++;
                    insideTag = false;
                }
                else if (currentChar == '/' && position + 1 < length && xmlCode[position + 1] == '>')
                {
                    // 自闭合标签
                    inlines.Add(CreateRun("/>", XmlTokenType.TagSymbol));
                    position += 2;
                    insideTag = false;
                }
                else
                {
                    // 解析属性
                    var attrResult = ParseAttribute(xmlCode, position);
                    inlines.AddRange(attrResult.Inlines);
                    position = attrResult.EndPosition;
                }
            }
            
            return (inlines, position);
        }

        private static (List<Inline> Inlines, int EndPosition) ParseAttribute(string xmlCode, int startPosition)
        {
            var inlines = new List<Inline>();
            int position = startPosition;
            int length = xmlCode.Length;
            
            // 跳过属性前的空白
            SkipWhitespace(xmlCode, ref position);
            
            // 解析属性名
            int attrNameStart = position;
            while (position < length && 
                   !char.IsWhiteSpace(xmlCode[position]) && 
                   xmlCode[position] != '=' && 
                   xmlCode[position] != '>' && 
                   xmlCode[position] != '/')
            {
                position++;
            }
            
            if (attrNameStart < position)
            {
                string attrName = xmlCode.Substring(attrNameStart, position - attrNameStart);
                inlines.Add(CreateRun(attrName, XmlTokenType.AttributeName));
            }
            
            // 跳过等号前的空白
            SkipWhitespace(xmlCode, ref position);
            
            if (position < length && xmlCode[position] == '=')
            {
                inlines.Add(CreateRun("=", XmlTokenType.TagSymbol));
                position++;
                
                // 跳过等号后的空白
                SkipWhitespace(xmlCode, ref position);
                
                if (position < length)
                {
                    // 解析属性值
                    char quoteChar = xmlCode[position];
                    if (quoteChar == '"' || quoteChar == '\'')
                    {
                        // 有引号的属性值
                        inlines.Add(CreateRun(quoteChar.ToString(), XmlTokenType.TagSymbol));
                        position++;
                        
                        int valueStart = position;
                        while (position < length && xmlCode[position] != quoteChar)
                        {
                            position++;
                        }
                        
                        if (position < length)
                        {
                            string attrValue = xmlCode.Substring(valueStart, position - valueStart);
                            inlines.Add(CreateRun(attrValue, XmlTokenType.AttributeValue));
                            
                            inlines.Add(CreateRun(quoteChar.ToString(), XmlTokenType.TagSymbol));
                            position++;
                        }
                    }
                    else
                    {
                        // 无引号的属性值
                        int valueStart = position;
                        while (position < length && 
                               !char.IsWhiteSpace(xmlCode[position]) && 
                               xmlCode[position] != '>' && 
                               xmlCode[position] != '/')
                        {
                            position++;
                        }
                        
                        if (valueStart < position)
                        {
                            string attrValue = xmlCode.Substring(valueStart, position - valueStart);
                            inlines.Add(CreateRun(attrValue, XmlTokenType.AttributeValue));
                        }
                    }
                }
            }
            
            return (inlines, position);
        }

        private static (string Text, int EndPosition) ParseUntil(string xmlCode, int startPosition, string endMarker)
        {
            int endPos = xmlCode.IndexOf(endMarker, startPosition);
            if (endPos >= 0)
            {
                endPos += endMarker.Length;
                string text = xmlCode.Substring(startPosition, endPos - startPosition);
                return (text, endPos);
            }
            return ("", startPosition);
        }

        private static (string Text, int EndPosition) ParseUntilChar(string xmlCode, int startPosition, char endChar)
        {
            int endPos = xmlCode.IndexOf(endChar, startPosition);
            if (endPos >= 0)
            {
                endPos++;
                string text = xmlCode.Substring(startPosition, endPos - startPosition);
                return (text, endPos);
            }
            return ("", startPosition);
        }

        private static void SkipWhitespace(string xmlCode, ref int position)
        {
            int length = xmlCode.Length;
            while (position < length && char.IsWhiteSpace(xmlCode[position]))
            {
                position++;
            }
        }

        private static Run CreateRun(string text, XmlTokenType type)
        {
            var brush = type switch
            {
                XmlTokenType.TagName => new SolidColorBrush(Color.FromRgb(0x56, 0x9C, 0xD6)),        // 蓝色
                XmlTokenType.AttributeName => new SolidColorBrush(Color.FromRgb(0x9C, 0xDC, 0xFE)),   // 青色
                XmlTokenType.AttributeValue => new SolidColorBrush(Color.FromRgb(0xCE, 0x91, 0x78)),  // 橙色
                XmlTokenType.TagSymbol => new SolidColorBrush(Colors.Gray),                          // 灰色
                XmlTokenType.Text => new SolidColorBrush(Color.FromRgb(0xD4, 0xD4, 0xD4)),            // 浅灰色
                XmlTokenType.Comment => new SolidColorBrush(Color.FromRgb(0x6A, 0x99, 0x55)),         // 绿色
                XmlTokenType.CData => new SolidColorBrush(Color.FromRgb(0x60, 0x8B, 0x4E)),           // 深绿色
                XmlTokenType.ProcessingInstruction => new SolidColorBrush(Color.FromRgb(0xC5, 0x86, 0xC0)), // 紫色
                XmlTokenType.Declaration => new SolidColorBrush(Color.FromRgb(0x4F, 0xC1, 0xFF)),     // 浅蓝色
                XmlTokenType.Error => new SolidColorBrush(Color.FromRgb(0xF4, 0x47, 0x47)),           // 红色
                _ => new SolidColorBrush(Colors.White)
            };

            
            return new Run(text) { Foreground = brush };
        }
    }

    public enum XmlTokenType
    {
        TagName,
        AttributeName,
        AttributeValue,
        TagSymbol,
        Text,
        Comment,
        CData,
        ProcessingInstruction,
        Declaration,
        Error
    }
}