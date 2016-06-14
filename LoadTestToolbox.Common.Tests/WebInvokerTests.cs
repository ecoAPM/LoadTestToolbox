using LoadTestToolbox.Common.Web;
using Moq;
using NUnit.Framework;
using System;

namespace LoadTestToolbox.Common.Tests
{
	[TestFixture]
	public class WebInvokerTests
	{
		[Test]
		public void TestInvokeCallsUrlOnClient() 
		{
			Mock<IWebClient> webClient = new Mock<IWebClient>();
			webClient.Setup(client => client.DownloadString(It.IsAny<Uri>()));

			WebInvoker invoker = new WebInvoker(new Uri("http://www.google.com"), webClient.Object);

			Assert.DoesNotThrow(() => invoker.Invoke());
			webClient.Verify(client => client.DownloadString(It.IsAny<Uri>()));
		}
	}
}
