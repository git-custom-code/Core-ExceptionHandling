namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using System.Linq;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("Business Exception")]
    public sealed class BusinessExceptionTests : TestCase
    {
        #region Callstack

        private static object ThrowBusinessException()
        {
            try
            {
                ThrowAndRethrowExceptionFromLevel2();
                return null;
            }
            catch(Exception e)
            {
                throw new BusinessException("Level1Key", e, "Level 1");
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
                throw new BusinessException("Level2Key", e, "Level 2");
            }
        }

        private static void ThrowExceptionFromLevel3()
        {
            throw new BusinessException("Level3Key", "Level 3");
        }

        #endregion

        [Fact(DisplayName = "Successfully get the business exception's context and causing context")]
        public void GetTechnicalExceptionContextsSuccessfully()
        {
            Given()
            .When(() => ThrowBusinessException())
            .ThenThrow<BusinessException>(exception =>
                {
                    var context = exception.Context;
                    context.FileName.Should().EndWith($"{nameof(BusinessExceptionTests)}.cs");
                    context.LineNumber.Should().Be(23);
                    context.MethodName.Should().Be($"{nameof(ThrowBusinessException)}");
                    context.TypeName.Should().Be($"{nameof(BusinessExceptionTests)}");

                    var causingContext = exception.CausingContexts.First();
                    causingContext.FileName.Should().EndWith($"{nameof(BusinessExceptionTests)}.cs");
                    causingContext.LineNumber.Should().Be(41);
                    causingContext.MethodName.Should().Be($"{nameof(ThrowExceptionFromLevel3)}");
                    causingContext.TypeName.Should().Be($"{nameof(BusinessExceptionTests)}");
                });
        }
    }
}