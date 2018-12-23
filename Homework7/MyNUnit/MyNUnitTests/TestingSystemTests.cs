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
            Assert.AreEqual("SimpleTest.Class1 Simple", result[0].Name);
            Assert.AreEqual("OK", result[0].Result.ToString());
            Assert.Pass();
        }

        [Test]
        public void TestIgnore()
        {
            path = path + @"\TestIgnore\bin";
            TestingSystem.Run(path);
            var result = TestingSystem.GetResultTestInfos();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("TestIgnore.Ignore TempTest", result[1].Name);
            Assert.AreEqual("OK", result[1].Result.ToString());
            Assert.AreEqual("TestIgnore.Ignore IgnoreTest", result[0].Name);
            Assert.AreEqual("IGNORED", result[0].Result.ToString());
            Assert.AreEqual("Просто так)", result[0].Message);
        }

        [Test]
        public void TestWithException()
        {
            path = path + @"\TestWithException\bin";
            TestingSystem.Run(path);
            var result = TestingSystem.GetResultTestInfos();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("TestWithException.Class1 Exception", result[0].Name);
            Assert.AreEqual("OK", result[0].Result.ToString());
        }

        [Test]
        public void TestWithBeforeClass()
        {
            path = path + @"\TestWithMethods\bin";
            TestingSystem.Run(path);
            var result = TestingSystem.GetResultTestInfos();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("TestWithMethods.TestWithBeforeClass Test", result[0].Name);
            Assert.AreEqual("OK", result[0].Result.ToString());
            Assert.AreEqual("TestWithMethods.TestWithBeforeClass Test1", result[1].Name);
            Assert.AreEqual("OK", result[1].Result.ToString());
        }
    }
}