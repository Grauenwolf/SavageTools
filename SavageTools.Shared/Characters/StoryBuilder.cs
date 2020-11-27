using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Tortuga.Anchor.Modeling;

namespace SavageTools.Characters
{
    public class StoryBuilder
    {
        readonly MissionOptions m_Settings;
        readonly StringBuilder m_Story = new StringBuilder();
        bool m_NeedsTab = true;

        int m_TabDepth;

        //TOOD: Add Markdown options

        public StoryBuilder(bool useHtml) : this(new MissionOptions() { UseHtml = useHtml })
        {
        }

        public StoryBuilder(MissionOptions? settings = null)
        {
            m_Settings = settings ?? new MissionOptions();
        }

        public MissionOptions Settings => m_Settings;

        public void Append(string value)
        {
            if (m_Settings.UseHtml)
            {
                if (m_NeedsTab)
                    m_Story.Append($"<p style=\"text-indent: {m_TabDepth * 25}px;\">");
                m_Story.Append(WebUtility.HtmlEncode(value));
                m_NeedsTab = false;
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

        public override string ToString()
        {
            return m_Story.ToString();
        }

        public void IncreaseIndent()
        {
            m_TabDepth += 1;
        }

        public void DecreaseIndent()
        {
            m_TabDepth -= 1;
        }

        public IDisposable Indent()
        {
            m_TabDepth += 1;
            return new IndentToken(this);
        }

        public void AppendLine()
        {
            AppendLine("");
        }

        class IndentToken : IDisposable
        {
            readonly StoryBuilder m_StoryBuilder;

            public IndentToken(StoryBuilder storyBuilder)
            {
                m_StoryBuilder = storyBuilder;
            }

            public void Dispose()
            {
                m_StoryBuilder.m_TabDepth -= 1;
            }
        }
    }
}
