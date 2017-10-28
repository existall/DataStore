using ExistsForAll.DataStore.Core;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExistsForAll.DataStore.DapperExtensions.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			X1<Person>(x => x.Name);
		}





		private void X1<T, TValue>(Expression<Func<T, TValue>> memeber)
		{
			//var l = Expression.Lambda<Func<T, object>>(

			//	Expression.Convert(memeber.Body, typeof(object))
			//);

			//var x = ReflectionHelper.Cast<T, TValue,object>(memeber);
			//X2(memeber);

			var memberInfo = ReflectionHelper.GetProperty(memeber);
		}

		private void X1<T>(Expression<Func<T, object>> memeber)
		{
			//var l = Expression.Lambda<Func<T, object>>(

			//	Expression.Convert(memeber.Body, typeof(object))
			//);
			//X2(memeber);
			X1<T,object>(memeber);
		}

		private void X2<T>(Expression<Func<T, object>> memeber)
		{
			var memberInfo = ReflectionHelper.GetProperty(memeber);

		}
	}

	public class Person
	{
		public string Name { get; set; }
	}
	public static class ReflectionHelper
	{

		public static Expression<Func<TModel, TToProperty>> Cast<TModel, TFromProperty, TToProperty>(
			Expression<Func<TModel, TFromProperty>> expression)
		{
			Expression converted = Expression.Convert(expression.Body, typeof(TToProperty));

			return Expression.Lambda<Func<TModel, TToProperty>>(converted, expression.Parameters);
		}

		private static List<Type> _simpleTypes = new List<Type>
							   {
								   typeof(byte),
								   typeof(sbyte),
								   typeof(short),
								   typeof(ushort),
								   typeof(int),
								   typeof(uint),
								   typeof(long),
								   typeof(ulong),
								   typeof(float),
								   typeof(double),
								   typeof(decimal),
								   typeof(bool),
								   typeof(string),
								   typeof(char),
								   typeof(Guid),
								   typeof(DateTime),
								   typeof(DateTimeOffset),
								   typeof(byte[])
							   };

		public static MemberInfo GetProperty(LambdaExpression lambda)
		{
			Expression expr = lambda;
			for (; ; )
			{
				switch (expr.NodeType)
				{
					case ExpressionType.Lambda:
						expr = ((LambdaExpression)expr).Body;
						break;
					case ExpressionType.Convert:
						expr = ((UnaryExpression)expr).Operand;
						break;
					case ExpressionType.MemberAccess:
						MemberExpression memberExpression = (MemberExpression)expr;
						MemberInfo mi = memberExpression.Member;
						return mi;
					default:
						return null;
				}
			}
		}

		public static IDictionary<string, object> GetObjectValues(object obj)
		{
			IDictionary<string, object> result = new Dictionary<string, object>();
			if (obj == null)
			{
				return result;
			}


			foreach (var propertyInfo in obj.GetType().GetProperties())
			{
				string name = propertyInfo.Name;
				object value = propertyInfo.GetValue(obj, null);
				result[name] = value;
			}

			return result;
		}

		public static string AppendStrings(this IEnumerable<string> list, string seperator = ", ")
		{
			return list.Aggregate(
				new StringBuilder(),
				(sb, s) => (sb.Length == 0 ? sb : sb.Append(seperator)).Append(s),
				sb => sb.ToString());
		}

		public static bool IsSimpleType(Type type)
		{
			Type actualType = type;
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				actualType = type.GetGenericArguments()[0];
			}

			return _simpleTypes.Contains(actualType);
		}

		public static string GetParameterName(this IDictionary<string, object> parameters, string parameterName, char parameterPrefix)
		{
			return string.Format("{0}{1}_{2}", parameterPrefix, parameterName, parameters.Count);
		}

		public static string SetParameterName(this IDictionary<string, object> parameters, string parameterName, object value, char parameterPrefix)
		{
			string name = parameters.GetParameterName(parameterName, parameterPrefix);
			parameters.Add(name, value);
			return name;
		}

		internal static void Cast<T>(Expression<Func<T, T>> memeber)
		{
			throw new NotImplementedException();
		}
	}

}