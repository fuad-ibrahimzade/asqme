using AsqMe.Data.Models;
using AsqMe.Data.Repositories;
using AsqMe.Helpers;
using AsqMe.Services;
using DapperExtensions.Sql;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AsqMe.Data
{
    public interface IUnitOfWork : IDisposable
    {
        bool CreateDefaults();
        //IApplicationUserRepository ApplicationUserRepository { get; }
        //DapperRepositoryBase<Answer> AnswerRepository { get; }
        RepositoryBase<Answer> AnswerRepository { get; }
        RepositoryBase<Category> CategoryRepository { get; }
        RepositoryBase<Earning> EarningRepository { get; }
        RepositoryBase<Question> QuestionRepository { get; }
        RepositoryBase<QuestionTag> QuestionTagRepository { get; }
        RepositoryBase<Tag> TagRepository { get; }
        RepositoryBase<ApplicationUser> ApplicationUserRepository { get; }
        void Commit();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbConnection _connection_postgresql;
        private IDbTransaction _transaction;
        //private IApplicationUserRepository _ApplicationUserRepository;
        private bool _disposed;

        public UnitOfWork(ConnectionString connectionString,
            IOptions<AppSettings> appSettings,
            UserManager<ApplicationUser> userManager)
        {
            DapperExtensions.DapperExtensions.SqlDialect = new PostgreSqlDialect();
            DapperExtensions.DapperAsyncExtensions.SqlDialect = new PostgreSqlDialect();
            //DapperExtensions.DapperExtensions.DefaultMapper = typeof(DapperClassMapper<>);

            ConnectionString = connectionString;
            //string connectionString
            //string connectionString = AsqMeDbContext.LocalSQLServerConnectionString;
            AppSettings = appSettings;
            UserManager = userManager;
            if (AsqMeDbContext.HerokuPostgreSqlConnectionString == null)
            {
                _connection = new SqlConnection(ConnectionString.Value);
                _connection.Open();
                _transaction = _connection.BeginTransaction();
            }
            else
            {
                _connection_postgresql = new NpgsqlConnection(ConnectionString.Value);
                _connection_postgresql.Open();
                _transaction = _connection_postgresql.BeginTransaction();
            }
                
            resetRepositories();
        }

        //public IApplicationUserRepository ApplicationUserRepository
        //{
        //    get { return _ApplicationUserRepository ?? (_ApplicationUserRepository = new ApplicationUserRepository(_transaction)); }
        //}
        //public DapperRepositoryBase<Answer> AnswerRepository { get; }
        public RepositoryBase<Answer> AnswerRepository { get; set; }
        public RepositoryBase<Category> CategoryRepository { get; set; }
        public RepositoryBase<Earning> EarningRepository { get; set; }
        public RepositoryBase<Question> QuestionRepository { get; set; }
        public RepositoryBase<QuestionTag> QuestionTagRepository { get; set; }
        public RepositoryBase<Tag> TagRepository { get; set; }
        public RepositoryBase<ApplicationUser> ApplicationUserRepository { get; set; }
        public ConnectionString ConnectionString { get; }
        public IOptions<AppSettings> AppSettings { get; }
        public UserManager<ApplicationUser> UserManager { get; }

        public bool CreateDefaults()
        {
            //bool AnswerRepositoryIsEmpty = AnswerRepository.Select().Count() == 0;
            //bool AnswerRepositoryIsEmpty = AnswerRepository.FindBy(item=>item.Description=="zzzz").Count() == 0;
            bool AnswerRepositoryIsEmpty = AnswerRepository.FindAllAsync().GetAwaiter().GetResult().Count() == 0;
            if (AnswerRepositoryIsEmpty == false) return false;
            bool CategoryRepositoryIsEmpty = CategoryRepository.FindAllAsync().GetAwaiter().GetResult().Count() == 0;
            bool EarningRepositoryIsEmpty = EarningRepository.FindAllAsync().GetAwaiter().GetResult().Count() == 0;
            bool QuestionRepositoryIsEmpty = QuestionRepository.FindAllAsync().GetAwaiter().GetResult().Count() == 0;
            bool TagRepositoryIsEmpty = TagRepository.FindAllAsync().GetAwaiter().GetResult().Count() == 0;
            //bool ApplicationUserRepositoryIsEmpty = ApplicationUserRepository.All().Count() == 0;
            bool ApplicationUserRepositoryIsEmpty = ApplicationUserRepository.FindAllAsync().GetAwaiter().GetResult().Count() == 0;
            ////aboutIsEmpty = false;
            //if (CategoryRepositoryIsEmpty == false) return false;
            if (CategoryRepositoryIsEmpty)
            {
                try
                {
                    string[] categoryNames = new string[] { "Categoryname1", "Categoryname2" };
                    foreach (var item in categoryNames)
                    {
                        var newItem = new Category
                        {
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            Name = item,
                            ImageOrIcon = "https://img.icons8.com/nolan/64/bug.png"
                        };
                        CategoryRepository.AddAsync(newItem).GetAwaiter().GetResult();
                    }
                    Commit();
                }
                catch (Exception e)
                {
                    throw e;
                    Console.WriteLine("{0} Exception caught.", e.Message);
                }
            }
            if (TagRepositoryIsEmpty)
            {
                try
                {
                    string[] tagNames = new string[] { "tag1", "tag2", "tag3", "tag4" };
                    foreach (var item in tagNames)
                    {
                        var newItem = new Tag
                        {
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            Name = item
                        };
                        TagRepository.AddAsync(newItem).GetAwaiter().GetResult();
                    }
                    Commit();
                }
                catch (Exception e)
                {
                    throw e;
                    Console.WriteLine("{0} Exception caught.", e.Message);
                }
            }
            if (ApplicationUserRepositoryIsEmpty)
            {
                try
                {
                    string[] userNames = new string[] { "User1", "User2", "User3", "User4" };
                    foreach (var item in userNames)
                    {
                        var newItem = new ApplicationUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            FacebookName = item,
                            ProfilePicture = "SeedPicture",
                            ProfilePictureWeb = "https://img.icons8.com/nolan/64/user.png",
                            ConcurrencyStamp = Guid.NewGuid().ToString(),
                            SecurityStamp = Guid.NewGuid().ToString("N").ToUpper(),
                            NormalizedEmail = item.ToLower() + "@asqme.com",
                            Email = item.ToLower() + "@asqme.com",
                            NormalizedUserName = item.ToLower() + "@asqme.com",
                            UserName = item.ToLower() + "@asqme.com",
                        };
                        //var newItem2 = new ApplicationUser
                        //{
                        //    CreatedDate = DateTime.Now,
                        //    UpdatedDate = DateTime.Now,
                        //    FacebookName = "FacebookName",
                        //    ProfilePicture = "SeedPicture",
                        //    ProfilePictureWeb = "https://img.icons8.com/nolan/64/user.png",
                        //    ConcurrencyStamp = Guid.NewGuid().ToString(),
                        //    SecurityStamp = Guid.NewGuid().ToString(),
                        //    NormalizedEmail = AppSettings.Value.AdminEmail,
                        //    Email = AppSettings.Value.AdminEmail,
                        //    NormalizedUserName = AppSettings.Value.AdminEmail,
                        //    UserName = AppSettings.Value.AdminEmail,
                        //};
                        ApplicationUserRepository.AddAsync(newItem).GetAwaiter().GetResult();
                        //UserManager.CreateAsync(newItem2).GetAwaiter().GetResult();
                    }
                    Commit();
                }
                catch (Exception e)
                {
                    throw e;
                    Console.WriteLine("{0} Exception caught.", e.Message);
                }
            }
            if (QuestionRepositoryIsEmpty)
            {
                try
                {
                    var users = ApplicationUserRepository.FindAllAsync().GetAwaiter().GetResult().ToList();
                    var tags = TagRepository.FindAllAsync().GetAwaiter().GetResult().ToList();
                    var categories = CategoryRepository.FindAllAsync().GetAwaiter().GetResult().ToList();
                    foreach (var item in tags)
                    {
                        var newItem = new Question
                        {
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            Title = "Question Title",
                            Description = "Question Description",
                            ApplicationUserId = users[new Random().Next(0, users.Count() - 1)].Id,
                            SolverAnswerId = int.MinValue,
                            CategoryId = categories[new Random().Next(0, categories.Count() - 1)].Id
                        };
                        QuestionRepository.AddAsync(newItem).GetAwaiter().GetResult();
                        //Commit();
                        //Id = QuestionTagRepository.All().Count() + 1,
                        //CreatedDate = DateTime.Now,
                        //UpdatedDate = DateTime.Now,
                        var newItem2 = new QuestionTag
                        {
                            QuestionId = newItem.Id,
                            TagId = tags[new Random().Next(0, tags.Count() - 1)].Id
                        };
                        QuestionTagRepository.AddAsync(newItem2).GetAwaiter().GetResult();
                    }
                    Commit();
                }
                catch (Exception e)
                {
                    throw e;
                    Console.WriteLine("{0} Exception caught.", e.Message);
                }
            }
            if (AnswerRepositoryIsEmpty)
            {
                var users = ApplicationUserRepository.FindAllAsync().GetAwaiter().GetResult().ToList();
                var questions = QuestionRepository.FindAllAsync().GetAwaiter().GetResult().ToList();
                try
                {
                    foreach (var item in questions)
                    {
                        var newItem = new Answer
                        {
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            QuestionId = item.Id,
                            ApplicationUserId = users[new Random().Next(0, users.Count() - 1)].Id,
                            Description = "Answer Description"
                        };
                        AnswerRepository.AddAsync(newItem).GetAwaiter().GetResult();
                        Commit();
                        item.SolverAnswerId = newItem.Id;
                        QuestionRepository.UpdateAsync(item).GetAwaiter().GetResult();
                        Commit();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    Console.WriteLine("{0} Exception caught.", e.Message);
                }
            }
            if (EarningRepositoryIsEmpty)
            {
                try
                {
                    var users = ApplicationUserRepository.FindAllAsync().GetAwaiter().GetResult().ToList();
                    //var earningsTypes = new string[] { "Received", "Sent" };
                    var earningsTypes = Enum.GetValues(typeof(Earning.EarningType));
                    foreach (var item in users)
                    {
                        foreach (var item2 in users)
                        {
                            var newItem = new Earning
                            {
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                EarningsType = (Earning.EarningType)earningsTypes.GetValue(new Random().Next(earningsTypes.Length)),
                                Amount = 1m,
                                SenderId = users.Where(u => u.Id != item.Id).ToList()
                                    [new Random().Next(0, users.Count() - 2)].Id,
                                ApplicationUserId = item.Id
                            };
                            EarningRepository.AddAsync(newItem).GetAwaiter().GetResult();
                        }
                        Commit();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    Console.WriteLine("{0} Exception caught.", e.Message);
                }
            }
            //Commit();
            if (CategoryRepositoryIsEmpty || TagRepositoryIsEmpty || ApplicationUserRepositoryIsEmpty ||
                QuestionRepositoryIsEmpty || AnswerRepositoryIsEmpty || EarningRepositoryIsEmpty)
            {
                return true;
            }
            else
                return false;
        }



        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                if(AsqMeDbContext.HerokuPostgreSqlConnectionString==null)
                    _transaction = _connection.BeginTransaction();
                else
                    _transaction = _connection_postgresql.BeginTransaction();
                resetRepositories();
            }
        }

        private void resetRepositories()
        {
            //_ApplicationUserRepository = null;
            AnswerRepository = null;
            CategoryRepository = null;
            EarningRepository = null;
            QuestionRepository = null;
            QuestionTagRepository = null;
            TagRepository = null;
            ApplicationUserRepository = null;

            //AnswerRepository = new DapperRepositoryBase<Answer>(_transaction);
            AnswerRepository = new RepositoryBase<Answer>(_transaction);
            CategoryRepository = new RepositoryBase<Category>(_transaction);
            EarningRepository = new RepositoryBase<Earning>(_transaction);
            QuestionRepository = new RepositoryBase<Question>(_transaction);
            QuestionTagRepository = new RepositoryBase<QuestionTag>(_transaction);
            TagRepository = new RepositoryBase<Tag>(_transaction);
            ApplicationUserRepository = new RepositoryBase<ApplicationUser>(_transaction);
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                    if (_connection_postgresql != null)
                    {
                        _connection_postgresql.Dispose();
                        _connection_postgresql = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
