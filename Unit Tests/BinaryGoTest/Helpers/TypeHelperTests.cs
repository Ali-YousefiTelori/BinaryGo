using BinaryGo.Runtime.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BinaryGoTest.Helpers
{
    public class Test1
    {

    }

    public class Test2
    {

    }

    public class Test3
    {
        public string Name { get; set; }
        public Test1 Test1 { get; set; }
    }

    public class Test4
    {
        public int Age { get; set; }
    }

    public class Test5
    {
        public string Age { get; set; }
    }

    public class TestLoop
    {
        public TestLoop Age { get; set; }
    }

    public class TestGeneric<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
    }

    public class TypeHelperTests
    {
        [Fact]
        public void TestHash()
        {
            List<string> items = new List<string>();
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test1)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test1)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test1)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test2)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test3)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test4)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test5)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test5)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(Test5)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(TestLoop)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(List<Test5>)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(List<>)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(int)));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(int[])));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(TestLoop[])));
            items.Add(new TypeHelper().GetTypeUniqueHash(typeof(TestGeneric<List<int>, string>)));
            items = items.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            Assert.True(items.Count == 12);
        }

        [Fact]
        public void TestDisplayName()
        {
            List<string> items = new List<string>();
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test1)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test1)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test1)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test2)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test3)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test4)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test5)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test5)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(Test5)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(TestLoop)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(List<Test5>)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(List<>)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(int)));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(int[])));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(TestLoop[])));
            items.Add(new TypeHelper().GetUniqueCompressedHash(typeof(TestGeneric<List<int>, string>)));
            items = items.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            Assert.True(items.Count == 12);
        }
    }
}
