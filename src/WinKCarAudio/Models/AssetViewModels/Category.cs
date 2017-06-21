using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinKCarAudio.Models.AssetViewModels
{
    public class Category
    {
        /// <summary>
        /// This holds asset category.
        /// </summary>
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        // Navigation properties for many-to-many relationship between
        // Asset and Category.
        // One category can have many assets.
        public ICollection<AssetCategory> AssetCategories { get; set; }

    }
}
