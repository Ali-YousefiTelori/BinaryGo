using BinaryGo.Runtime.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            items = items.Distinct().ToList();
            Assert.True(items.Count == 6);
        }
    }
}
