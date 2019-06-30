using System;
using System.Collections.Generic;
using JobScheduler.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobScheduler.Tests
{
    /// <summary>
    /// Fixture class for Job Scheduler Tests
    /// </summary>
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

        [TestMethod]
        public void Test_Job_List_With_Single_Job_And_No_Dependency_Returns_Single_Job()
        {
            var jobList = new List<string>
            {
                "a =>"
            };
            var scheduler = new JobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.AreEqual(sequence, "a");
        }

        [TestMethod]
        public void Test_Job_List_With_Multiple_Jobs_And_No_Dependency_Returns_Correct_Sequence()
        {
            var jobList = new List<string>
            {
                "a =>",
                "b =>",
                "c =>"
            };
            var scheduler = new JobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(sequence.Length == 3 &&
                sequence.Contains("a") &&
                sequence.Contains("b") &&
                sequence.Contains("c")
                );
        }

        [TestMethod]
        public void Test_Job_List_With_Multiple_Jobs_And_Single_Dependency_Returns_Correct_Sequence()
        {
            var jobList = new List<string>
            {
                "a =>",
                "b => c",
                "c =>"
            };
            var scheduler = new JobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(sequence.Length == 3 &&
                sequence.Contains("a") &&
                sequence.IndexOf("b") > sequence.IndexOf("c")
                );
        }

        [TestMethod]
        public void Test_Job_List_With_Multiple_Jobs_And_Multiple_Dependencies_Returns_Correct_Sequence()
        {
            var jobList = new List<string>
            {
                "a =>",
                "b => c",
                "c => f",
                "d => a",
                "e => b",
                "f =>"
            };
            var scheduler = new JobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(
                sequence.IndexOf('b') > sequence.IndexOf('c') &&
                sequence.IndexOf('c') > sequence.IndexOf('f') &&
                sequence.IndexOf('d') > sequence.IndexOf('a') &&
                sequence.IndexOf('e') > sequence.IndexOf('b')
                );
        }

        [TestMethod]
        public void Test_Job_List_With_Single_Self_Dependency_Throws_Exception()
        {
            var jobList = new List<string>
            {
                "a =>",
                "b =>",
                "c => c"
            };
            var scheduler = new JobSceduler();
            Assert.ThrowsException<SelfDependencyException>(() =>
                scheduler.Schedule(jobList)
            );
        }

        [TestMethod]
        public void Test_Job_List_With_Multiple_Self_Dependencies_Throws_Exception()
        {
            var jobList = new List<string>
            {
                "a =>",
                "b => b",
                "c => c"
            };
            var scheduler = new JobSceduler();
            Assert.ThrowsException<SelfDependencyException>(() =>
                scheduler.Schedule(jobList)
            );
        }

        [TestMethod]
        public void Test_Job_List_With_Single_Cycle_Throws_Exception()
        {
            var jobList = new List<string>
            {
                "a =>",
                "b => c",
                "c => f",
                "d => a",
                "e =>",
                "f => b"
            };
            var scheduler = new JobSceduler();
            Assert.ThrowsException<CyclicDependencyException>(() =>
                scheduler.Schedule(jobList)
            );
        }

        [TestMethod]
        public void Test_Job_List_With_Multiple_Cycles_Throws_Exception()
        {
            var jobList = new List<string>
            {
                "a => d",
                "b => c",
                "c => f",
                "d => a",
                "e =>",
                "f => b"
            };
            var scheduler = new JobSceduler();
            Assert.ThrowsException<CyclicDependencyException>(() =>
                scheduler.Schedule(jobList)
            );
        }
    }
}
