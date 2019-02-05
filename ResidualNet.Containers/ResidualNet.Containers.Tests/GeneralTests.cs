using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ResidualNet
{
    [TestClass]
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
    public class GeneralTests
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
    {
        public static T[] SubArray<T>(T[] objects, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(objects, index, result, 0, length);
            return result;
        }

        public static double[] GenerateRandom(int length)
        {
            Random generator = new Random();
            double[] origins = new double[length];
            for (int index = 0; index < origins.Length; index++)
                origins[index] = generator.NextDouble();
            return origins;
        }

        public static T[] GenerateRandom<T>(int size)
            where T : class
        {
            double reference = 0;
            Random generator = new Random();
            T[] origins = new T[size];
            for (int index = 0; index < origins.Length; index++)
                origins[index] = (T)Activator.CreateInstance(typeof(T), reference++ + generator.NextDouble());
            return origins;
        }

        public static T[] GenerateSequence<T>(params T[] objects)
        {
            List<T> sequence = new List<T>();
            if (objects != null && objects.Length > 0)
            {
                Random generator = new Random();
                List<T> interim = new List<T>(objects);
                do
                {
                    int index = generator.Next(interim.Count);
                    sequence.Add(interim[index]);
                    interim.RemoveAt(index);
                }
                while (interim.Count > 0);
            }
            return sequence.ToArray();
        }

        protected static E AssertError<E>(Action method, Func<E, bool> condition = null)
            where E : Exception
        {
            try { method?.Invoke(); }
            catch (E e) when (condition?.Invoke(e) ?? true)
            { return e; }
            catch (E e)
            { Assert.Fail($"Wrong exception condition for [{typeof(E).Name}]: {e.Message}"); }
            catch (Exception e)
            { Assert.AreEqual(typeof(E), e.GetType(), $"Wrong exception thrown for [{e.GetType().Name}]: {e.Message}"); }
            Assert.Fail("No exception thrown.");
            return null;
        }

        protected static void AssertErrorIf<E>(Action method, Func<E, bool> condition = null)
            where E : Exception
        {
            try { method?.Invoke(); }
            catch (E e) when (condition?.Invoke(e) ?? true)
            { }
            catch (E e)
            { Assert.Fail($"Wrong exception condition for [{typeof(E).Name}]: {e.Message}"); }
            catch (Exception e)
            { Assert.AreEqual(typeof(E), e.GetType(), $"Wrong exception thrown for [{e.GetType().Name}]: {e.Message}"); }
        }

        protected async static Task<E> AssertErrorAsync<E>(Func<Task> method, Func<E, bool> condition = null)
            where E : Exception
        {
            try { await (method?.Invoke()).ConfigureAwait(false); }
            catch (E e) when (condition?.Invoke(e) ?? true)
            { return e; }
            catch (E e)
            { Assert.Fail($"Wrong exception condition for [{typeof(E).Name}]: {e.Message}"); }
            catch (Exception e)
            { Assert.AreEqual(typeof(E), e.GetType(), $"Wrong exception thrown for [{e.GetType().Name}]: {e.Message}"); }
            Assert.Fail("No exception thrown.");
            return null;
        }

        protected static E AssertError<E, T>(Action<T> method, T data, Func<E, bool> condition = null)
            where E : Exception
        {
            try { method?.Invoke(data); }
            catch (E e) when (condition?.Invoke(e) ?? true)
            { return e; }
            catch (E e)
            { Assert.Fail($"Wrong exception condition for [{typeof(E).Name}]: {e.Message}"); }
            catch (Exception e)
            { Assert.AreEqual(typeof(E), e.GetType(), $"Wrong exception thrown for [{e.GetType().Name}]: {e.Message}"); }
            Assert.Fail("No exception thrown.");
            return null;
        }

        protected static async Task<E> AssertErrorAsync<E, T>(Func<Task<T>> method, Func<E, bool> condition = null)
            where E : Exception
        {
            try { await (method?.Invoke()).ConfigureAwait(false); }
            catch (E e) when (condition?.Invoke(e) ?? true)
            { return e; }
            catch (E e)
            { Assert.Fail($"Wrong exception condition for [{typeof(E).Name}]: {e.Message}"); }
            catch (Exception e)
            { Assert.AreEqual(typeof(E), e.GetType(), $"Wrong exception thrown for [{e.GetType().Name}]: {e.Message}"); }
            Assert.Fail("No exception thrown.");
            return null;
        }

        protected static Expression AssertExpression(UnaryExpression expression, ExpressionType node, Type type, MethodInfo method = null)
        {
            Assert.IsNotNull(expression);
            Assert.AreEqual(node, expression.NodeType);
            Assert.AreEqual(type, expression.Type);
            Assert.AreEqual(method, expression.Method);
            return expression.Operand;
        }

        protected static (Expression, Expression) AssertExpressions<T>(T expression, ExpressionType node, Type type)
            where T : BinaryExpression
        {
            Assert.IsNotNull(expression);
            Assert.AreEqual(node, expression.NodeType);
            Assert.AreEqual(type, expression.Type);
            return (expression.Left, expression.Right);
        }

        protected static Expression AssertExpression(MemberExpression expression, ExpressionType node, out MemberInfo member)
        {
            Assert.IsNotNull(expression);
            Assert.AreEqual(node, expression.NodeType);
            member = expression.Member;
            return expression.Expression;
        }

        protected static object AssertExpression(ConstantExpression expression, ExpressionType node, Type type)
        {
            Assert.IsNotNull(expression);
            Assert.AreEqual(node, expression.NodeType);
            Assert.AreEqual(type, expression.Type);
            return expression.Value;
        }

        protected static IList<Expression> AssertExpression(NewArrayExpression expression, ExpressionType node, Type type)
        {
            Assert.IsNotNull(expression);
            Assert.AreEqual(node, expression.NodeType);
            Assert.AreEqual(type, expression.Type);
            return expression.Expressions;
        }

        protected static (Expression, IList<Expression>) AssertExpressions(MethodCallExpression expression, ExpressionType node, Type type, out MethodInfo method)
        {
            Assert.IsNotNull(expression);
            Assert.AreEqual(node, expression.NodeType);
            Assert.AreEqual(type, expression.Type);
            method = expression.Method;
            return (expression.Object, expression.Arguments);
        }

        protected static void AssertMember(PropertyInfo property, MemberTypes member, Type type, string name)
        {
            Assert.IsNotNull(property);
            Assert.AreEqual(member, property.MemberType);
            Assert.AreEqual(type, property.PropertyType);
            Assert.AreEqual(name, property.Name);
        }

        protected static void AssertMethod(MethodInfo method, Type declaringType, Type returnType, string name, bool isStatic)
        {
            Assert.IsNotNull(method);
            Assert.AreEqual(declaringType, method.DeclaringType);
            Assert.AreEqual(returnType, method.ReturnType);
            Assert.AreEqual(name, method.Name);
            Assert.AreEqual(isStatic, method.IsStatic);
        }
    }
}