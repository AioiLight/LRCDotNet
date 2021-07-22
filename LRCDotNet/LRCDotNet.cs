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
                var l = Parser.GetLyric(line);
                if (l != null)
                {
                    list.Add(l);
                }
            }

            list.Sort((x, y) => x.CompareTo(y.Time));

            return result;
        }
    }
}
