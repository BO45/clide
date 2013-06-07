using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Clide
{
	[TestClass]
	public class DevEnvSpec : VsHostedSpec
	{
		internal static readonly IAssertion Assert = new Assertion();

        [HostType("VS IDE")]
        [TestMethod]
        public void when_getting_global_devenv_then_succeeds()
        {
            var dev = DevEnv.Get(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider);

            Assert.NotNull(dev);
        }

		[HostType("VS IDE")]
		[TestMethod]
		public void WhenEnvironmentInitialized_ThenRaisesInitializedEvent()
		{
            var devenv = ServiceLocator.GetInstance<IDevEnv>();

			var called = false;

			devenv.Initialized += (sender, args) => called = true;

			var maxWait = DateTime.Now.AddSeconds(5);
			while (!devenv.IsInitialized && DateTime.Now < maxWait)
			{
				Thread.Sleep(50);
			}

			Assert.True(devenv.IsInitialized);
			Assert.True(called);
		}
	}
}
