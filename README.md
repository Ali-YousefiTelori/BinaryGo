# BinaryGo
BinaryGo is an easy to use and very fast JSON-BINARY serializer/deserializer

Nuget:
https://www.nuget.org/packages/JsonGo/


```csharp
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            UserInfo userInfo = new UserInfo()
            {
                Id = 1,
                BirthDate = DateTime.Now,
                Name = "ali"
            };
            var value = JsonGo.Serializer.SingleIntance.Serialize(userInfo);
            Console.WriteLine(value);
            var deserialize = JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<UserInfo>(value);
            Console.WriteLine(deserialize.Name);
            Console.ReadLine();
        }
    }
```
