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
                list.AddRange(Parser.GetLyrics(line));
            }

            list.Sort();

            return result;
        }
    }
}
