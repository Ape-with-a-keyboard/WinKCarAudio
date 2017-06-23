using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WinKCarAudio.Data;
using WinKCarAudio.Models.AssetViewModels;

namespace WinKCarAudio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Index()
        {
            var viewModel = new AssetIndexData();
            viewModel.Assets = _context.Asset
                .Include(a => a.ImageGallery)
                    .ThenInclude(i => i.Images)
                .Where(m => m.featuredItem == true);

            return View(viewModel);
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
            return View();
        }
    }
}
