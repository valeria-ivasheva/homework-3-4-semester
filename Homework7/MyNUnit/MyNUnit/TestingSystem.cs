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
    /// <summary>
    /// Тестирующая система
    /// </summary>
    public static class TestingSystem
    {
        private static ConcurrentBag<TestResultInfo> testsResult;
        private static readonly object lockObject = new object();

        /// <summary>
        /// Запуск тестов, находящихся во всех сборках, расположенных по пути path
        /// </summary>
        /// <param name="path"> Путь для тестов</param>
        public static void Run(string path)
        {
            var pathToAssembly = GetFilesList(path, "*.dll");
            var types = new List<Type>();
            foreach (var pathTemp in pathToAssembly)
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
            var result = GetResultTestInfos();
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Write();
            }
        }

        /// <summary>
        /// Получить результаты тестов
        /// </summary>
        /// <returns> Список результатов тестов</returns>
        public static List<TestResultInfo> GetResultTestInfos()
        {
            var result = testsResult.ToList();
            var resultNew = new List<TestResultInfo>();
            result.Sort();
            int index = 0;
            while (index < result.Count)
            {
                var name = result[index].Name;
                var temp = result.Where(x => x.Name == name);
                if (temp.Count() == 1)
                {
                    resultNew.Add(result[index]);
                }
                else
                {
                    var add = temp.Where(x => x.Result.ToString() == "FAILED").ToList();
                    resultNew.Add(add[0]);
                }
                index += temp.Count();
            }
            return resultNew;
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
            var instanceOfType = Activator.CreateInstance(type);
            var passedMethodsBeforeClass = RunMethodsWithAnnotation(methodsBeforeClass, methodsForTesting, "BeforeClass", instanceOfType);
            if (!passedMethodsBeforeClass)
            {
                return;
            }
            Parallel.For(0, methodsForTesting.Count, i => RunTest(methodsForTesting[i], instanceOfType));
            var passedMethodsAfterTest = RunMethodsWithAnnotation(methodsAfterClass, methodsForTesting, "AfterClass", instanceOfType);
        }

        private static bool RunMethodsWithAnnotation(List<MethodInfo> methodsWithAnnotation, List<MethodInfo> methodsTest, string annotation, object instanceOfType)
        {
            var resultMethods = RunMethodsClass(methodsWithAnnotation, instanceOfType);
            var passedMethods = CheckResultsMethod(resultMethods, methodsTest, annotation);
            return passedMethods;
        }

        private static void RunTest(MethodInfo method, object instanceOfType)
        {
            var attrTemp = Attribute.GetCustomAttribute(method, typeof(TestAttribute));
            var attr = (TestAttribute)Attribute.GetCustomAttributes(method).Where(t => Equals(t.GetType(), typeof(TestAttribute))).First(); //Здесь нет элементов из-за приведения типа
            var ignore = attr.Ignore;
            var expectedException = attr.Expected;
            var methodsBeforeTest = MethodsWithAttributes<BeforeTestAttribute>(method.DeclaringType);
            var passedMethodsBeforeTest = RunMethodsWithAnnotation(methodsBeforeTest, new List<MethodInfo> { method }, "BeforeTest", instanceOfType);
            if (!passedMethodsBeforeTest)
            {
                return;
            }
            if (ignore != null)
            {
                var resultTest = new TestResultInfo(method.DeclaringType + " " + method.Name, true, ignore);
                testsResult.Add(resultTest);
                return;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var result = RunMethod(method, instanceOfType);
            watch.Stop();
            var elapsedTime = watch.ElapsedMilliseconds;
            var condTrueResultException = expectedException != null && (result.Exception) == expectedException;
            if (!condTrueResultException && expectedException != null)
            {
                var resultTest = new TestResultInfo(method.DeclaringType + " " + method.Name, false, $"Expected exception {expectedException}");
                testsResult.Add(resultTest);
                return;
            }
            if (result.Exception != null && expectedException == null)
            {
                var resultTest = new TestResultInfo(method.DeclaringType + " " + method.Name, false, $"Exception {result.Exception}");
                testsResult.Add(resultTest);
            }
            var methodsAfterTest = MethodsWithAttributes<AfterTestAttribute>(method.DeclaringType);
            var passedAfterTest = RunMethodsWithAnnotation(methodsAfterTest, new List<MethodInfo> { method }, "AfterTest", instanceOfType);
            if (!passedAfterTest)
            {
                return;
            }
            if (condTrueResultException || result.Result)
            {
                var resultTest = new TestResultInfo(method.DeclaringType + " " + method.Name, elapsedTime);
                testsResult.Add(resultTest);
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
                testsResult.Add(new TestResultInfo(methodsTest[i].DeclaringType + " " + methodsTest[i].Name, false, message));
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

        private static List<MethodInfo> MethodsWithAttributes<T>(Type type) where T : Attribute
        {
            var result = new List<MethodInfo>();
            foreach (var method in type.GetMethods())
            {
                foreach (var temp in method.CustomAttributes)
                {
                    if (Equals(temp.AttributeType.ToString(), typeof(T).ToString()))
                    {
                        result.Add(method);
                    }
                }
            }
            return result;
        }

        private static List<InfoMethod> RunMethodsClass(List<MethodInfo> methods, object instanceOfType)
        {
            var tasks = new Task<InfoMethod>[methods.Count];
            var result = new List<InfoMethod>();
            for (int i = 0; i < tasks.Length; i++)
            {
                int j = i;
                tasks[i] = Task.Factory.StartNew(() => RunMethod(methods[j], instanceOfType));
            }
            Task.WaitAll(tasks);
            foreach (var task in tasks)
            {
                result.Add(task.Result);
            }
            return result;
        }

        private static InfoMethod RunMethod(MethodInfo methodInfo, object instanceOfType)
        {
            var result = new InfoMethod(methodInfo.Name);
            try
            {
                var resultMethod = methodInfo.Invoke(instanceOfType, null);
            }
            catch (Exception e)
            {
                var typeException = e.InnerException.GetType();
                result = new InfoMethod(methodInfo.Name, typeException);
            }
            return result;
        }
    }
}
