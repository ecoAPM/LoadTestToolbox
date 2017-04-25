using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NSubstitute;
using Xunit;

namespace LoadTestToolbox.Tests
{
    public class ChartHelperTests
    {
        [Fact]
        public void CanGetMagnitudeForInt()
        {
            //arrange
            var max = 200;

            //act
            var magnitude = max.GetMagnitude();

            //assert
            Assert.Equal(100, magnitude);
        }

        [Fact]
        public void CanGetMagnitudeForDouble()
        {
            //arrange
            var max = 205.7;

            //act
            var magnitude = max.GetMagnitude();

            //assert
            Assert.Equal(100, magnitude);
        }

        [Fact]
        public void XStepSizeForSmallNumbersIsReturned()
        {
            //arrange
            var max = 499;

            //act
            var stepSize = max.GetXStepSize();

            //assert
            Assert.Equal(10, stepSize);
        }

        [Fact]
        public void XStepSizeForBigNumbersIsDoubled()
        {
            //arrange
            var max = 500;

            //act
            var stepSize = max.GetXStepSize();

            //assert
            Assert.Equal(20, stepSize);
        }

        [Fact]
        public void YStepSizeForReallySmallNumbersIsDivided()
        {
            //arrange
            var max = 150.0;

            //act
            var stepSize = max.GetYStepSize();

            //assert
            Assert.Equal(20, stepSize);
        }

        [Fact]
        public void YStepSizeForSmallNumbersIsDivided()
        {
            //arrange
            var max = 300.0;

            //act
            var stepSize = max.GetYStepSize();

            //assert
            Assert.Equal(50, stepSize);
        }

        [Fact]
        public void YStepSizeForNormalNumbersIsRetured()
        {
            //arrange
            var max = 500.0;

            //act
            var stepSize = max.GetYStepSize();

            //assert
            Assert.Equal(100, stepSize);
        }

        [Fact]
        public void YStepSizeForBigNumbersIsMultiplied()
        {
            //arrange
            var max = 900.0;

            //act
            var stepSize = max.GetYStepSize();

            //assert
            Assert.Equal(200, stepSize);
        }

        [Fact]
        public void CanGetYAxisMaxForMaxValue()
        {
            //arrange
            var max = 900.0;

            //act
            var axisMax = max.GetYAxisMax();

            //assert
            Assert.Equal(1000, axisMax);
        }

        [Fact]
        public void CanGetYAxisMaxForResults()
        {
            //arrange
            var results = new Dictionary<int, double> { { 1, 750 }};

            //act
            var axisMax = results.GetYAxisMax();

            //assert
            Assert.Equal(800, axisMax);
        }

        [Fact]
        public void CanGetImageData()
        {
            //arrange
            var visualizer = Substitute.For<IVisualizer>();
            visualizer.GetChart(Arg.Any<IDictionary<int,double>>()).Returns("data:image/png;base64," + Convert.ToBase64String(Encoding.UTF8.GetBytes("x123")));

            //act
            var imageData = visualizer.GetImageData(null);

            //assert
            Assert.Equal("x123", Encoding.UTF8.GetString(imageData));
        }

        [Fact]
        public void CanSaveChartToStream()
        {
            //arrange
            var outputStream = new MemoryStream();
            var visualizer = Substitute.For<IVisualizer>();
            visualizer.GetChart(Arg.Any<IDictionary<int,double>>()).Returns("data:image/png;base64," + Convert.ToBase64String(Encoding.UTF8.GetBytes("x123")));

            //act
            visualizer.SaveChart(null, outputStream);

            //assert
            Assert.Equal("x123", Encoding.UTF8.GetString(outputStream.ToArray()));
        }
    }
}