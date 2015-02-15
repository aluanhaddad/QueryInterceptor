using System;
using System.Reflection;
#if ASPNETCORE50
using System.Collections.Generic;
#endif

namespace QueryInterceptor
{
    /// <summary>
    /// Some 'proxy' methods to make compilation possible for NET4.5, ASPNET50 and ASPNETCORE50
    /// </summary>
    public static class TypeExtensionsHelper
    {
        public static bool IsGenericType(this Type type)
        {
#if ASPNETCORE50
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static bool IsInterface(this Type type)
        {
#if ASPNETCORE50
            return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

        public static Type BaseType(this Type type)
        {
#if ASPNETCORE50
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        public static bool IsValueType(this Type type)
        {
#if ASPNETCORE50
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }

        public static bool IsEnum(this Type type)
        {
#if ASPNETCORE50
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static Assembly Assembly(this Type type)
        {
#if ASPNETCORE50
            return type.GetTypeInfo().Assembly;
#else
            return type.Assembly;
#endif
        }

#if ASPNETCORE50
        public static IEnumerable<MethodInfo> GetMethods(this Type someType)
        {
            var t = someType;
            while (t != null)
            {
                var ti = t.GetTypeInfo();
                foreach (var m in ti.DeclaredMethods)
                    yield return m;
                t = ti.BaseType;
            }
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments;
        }
#endif
    }
}