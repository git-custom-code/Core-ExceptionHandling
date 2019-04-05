namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("Method Names")]
    public sealed class MethodNameTests : TestCase
    {
        #region Callstack

        private static Exception ThrowException()
        {
            try
            {
                ThrowAndRethrowExceptionFromLevel2();
                return new Exception("Unreachable code");
            }
            catch(Exception e)
            {
                return e;
            }
        }

        private static void ThrowAndRethrowExceptionFromLevel2()
        {
            try
            {
                ThrowExceptionFromLevel3();
            }
            catch(Exception e)
            {
                throw new Exception("Level 2", e);
            }
        }

        private static void ThrowExceptionFromLevel3()
        {
            throw new Exception("Level 3");
        }

        #endregion

        [Fact(DisplayName = "Successfully get the exception's method name")]
        public void GetMethodNameSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception => exception.GetMethodName())
            .Then(methodName => methodName.Should().Be($"{nameof(ThrowAndRethrowExceptionFromLevel2)}"));
        }

        [Fact(DisplayName = "Successfully get the exception's causing method name")]
        public void GetCausingMethodNameSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception =>
                {
                    exception.TryGetCausingMethodName(out var causingMethodName);
                    return causingMethodName;
                })
            .Then(methodName => methodName.Should().Be($"{nameof(ThrowExceptionFromLevel3)}"));
        }
    }
}