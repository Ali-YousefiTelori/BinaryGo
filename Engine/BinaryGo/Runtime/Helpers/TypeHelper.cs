using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
}
