using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TheBlog.DataAccess;
using TheBlog.Models;
using TheBlog.Utilities;

namespace TheBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogDbContext _blogDbContext;

        public HomeController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<IActionResult> Index()
        {
            // Get all blog post and order by date
            var blogPosts = await _blogDbContext.BlogPosts.OrderByDescending(b => b.Date).ToListAsync();

            // Html encodes all input data from user - even though html encoding is performed when the user creates a blog post.
            blogPosts.HtmlEncode();

            return View(blogPosts);
        }

        [Authorize]
        public IActionResult CreateBlogPost()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost(BlogPostEntity blogPost)
        {
            if (ModelState.IsValid == false)
                return View("CreateBlogPost");

            // Html encodes all input data from user.
            blogPost.HtmlEncode();
            blogPost.Date = DateTime.Now;
            await _blogDbContext.AddAsync(blogPost);

            await _blogDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}