namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("Line Numbers")]
    public sealed class LineNumberTests : TestCase
    {
        #region Callstack

        private static Exception ThrowException()
        {
            try
            {
                ThrowAndRethrowExceptionFromLevel2();
                return null;
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

        [Fact(DisplayName = "Successfully get the exception's line number")]
        public void GetLineNumberSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception => exception.GetLineNumber())
            .Then(lineNumber => lineNumber.Should().Be(34));
        }

        [Fact(DisplayName = "Successfully get the exception's causing line number")]
        public void GetCausingLineNumberSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception =>
                {
                    exception.TryGetCausingLineNumber(out var causingLineNumber);
                    return causingLineNumber;
                })
            .Then(lineNumber => lineNumber.Should().Be(40));
        }
    }
}