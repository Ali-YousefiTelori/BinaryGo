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
            items = items.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            Assert.True(items.Count == 11);
        }
    }
}
