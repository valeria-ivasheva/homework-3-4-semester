using NUnit.Framework;
using MyNUnit;
using System.IO;

namespace Tests
{
    public class Tests
    {
        private string path;

        [SetUp]
        public void BeforeTest()
        {
            path = Directory.GetCurrentDirectory();
            for (int i =0; i < 4; i++)
            {
                path = Path.GetDirectoryName(path);
            }
            path = path + @"\TestProjects";
        }

        [Test]
        public void SimpleTest()
        {
            path = path + @"\SimpleTest\bin";
            MyNUnit.TestingSystem.Run(path);
            var result = TestingSystem.GetResultTestInfos();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(TestPassedOk(result[0], "SimpleTest.Class1 Simple"));
        }

        [Test]
        public void TestIgnore()
        {
            path = path + @"\TestIgnore\bin";
            TestingSystem.Run(path);
            var result = TestingSystem.GetResultTestInfos();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(TestPassedOk(result[1], "TestIgnore.Ignore TempTest"));
            Assert.IsTrue(TestPassedIgnore(result[0], "TestIgnore.Ignore IgnoreTest", "Просто так)"));
        }

        [Test]
        public void TestWithException()
        {
            path = path + @"\TestWithException\bin";
            TestingSystem.Run(path);
            var result = TestingSystem.GetResultTestInfos();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(TestPassedOk(result[0], "TestWithException.Class1 Exception"));
        }

        [Test]
        public void TestWithAnnotations()
        {
            path = path + @"\TestWithMethods\bin";
            TestingSystem.Run(path);
            var result = TestingSystem.GetResultTestInfos();
            result.Sort();
            Assert.AreEqual(8, result.Count);
            Assert.IsTrue(TestFailed(result[0], "TestWithMethods.TestAfter Test"));
            Assert.IsTrue(TestFailed(result[1], "TestWithMethods.TestAfter Test1"));
            Assert.IsTrue(TestFailed(result[2], "TestWithMethods.TestAfterClass Test"));
            Assert.IsTrue(TestFailed(result[3], "TestWithMethods.TestAfterClass Test1"));
            Assert.IsTrue(TestPassedOk(result[4], "TestWithMethods.TestBefore Test"));
            Assert.IsTrue(TestPassedOk(result[7], "TestWithMethods.TestWithBeforeClass Test1"));
            Assert.IsTrue(TestPassedOk(result[6], "TestWithMethods.TestWithBeforeClass Test"));
            Assert.IsTrue(TestPassedOk(result[5], "TestWithMethods.TestBefore Test1"));
        }

        [Test]
        public void WrongTests()
        {
            path = path + @"\WrongTests\bin";
            TestingSystem.Run(path);
            var result = TestingSystem.GetResultTestInfos();
            result.Sort();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(TestFailed(result[0], "WrongTests.TestClass Test", "Не пройдены методы BeforeTest"));
            Assert.IsTrue(TestFailed(result[1], "WrongTests.WrongTest Test", "Exception System.Exception"));
        }

        private bool TestFailed(TestResultInfo testResult, string name)
        {
            var result = testResult.Name == name && testResult.Result.ToString() == "FAILED";
            return result;
        }

        private bool TestFailed(TestResultInfo testResult, string name, string message)
        {
            var result = testResult.Name == name && testResult.Result.ToString() == "FAILED";
            result = result && message == testResult.Message;
            return result;
        }


        private bool TestPassedOk(TestResultInfo testResult, string name)
        {
            var result = testResult.Name == name && testResult.Result.ToString() == "OK";
            return result;
        }

        private bool TestPassedIgnore(TestResultInfo testResult, string name, string message)
        {
            var result = testResult.Name == name && testResult.Result.ToString() == "IGNORED" && message == testResult.Message;
            return result;
        }
    }
}