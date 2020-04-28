using AsqMe.Data.Models;
using AsqMe.Helpers;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsqMe.Data
{
    public static class DapperMappings
    {
        public static bool IsSqlServer { get; set; }
        public static void Initialize()
        {
            IsSqlServer = AsqMeDbContext.HerokuPostgreSqlConnectionString == null && AsqMeDbContext.LocalSQLServerConnectionString != null;
            DapperExtensions.DapperExtensions.SqlDialect = new PostgreSqlDialect();
            DapperExtensions.DapperAsyncExtensions.SqlDialect = new PostgreSqlDialect();
            //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            DapperExtensions.DapperExtensions.DefaultMapper = typeof(CustomPluralizedMapper<>);

            DapperExtensions.DapperExtensions.SetMappingAssemblies(new[]
            {
            typeof(DapperMappings).Assembly
            });
        }

        public class CustomPluralizedMapper<T> : PluralizedAutoClassMapper<T>
        where T : class
        {

        }

        public class AnswerMapper : ClassMapper<Answer>
        {
            public AnswerMapper()
            {
                if (IsSqlServer) Schema("dbo");
                else Schema("public");
                TableName = typeof(Answer).Name.ToLower();
                Table(TableName);
                Map(x => x.Question).Ignore();
                Map(x => x.ApplicationUser).Ignore();
                AutoMap();
            }
        }
        public class ApplicationUserMapper : ClassMapper<ApplicationUser>
        {
            //public new string TableName { get; }
            public ApplicationUserMapper()
            {
                if (IsSqlServer) Schema("dbo");
                else Schema("public");
                TableName = "AspNetUsers".ToLower();
                Table(TableName);
                AutoMap();
            }
        }
        public class EarningMapper : ClassMapper<Earning>
        {
            public EarningMapper()
            {
                if (IsSqlServer) Schema("dbo");
                else Schema("public");
                TableName = typeof(Earning).Name.ToLower();
                Table(TableName);
                Map(x => x.ApplicationUser).Ignore();
                AutoMap();
            }
        }
        public class QuestionMapper : ClassMapper<Question>
        {
            public QuestionMapper()
            {
                if (IsSqlServer) Schema("dbo");
                else Schema("public");
                TableName = typeof(Question).Name.ToLower();
                Table(TableName);
                Map(x => x.ApplicationUser).Ignore();
                Map(x => x.Category).Ignore();
                Map(x => x.QuestionTags).Ignore();
                AutoMap();
            }
        }
        public class QuestionTagMapper : ClassMapper<QuestionTag>
        {
            public QuestionTagMapper()
            {
                if (IsSqlServer) Schema("dbo");
                else Schema("public");
                TableName = typeof(QuestionTag).Name.ToLower();
                Table(TableName);
                Map(x => x.Question).Ignore();
                Map(x => x.Tag).Ignore();
                Map(x => x.QuestionId).Key(KeyType.NotAKey);
                Map(x => x.TagId).Key(KeyType.NotAKey);
                AutoMap();
            }
        }
        public class TagMapper : ClassMapper<Tag>
        {
            public TagMapper()
            {
                if (IsSqlServer) Schema("dbo");
                else
                {
                    Schema("public");
                }
                TableName = typeof(Tag).Name.ToLower();
                Table(TableName);
                Map(x => x.QuestionTags).Ignore();
                AutoMap();
            }
        }
    }
}
