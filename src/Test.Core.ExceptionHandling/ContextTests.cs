namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("Context")]
    public sealed class ContextTests : TestCase
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

        [Fact(DisplayName = "Successfully get the exception's context")]
        public void GetContextSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception => exception.GetContext())
            .Then(context =>
                {
                    context.FileName.Should().EndWith($"{nameof(ContextTests)}.cs");
                    context.LineNumber.Should().Be(34);
                    context.MethodName.Should().Be($"{nameof(ThrowAndRethrowExceptionFromLevel2)}");
                    context.TypeName.Should().Be($"{nameof(ContextTests)}");
                });
        }

        [Fact(DisplayName = "Successfully get the exception's causing context")]
        public void GetCausingContextSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception =>
                {
                    exception.TryGetCausingContext(out var causingContext);
                    return causingContext;
                })
            .Then(context =>
                {
                    context.ShouldNot().BeNull();
                    context?.FileName.Should().EndWith($"{nameof(ContextTests)}.cs");
                    context?.LineNumber.Should().Be(40);
                    context?.MethodName.Should().Be($"{nameof(ThrowExceptionFromLevel3)}");
                    context?.TypeName.Should().Be($"{nameof(ContextTests)}");
                });
        }
    }
}