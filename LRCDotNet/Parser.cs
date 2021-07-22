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

        internal static Lyric[] GetLyrics(string str)
        {
            if (!str.Contains("[") || !str.Contains("]"))
            {
                return null;
            }

            var eol = '\n';
            var text = GetText(str);
            // ] → ]\n して時間を分ける
            var times = GetTimesString(str).Replace("]", $"]{eol}").Split(new char[] { eol }, options: StringSplitOptions.RemoveEmptyEntries);

            var timespans = GetTimeSpans(times);

            var results = new Lyric[timespans.Length];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new Lyric(timespans[i], text);
            }

            return results;
        }

        internal static TimeSpan[] GetTimeSpans(string[] times)
        {
            var results = new List<TimeSpan>(times.Length);

            for (int i = 0; i < times.Length; i++)
            {
                try
                {
                    results.Add(GetTimeSpan(times[i]));
                }
                catch (Exception)
                {
                    // なにもしない。
                }
            }

            return results.ToArray();
        }

        internal static string GetText(string str)
        {
            var i = str.LastIndexOf(']');

            if (i < 0)
            {
                throw new FormatException();
            }

            if (str.Length <= i + 1)
            {
                throw new FormatException();
            }

            return str.Substring(i + 1);
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

        internal static string GetTimesString(string str)
        {
            var index = str.IndexOf('[');
            return str.Substring(index, str.LastIndexOf(']') + 1 - index);
        }

        private static readonly Regex FullTime = new Regex(@"\[(?<m>\d+):(?<s>\d+)[:.](?<x>\d+)\]");
        private static readonly Regex ShortenTime = new Regex(@"\[(?<m>\d+):(?<s>\d+)\]");
    }
}
