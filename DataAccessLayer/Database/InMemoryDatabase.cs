using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Database
{
    public class InMemoryDatabase<T> : IDbWrapper<T> where T : DataEntity
    {
        private Dictionary<Tuple<string, string>, DataEntity> DatabaseInstance;

        public InMemoryDatabase()
        {
            DatabaseInstance = new Dictionary<Tuple<string, string>, DataEntity>();
        }

        public async Task<bool> InsertAsync(T data)
        {
            try
            {
                // Offload the synchronous Add operation to a background thread
                await Task.Run(() => DatabaseInstance.Add(Tuple.Create(data.SiteId, data.CompanyCode), data));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T data)
        {
            try
            {

                return await Task.Run(async () =>
                {
                    if (DatabaseInstance.ContainsKey(Tuple.Create(data.SiteId, data.CompanyCode)))
                    {
                        DatabaseInstance.Remove(Tuple.Create(data.SiteId, data.CompanyCode));
                        await InsertAsync(data);
                        return true;
                    }
                    return false;
                });
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                var entities = await Task.Run(() => FindAll());
                return entities.Where(expression.Compile());
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            try
            {
                return await Task.Run(() => DatabaseInstance.Values.OfType<T>());
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            try
            {

                var entities = await FindAllAsync();
                var entity = entities.Where(expression.Compile());

                foreach (var dataEntity in entity)
                {
                    DatabaseInstance.Remove(Tuple.Create(dataEntity.SiteId, dataEntity.CompanyCode));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAll()
        {
            try
            {
                DatabaseInstance.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAllAsync(Expression<Func<T, bool>> filter, string fieldToUpdate, object newValue)
        {
            try
            {
                var entities = await FindAllAsync();
                var entity = entities.Where(filter.Compile());
                var insertTasks = new List<Task>();

                foreach (var dataEntity in entity)
                {
                    var newEntity = UpdateProperty(dataEntity, fieldToUpdate, newValue);

                    DatabaseInstance.Remove(Tuple.Create(dataEntity.SiteId, dataEntity.CompanyCode));
                    insertTasks.Add(InsertAsync(newEntity));
                }
                await Task.WhenAll(insertTasks);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private T UpdateProperty(T dataEntity, string key, object value)
        {
            Type t = typeof(T);
            var prop = t.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop == null)
            {
                throw new Exception("Property not found");
            }

            prop.SetValue(dataEntity, value, null);
            return dataEntity;
        }
        public async Task<IEnumerable<T>> FindAllAsync()
        {
            try
            {
                return await FindAll();
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }

        public Task<bool> DeleteAllAsync()
        {
            return Task.FromResult(DeleteAll());
        }

    }
}