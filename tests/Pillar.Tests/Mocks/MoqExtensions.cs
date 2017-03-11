using System.Linq;
using System.Reflection;
using Moq.Language;
using Moq.Language.Flow;

namespace Pillar.Tests.Mocks
{
    public static class MoqExtensions
    {
        public delegate void OutAction<in T1, TOut>(out TOut outVal, T1 arg);

        public static IReturnsThrows<TMock, TReturn> OutCallback<TMock, TReturn, T1, TOut>(this ICallback<TMock, TReturn> mock, OutAction<T1, TOut> action)
            where TMock : class
        {
            return OutCallbackInternal(mock, action);
        }

        private static IReturnsThrows<TMock, TReturn> OutCallbackInternal<TMock, TReturn>(ICallback<TMock, TReturn> mock, object action)
            where TMock : class
        {
            var method = mock.GetType().GetTypeInfo()
                .Assembly
                .GetType("Moq.MethodCall")
                .GetRuntimeMethods()
                .SingleOrDefault(m => m.Name == "SetCallbackWithArguments" && !m.IsPublic && !m.IsStatic);

            method.Invoke(mock, new[] { action });

            return mock as IReturnsThrows<TMock, TReturn>;
        }
    }
}
