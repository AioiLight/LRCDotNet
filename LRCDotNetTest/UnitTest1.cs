using Space.AioiLight.LRCDotNet;
using Xunit;

namespace LRCDotNetTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("[01:02:22]‰ÌŽŒ", "‰ÌŽŒ")]
        [InlineData("[01:02:22][01:02:22]‰ÌŽŒ", "[01:02:22]‰ÌŽŒ")]
        public void GetText(string x, string y)
        {
            Parser.GetLyricString(x).Is(y);
        }

        [Theory]
        [InlineData("[01:02:22]‰ÌŽŒ", "[01:02:22]")]
        [InlineData("[01:02.22]‰ÌŽŒ", "[01:02.22]")]
        [InlineData("[01:02:22][01:03:22]‰ÌŽŒ", "[01:02:22]")]
        [InlineData("–³Ž‹[01:02:22][01:03:22]‰ÌŽŒ", "[01:02:22]")]
        public void GetTimeString(string x, string y)
        {
            Parser.GetTimeString(x).Is(y);
        }
    }
}
