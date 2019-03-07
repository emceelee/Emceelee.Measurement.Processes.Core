using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emceelee.Measurement.Summarization.Test
{
    [TestClass]
    public class InitializeAndCleanup
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext tc)
        {
            TestData.Initialize();
        }
    }
}
