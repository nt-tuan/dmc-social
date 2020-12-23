using System.Reflection;
using System;
using Xunit;
using ThanhTuan.Blogs.Entities;

namespace Test.Repositories
{
  public class TagRepository
  {
    [Fact]
    public void New()
    {
      var tag = new Tag("Chuỗi", "admin");
      Assert.Equal("chuoi", tag.Slug);
    }
  }
}
