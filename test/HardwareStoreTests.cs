using System.Collections.Generic;
using Xunit;

namespace LoadTestToolbox.Tests
{
    public class HardwareStoreTests
    {
        [Theory]
        [InlineData(1, 1, new[] { 1 })]
        [InlineData(1, 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [InlineData(10, 10, new[] { 10 })]
        [InlineData(10, 100, new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 })]
        [InlineData(10, 95, new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 95 })]
        [InlineData(5, 500, new[] { 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500 })]
        public void GetsCorrectHammers(int min, int max, IEnumerable<int> expected)
        {
            Assert.Equal(expected, HardwareStore.GetHammers(min, max));
        }
    }
}