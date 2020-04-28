using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsqMe.Data.Repositories
{
    using Dapper;
    using DapperExtensions;
    using global::AsqMe.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    namespace AsqMe.Data.Repositories
    {
        public class RepositoryBaseOld<TEntity> where TEntity : class
        {
            protected IDbTransaction Transaction { get; private set; }
            protected IDbConnection Connection { get { return Transaction.Connection; } }
            public string TableName { get; }
            public string TableSqlPairs { get; }

            public RepositoryBaseOld(IDbTransaction transaction)
            {
                Transaction = transaction;
                TableName = $"[dbo].[{(typeof(TEntity).Name == "ApplicationUser" ? "AspNetUsers" : typeof(TEntity).Name)}]";
                TableSqlPairs = GetSqlPairsFromEntity();
            }

            public virtual async Task<IEnumerable<TEntity>> FindAllAsync(int limit = int.MinValue)
            {
                //string sql = limit != int.MinValue ? $"SELECT TOP({limit}) * FROM {TableName}" : $"SELECT * FROM {TableName}";
                //return Connection.Query<TEntity>(
                //    sql,
                //    transaction: Transaction
                //).ToList();
                //using (SqlConnection cn = new SqlConnection(_connectionString))
                //{
                //    cn.Open();
                //    var predicate = Predicates.Field<TEntity>(f => f.GetType(), Operator.Eq, true);
                //    IEnumerable<Person> list = Connection.GetList<Person>(predicate);
                //    cn.Close();
                //}
                return await Connection.GetListAsync<TEntity>(transaction: Transaction);
            }

            public virtual async Task<TEntity> FindByIdAsync(int id)
            {
                //return Connection.Query<TEntity>(
                //    $"SELECT * FROM {TableName} WHERE Id = @Id",
                //    param: new { Id = id.ToString() },
                //    transaction: Transaction
                //).FirstOrDefault();
                return await Connection.GetAsync<TEntity>(id: id, transaction: Transaction);
            }
            //public virtual TEntity Get(object properties)
            //{
            //    return Connection.Query<TEntity>(
            //        $"SELECT * FROM {TableName} WHERE Id = @Id",
            //        param: properties,
            //        transaction: Transaction
            //    ).FirstOrDefault();
            //}

            public virtual async Task AddAsync(TEntity entity)
            {
                //entity.Id = Connection.ExecuteScalar<int>(
                //        "INSERT INTO dbo.AspNetUsers(CreatedDate,UpdatedDate,FacebookName,ProfilePicture,ProfilePictureWeb," +
                //        "LockoutEnd,TwoFactorEnabled,PhoneNumberConfirmed,PhoneNumber,ConcurrencyStamp,SecurityStamp," +
                //        "PasswordHash,EmailConfirmed,NormalizedEmail,Email,NormalizedUserName,UserName,LockoutEnabled,AccessFailedCount) " +
                //        "VALUES(@Name); SELECT SCOPE_IDENTITY()",
                //        param: new
                //        {
                //            Name = entity.UserName
                //        },
                //        transaction: Transaction
                //    ).ToString();

                List<string> sqlPairs = TableSqlPairs.Split(",").ToList();
                List<string> sqlPairsStart = new List<string>();
                List<string> sqlPairsEnd = new List<string>();
                foreach (var item in sqlPairs)
                {
                    var itemProp = item.Substring(0, item.IndexOf("=@"));
                    bool cond = entity.GetType().GetProperty(itemProp).GetValue(entity) == null;
                    //|| 
                    //(entity.GetType().GetProperty(itemProp).PropertyType.Name==typeof(int).Name &&
                    //entity.GetType().GetProperty(itemProp).GetValue(entity).ToString() == "0");
                    if (cond)
                        continue;
                    sqlPairsStart.Add(item.Substring(0, item.IndexOf("=@")));
                    sqlPairsEnd.Add(item.Substring(item.IndexOf("=@") + 1));
                }
                var sqlPairsStartString = string.Join(",", sqlPairsStart);
                var sqlPairsEndString = string.Join(",", sqlPairsEnd);
                var id = Connection.ExecuteScalar<int>(
                        $"INSERT INTO {TableName}({sqlPairsStartString}) " +
                        $"VALUES({sqlPairsEndString});" +
                        " SELECT SCOPE_IDENTITY()",
                        param: entity,
                        transaction: Transaction
                    ).ToString();
                entity.GetType().GetProperty("Id")?.SetValue(entity,
                    Convert.ChangeType(id, entity.GetType().GetProperty("Id").PropertyType)
                );
                //return await Connection.InsertAsync<TEntity>(entity, transaction: Transaction);
            }

            public virtual void Update(TEntity entity)
            {
                //var parameters = new DynamicParameters();
                //parameters.Add("@MyParameter", value);

                //Connection.Execute(
                //    $"UPDATE {TableName} " +
                //    $"SET CreatedDate=@CreatedDate,UpdatedDate=@UpdatedDate,FacebookName=@FacebookName," +
                //    "ProfilePicture=@ProfilePicture,ProfilePictureWeb=@ProfilePictureWeb," +
                //    "ConcurrencyStamp=@ConcurrencyStamp,SecurityStamp=@SecurityStamp,NormalizedEmail=@NormalizedEmail," +
                //    "Email=@Email,NormalizedUserName=@NormalizedUserName,UserName= @UserName " +
                //    "WHERE BreedId = @BreedId",
                //    param: entity,
                //    transaction: Transaction
                //);
                var a = 0;
                Connection.Execute(
                    $"UPDATE {TableName} " +
                    $"SET {TableSqlPairs}" + " " +
                    "WHERE Id = @Id",
                    param: entity,
                    transaction: Transaction
                );
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

            public virtual void Delete(TEntity entity)
            {
                Delete(id: typeof(TEntity).Name == "ApplicationUser" ? int.MinValue :
                    int.Parse(entity.GetType().GetProperty("Id").GetValue(entity).ToString()),
                    stringId: typeof(TEntity).Name == "ApplicationUser" ?
                    entity.GetType().GetProperty("Id").GetValue(entity).ToString() : null);
            }

            public enum Comparision
            {
                Equal,
                NotEqual,
                GreaterThan,
                LessThan
            }

            public virtual IList<TEntity> FindBy(Expression<Func<TEntity, object>> predicate, Comparision comparision, object matchObject, int limit = int.MinValue)
            {
                //var (propertyName, propertyValue) = GetSqlPairFromProperty(predicate);
                var propertyName = GetSqlPairNameFromProperty(predicate);
                //return Connection.Query<TEntity>($"SELECT * FROM {TableName}", null).Where(predicate).ToList();
                var list = (IList<object>)matchObject;
                object element = null;
                //foreach (var item in list)
                //{
                //    item.GetType().GetProperty(propertyName).GetValue(item)==
                //}
                var propertyDictionary = new Dictionary<string, object>
            {
                { $"@{propertyName}", matchObject.GetType().GetProperty(propertyName).GetValue(matchObject) }
            };
                if (propertyDictionary[propertyName] == null) return null;
                //var sqlComparision = comparision == Comparision.Equal ? ""

                var parameters = new DynamicParameters(propertyDictionary);
                string sql = limit != int.MinValue ?
                    $"SELECT TOP({limit}) * FROM {TableName} WHERE {propertyName} = @{propertyName}" :
                    $"SELECT * FROM {TableName} WHERE {propertyName} = @{propertyName}";
                return Connection.Query<TEntity>(
                    sql,
                    param: parameters,
                    transaction: Transaction
                ).ToList();
            }

            public static string GetSqlPairNameFromProperty<T, Object>(System.Linq.Expressions.Expression<System.Func<T, Object>> expression)
            {
                List<string> pairs = new List<string>();
                //if (expression.Body.Type.Name is typeof(System.Linq.Expressions.MethodCallExpression).Name)
                //{
                //    var propertyName = ((System.Linq.Expressions.MemberExpression)
                //        ((System.Linq.Expressions.BinaryExpression)expression.Body).Left).Member.Name;
                //    var propertyValue = ((System.Linq.Expressions.ConstantExpression)
                //        ((System.Linq.Expressions.BinaryExpression)expression.Body).Right).Value.ToString();
                //}
                var binaryExpression = ((BinaryExpression)expression.Body);
                //FullConditionalExpression
                var propertyName = ((MemberExpression)binaryExpression.Left).Member.Name;
                var propertyValue = ((System.Linq.Expressions.ConstantExpression)binaryExpression.Right).Value?.ToString();
                //string propertyValue = null;
                //if (binaryExpression.Right is ConstantExpression)
                //{
                //    propertyValue = (binaryExpression.Right as ConstantExpression).Value.ToString();
                //}
                //else if (binaryExpression.Right is MethodCallExpression)
                //{
                //    propertyValue = (((binaryExpression.Right as MethodCallExpression)
                //        .Arguments).FirstOrDefault() as ConstantExpression)?.Value.ToString();
                //}
                //var propertyValue2 = (((binaryExpression.Right as MethodCallExpression)
                //        .).FirstOrDefault() as ConstantExpression)?.Value.ToString();
                //var memberExpression = (System.Linq.Expressions.MemberExpression)binaryExpression.Right;
                //var captureExpression = (System.Linq.Expressions.MemberExpression)memberExpression.Expression;
                //var captureConst = (System.Linq.Expressions.ConstantExpression)captureExpression.Expression;
                //object entity = ((System.Reflection.FieldInfo)captureExpression.Member).GetValue(captureConst.Value);
                //object value = ((System.Reflection.PropertyInfo)memberExpression.Member).GetValue(entity, null);
                //var propertyValue = value.ToString();
                //var expectedValue = ExpressionUtilities.GetValueUsingCompile(binaryExpression);
                //var value = ExpressionUtilities.GetValueWithoutCompiling(binaryExpression);

                //var propertyValue = ((System.Linq.Expressions.ConditionalExpression)binaryExpression.Right).IfTrue.ToString();
                ////var propertyValue2 = ((System.Linq.Expressions.ConditionalExpression)binaryExpression.Right).IfTrue;
                pairs.Add(propertyName);
                pairs.Add(propertyValue ?? "-1");
                return propertyName;
                //if (expression.Body is System.Linq.Expressions.MemberExpression)
                //{
                //    return ((System.Linq.Expressions.MemberExpression)expression.Body).Member.Name;
                //}
                //else if (expression.Body is System.Linq.Expressions.UnaryExpression)
                //{
                //    var op = ((System.Linq.Expressions.UnaryExpression)expression.Body).Operand;
                //    return ((System.Linq.Expressions.MemberExpression)op).Member.Name;
                //}
                //else
                //{
                //    var op = ((System.Linq.Expressions.BinaryExpression)expression.Body).Right;
                //    return ((System.Linq.Expressions.ConstantExpression)op).Value.ToString();
                //}
            }

            private string GetSqlPairsFromEntity()
            {
                var properties = typeof(TEntity).GetProperties();
                List<string> sqlpairs = new List<string>();
                foreach (var property in properties)
                {
                    //if (property.Name == "Id" && typeof(TEntity).Name != "ApplicationUser") continue;
                    //if (property.PropertyType.GetInterface("IEntity") != null ||
                    //    (property.PropertyType.Name == "ApplicationUser")) continue;
                    if (property.Name == "Id" && typeof(TEntity).Name != typeof(Models.ApplicationUser).Name) continue;
                    if (property.PropertyType.GetInterface(typeof(Models.Interfaces.IEntity).Name) != null ||
                        (property.PropertyType.Name == typeof(Models.ApplicationUser).Name)) continue;
                    if (property.PropertyType.IsArray || property.PropertyType.IsGenericList()) continue;
                    //|| property.PropertyType.IsList()
                    var pair = $"{property.Name}=@{property.Name}";
                    //var check = property.PropertyType.IsGenericList();
                    sqlpairs.Add(pair);
                }
                var pairsString = string.Join(",", sqlpairs.ToArray());
                return pairsString;
            }
        }
    }

}
