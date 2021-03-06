﻿using BinaryGo;
using BinaryGo.CompileTime;
using BinaryGo.Json;
using BinaryGoPerformance.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryGoPerformance
{
    public static class BinaryGoModelBuilder
    {
        static void ArrayInitializer<T>(Serializer serializer, StringBuilder builder, IEnumerable<T> obj)
        {
                return;
            //if (obj == null)
            //    return;
            //if (serializer.SerializedObjects.TryGetValue(obj, out int index))
            //{
            //    builder.Append("\"{\"$ref\":\"");
            //    builder.Append(index);
            //    builder.Append("\"}\"");
            //}
            //else
            //{
            //    serializer.ReferencedIndex++;
            //    serializer.SerializedObjects[obj] = serializer.ReferencedIndex;
            //    builder.Append("\"{\"$id\":\"");
            //    builder.Append(serializer.ReferencedIndex);
            //    builder.Append("\",\"$values\":[\"");
            //    foreach (var item in obj)
            //    {
            //        if (item == null)
            //            continue;
            //        serializer.ContinueSerializeCompile(item);
            //        builder.Append(',');
            //    }
            //    serializer.RemoveLastCama();
            //    builder.AppendLine("]}");
            //}
        }
        public static void Initialize()
        {
            return;

            //Console.WriteLine("initialized compile time");
            //TypeBuilder<BinaryGoPerformance.Models.CarInfo>.Create().SerializeObject((serializer, builder, obj) =>
            //{
            //    if (obj == null)
            //        return;
            //    if (serializer.SerializedObjects.TryGetValue(obj, out int index))
            //    {
            //        builder.Append("\"{\"$ref\":\"");
            //        builder.Append(index);
            //        builder.Append("\"}\"");
            //    }
            //    else
            //    {
            //        serializer.ReferencedIndex++;
            //        serializer.SerializedObjects[obj] = serializer.ReferencedIndex;
            //        builder.Append("\"{\"$id\":\"");
            //        builder.Append(serializer.ReferencedIndex);
            //        builder.Append("\",\"Id\":\"");
            //        builder.Append(obj.Id);
            //        builder.Append("\",\"Name\":\"");
            //        builder.Append(obj.Name);
            //        if (obj.CompanyInfo != null)
            //        {
            //            builder.Append("\",\"CompanyInfo\":\"");
            //            serializer.ContinueSerializeCompile(obj.CompanyInfo);
            //        }
            //    }
            //}).Build();

            //TypeBuilder<BinaryGoPerformance.Models.CompanyInfo>.Create().SerializeObject((serializer, builder, obj) =>
            //{
            //    if (obj == null)
            //        return;
            //    if (serializer.SerializedObjects.TryGetValue(obj, out int index))
            //    {
            //        builder.Append("\"{\"$ref\":\"");
            //        builder.Append(index);
            //        builder.Append("\"}\"");
            //    }
            //    else
            //    {
            //        serializer.ReferencedIndex++;
            //        serializer.SerializedObjects[obj] = serializer.ReferencedIndex;
            //        builder.Append("\"{\"$id\":\"");
            //        builder.Append(serializer.ReferencedIndex);
            //        builder.Append("\",\"Id\":\"");
            //        builder.Append(obj.Id);
            //        builder.Append("\",\"Name\":\"");
            //        builder.Append(obj.Name);
            //        if (obj.Users != null)
            //        {
            //            builder.Append("\",\"Users\":\"");
            //            serializer.ContinueSerializeCompile(obj.Users);
            //        }
            //        if (obj.Cars != null)
            //        {
            //            builder.Append("\",\"Cars\":\"");
            //            serializer.ContinueSerializeCompile(obj.Cars);
            //        }
            //    }
            //}).Build();

            //TypeBuilder<BinaryGoPerformance.Models.ProductInfo>.Create().SerializeObject((serializer, builder, obj) =>
            //{
            //    if (obj == null)
            //        return;
            //    if (serializer.SerializedObjects.TryGetValue(obj, out int index))
            //    {
            //        builder.Append("\"{\"$ref\":\"");
            //        builder.Append(index);
            //        builder.Append("\"}\"");
            //    }
            //    else
            //    {
            //        serializer.ReferencedIndex++;
            //        serializer.SerializedObjects[obj] = serializer.ReferencedIndex;
            //        builder.Append("\"{\"$id\":\"");
            //        builder.Append(serializer.ReferencedIndex);
            //        builder.Append("\",\"Id\":\"");
            //        builder.Append(obj.Id);
            //        builder.Append("\",\"Name\":\"");
            //        builder.Append(obj.Name);
            //        builder.Append("\",\"CreatedDate\":\"");
            //        builder.Append(obj.CreatedDate);
            //        if (obj.UserInfo != null)
            //        {
            //            builder.Append("\",\"UserInfo\":\"");
            //            serializer.ContinueSerializeCompile(obj.UserInfo);
            //        }
            //    }
            //}).Build();

            //TypeBuilder<BinaryGoPerformance.Models.RoleInfo>.Create().SerializeObject((serializer, builder, obj) =>
            //{
            //    if (obj == null)
            //        return;
            //    if (serializer.SerializedObjects.TryGetValue(obj, out int index))
            //    {
            //        builder.Append("\"{\"$ref\":\"");
            //        builder.Append(index);
            //        builder.Append("\"}\"");
            //    }
            //    else
            //    {
            //        serializer.ReferencedIndex++;
            //        serializer.SerializedObjects[obj] = serializer.ReferencedIndex;
            //        builder.Append("\"{\"$id\":\"");
            //        builder.Append(serializer.ReferencedIndex);
            //        builder.Append("\",\"Id\":\"");
            //        builder.Append(obj.Id);
            //        if (obj.UserInfo != null)
            //        {
            //            builder.Append("\",\"UserInfo\":\"");
            //            serializer.ContinueSerializeCompile(obj.UserInfo);
            //        }
            //        builder.Append("\",\"Type\":\"");
            //        builder.Append((int)obj.Type);
            //    }
            //}).Build();

            //TypeBuilder<BinaryGoPerformance.Models.UserCarInfo>.Create().SerializeObject((serializer, builder, obj) =>
            //{
            //    if (obj == null)
            //        return;
            //    if (serializer.SerializedObjects.TryGetValue(obj, out int index))
            //    {
            //        builder.Append("\"{\"$ref\":\"");
            //        builder.Append(index);
            //        builder.Append("\"}\"");
            //    }
            //    else
            //    {
            //        serializer.ReferencedIndex++;
            //        serializer.SerializedObjects[obj] = serializer.ReferencedIndex;
            //        builder.Append("\"{\"$id\":\"");
            //        builder.Append(serializer.ReferencedIndex);
            //        builder.Append("\",\"Id\":\"");
            //        builder.Append(obj.Id);
            //        builder.Append("\",\"CreateDate\":\"");
            //        builder.Append(obj.CreateDate);
            //        if (obj.CarInfo != null)
            //        {
            //            builder.Append("\",\"CarInfo\":\"");
            //            serializer.ContinueSerializeCompile(obj.CarInfo);
            //        }
            //        if (obj.UserInfo != null)
            //        {
            //            builder.Append("\",\"UserInfo\":\"");
            //            serializer.ContinueSerializeCompile(obj.UserInfo);
            //        }
            //    }
            //}).Build();

            //TypeBuilder<BinaryGoPerformance.Models.UserInfo>.Create().SerializeObject((serializer, builder, obj) =>
            //{
            //    if (obj == null)
            //        return;
            //    if (serializer.SerializedObjects.TryGetValue(obj, out int index))
            //    {
            //        builder.Append("\"{\"$ref\":\"");
            //        builder.Append(index);
            //        builder.Append("\"}\"");
            //    }
            //    else
            //    {
            //        serializer.ReferencedIndex++;
            //        serializer.SerializedObjects[obj] = serializer.ReferencedIndex;
            //        builder.Append("\"{\"$id\":\"");
            //        builder.Append(serializer.ReferencedIndex);
            //        builder.Append("\",\"Id\":\"");
            //        builder.Append(obj.Id);
            //        builder.Append("\",\"FullName\":\"");
            //        builder.Append(obj.FullName);
            //        builder.Append("\",\"Age\":\"");
            //        builder.Append(obj.Age);
            //        builder.Append("\",\"CreatedDate\":\"");
            //        builder.Append(obj.CreatedDate);
            //        if (obj.Products != null)
            //        {
            //            builder.Append("\",\"Products\":\"");
            //            serializer.ContinueSerializeCompile(obj.Products);
            //        }
            //        if (obj.Roles != null)
            //        {
            //            builder.Append("\",\"Roles\":\"");
            //            serializer.ContinueSerializeCompile(obj.Roles);
            //        }
            //        if (obj.CompanyInfo != null)
            //        {
            //            builder.Append("\",\"CompanyInfo\":\"");
            //            serializer.ContinueSerializeCompile(obj.CompanyInfo);
            //        }
            //    }
            //}).Build();
            //TypeBuilder<System.Collections.Generic.List<BinaryGoPerformance.Models.CompanyInfo>>.Create().SerializeObject(ArrayInitializer).Build();
            //TypeBuilder<System.Collections.Generic.List<BinaryGoPerformance.Models.RoleInfo>>.Create().SerializeObject(ArrayInitializer).Build();
            //TypeBuilder<System.Collections.Generic.List<BinaryGoPerformance.Models.ProductInfo>>.Create().SerializeObject(ArrayInitializer).Build();
            //TypeBuilder<System.Collections.Generic.List<BinaryGoPerformance.Models.CarInfo>>.Create().SerializeObject(ArrayInitializer).Build();
            //TypeBuilder<System.Collections.Generic.List<BinaryGoPerformance.Models.UserInfo>>.Create().SerializeObject(ArrayInitializer).Build()
            //    .On<System.Collections.Generic.IEnumerable<BinaryGoPerformance.Models.UserInfo>>();

        }
    }
}
