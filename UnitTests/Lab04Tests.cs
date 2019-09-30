using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreTest201908.Api.Lab04_HttpContext;
using AspNetCoreTest201908.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace UnitTests
{
    public class Lab04Tests
    {
        [Fact]
        public void Http01()
        {
            var genericIdentity = new GenericIdentity("Email");
            IHttpContextAccessor httpContext = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(genericIdentity)
                }
            };

            var lab04Controller = new Lab04Controller(httpContext);

            var result = lab04Controller.Index1() as OkObjectResult;

            result.Value.As<AuthResult>().IsAuth.Should().BeTrue();
        }

        [Fact]
        public void Http02()
        {
            var enumerable = new Claim[]
            {
                new Claim(ClaimTypes.Email, "cc@cc.cc"),
                new Claim("MyType", "myType"),
            };
            var claimsIdentities = new ClaimsIdentity[]
            {
                new ClaimsIdentity(enumerable)
            };
            IHttpContextAccessor httpContext = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(claimsIdentities)
                }
            };

            var lab04Controller = new Lab04Controller(httpContext);

            var result = lab04Controller.Index2() as OkObjectResult;

            var authClaim = result.Value.As<AuthClaim>();
            authClaim.Email.Should().Be("cc@cc.cc");
            authClaim.MyType.Should().Be("myType");
        }

        [Fact]
        public void Http03()
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    Session = new TestSession()
                }
            };

            var lab04Controller = new Lab04Controller(httpContext);

            var result = lab04Controller.Index3() as OkObjectResult;
            result.Value.As<AuthUser>().User.Should().Be("cash");
        }

        [Fact]
        public void Http03_2()
        {
            ISession session = Substitute.For<ISession>();

            session.TryGetValue(Arg.Any<string>(), out Arg.Any<byte[]>())
                   .Returns(a =>
                   {
                       a[1] = Encoding.UTF8.GetBytes("cash");
                       return true;
                   });
            IHttpContextAccessor httpContext = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    Session = session
                }
            };

            var lab04Controller = new Lab04Controller(httpContext);

            var result = lab04Controller.Index3() as OkObjectResult;
            result.Value.As<AuthUser>().User.Should().Be("cash");
        }

        [Fact]
        public void Http04()
        {
//            var dictionary = new Dictionary<string, string>
//            {
//                { "user", "cash" }
//            };
//            IHttpContextAccessor httpContext = new HttpContextAccessor
//            {
//                HttpContext = new DefaultHttpContext
//                {
//                    Request =
//                    {
//                        Cookies = new RequestCookieCollection(dictionary)
//                    }
//                }
//            };
//
//            var lab04Controller = new Lab04Controller(httpContext);
//
//            var result = lab04Controller.Index4() as OkObjectResult;
//            result.Value.As<AuthUser>().User.Should().Be("cash");
        }
    }

    public class TestSession : ISession
    {
        public Task LoadAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            value = Encoding.UTF8.GetBytes("cash");
            return true;
        }

        public void Set(string key, byte[] value)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool IsAvailable { get; }
        public string Id { get; }
        public IEnumerable<string> Keys { get; }
    }
}