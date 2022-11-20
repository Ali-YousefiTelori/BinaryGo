using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BinaryGo.Runtime.Helpers
{
    /// <summary>
    /// help you about types
    /// </summary>
    public class TypeHelper
    {
        static ConcurrentDictionary<Type, string> HashedTypes { get; set; } = new ConcurrentDictionary<Type, string>();
        /// <summary>
        /// Get a unique hash from a type
        /// so if the type has any change you will see new hash
        /// </summary>
        /// <param name="type"></param>
        /// <param name="caculatedHash"></param>
        /// <returns></returns>
        public string GetTypeUniqueHash(Type type, HashSet<Type> caculatedHash = null)
        {
            if (HashedTypes.TryGetValue(type, out string hash))
                return hash;
            if (ReflectionHelper.VariableTypes.TryGetValue(type, out string variable))
                return AddToHash(type, variable);
            if (caculatedHash == null)
                caculatedHash = new HashSet<Type>();
            //skip the stackoverflow exception whn you have loop reference
            else if (caculatedHash.Contains(type))
                return "calculating...";
            else if (string.IsNullOrEmpty(type.FullName))
                return "EmptyFullName";// generic argument T
            caculatedHash.Add(type);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(type.FullName);
            foreach (var property in ReflectionHelper.GetListOfProperties(type))
            {
                stringBuilder.Append(property.Name);
                stringBuilder.Append(':');
                stringBuilder.AppendLine(GetTypeUniqueHash(property.PropertyType, caculatedHash));
            }

            foreach (var genericType in type.GetGenericArguments())
            {
                stringBuilder.AppendLine(GetTypeUniqueHash(genericType, caculatedHash));
            }

            if (type.HasElementType)
                stringBuilder.AppendLine(GetTypeUniqueHash(type.GetElementType(), caculatedHash));

            return AddToHash(type, stringBuilder.ToString());
        }

        string AddToHash(Type type, string fullName)
        {
            var hash = GetSHA1Hash(fullName);
            HashedTypes[type] = hash;
            return hash;
        }

        /// <summary>
        /// Get unique full name of a type and compress to hash
        /// it's unique of each type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetUniqueCompressedHash(Type type)
        {

            StringBuilder builder = new StringBuilder();
            string fileName = Path.GetFileName(type.Assembly.GetName().Name);
            builder.Append(fileName);
            builder.Append('-');
            builder.Append(GetTypeName(type));
            var generics = type.GetGenericArguments();
            if (generics.Length > 0)
            {
                builder.Append("<");
                for (int i = 0; i < generics.Length; i++)
                {
                    builder.Append(GetUniqueCompressedHash(generics[i]));
                    if (i < generics.Length - 1)
                        builder.Append(",");
                }
                builder.Append(">");
            }

            return GetSHA1Hash(builder.ToString());
        }

        string GetTypeName(Type type)
        {
            return $"{type.Namespace}.{type.Name.Split('`').First()}";
        }

        string CompressNameWithIndex(string name, TypeHelperOption typeHelperOption)
        {
            StringBuilder builder = new StringBuilder();
            var splitNames = name.Split('.');
            for (int i = 0; i < splitNames.Length; i++)
            {
                builder.Append(GetNameWithIndex(splitNames[i], typeHelperOption));
                if (i < splitNames.Length - 1)
                    builder.Append(".");

            }
            return builder.ToString();
        }

        string GetNameWithIndex(string name, TypeHelperOption typeHelperOption)
        {
            if (typeHelperOption.NameIndexes.TryGetValue(name, out int index))
                return index.ToString();
            else
            {
                typeHelperOption.Index++;
                typeHelperOption.NameIndexes.Add(name, typeHelperOption.Index);
                return name + typeHelperOption.Index;
            }
        }

        /// <summary>
        /// Get SHA1 hash of string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetSHA1Hash(string input)
        {
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }

    /// <summary>
    /// options of data save in memoery
    /// </summary>
    public class TypeHelperOption
    {
        /// <summary>
        /// indexed names
        /// </summary>
        public Dictionary<string, int> NameIndexes { get; private set; } = new Dictionary<string, int>();
        /// <summary>
        /// current index number
        /// </summary>
        public int Index { get; set; } = 0;
    }
}
