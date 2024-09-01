using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class Markdown
{
    private static readonly Dictionary<string, string> TagMap = new()
    {
        { "__", "strong" },
        { "_", "em" }
    };

    private static string WrapWithTag(string text, string tag) => $"<{tag}>{text}</{tag}>";

    private static string ReplaceMarkdownWithHtml(string markdown, string delimiter, string tag)
    {
        string pattern = $"{Regex.Escape(delimiter)}(.+?){Regex.Escape(delimiter)}";
        return Regex.Replace(markdown, pattern, match => WrapWithTag(match.Groups[1].Value, tag));
    }

    private static string ParseTextElements(string markdown)
    {
        foreach (var tag in TagMap)
        {
            markdown = ReplaceMarkdownWithHtml(markdown, tag.Key, tag.Value);
        }
        return markdown;
    }

    private static string ParseLine(string line, ref bool inList)
    {
        if (line.StartsWith("#"))
        {
            if (inList)
            {
                inList = false;
                return $"</ul>{WrapWithTag(line.TrimStart('#').Trim(), $"h{line.TakeWhile(c => c == '#').Count()}")}";
            }
            return WrapWithTag(line.TrimStart('#').Trim(), $"h{line.TakeWhile(c => c == '#').Count()}");
        }

        if (line.StartsWith("*"))
        {
            string listItem = WrapWithTag(ParseTextElements(line[2..].Trim()), "li");
            if (inList)
            {
                return listItem;
            }
            else
            {
                inList = true;
                return $"<ul>{listItem}";
            }
        }

        if (inList)
        {
            inList = false;
            return $"</ul>{WrapWithTag(ParseTextElements(line), "p")}";
        }

        return WrapWithTag(ParseTextElements(line), "p");
    }

    public static string Parse(string markdown)
    {
        var lines = markdown.Split('\n');
        bool inList = false;
        string result = "";

        foreach (var line in lines)
        {
            string parsedLine = ParseLine(line.Trim(), ref inList);
            result += parsedLine;
        }

        return inList ? result + "</ul>" : result;
    }
}
