namespace Space.AioiLight.LRCDotNet
{
    public static class LRCDotNet
    {
        public static LyRiCs Parse(string lrc)
        {
            var lines = Parser.SplitLine(lrc);          

            var result = new LyRiCs();

            var list = result.Lyrics;

            foreach (var line in lines)
            {
                var l = Parser.GetLyrics(line);
                if (l != null)
                {
                    list.AddRange(l);
                }
            }

            list.Sort((x, y) => x.CompareTo(y.Time));

            return result;
        }
    }
}
