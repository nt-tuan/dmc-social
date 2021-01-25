using Microsoft.AspNetCore.Http;

namespace ThanhTuan.Blogs.Repositories
{
  public class Authenticate
  {
    private readonly IHttpContextAccessor _contextAccessor;
    public Authenticate(IHttpContextAccessor contextAccessor)
    {
      _contextAccessor = contextAccessor;
    }
    public string GetUser()
    {
      var user = _contextAccessor.HttpContext.Request.Headers["X-Subject"];
      if (string.IsNullOrEmpty(user))
        return "anonymous";
      return user;
    }
  }
}