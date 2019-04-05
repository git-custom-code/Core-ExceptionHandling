namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("Type Names")]
    public sealed class TypeNameTests : TestCase
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

        [Fact(DisplayName = "Successfully get the exception's type name")]
        public void GetTypeNameSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception => exception.GetTypeName())
            .Then(typeName => typeName.Should().Be($"{nameof(TypeNameTests)}"));
        }

        [Fact(DisplayName = "Successfully get the exception's causing type name")]
        public void GetCausingTypeNameSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception =>
                {
                    exception.TryGetCausingTypeName(out var causingTypeName);
                    return causingTypeName;
                })
            .Then(typeName => typeName.Should().Be($"{nameof(TypeNameTests)}"));
        }
    }
}