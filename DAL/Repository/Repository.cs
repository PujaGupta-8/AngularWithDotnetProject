using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Neosoft_puja_backend.DAL.Interface;
using System.Linq.Expressions;

namespace Neosoft_puja_backend.DAL.Repository
{
    public abstract class Repository<T, Tcontext> : IRepository<T> where T : class where Tcontext : DbContext
    {
        protected readonly Tcontext employeeDbContext = null;

        public Repository(Tcontext context)
        {
            this.employeeDbContext = context;
        }


        public bool IsTransactionRunning()
        {
            return this.employeeDbContext.Database.CurrentTransaction == null ? false : true;
        }
        private IDbContextTransaction BeginTran()
        {
            return this.employeeDbContext.Database.BeginTransaction();
        }


        public IExecutionStrategy GetExecutionStrategy()
        {
            return this.employeeDbContext.Database.CreateExecutionStrategy();
        }


        public IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> result = this.employeeDbContext.Set<T>();
            if (condition != null)
            {
                result = result.Where(condition);
            }

            return result;
        }

        public async Task<ICollection<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> result = this.employeeDbContext.Set<T>();
            if (condition != null)
            {
                result = result.Where(condition);
            }

            return await result.ToListAsync();
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> result = this.employeeDbContext.Set<T>();
            return result;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            IQueryable<T> result = this.employeeDbContext.Set<T>();
            return await result.ToListAsync();
        }

        public T GetSingle(Expression<Func<T, bool>> condition)
        {
            return this.employeeDbContext.Set<T>().Where(condition).FirstOrDefault();
        }
        public async Task<T> GetSingleAysnc(Expression<Func<T, bool>> condition)
        {
            var retValue = await this.employeeDbContext.Set<T>().Where(condition).SingleOrDefaultAsync();

            return retValue;
        }



        public bool Add(T entity)
        {
            this.employeeDbContext.Set<T>().Add(entity);
            return true;
        }

        public bool Update(T entity)
        {
            this.employeeDbContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public bool Delete(T entity)
        {
            this.employeeDbContext.Set<T>().Remove(entity);
            return true;
        }

        public bool RemoveRange(T entity)
        {
            this.employeeDbContext.Set<T>().RemoveRange(entity);
            return true;
        }

        public void SaveChangesManaged()
        {
            this.employeeDbContext.SaveChanges();
            //this.ifmsDbContext.SaveChanges();
        }

    }
}
