using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LoadTestToolbox.Common.Tests
{
    [TestClass()]
    public class WorkerTests
    {
        private double duration = -1;

        [TestMethod(), TestCategory("Integration")]
        public void RunTest()
        {
            var uri = new Uri("http://www.google.com");
            var sut = new Worker(uri);
            sut.OnComplete += Sut_OnComplete;
            sut.Run();
            var deadline = DateTime.UtcNow.AddSeconds(30);

            while (duration == -1 && DateTime.UtcNow < deadline)
            {
                System.Threading.Thread.Sleep(0);
            }

            Assert.IsTrue(duration != -1);
            Console.WriteLine("Worker retrieved {0} in {1}ms", uri, duration);
        }

        private void Sut_OnComplete(object sender, EventArgs e)
        {
            duration = (double)sender;
        }
    }
}