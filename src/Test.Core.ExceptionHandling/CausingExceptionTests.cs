namespace CustomCode.Core.ExceptionHandling.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Test.BehaviorDrivenDevelopment;
    using Xunit;

    [UnitTest]
    [Category("Causing Exception")]
    public sealed class CausingExceptionTests : TestCase
    {
        #region Callstack

        private static Exception ThrowException()
        {
            return new Exception("Level 1");
        }

        private static Exception ThrowAndRethrowException()
        {
            try
            {
                ThrowAndRethrowExceptionFromLevel2();
                return new Exception("Unreachable code");
            }
            catch (Exception ex)
            {
                return new Exception("Level 1", ex);
            }
        }

        private static void ThrowAndRethrowExceptionFromLevel2()
        {
            try
            {
                ThrowExceptionFromLevel3();
            }
            catch (Exception ex)
            {
                throw new Exception("Level 2", ex);
            }
        }

        private static void ThrowExceptionFromLevel3()
        {
            throw new Exception("Level 3");
        }

        private static Exception ThrowAndRethrowAggregateExceptionWithMultipleExceptions()
        {
            try
            {
                var task1 = Task.Run(() => ThrowAndRethrowExceptionFromLevel2());
                var task2 = Task.Run(() => ThrowExceptionFromLevel3());
                Task.WaitAll(task1, task2);
                return new Exception("Unreachable code");
            }
            catch(Exception ex)
            {
                return new Exception("Level 1", ex);
            }
        }

        private static Exception ThrowAndRethrowAggregateExceptionWithSingleException()
        {
            try
            {
                var task = Task.Run(() => ThrowAndRethrowExceptionFromLevel2());
                Task.WaitAll(task);
                return new Exception("Unreachable code");
            }
            catch (Exception ex)
            {
                return new Exception("Level 1", ex);
            }
        }

        #endregion

        [Fact(DisplayName = "Successfully get null without causing exception")]
        public void GetNullFromSingleExceptionSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception =>
                {
                    exception.TryGetCausingException(out var causingException);
                    return causingException;
                })
            .Then(causingException => causingException.Should().BeNull());
        }

        [Fact(DisplayName = "Successfully get a single causing exception")]
        public void GetSingleCausingExceptionSuccessfully()
        {
            Given(() => ThrowAndRethrowException())
            .When(exception =>
                {
                    exception.TryGetCausingException(out var causingException);
                    return causingException;
                })
            .Then(causingException =>
                {
                    causingException.ShouldNot().BeNull();
                    causingException?.Message.Should().Be("Level 3");
                });
        }

        [Fact(DisplayName = "Successfully get a single causing exception from an asynchronous task")]
        public void GetSingleCausingExceptionFromAsyncTaskSuccessfully()
        {
            Given(() => ThrowAndRethrowAggregateExceptionWithSingleException())
            .When(exception =>
                {
                    exception.TryGetCausingException(out var causingException);
                    return causingException;
                })
            .Then(causingException =>
                {
                    causingException.ShouldNot().BeNull();
                    causingException?.Message.Should().Be("Level 3");
                });
        }

        [Fact(DisplayName = "Successfully get null as causing exception from multiple asynchronous tasks")]
        public void GetNullFromMultipleAsyncTasksSuccessfully()
        {
            Given(() => ThrowAndRethrowAggregateExceptionWithMultipleExceptions())
            .When(exception =>
                {
                    exception.TryGetCausingException(out var causingException);
                    return causingException;
                })
            .Then(causingException => causingException.Should().BeNull());
        }

        [Fact(DisplayName = "Successfully get empty collection without causing exception")]
        public void GetEmptyCollectionFromSingleExceptionSuccessfully()
        {
            Given(() => ThrowException())
            .When(exception => exception.GetCausingExceptions())
            .Then(causingExceptions => causingExceptions.Count().Should().Be(0));
        }

        [Fact(DisplayName = "Successfully get collection with single causing exception from asynchronous tasks")]
        public void GetSingleElementCollectionFromMultipleAsyncTasksSuccessfully()
        {
            Given(() => ThrowAndRethrowAggregateExceptionWithSingleException())
            .When(exception => exception.GetCausingExceptions())
            .Then(causingExceptions =>
                {
                    causingExceptions.Count().Should().Be(1);
                    causingExceptions.First().Message.Should().Be("Level 3");
                });
        }

        [Fact(DisplayName = "Successfully get multiple causing exceptions from multiple asynchronous tasks")]
        public void GetMultipleCausingExceptionsFromMultipleAsyncTasksSuccessfully()
        {
            Given(() => ThrowAndRethrowAggregateExceptionWithMultipleExceptions())
            .When(exception => exception.GetCausingExceptions())
            .Then(causingExceptions =>
                {
                    causingExceptions.Count().Should().Be(2);
                    causingExceptions.First().Message.Should().Be("Level 3");
                    causingExceptions.Last().Message.Should().Be("Level 3");
                });
        }
    }
}