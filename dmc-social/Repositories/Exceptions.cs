using System;
namespace DmcSocial.Repositories
{
  public static class TagException
  {
    public static Exception TagNotFound = new Exception("tag-not-found");
    public static Exception TagExisted = new Exception("tag-existed");
  }

  public static class PostException
  {
    public static Exception PostNotFound = new Exception("post-not-found");
    public static Exception PostExisted = new Exception("post-existed");
  }
}