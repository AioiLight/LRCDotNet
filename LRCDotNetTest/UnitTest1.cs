using System.IO;
using Space.AioiLight.LRCDotNet;
using Xunit;

namespace LRCDotNetTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("[01:02:22]�̎�", "�̎�")]
        [InlineData("[01:02:22][01:02:22]�̎�", "�̎�")]
        public void GetText(string x, string y)
        {
            Parser.GetText(x).Is(y);
        }

        [Theory]
        [InlineData("[01:02:22]�̎�", "[01:02:22]")]
        [InlineData("[01:02:22][01:03:22]�̎�", "[01:02:22][01:03:22]")]
        [InlineData("����[01:02:22][01:03:22]�̎�", "[01:02:22][01:03:22]")]
        public void GetTimesString(string x, string y)
        {
            Parser.GetTimesString(x).Is(y);
        }
    }
}
