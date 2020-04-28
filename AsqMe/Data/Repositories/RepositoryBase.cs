using AsqMe.Helpers;
using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AsqMe.Data.Repositories
{
    public class RepositoryBase<TEntity> where TEntity : class
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }
        public string TableName { get; }
        public string TableSqlPairs { get; }

        public RepositoryBase(IDbTransaction transaction)
        {
            Transaction = transaction;
            TableName = $"[dbo].[{(typeof(TEntity).Name == "ApplicationUser" ? "AspNetUsers" : typeof(TEntity).Name)}]";
        }
        class RandomSort : ISort
        {
            public string PropertyName { get; set; } = "Id";
            public bool Ascending { get; set; } = new Random().Next(0, 1) == 0 ? true : false;
        }
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(int limit = int.MinValue)
        {
            if (limit != int.MinValue)
            {
                var predicateObject = Predicates.Field<TEntity>(item=>item.GetType().GetProperty("Id"), Operator.Eq, ((Stack<int>)Enumerable.Range(1, limit)).Pop());
                return await Connection.GetListAsync<TEntity>(predicate: predicateObject, transaction: Transaction);
                //IList<ISort> sort = (IList < ISort >)new List<ISort>();
                //for (int i = 0; i < limit; i++)
                //{
                //    sort.Add(new RandomSort());
                //}
                //return await Connection.GetSetAsync<TEntity>(predicate: null, sort: sort, maxResults: limit, transaction: Transaction);
            }
            //var predicateObject = new GetMultiplePredicate();
            //predicateObject.Add<TEntity>(Predicates.Field<TEntity>(item=> item, Operator.Eq,null,true));

            return await Connection.GetListAsync<TEntity>(transaction: Transaction);
        }

        public virtual async Task<TEntity> FindByIdAsync(int id)
        {
            return await Connection.GetAsync<TEntity>(id: id, transaction: Transaction);
        }

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, object>> predicateExpression,
            Operator op, object value, bool not = false)
        {
            var predicateObject = Predicates.Field<TEntity>(predicateExpression, op, value, not);
            return await Connection.GetListAsync<TEntity>(predicateObject, transaction: Transaction);
        }

        public virtual async Task<dynamic> AddAsync(TEntity entity)
        {
            return await Connection.InsertAsync<TEntity>(entity, transaction: Transaction);
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            return await Connection.UpdateAsync<TEntity>(entity, transaction: Transaction);
        }

        public virtual void Delete(int id = int.MinValue, string stringId = null)
        {
            string localId = id.ToString();
            if (stringId != null)
                localId = stringId;
            Connection.Execute(
                $"DELETE FROM {TableName}" +
                $" WHERE Id = @Id",
                param: new { Id = localId },
                transaction: Transaction
            );
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            return await Connection.DeleteAsync<TEntity>(entity, transaction: Transaction);
        }



    }
}
