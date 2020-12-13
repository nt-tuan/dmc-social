using System.Linq;
using System.Threading.Tasks;
using DmcSocial.Models;
using DmcSocial.Repositories;

namespace DmcSocial.Interfaces
{
  interface IRepository
  {
    void Add<T>(T entity, string actor) where T : BaseEntity;
    void Update<T>(T entity, string actor) where T : BaseEntity;
    void Delete<T>(T entity, string actor) where T : BaseEntity;
    IQueryable<T> GetQuery<T>() where T : BaseEntity;
  }
}