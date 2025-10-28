using System;
using System.Reflection;
using System.Collections.Generic;

namespace Plugin.ApkImageView.Bll
{
	internal static class TypeExtender
	{
		public static String GetMemberName(this Type type, String originalName)
		{
			String result = originalName;
			if(type.IsGenericType)
				if(type.GetGenericTypeDefinition() == typeof(System.Nullable<>))
					result += "?";
				else result = "'1 " + result;
			if(type.BaseType == typeof(Array))
				result += "[]";
			return result;
		}

		public static Type GetRealType(this Type type)
		{
			if(type.IsGenericType)
			{
				Type genericType = type.GetGenericTypeDefinition();
				if(genericType == typeof(System.Nullable<>)
					|| genericType == typeof(System.Collections.Generic.IEnumerator<>)
					|| genericType == typeof(System.Collections.Generic.IEnumerable<>)
					/*|| genericType == typeof(System.Collections.Generic.SortedList<,>)*/)
					return type.GetGenericArguments()[0].GetRealType();
			}
			if(type.HasElementType)
				//if(type.BaseType == typeof(Array))//+Для out и ref параметров
				return type.GetElementType().GetRealType();
			return type;
		}

		public static IEnumerable<MemberInfo> GetSearchableMembers(this Type type)
		{
			foreach(MemberInfo member in type.GetMembers())
				if(type.IsMemberSearchable(member))
					yield return member;
		}

		private static Boolean IsMemberSearchable(this Type type, FieldInfo field)
			=> true;

		private static Boolean IsMemberSearchable(this Type type, PropertyInfo property)
			=> property.GetIndexParameters().Length == 0;

		private static Boolean IsMemberSearchable(this Type type, MethodInfo method)
		{
			Boolean result = method.ReturnType != typeof(void)
				&& !method.Name.StartsWith("get_")
				&& !method.Name.StartsWith("set_");
			if(result)
			{//TODO: Получение значения по енумам
				ParameterInfo[] prms = method.GetParameters();
				if(prms.Length == 1)
					if(!prms[0].ParameterType.IsEnum)
						return false;
			}
			return result;
		}

		private static Boolean IsMemberSearchable(this Type type, MemberInfo member)
		{
			if(member.DeclaringType != type && member.DeclaringType != type.BaseType)
				return false;//member.DeclaringType == type.BaseType Используется для отображения наследованных классов (Пример: StringHeap:StreamHeaderTyped<String>.Data). Но возможно появление бесконечных рекурсий

			switch(member.MemberType)
			{
			case MemberTypes.Property:
				return type.IsMemberSearchable((PropertyInfo)member);
			case MemberTypes.Method:
				return type.IsMemberSearchable((MethodInfo)member);
			case MemberTypes.Field:
				return type.IsMemberSearchable((FieldInfo)member);
			default: return false;
			}
		}

		public static Type GetMemberType(this MemberInfo member)
		{
			switch(member.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).FieldType;
			case MemberTypes.Property:
				return ((PropertyInfo)member).PropertyType;
			case MemberTypes.Method:
				return ((MethodInfo)member).ReturnType;
			case MemberTypes.TypeInfo:
			case MemberTypes.NestedType:
				return (Type)member;
			default:
				throw new NotImplementedException();
			}
		}

		public static Object GetMemberValue(this MemberInfo member, Object obj)
		{
			switch(member.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).GetValue(obj);
			case MemberTypes.Property:
				//return ((PropertyInfo)member).PropertyType.ToString();
				return ((PropertyInfo)member).GetValue(obj, null);
			default: throw new NotImplementedException();
			}
		}

		/// <summary>This type from Basic Class Library</summary>
		/// <param name="type">Type to check</param>
		/// <returns>Type from BCL</returns>
		public static Boolean IsBclType(this Type type)
		{
			switch(type.Assembly.GetName().Name)
			{
			case "mscorlib":
			case "System.Private.CoreLib":
				return true;
			default:
				return false;
			}
		}
	}
}