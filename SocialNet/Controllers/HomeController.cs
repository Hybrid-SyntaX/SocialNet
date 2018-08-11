using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialNet.Data.Interfaces;
using SocialNet.Models;

namespace SocialNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Post> _postRepository;

        public HomeController(IRepository<Post> postRepository)
        {
            this._postRepository = postRepository;
        }
        public IActionResult Index()
        {
            var posts = _postRepository.ReadAll();
            return View(posts);
            //return Json(posts);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
