using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ThanhTuan.Blogs.Entities
{
  // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
  public abstract class BaseEntity : IEqualityComparer<BaseEntity>, IComparable<BaseEntity>
  {
    public static void OnModelCreating<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
      modelBuilder.Entity<T>().HasIndex(u => u.DateRemoved);
      modelBuilder.Entity<T>().HasIndex(u => u.LastModifiedTime);
    }
    public enum ListOrder { ASC = 1, DESC = 0 }
    public int Id { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? DateRemoved { get; set; }
    public string RemovedBy { get; set; }
    public DateTimeOffset LastModifiedTime { get; set; }
    public string LastModifiedBy { get; set; }
    public BaseEntity()
    {
      var now = DateTimeOffset.Now;
      LastModifiedTime = now;
      DateCreated = now;
    }
    public BaseEntity(string actor) : base()
    {

      LastModifiedBy = actor;

      CreatedBy = actor;
    }
    public virtual bool Equals(BaseEntity x, BaseEntity y)
    {
      return x.Id == y.Id;
    }

    public virtual int GetHashCode(BaseEntity obj)
    {
      return obj.GetHashCode();
    }

    public virtual int CompareTo(BaseEntity obj)
    {
      return Comparer<int>.Default.Compare(Id, obj.Id);
    }

    public virtual void UpdateFrom(BaseEntity entity)
    {

    }
  }
}