using Ganss.XSS;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace SavageToolsWeb.Controls
{
    partial class MarkdownView
    {
        private string? _content;

        [Inject] public IHtmlSanitizer HtmlSanitizer { get; set; } = null!;

        [Parameter]
        public string? Content
        {
            get => _content;
            set
            {
                _content = value;
                HtmlContent = ConvertStringToMarkupString(_content);
            }
        }

        public MarkupString HtmlContent { get; private set; }

        private MarkupString ConvertStringToMarkupString(string? value)
        {
            if (!string.IsNullOrWhiteSpace(_content))
            {
                // Convert markdown string to HTML
                var html = Markdig.Markdown.ToHtml(value, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());

                // Sanitize HTML before rendering
                var sanitizedHtml = HtmlSanitizer.Sanitize(html);

                // Return sanitized HTML as a MarkupString that Blazor can render
                return new MarkupString(sanitizedHtml);
            }

            return new MarkupString();
        }
    }
}
