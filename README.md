# BinaryGo
BinaryGo is an easy to use and very fast JSON-BINARY serializer/deserializer

Nuget:
https://www.nuget.org/packages/BinaryGo/0.1.0-preview8


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
            var value = BinaryGo.Serializer.SingleIntance.Serialize(userInfo);
            Console.WriteLine(value);
            var deserialize = BinaryGo.Deserialize.Deserializer.SingleIntance.Deserialize<UserInfo>(value);
            Console.WriteLine(deserialize.Name);
            Console.ReadLine();
        }
    }
```
