using Xunit;

namespace LoadTestToolbox.Tests
{
    public class ChartHelperTests
    {
        [Fact]
        public void CanGetMagnitudeForInt()
        {
            //arrange
            var x = 200;

            //act
            var magnitude = x.GetMagnitude();

            //assert
            Assert.Equal(100, magnitude);
        }

        [Fact]
        public void CanGetMagnitudeForDouble()
        {
            //arrange
            var x = 205.7;

            //act
            var magnitude = x.GetMagnitude();

            //assert
            Assert.Equal(100, magnitude);
        }

        [Fact]
        public void XStepSizeForSmallNumbersIsReturned()
        {
            //arrange
            var x = 499;

            //act
            var stepSize = x.GetXStepSize();

            //assert
            Assert.Equal(10, stepSize);
        }

        [Fact]
        public void XStepSizeForBigNumbersIsDoubled()
        {
            //arrange
            var x = 500;

            //act
            var stepSize = x.GetXStepSize();

            //assert
            Assert.Equal(20, stepSize);
        }

        [Fact]
        public void YStepSizeForReallySmallNumbersIsDivided()
        {
            //arrange
            var x = 150.0;

            //act
            var stepSize = x.GetYStepSize();

            //assert
            Assert.Equal(20, stepSize);
        }

        [Fact]
        public void YStepSizeForSmallNumbersIsDivided()
        {
            //arrange
            var x = 300.0;

            //act
            var stepSize = x.GetYStepSize();

            //assert
            Assert.Equal(50, stepSize);
        }

        [Fact]
        public void YStepSizeForNormalNumbersIsRetured()
        {
            //arrange
            var x = 500.0;

            //act
            var stepSize = x.GetYStepSize();

            //assert
            Assert.Equal(100, stepSize);
        }

        [Fact]
        public void YStepSizeForBigNumbersIsMultiplied()
        {
            //arrange
            var x = 900.0;

            //act
            var stepSize = x.GetYStepSize();

            //assert
            Assert.Equal(200, stepSize);
        }
    }
}