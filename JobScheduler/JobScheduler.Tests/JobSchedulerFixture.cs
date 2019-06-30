using System;
using System.Collections.Generic;
using JobScheduler.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using JobScheduler.Validators;

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
            var scheduler = GetJobSceduler();
            string sequence = scheduler.Schedule(null);
            Assert.IsTrue(sequence == string.Empty);
        }

        [TestMethod]
        public void Test_Empty_Job_List_Returns_Empty_Sequence()
        {
            var jobList = new List<string>();
            var scheduler = GetJobSceduler();
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
            var scheduler = GetJobSceduler();
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
            var scheduler = GetJobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(AreAnagrams("abc", sequence));
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
            var scheduler = GetJobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(AreAnagrams("abc", sequence) &&
                sequence.IndexOf("b") > sequence.IndexOf("c")
                );
        }

        

        [TestMethod]
        public void Test_Job_List_With_Multiple_Jobs_And_Duplications_And_Single_Dependency_Returns_Correct_Sequence()
        {
            var jobList = new List<string>
            {
                "a =>",
                "b => c",
                "c =>",
                "a =>",
                "b => c",
                "c =>"
            };
            var scheduler = GetJobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(AreAnagrams("abc", sequence) &&
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
            var scheduler = GetJobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(
                AreAnagrams("abcdef", sequence) &&
                sequence.IndexOf('b') > sequence.IndexOf('c') &&
                sequence.IndexOf('c') > sequence.IndexOf('f') &&
                sequence.IndexOf('d') > sequence.IndexOf('a') &&
                sequence.IndexOf('e') > sequence.IndexOf('b')
                );
        }

        [TestMethod]
        public void Test_Large_Job_List_With_Multiple_Jobs_And_Multiple_Dependencies_Returns_Correct_Sequence()
        {
            var jobList = new List<string>
            {
                "a =>",
                "b => c",
                "c => f",
                "d => a",
                "e => b",
                "f =>",
                "p =>",
                "q => b",
                "r => z",
                "z => x",
                "x => y",
                "m => y",
                "n =>",
                "o => b",
                "x => n",
                "q => c",
                "q => f",
                "z => p",
            };
            var scheduler = GetJobSceduler();
            string sequence = scheduler.Schedule(jobList);
            Assert.IsTrue(
                AreAnagrams("abcdefmnopqrxyz", sequence) &&
                sequence.IndexOf('b') > sequence.IndexOf('c') &&
                sequence.IndexOf('c') > sequence.IndexOf('f') &&
                sequence.IndexOf('d') > sequence.IndexOf('a') &&
                sequence.IndexOf('e') > sequence.IndexOf('b') &&
                sequence.IndexOf('q') > sequence.IndexOf('b') &&
                sequence.IndexOf('r') > sequence.IndexOf('z') &&
                sequence.IndexOf('z') > sequence.IndexOf('x') &&
                sequence.IndexOf('x') > sequence.IndexOf('y') &&
                sequence.IndexOf('m') > sequence.IndexOf('y') &&
                sequence.IndexOf('o') > sequence.IndexOf('b') &&
                sequence.IndexOf('x') > sequence.IndexOf('n') &&
                sequence.IndexOf('q') > sequence.IndexOf('c') &&
                sequence.IndexOf('q') > sequence.IndexOf('f') &&
                sequence.IndexOf('z') > sequence.IndexOf('p')
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
            var scheduler = GetJobSceduler();
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
            var scheduler = GetJobSceduler();
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
            var scheduler = GetJobSceduler();
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
            var scheduler = GetJobSceduler();
            Assert.ThrowsException<CyclicDependencyException>(() =>
                scheduler.Schedule(jobList)
            );
        }

        [TestMethod]
        public void Test_Job_List_With_Multi_Char_Job_Name_Throws_Exception()
        {
            var jobList = new List<string>
            {
                "ab => 12",
                "b => c",
                "c => f"
            };
            var scheduler = GetJobSceduler();
            Assert.ThrowsException<InvalidJobNameException>(() =>
                scheduler.Schedule(jobList)
            );
        }

        [TestMethod]
        public void Test_Job_List_With_Empty_Main_Job_Name_Throws_Exception()
        {
            var jobList = new List<string>
            {
                " => 1",
                "b => c",
                "c => f"
            };
            var scheduler = GetJobSceduler();
            Assert.ThrowsException<InvalidJobNameException>(() =>
                scheduler.Schedule(jobList)
            );
        }

        [TestMethod]
        public void Test_Job_List_With_Invalid_Format_Throws_Exception()
        {
            var jobList = new List<string>
            {
                "xyz" // no "=>"
            };
            var scheduler = GetJobSceduler();
            Assert.ThrowsException<InvalidOperationException>(() =>
                scheduler.Schedule(jobList)
            );
        }

        private bool AreAnagrams(string a, string b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            return string.Concat(a.OrderBy(c => c)) == String.Concat(b.OrderBy(c => c));
        }

        private JobSceduler GetJobSceduler()
        {
            return new JobSceduler(new InputParser(new InputValidator()), new Graph());
        }
    }
}
