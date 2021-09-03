﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LoadTestToolbox.Tests
{
	public class DrillTests
	{
		[Fact]
		public async Task NumberOfResultsMatchRequests()
		{
			//arrange
			var http = new HttpClient(new MockHttpMessageHandler());
			var drill = new Drill(http, new Uri("http://localhost"), 5, 0);

			//act
			await drill.Run();

			//assert
			Assert.Equal(5, drill.Results.Count);
		}
	}
}