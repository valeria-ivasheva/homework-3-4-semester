using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyNUnit.Attributes;

namespace MyNUnit
{
    public static class TestingSystem
    {
        private static ConcurrentBag<TestResultInfo> testsResult;

        public static void Run(string path)
        {
            var pathToAssebmly = GetFilesList(path, "*.exe,*.dll");
            var types = new List<Type>();
            foreach(var pathTemp in pathToAssebmly)
            {
                var assembly = Assembly.LoadFrom(pathTemp);
                var typesAssembly = assembly.ExportedTypes.ToList();
                if (typesAssembly.Count > 0)
                {
                    types.AddRange(typesAssembly);
                }
            }
            testsResult = new ConcurrentBag<TestResultInfo>();
            Parallel.ForEach(types, RunTests);
            WriteResult();
        }

        private static void WriteResult()
        {
            var result = testsResult.ToList();
            for (int i = 0; i < testsResult.Count; i++)
            {
                Console.WriteLine($"{result[i].Name} {result[i].Result} {result[i].Message}");
            }
        }

        private static List<string> GetFilesList(string path, string pattern)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file => pattern.Contains(Path.GetExtension(file))).ToList();
        }

        private static void RunTests(Type type)
        {
            var methodsBeforeClass = MethodsWithAttributes<MyNUnit.Attributes.BeforeClassAttribute>(type);
            var methodsAfterClass = MethodsWithAttributes<MyNUnit.Attributes.AfterClassAttribute>(type);
            var methodsForTesting = MethodsWithAttributes<MyNUnit.Attributes.TestAttribute>(type);
            if (methodsForTesting.Count() == 0)
            {
                return;
            }
            var passedMethodsBeforeClass = RunMethodsWithAnnotation(methodsBeforeClass, methodsForTesting, "BeforeClass");
            if (!passedMethodsBeforeClass)
            {
                return;
            }
            Parallel.ForEach(methodsForTesting, RunTest);
            var passedMethodsAfterTest = RunMethodsWithAnnotation(methodsAfterClass, methodsForTesting, "AfterClass");
        }

        private static bool RunMethodsWithAnnotation(List<MethodInfo> methodsWithAnnotation, List<MethodInfo> methodsTest, string annotation)
        {
            var resultMethods = RunMethodsClass(methodsWithAnnotation);
            var passedMethods = CheckResultsMethod(resultMethods, methodsTest, annotation);
            return passedMethods;
        }

        private static void RunTest(MethodInfo method)
        {
            Console.WriteLine(method.Name);
            var attr = (TestAttribute)Attribute.GetCustomAttributes(method).Where(t =>Equals(t.GetType(), typeof(TestAttribute))).First(); //Здесь нет элементов из-за приведения типа
            var ignore = ((TestAttribute)attr).Ignore;
            var expectedException = ((TestAttribute)attr).Expected;
            var methodsBeforeTest = MethodsWithAttributes<BeforeTestAttribute>(method.DeclaringType);
            var passedMethodsBeforeTest = RunMethodsWithAnnotation(methodsBeforeTest, new List<MethodInfo> { method }, "BeforeTest");
            if (!passedMethodsBeforeTest)
            {
                return;
            }
            if (ignore != "")
            {
                var resultTest = new TestResultInfo(method.DeclaringType + " " + method.Name, true, ignore);
                return;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var result = RunMethod(method);
            watch.Stop();
            var elapsedTime = watch.ElapsedMilliseconds;
            var condTrueResultException = expectedException != null && (result.Exception).GetType() == expectedException;            
            if (!condTrueResultException)
            {
                var resultTest = new TestResultInfo(method.DeclaringType + " " + method.Name, false, $"Expected exception {expectedException}");
                return;
            }
            var methodsAfterTest = MethodsWithAttributes<AfterTestAttribute>(method.DeclaringType);
            var passedAfterTest = RunMethodsWithAnnotation(methodsAfterTest, new List<MethodInfo> { method }, "AfterTest");
            if (!passedAfterTest)
            {
                return;
            }
            if (condTrueResultException || result.Result)
            {
                var resultTest = new TestResultInfo(method.DeclaringType + " " + method.Name, elapsedTime);
                return;
            }
        }

        private static bool CheckResultsMethod(List<InfoMethod> infoMethods, List<MethodInfo> methodsTest, string annotation)
        {
            var temp = infoMethods.ConvertAll(new Converter<InfoMethod, bool>((InfoMethod info) => info.Result)).Contains(false);
            if (!temp)
            {
                return true;
            }
            string message = $"Не пройдены методы {annotation}";
            for (int i = 0; i < methodsTest.Count; i++)
            {
                var result = new TestResultInfo(methodsTest[i].DeclaringType + " " + methodsTest[i].Name, false, message);
                var haveThisTests = IsHaveThisTest(methodsTest[i].DeclaringType + " " + methodsTest[i].Name);
                if (haveThisTests == null)
                {
                    testsResult.Add(result);
                }
                else
                {
                    testsResult.TryTake(out haveThisTests);
                    testsResult.Add(result);
                }
            }
            return false;
        }

        private static TestResultInfo IsHaveThisTest(string name)
        {
            var temp = testsResult.ToList();
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Name == name)
                {
                    return temp[i];
                }
            }
            return null;
        }

        private static List<MethodInfo> MethodsWithAttributes<T>(Type type) where T:Attribute
        {
            var result = new List<MethodInfo>();
            foreach(var method in type.GetMethods())
            {
                foreach(var temp in method.CustomAttributes)
                {
                    if  (Equals(temp.AttributeType.ToString(), typeof(T).ToString()))
                    {
                        result.Add(method);
                    }
                }
            }
            return result;
        }

        private static List<InfoMethod> RunMethodsClass(List<MethodInfo> methods)
        {
            var tasks = new Task<InfoMethod>[methods.Count];
            var result = new List<InfoMethod>();
            for (int i = 0; i < tasks.Length; i++)
            {
                int j = i;
                tasks[i] = Task.Factory.StartNew(() => RunMethod(methods[j]));
            }
            Task.WaitAll(tasks);
            foreach(var task in tasks)
            {
                result.Add(task.Result);
            }
            return result;
        }
        

        private static InfoMethod RunMethod(MethodInfo methodInfo)
        {
            var result = new InfoMethod(methodInfo.Name);
            try
            {
                var obj = Activator.CreateInstance(methodInfo.DeclaringType);
                var resultMethod = methodInfo.Invoke(obj, null);
            }
            catch(Exception e)
            {
                result = new InfoMethod(methodInfo.Name, e);
            }
            return result;
        }
    }
}
