using AsqMe.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AsqMe.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<ApplicationUser> WithoutPasswords(this IEnumerable<ApplicationUser> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static ApplicationUser WithoutPassword(this ApplicationUser user)
        {
            //typeof(ApplicationUser).GetProperty
            if (user.GetType().GetProperty("Password") != null)
                user.GetType().GetProperty("Password").SetValue(user, null);
            if (user.GetType().GetProperty("PasswordHash") != null)
                //user.PasswordHash = null;
                user.GetType().GetProperty("PasswordHash").SetValue(user, null);
            return user;
        }

        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Any());
            }
            return null;
        }

        public static string GetHerokuPostgreSQLConnectionString(this IServiceCollection services)
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            //databaseUrl = "";
            //databaseUrl = "";
            if (String.IsNullOrEmpty(databaseUrl)) return null;

            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = Npgsql.SslMode.Prefer,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        //public static bool IsList(this Type type)
        //{
        //    //, object o
        //    if (type == null) return false;
        //    return type is IList &&
        //           type.GetType().IsGenericType;
        //           //type.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        //}

        public static bool IsGenericList(this Type type)
        {
            //, object o
            if (type == null) return false;
            return type.IsGenericType && type.GetGenericTypeDefinition().Name == typeof(List<>).Name;
        }

        //public static object BinaryConditionalExpressionResult<T>(this Expression<Func<T,bool>> expression)
        //{
        //    ConditionalExpression right = (ConditionalExpression)((BinaryExpression)expression.Body).Right;
        //    //res = prop.GetValue(obj);
        //    //var val = Expression.Lambda(expression.Body).Compile().DynamicInvoke();
        //    //MemberExpression memberExpression = (MemberExpression)((BinaryExpression)expression.Body);
        //    var ce = right;
        //    //var cond = ce.Test;
        //    var me = Expression.Lambda(ce.IfTrue).Compile().DynamicInvoke();
        //    //right.IfTrue

        //    return Expression.Lambda(expression).Compile().DynamicInvoke();
        //}

        //public static Type GetDeclaredType<T>(this T obj)
        //{
        //    return typeof(T);
        //}

    }
}
