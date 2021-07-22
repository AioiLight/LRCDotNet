using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleToAttribute("LRCDotNetTest")]
namespace Space.AioiLight.LRCDotNet
{
    internal static class Parser
    {
        /// <summary>
        /// \r\n \r を \n へ統一する
        /// </summary>
        /// <param name="str">統一したい文字列。</param>
        /// <returns>統一後の文字列</returns>
        internal static string[] SplitLine(string str)
        {
            var eol = '\n';
            var eolNormalize = str.Replace("\r\n", eol.ToString()).Replace("\r", eol.ToString());
            return eolNormalize.Split(new char[] { eol }, options: StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 1行の文字列から、歌詞を生成する。
        /// </summary>
        /// <param name="str">1行の文字列。</param>
        /// <returns>歌詞。正しくパースできない場合、null を返す。</returns>
        internal static Lyric GetLyric(string str)
        {
            // タイムコードである [ ] のどちらかがなければ、歌詞として認めない。
            if (!str.Contains("[") || !str.Contains("]"))
            {
                return null;
            }

            try
            {
                var lyric = GetLyricString(str);
                var timeSpan = GetTimeSpan(GetTimeString(str));

                return new Lyric(timeSpan, lyric ?? "");
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// 歌詞を取得する。
        /// </summary>
        /// <param name="str">歌詞。</param>
        /// <returns>歌詞。ない場合nullを返す。</returns>
        internal static string GetLyricString(string str)
        {
            var lyric = Time.Replace(str.Substring(str.IndexOf('[')), "", 1);

            return !string.IsNullOrWhiteSpace(lyric) ? lyric.Trim() : null;
        }

        /// <summary>
        /// 文字列から時間を返す。
        /// </summary>
        /// <param name="str">文字列。</param>
        /// <returns>TimeSpan。</returns>
        internal static TimeSpan GetTimeSpan(string str)
        {
            var result = FullTime.Match(str);

            if (result.Success)
            {
                var m = int.Parse(result.Groups["m"].Value);
                var s = int.Parse(result.Groups["s"].Value);
                // 1/100 ミリ秒が欲しいため、桁数が足りなければ0で埋める。
                var x = int.Parse(result.Groups["x"].Value.PadRight(3, '0'));
                return new TimeSpan(0, 0, m, s, x);
            }
            else
            {
                result = ShortenTime.Match(str);
                if (result.Success)
                {
                    var m = int.Parse(result.Groups["m"].Value);
                    var s = int.Parse(result.Groups["s"].Value);
                    return new TimeSpan(0, 0, m, s);
                }
            }

            throw new FormatException();
        }

        internal static string GetTimeString(string str)
        {
            var i = str.IndexOf('[');
            return str.Substring(i, str.IndexOf(']') + 1 - i);
        }

        private static readonly Regex Time = new Regex(@"\[(?<m>\d+):(?<s>\d+)([:.](?<x>\d+))*\]");
        private static readonly Regex FullTime = new Regex(@"\[(?<m>\d+):(?<s>\d+)[:.](?<x>\d+)\]");
        private static readonly Regex ShortenTime = new Regex(@"\[(?<m>\d+):(?<s>\d+)\]");
    }
}
