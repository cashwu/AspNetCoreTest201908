using AspNetCoreTest201908.Api.Lab01_ILogger;
using AspNetCoreTest201908.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace UnitTests
{
    public class Lab01Tests
    {
        [Fact]
        public void Logger()
        {
            var lab01Controller = new Lab01Controller(new NullLogger<Lab01Controller>());

            var result = lab01Controller.Index1() as OkObjectResult;
            result.Value.As<AuthResult>().IsAuth.Should().BeTrue();
        } 
        
        [Fact]
        public void Logger2()
        {
//            var testLogger = new Nulllogger<Lab01Controller>();
//            var lab01Controller = new Lab01Controller(testLogger);
//
//            var result = lab01Controller.Index1() as OkObjectResult;
//            result.Value.As<AuthResult>().IsAuth.Should().BeTrue();
//
//            testLogger.Get()[0].Value.Should().Be("log information");
        } 
    }

//    public class TestLogger<T> : ILogger<T>
//    {
//        private FormattedLogValues _formattedLogValues;
//
//        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
//        {
//            _formattedLogValues = state as FormattedLogValues;
//        }
//
//        public FormattedLogValues Get()
//        {
//            return _formattedLogValues;
//        }
//
//        public bool IsEnabled(LogLevel logLevel)
//        {
//            throw new NotImplementedException();
//        }
//
//        public IDisposable BeginScope<TState>(TState state)
//        {
//            throw new NotImplementedException();
//        }
//    }
}