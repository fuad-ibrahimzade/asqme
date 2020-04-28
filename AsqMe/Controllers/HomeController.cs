using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AsqMe.Data.Models;
using AsqMe.Data;
using DapperExtensions;
using AsqMe.Data.ViewModels;

namespace AsqMe.Controllers
{
    [Route("/[action]")]
    public class HomeController : Controller, IDisposable
    {
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SerializedData { get; set; }
        public IUnitOfWork UnitOfWork { get; }
        private readonly ILogger<HomeController> _logger;
        private bool disposed;
        public dynamic AllData { get; set; }
        public HomeController(ILogger<HomeController> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            UnitOfWork = unitOfWork;
        }

        [Route("/")]
        [Route("/[action]")]
        public async Task<IActionResult> IndexAsync()
        {

            var users = await UnitOfWork.ApplicationUserRepository.FindAllAsync();
            var categories = await UnitOfWork.CategoryRepository.FindAllAsync();
            var questions = await UnitOfWork.QuestionRepository.FindAllAsync();
            List<QuestionTag> questionTags = new List<QuestionTag>();
            List<Answer> answers = new List<Answer>();
            foreach (var item in questions)
            {
                item.ApplicationUser = (await UnitOfWork.ApplicationUserRepository.FindByAsync(u=>u.Id,Operator.Eq,item.ApplicationUserId))
                    .FirstOrDefault();
                item.Category = (await UnitOfWork.CategoryRepository.FindByAsync(u => u.Id, Operator.Eq, item.CategoryId))
                    .FirstOrDefault();
                var qts = await UnitOfWork.QuestionTagRepository.FindByAsync(item => item.QuestionId, Operator.Eq, item.Id);
                foreach (var itemq in qts)
                {
                    itemq.Question = item;
                    itemq.Tag = (await UnitOfWork.TagRepository.FindByAsync(u=>u.Id,Operator.Eq,itemq.TagId))
                    .FirstOrDefault();
                    questionTags.Add(itemq);
                }
                var ans = await UnitOfWork.AnswerRepository.FindByAsync(item => item.QuestionId, Operator.Eq, item.Id);
                foreach (var itema in ans)
                {
                    itema.ApplicationUser = (await UnitOfWork.ApplicationUserRepository.FindByAsync(u => u.Id, Operator.Eq, itema.ApplicationUserId))
                    .FirstOrDefault();
                    answers.Add(itema);
                }
            }
            List<Tag> tags = new List<Tag>();
            foreach (var item in questionTags)
            {
                tags.Add(await UnitOfWork.TagRepository.FindByIdAsync(item.TagId));
            }
            var earnings = await UnitOfWork.EarningRepository.FindAllAsync();
            foreach (var item in earnings)
            {
                item.ApplicationUser = (await UnitOfWork.ApplicationUserRepository.FindByAsync(u => u.Id, Operator.Eq, item.ApplicationUserId))
                    .FirstOrDefault();
            }
            IndexViewModel model = new IndexViewModel()
            {
                Answers = answers,
                Categories = categories,
                Questions = questions,
                QuestionTags = questionTags,
                Tags = tags,
                Users = users,
                Earnings = earnings
            };


            if (ErrorMessage != null)
                ModelState.AddModelError(string.Empty, ErrorMessage);
            if (SerializedData != null)
            {
                dynamic data = Newtonsoft.Json.Linq.JObject.Parse(SerializedData);
                ViewBag.SerializedData = data;
                //ViewBag.LayoutViewModel = data;
                //return View(data);
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/[action]/{categoryName}")]
        public IActionResult Categories(string categoryName)
        {
            return View("Questions");
        }

        public IActionResult Categories()
        {
            return View("Categories");
        }
        [Route("/[action]/{id}")]
        public IActionResult Questions(int id)
        {
            return View("Question");
        }
        public IActionResult Questions()
        {
            return View();
        }
        [Route("/[action]/{tagName}")]
        public IActionResult Tags(string tagName)
        {
            return View("Questions");
        }
        public IActionResult Tags()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }
        [Route("/users/{id}/answers")]
        public IActionResult UserAnswers(int id)
        {
            return View();
        }
        [Route("/users/{id}/earnings")]
        public IActionResult UserEarnings(int id)
        {
            return View();
        }
        [Route("/users/{id}")]
        [Route("/users/{id}/questions")]
        public IActionResult UserQuestions(int id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                UnitOfWork.Dispose();
            }

            disposed = true;
            base.Dispose(disposing);
        }
    }
}
