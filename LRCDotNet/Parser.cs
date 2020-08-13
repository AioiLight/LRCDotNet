using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Space.AioiLight.LRCDotNet
{
    internal static class Parser
    {
        internal static string[] SplitLine(string str)
        {
            // \r\n \r を \n へ統一する
            var eol = "\n";
            var eolNormalize = str.Replace("\r\n", eol).Replace("\r", eol);
            return eolNormalize.Split(eol, StringSplitOptions.RemoveEmptyEntries);
        }

        internal static TimeSpan GetTimeSpan(string str)
        {
            // [, ] の数をカウントする
            var countStartBrackets = str.Count(c => c == '[');
            var countEndBrackets = str.Count(c => c == ']');

            // 括弧の数が多すぎたり少なすぎたりしたら例外を投げる。
            if (countStartBrackets != 1 || countEndBrackets != 1)
            {
                throw new ArgumentException();
            }

            // 括弧の数が一致しなくても例外を投げる。
            if (countStartBrackets != countEndBrackets)
            {
                throw new ArgumentException();
            }

            var timeStr = str.Substring(str.IndexOf('['), str.Length - str.IndexOf(']'));

            var result = Time.Match(timeStr);

            if (result.Success)
            {
                return new TimeSpan(0, 0, int.Parse(result.Groups["m"].Value), int.Parse(result.Groups["s"].Value), int.Parse(result.Groups["x"].Value));
            }

            throw new ArgumentException();
        }

        private static readonly Regex Time = new Regex("(?<m>dd):(?<s>dd)[:.](?<x>dd)");
    }
}
