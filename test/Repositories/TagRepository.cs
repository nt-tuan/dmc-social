using System.Reflection;
using System;
using Xunit;
using DmcSocial.Models;

namespace test.Repositories
{
    public class TagRepository
    {
        [Fact]
        public void New()
        {
            var tag = new Tag("Công nghệ thông tin");
            Assert.Equal("congnghethongtin", tag.NormalizeValue);
        }
    }
}
