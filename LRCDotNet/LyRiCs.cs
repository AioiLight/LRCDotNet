using System;
using System.Collections.Generic;

namespace Space.AioiLight.LRCDotNet
{
    public class LyRiCs
    {
        public LyRiCs()
        {
            Lyrics = new List<Lyric>();
        }

        public List<Lyric> Lyrics { get; internal set; }
    }


    public class Lyric : IComparable<TimeSpan>
    {
        public Lyric(TimeSpan time, string text)
        {
            Time = time;
            Text = text;
        }

        public int CompareTo(TimeSpan other)
        {
            return Time.CompareTo(other);
        }

        public override string ToString()
        {
            var t = Time.ToString("[mm:ss:ff]");
            return $"{t}{Text}";
        }

        public TimeSpan Time { get; private set; }
        public string Text { get; private set; }
    }
}
