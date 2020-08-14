using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleToAttribute("LRCDotNetTest")]
namespace Space.AioiLight.LRCDotNet
{
    internal static class Parser
    {
        internal static string[] SplitLine(string str)
        {
            // \r\n \r を \n へ統一する
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

        internal static TimeSpan GetTimeSpan(string str)
        {
            var result = FullTime.Match(str);

            if (result.Success)
            {
                return new TimeSpan(0, 0, int.Parse(result.Groups["m"].Value), int.Parse(result.Groups["s"].Value), int.Parse(result.Groups["x"].Value));
            }
            else
            {
                result = ShortenTime.Match(str);
                if (result.Success)
                {
                    return new TimeSpan(0, 0, int.Parse(result.Groups["m"].Value), int.Parse(result.Groups["s"].Value));
                }
            }

            throw new FormatException();
        }

        internal static string GetTimesString(string str)
        {
            var index = str.IndexOf('[');
            return str.Substring(index, str.LastIndexOf(']') + 1 - index);
        }

        private static readonly Regex FullTime = new Regex(@"\[(?<m>\d\d):(?<s>\d\d)[:.](?<x>\d\d)\]");
        private static readonly Regex ShortenTime = new Regex(@"\[(?<m>\d\d):(?<s>\d\d)\]");
    }
}
