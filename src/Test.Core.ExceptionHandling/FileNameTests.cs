namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("File Names")]
    public sealed class FileNameTests : TestCase
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

        [Fact(DisplayName = "Successfully get file name")]
        public void GetLineNumberSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception => exception.GetFileName())
            .Then(fileName => fileName.Should().Be($"{nameof(FileNameTests)}.cs"));
        }

        [Fact(DisplayName = "Successfully get causing file name")]
        public void GetCausingLineNumberSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception =>
                {
                    exception.TryGetCausingFileName(out var causingFileName);
                    return causingFileName;
                })
            .Then(fileName => fileName.Should().Be($"{nameof(FileNameTests)}.cs"));
        }
    }
}