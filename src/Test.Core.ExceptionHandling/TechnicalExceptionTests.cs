namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using System.Linq;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("Technical Exception")]
    public sealed class TechnicalExceptionTests : TestCase
    {
        #region Callstack

        private static object ThrowTechnicalException()
        {
            try
            {
                ThrowAndRethrowExceptionFromLevel2();
                return new object();
            }
            catch(Exception e)
            {
                throw new TechnicalException(e, "Level 1");
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
                throw new TechnicalException(e, "Level 2");
            }
        }

        private static void ThrowExceptionFromLevel3()
        {
            throw new TechnicalException("Level 3");
        }

        #endregion

        [Fact(DisplayName = "Successfully get the technical exception's context and causing context")]
        public void GetTechnicalExceptionContextsSuccessfully()
        {
            Given()
            .When(() => ThrowTechnicalException())
            .ThenThrow<TechnicalException>(exception =>
                {
                    var context = exception.Context;
                    context.FileName.Should().EndWith($"{nameof(TechnicalExceptionTests)}.cs");
                    context.LineNumber.Should().Be(23);
                    context.MethodName.Should().Be($"{nameof(ThrowTechnicalException)}");
                    context.TypeName.Should().Be($"{nameof(TechnicalExceptionTests)}");

                    var causingContext = exception.CausingContexts.First();
                    causingContext.FileName.Should().EndWith($"{nameof(TechnicalExceptionTests)}.cs");
                    causingContext.LineNumber.Should().Be(41);
                    causingContext.MethodName.Should().Be($"{nameof(ThrowExceptionFromLevel3)}");
                    causingContext.TypeName.Should().Be($"{nameof(TechnicalExceptionTests)}");
                });
        }
    }
}