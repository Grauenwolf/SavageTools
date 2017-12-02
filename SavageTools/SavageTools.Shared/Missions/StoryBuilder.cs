using System;
using System.Net;
using System.Text;

namespace SavageTools
{
    public class StoryBuilder
    {
        readonly MissionGeneratorSettings m_Settings;
        readonly StringBuilder m_Story = new StringBuilder();
        bool m_NeedsTab = true;

        int m_TabDepth;

        public StoryBuilder(MissionGeneratorSettings settings)
        {
            m_Settings = settings ?? throw new ArgumentNullException(nameof(settings), $"{nameof(settings)} is null.");
        }

        public MissionGeneratorSettings Settings => m_Settings;

        public void Append(string value)
        {
            if (m_Settings.UseHtml)
            {
                if (m_NeedsTab)
                    m_Story.Append($"<p style=\"text-indent: {m_TabDepth * 25}px;\">");
                m_Story.Append(WebUtility.HtmlEncode(value));
            }
            else
            {
                if (m_NeedsTab)
                    for (int i = 1; i <= m_TabDepth; i++)
                        m_Story.Append("\t");
                m_NeedsTab = false;
                m_Story.Append(value);
            }
        }

        public void AppendLine(string value)
        {
            if (m_Settings.UseHtml)
            {
                if (m_NeedsTab)
                    m_Story.Append($"<p style=\"text-indent: {m_TabDepth * 25}px;\">");
                m_Story.Append(WebUtility.HtmlEncode(value));
                m_Story.AppendLine("</p>");
            }
            else
            {
                if (m_NeedsTab)
                    for (int i = 1; i <= m_TabDepth; i++)
                        m_Story.Append("\t");
                m_NeedsTab = false;
                m_Story.AppendLine(value);
            }


            m_NeedsTab = true;
        }

        public void Dedent() => m_TabDepth -= 1;

        public void Indent() => m_TabDepth += 1;
        public override string ToString()
        {
            return m_Story.ToString();
        }
    }
}
