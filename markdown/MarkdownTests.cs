using Xunit;

public class MarkdownTests
{
    [Theory]
    [InlineData("This will be a paragraph", "<p>This will be a paragraph</p>")]
    [InlineData("_This will be italic_", "<p><em>This will be italic</em></p>")]
    [InlineData("__This will be bold__", "<p><strong>This will be bold</strong></p>")]
    [InlineData("This will _be_ __mixed__", "<p>This will <em>be</em> <strong>mixed</strong></p>")]
    [InlineData("This is a paragraph with # and * in the text", "<p>This is a paragraph with # and * in the text</p>")]
    public void Parse_ParagraphsAndTextElements_ReturnsExpectedHtml(string markdown, string expected)
    {
        Assert.Equal(expected, Markdown.Parse(markdown));
    }

    [Theory]
    [InlineData("# This will be an h1", "<h1>This will be an h1</h1>")]
    [InlineData("## This will be an h2", "<h2>This will be an h2</h2>")]
    [InlineData("###### This will be an h6", "<h6>This will be an h6</h6>")]
    public void Parse_Headers_ReturnsExpectedHtml(string markdown, string expected)
    {
        Assert.Equal(expected, Markdown.Parse(markdown));
    }

    [Fact]
    public void Parse_UnorderedLists_ReturnsExpectedHtml()
    {
        var markdown = "* Item 1\n* Item 2";
        var expected = "<ul><li>Item 1</li><li>Item 2</li></ul>";
        Assert.Equal(expected, Markdown.Parse(markdown));
    }

    [Theory]
    [InlineData("# Header!\n* __Bold Item__\n* _Italic Item_", "<h1>Header!</h1><ul><li><strong>Bold Item</strong></li><li><em>Italic Item</em></li></ul>")]
    [InlineData("# This is a header with # and * in the text", "<h1>This is a header with # and * in the text</h1>")]
    public void Parse_ComplexMarkdown_ReturnsExpectedHtml(string markdown, string expected)
    {
        Assert.Equal(expected, Markdown.Parse(markdown));
    }

    [Fact]
    public void Parse_UnorderedListsCloseProperly_WithPrecedingAndFollowingLines()
    {
        var markdown = "# Start a list\n* Item 1\n* Item 2\nEnd a list";
        var expected = "<h1>Start a list</h1><ul><li>Item 1</li><li>Item 2</li></ul><p>End a list</p>";
        Assert.Equal(expected, Markdown.Parse(markdown));
    }
}
