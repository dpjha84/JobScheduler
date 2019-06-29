using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobScheduler.Tests
{
    [TestClass]
    public class JobSchedulerFixture
    {
        [TestMethod]
        public void Test_Null_Job_List_Returns_Empty_Sequence()
        {
            var scheduler = new JobSceduler();
            string sequence = scheduler.Schedule(null);
            Assert.IsTrue(sequence == string.Empty);
        }

        [TestMethod]
        public void Test_Empty_Job_List_Returns_Empty_Sequence()
        {
            var jobList = new List<string>();
            var scheduler = new JobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(sequence == string.Empty);
        }
    }
}
