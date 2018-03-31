using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace YeeTech.Infrastructure.Text
{
    /// <summary>
    ///     文本筛选类型
    /// </summary>
    [Flags]
    public enum FilterType
    {
        /// <summary>
        ///     脚本
        /// </summary>
        Script = 1,

        /// <summary>
        ///     Html代码
        /// </summary>
        Html = 2,

        /// <summary>
        ///     对象
        /// </summary>
        Object = 3,

        /// <summary>
        ///     链接脚本
        /// </summary>
        AHrefScript = 4,

        /// <summary>
        ///     IFRAME
        /// </summary>
        Iframe = 5,

        /// <summary>
        ///     FRAMSET
        /// </summary>
        Frameset = 6,

        /// <summary>
        ///     SRC
        /// </summary>
        Src = 7,

        /// <summary>
        ///     脏字
        /// </summary>
        BadWords = 8,

        /// <summary>
        ///     全部
        /// </summary>
        All = 0x10
    }

    /// <summary>
    ///     文本过滤类
    /// </summary>
    public class TextFilter
    {
        //匹配回车和换行符的正则表达式
        private static readonly Regex REGEX_BR = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);

        /// <summary>
        ///     过滤链接脚本
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤链接脚本后的内容</returns>
        public static string FilterAHrefScript(string content)
        {
            var input = FilterScript(content);
            const string pattern = @" href[ ^=]*= *[\s\S]*script *:";
            return Regex.Replace(input, pattern, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     过滤全部
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>全部过滤后的内容</returns>
        public static string FilterAll(string content)
        {
            content = FilterHtml(content);
            content = FilterScript(content);
            content = FilterAHrefScript(content);
            content = FilterObject(content);
            content = FilterIframe(content);
            content = FilterFrameset(content);
            content = FilterSrc(content);
            content = FilterBadWords(content);
            return content;
        }

        /// <summary>
        ///     过滤脏字字符串
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤脏字后的内容</returns>
        private static string FilterBadWords(string content)
        {
            //脏字 - 用#号分隔
            const string badwords = "";
            if (string.IsNullOrEmpty(content)) return string.Empty;
            var badwordArray = badwords.Split('#');
            var builder = new StringBuilder();
            foreach (var bandword in badwordArray)
            {
                var pattern = bandword.Trim();
                var match = new Regex(pattern,
                    RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(content);
                if (match.Success)
                {
                    var length = match.Value.Length;
                    builder.Insert(0, "*", length);
                    var replacement = builder.ToString();
                    content = Regex.Replace(content, pattern, replacement,
                        RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                }

                builder.Remove(0, builder.Length);
            }

            return content;
        }

        /// <summary>
        ///     过滤给定字符串中的回车及换行符
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤回车和换行符后的内容</returns>
        public static string FilterBR(string content)
        {
            for (var match = REGEX_BR.Match(content); match.Success; match = match.NextMatch())
                content = content.Replace(match.Groups[0].ToString(), "");
            return content;
        }

        /// <summary>
        ///     过滤Frameset标签
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤Frameset标签后的内容</returns>
        public static string FilterFrameset(string content)
        {
            const string pattern = @"(?i)<Frameset([^>])*>(\w|\W)*</Frameset([^>])*>";
            return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     Html代码过滤
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤HTML代码后的内容</returns>
        public static string FilterHtml(string content)
        {
            var input = FilterScript(content);
            const string pattern = "<[^>]*>";
            return Regex.Replace(input, pattern, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     过滤Iframe标签
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤Iframe后的内容</returns>
        public static string FilterIframe(string content)
        {
            const string pattern = @"(?i)<Iframe([^>])*>(\w|\W)*</Iframe([^>])*>";
            return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     过滤 Object 标签
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤 Object 标签后的内容</returns>
        public static string FilterObject(string content)
        {
            var pattern = @"(?i)<Object([^>])*>(\w|\W)*</Object([^>])*>";
            return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     脚本过滤
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤脚本后的内容</returns>
        public static string FilterScript(string content)
        {
            const string str = @"(?'comment'<!--.*?--[ \n\r]*>)";
            const string str2 = @"(\/\*.*?\*\/|\/\/.*?[\n\r])";
            var str3 = string.Format(@"(?'script'<[ \n\r]*script[^>]*>(.*?{0}?)*<[ \n\r]*/script[^>]*>)", str2);
            var pattern = string.Format("(?s)({0}|{1})", str, str3);
            return StripScriptAttributesFromTags(Regex.Replace(content, pattern, string.Empty,
                RegexOptions.IgnoreCase));
        }

        /// <summary>
        ///     过滤包含 Src 的标签
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤 Src 标签后的内容</returns>
        public static string FilterSrc(string content)
        {
            var input = FilterScript(content);
            const string pattern = " src *= *['\"]?[^\\.]+\\.(js|vbs|asp|aspx|php|jsp)['\"]";
            return Regex.Replace(input, pattern, "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     从HTML中获取文本,保留br,p,img标签
        /// </summary>
        /// <param name="contentHtml">要过滤的Html文本</param>
        /// <returns>包含br,p,img标签的文本内容</returns>
        public static string FilterTextFromHTML(string contentHtml)
        {
            var regex = new Regex("</?(?!br|/?p|img)[^>]*>", RegexOptions.IgnoreCase);
            return regex.Replace(contentHtml, "");
        }

        /// <summary>
        ///     过滤字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤字符串结尾的回车/换行/空格后的内容</returns>
        public static string FilterTrim(string content)
        {
            for (var i = content.Length - 1; i >= 0; i--)
            {
                var c = content[i];
                if (c.Equals(' ') || c.Equals('\r') || c.Equals('\n')) content = content.Remove(i, 1);
            }

            return content;
        }

        /// <summary>
        ///     过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>过滤不安全标签后的内容</returns>
        public static string FilterUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2",
                RegexOptions.IgnoreCase);
            return content;
        }

        /// <summary>
        ///     过滤输入字符串为字母和数字,@,-
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FilterUserInput(string inputStr)
        {
            return Regex.Replace(inputStr.Trim(), @"[^\w\.@-]", "");
        }

        /// <summary>
        ///     过滤所有 XHTML 标签,并编码过滤后返回的字符串
        /// </summary>
        /// <param name="content">要过滤的内容</param>
        /// <returns>编码后的过滤 XHTML 内容</returns>
        public static string FilterXHtml(string content)
        {
            return FilterXHtml(content, true);
        }

        /// <summary>
        ///     过滤所有 XHTML 标签
        /// </summary>
        /// <param name="content">XHtml 内容</param>
        /// <param name="encode">是否对过滤后的内容编码</param>
        /// <returns></returns>
        public static string FilterXHtml(string content, bool encode)
        {
            if (string.IsNullOrEmpty(content)) return content;

            content = Regex.Replace(content, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "-->", "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<!--.*", "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            content.Replace("<", "");
            content.Replace(">", "");
            content.Replace("\r\n", "");
            if (encode) content = WebUtility.HtmlEncode(content).Trim();

            return content;
        }

        /// <summary>
        ///     过滤XHtml标签
        /// </summary>
        /// <param name="filterType">过滤类型</param>
        /// <param name="filterContent">要过滤内容</param>
        /// <returns>过滤后的内容</returns>
        public static string Process(FilterType filterType, string filterContent)
        {
            switch (filterType)
            {
                case FilterType.Script:
                    filterContent = FilterScript(filterContent);
                    return filterContent;

                case FilterType.Html:
                    filterContent = FilterHtml(filterContent);
                    return filterContent;

                case FilterType.Object:
                    filterContent = FilterObject(filterContent);
                    return filterContent;

                case FilterType.AHrefScript:
                    filterContent = FilterAHrefScript(filterContent);
                    return filterContent;

                case FilterType.Iframe:
                    filterContent = FilterIframe(filterContent);
                    return filterContent;

                case FilterType.Frameset:
                    filterContent = FilterFrameset(filterContent);
                    return filterContent;

                case FilterType.Src:
                    filterContent = FilterSrc(filterContent);
                    return filterContent;

                case FilterType.BadWords:
                    filterContent = FilterBadWords(filterContent);
                    return filterContent;

                case FilterType.BadWords | FilterType.Script:
                case FilterType.BadWords | FilterType.Html:
                case FilterType.BadWords | FilterType.Object:
                case FilterType.BadWords | FilterType.AHrefScript:
                case FilterType.BadWords | FilterType.Iframe:
                case FilterType.BadWords | FilterType.Frameset:
                case FilterType.BadWords | FilterType.Src:
                    return filterContent;

                case FilterType.All:
                    filterContent = FilterAll(filterContent);
                    return filterContent;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
            }
        }

        /// <summary>
        ///     去除属性 attribute
        /// </summary>
        /// <param name="m">单个正则表达式匹配的结果</param>
        /// <returns>去除后的字符串</returns>
        private static string StripAttributesHandler(Match m)
        {
            return m.Groups["attribute"].Success
                ? m.Value.Replace(m.Groups["attribute"].Value, "")
                : m.Value;
        }

        /// <summary>
        ///     去除Form标签
        /// </summary>
        /// <param name="content">要去除Form标签的内容</param>
        /// <returns>去除Form标签后的内容</returns>
        private static string StripScriptAttributesFromTags(string content)
        {
            const string str =
                "on(blur|c(hange|lick)|dblclick|focus|keypress|(key|mouse)(down|up)|(un)?load\r\n                    |mouse(move|o(ut|ver))|reset|s(elect|ubmit))";
            var regex = new Regex(string.Format(
                "(?inx)\r\n\t\t\t\t\\<(\\w+)\\s+\r\n\t\t\t\t\t(\r\n\t\t\t\t\t\t(?'attribute'\r\n\t\t\t\t\t\t(?'attributeName'{0})\\s*=\\s*\r\n\t\t\t\t\t\t(?'delim'['\"]?)\r\n\t\t\t\t\t\t(?'attributeValue'[^'\">]+)\r\n\t\t\t\t\t\t(\\3)\r\n\t\t\t\t\t)\r\n\t\t\t\t\t|\r\n\t\t\t\t\t(?'attribute'\r\n\t\t\t\t\t\t(?'attributeName'href)\\s*=\\s*\r\n\t\t\t\t\t\t(?'delim'['\"]?)\r\n\t\t\t\t\t\t(?'attributeValue'javascript[^'\">]+)\r\n\t\t\t\t\t\t(\\3)\r\n\t\t\t\t\t)\r\n\t\t\t\t\t|\r\n\t\t\t\t\t[^>]\r\n\t\t\t\t)*\r\n\t\t\t\\>",
                str));
            return regex.Replace(content, StripAttributesHandler);
        }
    }
}