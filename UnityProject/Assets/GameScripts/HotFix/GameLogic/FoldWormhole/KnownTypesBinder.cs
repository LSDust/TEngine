using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace GameLogic
{
    // 简单的白名单 binder（实现 ISerializationBinder）
    public class KnownTypesBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes { get; set; } = new List<Type>();

        public Type BindToType(string assemblyName, string typeName)
        {
            // 优先通过已知类型匹配 fullname 或 typeName
            var byFullName = KnownTypes.FirstOrDefault(t => t.FullName == typeName || t.FullName == (typeName));
            if (byFullName != null) return byFullName;

            // 尝试根据传入的 assembly+type 解析（谨慎使用）
            try
            {
                var qualified = string.IsNullOrEmpty(assemblyName) ? typeName : $"{typeName}, {assemblyName}";
                var t = Type.GetType(qualified);
                if (t != null && KnownTypes.Contains(t))
                {
                    return t;
                }
            }
            catch
            {
                // ignored
            }
            throw new JsonSerializationException($"Type '{typeName}, {assemblyName}' is not allowed for deserialization.");
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.GetName().Name;
            typeName = serializedType.FullName;
        }
    }
}
