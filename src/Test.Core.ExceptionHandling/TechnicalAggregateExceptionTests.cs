namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using System.Linq;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("Technical Exception")]
    public sealed class TechnicalAggregateExceptionTests : TestCase
    {
        #region Callstack

        private static object ThrowTechnicalAggregateException()
        {
            try
            {
                ThrowAndRethrowExceptionFromLevel2();
                return null;
            }
            catch(Exception e)
            {
                throw new TechnicalAggregateException(new[] { e, e }, "Level 1");
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

        [Fact(DisplayName = "Successfully get the technical aggregate exception's context and causing context")]
        public void GetTechnicalExceptionContextsSuccessfully()
        {
            Given()
            .When(() => ThrowTechnicalAggregateException())
            .ThenThrow<TechnicalException>(exception =>
                {
                    var context = exception.Context;
                    context.FileName.Should().EndWith($"{nameof(TechnicalAggregateExceptionTests)}.cs");
                    context.LineNumber.Should().Be(23);
                    context.MethodName.Should().Be($"{nameof(ThrowTechnicalAggregateException)}");
                    context.TypeName.Should().Be($"{nameof(TechnicalAggregateExceptionTests)}");

                    exception.CausingContexts.Count().Should().Be(2);

                    var causingContext = exception.CausingContexts.First();
                    causingContext.FileName.Should().EndWith($"{nameof(TechnicalAggregateExceptionTests)}.cs");
                    causingContext.LineNumber.Should().Be(41);
                    causingContext.MethodName.Should().Be($"{nameof(ThrowExceptionFromLevel3)}");
                    causingContext.TypeName.Should().Be($"{nameof(TechnicalAggregateExceptionTests)}");

                    causingContext = exception.CausingContexts.Last();
                    causingContext.FileName.Should().EndWith($"{nameof(TechnicalAggregateExceptionTests)}.cs");
                    causingContext.LineNumber.Should().Be(41);
                    causingContext.MethodName.Should().Be($"{nameof(ThrowExceptionFromLevel3)}");
                    causingContext.TypeName.Should().Be($"{nameof(TechnicalAggregateExceptionTests)}");
                });
        }
    }
}