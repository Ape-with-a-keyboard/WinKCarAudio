using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using WinKCarAudio.Data;
using WinKCarAudio.Models;
using WinKCarAudio.Models.AssetViewModels;

namespace WinKCarAudio.Controllers
{
    public class AssetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _env;
        public AssetsController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, IHostingEnvironment env
        )

        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        
        /*============================ */


        // GET: Assets - Filtered
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? id, int? assetId, string searchString, string sortby, bool recent, bool accessory, string assetLocation, bool featuredItem, int Categoryid, string mainCategoryname, int Makeid)
        {
            var viewModel = new AssetIndexData();
            viewModel.Assets = await _context.Asset
                .Include(a => a.AssetCategories)
                    .ThenInclude(a => a.Category)
                .Include(a => a.Make)
                .Include(a => a.ImageGallery)
                    .ThenInclude(a => a.Images)
                .AsNoTracking()
                .OrderBy(a => a.addDate)
                .ToListAsync();
            viewModel.Categories = _context.Category;
            viewModel.Makes = _context.Make;

            // default set to only assets first 
            //viewModel.Assets = viewModel.Assets.Where(s => s.request.Equals(false));

            if (Categoryid > 0)
            {
                viewModel.Assets = viewModel.Assets.Where(b => b.AssetCategories.Any(s => s.CategoryId == Categoryid));
            }
            if (!String.IsNullOrEmpty(mainCategoryname))
            {
                viewModel.Assets = viewModel.Assets.Where(b => b.AssetCategories.Any(s => s.Category.CategoryName == mainCategoryname));
            }

            if (Makeid > 0)
            {
                viewModel.Assets = viewModel.Assets.Where(b => b.MakeId == Makeid);
            }
            // Assign a city to the asset
            if (id != null)
            {
                ViewData["AssetID"] = id.Value;
                Asset asset = viewModel.Assets.Where(
                    a => a.assetID == id.Value).Single();
                viewModel.Categories = asset.AssetCategories.Select(s => s.Category);
            }
            if (featuredItem == true)
            {
                viewModel.Assets = viewModel.Assets.Where(s => s.featuredItem == true);
            }

            if (recent == true)
            {
                viewModel.Assets = viewModel.Assets.OrderByDescending(s => s.addDate);
            }
            //if (!String.IsNullOrEmpty(assetLocation))
            //{
            //    viewModel.Assets = viewModel.Assets.Where(x => x.Address == assetLocation);
            //}

            if (!String.IsNullOrEmpty(searchString))
            {
                viewModel.Assets = viewModel.Assets.Where(x => x.name.Contains(searchString));
            }

            SetCategoryViewBag();

            return View(viewModel);
        }

        /*============================= */


        /// <summary>
        /// GET: Assets/Details/5
        /// </summary>
        /// <param name="id">info of a item id</param>
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Asset
                .Include(a => a.Make)
                .Include(a => a.AssetCategories)
                    .ThenInclude(a => a.Category)
                .Include(a => a.ImageGallery)
                    .ThenInclude(a => a.Images)
                .SingleOrDefaultAsync(m => m.assetID == id);
            if (asset == null)
            {
                return NotFound();
            }

            HttpContext.Response.Cookies.Append("assetId", id.ToString(),
                new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    Secure = false
                }
            );
            if (asset.ImageGalleryId != null)
            {
                HttpContext.Response.Cookies.Append("imageGallaryId", asset.ImageGalleryId.ToString(),
                    new CookieOptions()
                    {
                        Path = "/",
                        HttpOnly = false,
                        Secure = false
                    }
                );
            }

            SetCategoryViewBag(asset.AssetCategories);
            SetMakeViewBag();

            return View(asset);
        }
        /// <summary>
        /// GET: Assets/Create
        /// </summary>
        public IActionResult Create()
        {
            // Populate asset categories
            SetCategoryViewBag();
            // Populate asset makes
            SetMakeViewBag();

            Asset asset = new Asset();
            asset.price = (decimal)0.00;
            return View(asset);
        }
        /// <summary>
        /// POST: Assets/Create
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="asset">binding view data for asset</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("assetID,addDate,description,name,price,AssetCategories")]
            Asset asset)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            if (ModelState.IsValid)
            {
                var userId = user?.Id;
                var userName = user?.UserName;
                DateTime today = DateTime.Now;
                asset.addDate = today;

                // Assign make to asset
                var myMakeId = HttpContext.Request.Form["Makes"];
                var myMakeIdNumVal = int.Parse(myMakeId);
                asset.MakeId = myMakeIdNumVal;

                /* === Add Images === */
                // Get images from form
                var uploadedFiles = HttpContext.Request.Form.Files;
                // Get the wwwroot folder
                var webRootPath = _env.WebRootPath;
                // Set assets image folder
                var uploadsPath = Path.Combine(webRootPath, "images\\uploads\\assets");

                // Create Image Gallery to hold images only when there is
                // at least one image uploaded
                ImageGallery ImageGallery;
                int ImageGalleryId = -1;

                if (uploadedFiles.Count > 0)
                {
                    ImageGallery = new ImageGallery();
                    ImageGallery.Title = "My cool gallery";
                    // Add Image gallery to DB
                    _context.Add(ImageGallery);
                    await _context.SaveChangesAsync();
                    //Get Id of recently added Image Gallery
                    ImageGalleryId = ImageGallery.ImageGalleryId;

                    foreach (var uploadedFile in uploadedFiles)
                    {
                        if (uploadedFile != null && uploadedFile.Length > 0)
                        {
                            var file = uploadedFile;
                            if (file.Length > 0)
                            {
                                // 1) Add image to DB
                                Image Image = new Image();
                                Image.ImageGalleryId = ImageGalleryId;
                                Image.FileLink = Path.Combine("images/uploads/assets/", file.FileName);
                                _context.Add(Image);
                                await _context.SaveChangesAsync();

                                // 2) Get Id or Guid of recently added image from DB
                                //int ImageId = Image.ImageId;
                                Guid ImageGuid = Image.ImageGuid; // Better

                                // 3) Save image to disk with Guid
                                var fileName = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition).FileName.Trim('"');
                                // 3.1) Get File extension from file
                                string fileExtesion = Path.GetExtension(fileName);
                                // 3.2) Change file name to Guid
                                fileName = ImageGuid + fileExtesion;
                                Console.WriteLine(fileName);
                                // 3.3) Save image to disk with new file name
                                using (var fileStream = new FileStream(Path.Combine(uploadsPath, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }
                                // 4) Update FileLink in DB 
                                Image.FileLink = Path.Combine("images/uploads/assets/", fileName);
                                await _context.SaveChangesAsync();

                            }
                        }
                    }
                }

                // Attach Gallery to Asset if a new gallery is created
                if (ImageGalleryId != -1)
                {
                    asset.ImageGalleryId = ImageGalleryId;
                }

                // Save asset to DB
                _context.Add(asset);
                await _context.SaveChangesAsync();

                // Get the category from select form
                var myCategoryId = HttpContext.Request.Form["AssetCategories"];
                var myCategoryIdNumVal = int.Parse(myCategoryId);

                // Assign a category to the asset
                AssetCategory AssetCategory = new AssetCategory();
                AssetCategory.AssetId = asset.assetID;
                AssetCategory.CategoryId = myCategoryIdNumVal;

                // Save asset category to DB
                _context.AssetCategory.Add(AssetCategory);

                // Save categories to DB
                await _context.SaveChangesAsync();

                // Save asset to DB
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            SetCategoryViewBag(asset.AssetCategories);
            SetMakeViewBag();
            return View(asset);
        }

        /// <summary>
        /// GET: Assets/Delete/5
        /// Used for delete item from server
        /// </summary>
        /// <param name="id">check if this item can be delete base on id</param> 
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Asset.SingleOrDefaultAsync(m => m.assetID == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        /// <summary>
        /// POST: Assets/Delete/5
        /// Used for delete item from server
        /// </summary>
        /// <param name="id">delete item by this id</param> 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            var asset = await _context.Asset.SingleOrDefaultAsync(m => m.assetID == id);
            _context.Asset.Remove(asset);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// GET: Asset/Check
        /// Used for delete item from server
        /// </summary>
        /// <param name="id">delete item by this id</param> 
        private bool AssetExists(int id)
        {
            return _context.Asset.Any(e => e.assetID == id);
        }

        // Get all categories from the database
        private void SetCategoryViewBag(ICollection<AssetCategory> AssetCategories = null)
        {

            if (AssetCategories == null)
            {
                ViewBag.AssetCategories = new SelectList(_context.Category,"CategoryId", "CategoryName");
            }
            else
            {
                ViewBag.AssetCategories = new SelectList(_context.Category.AsEnumerable(), "CategoryId", "CategoryName", AssetCategories);
            }
        }



        // Get all the makes from the database
        private void SetMakeViewBag(ICollection<Make> Makes = null)
        {

            if (Makes == null)

                ViewBag.Makes = new SelectList(_context.Make, "MakeId", "Name");

            else
                ViewBag.Makes = new SelectList(_context.Make.AsEnumerable(), "MakeId", "Name", Makes);
        }

    }
}