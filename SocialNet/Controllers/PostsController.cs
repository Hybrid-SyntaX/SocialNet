using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNet.Data;
using SocialNet.Data.Interfaces;
using SocialNet.Models;

namespace SocialNet.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository userRepository;

        //private readonly UserManager<ApplicationUser> _usersManager;
        private readonly IRepository<Post> repository;
        //private readonly IPostRepository _postRepository;

        public PostsController(ApplicationDbContext context,
            IUserRepository userRepository,
            //IPostRepository postRepository
            IRepository<Post> postRepository
            )
        {
            _context = context;
            this.userRepository = userRepository;
            ///TODO: Replace with IUserRepoisotry
            //_usersManager = userRepository;
            repository = postRepository;
        }


        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var posts = await repository.ReadAll().Include(p => p.OriginalPoster).ToListAsync();
            //var posts = await _context.Posts.ToListAsync();
            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = repository.ReadAll().Include(p => p.OriginalPoster);
            //_context.Set<Post>().Include
            //var post = await repository.ReadAsync(id);
            var post = await posts.SingleOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content", "OriginalPoster")] Post post)
        {

            //try
            //{
            //ModelState.MarkFieldValid(nameof(post.OriginalPoster));

            //OriginalPoster is set manually
            ModelState.Remove(nameof(post.OriginalPoster));

            if (ModelState.IsValid)
            {

                post.OriginalPoster = await userRepository?.GetUserAsync(HttpContext?.User);
                if (post.OriginalPoster == null) return Unauthorized();

                post.LastUpdated = DateTime.Now;
                post.Created = DateTime.Now;
                await repository.CreateAsync(post);
                //await _postRepository.CreatePostAsync(post);
                return RedirectToAction(nameof(Index));
            }
            //}
            //catch
            else
            {
                return Json(ModelState);
                //return View(post);
            }
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("Id,Title,Content")] Post post)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //post.OriginalPoster =  userRepository.GetUserAsync(HttpContext.User);
        //        _postRepository.CreatePost(post);

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(post);
        //}


        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Content")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(Guid id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }


    }
}
