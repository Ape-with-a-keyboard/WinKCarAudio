using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WinKCarAudio.Models.AssetViewModels
{
    public class Asset
    {
        public Asset() { }

        /// <summary>
        /// Contructor for asset
        /// </summary>
        /// <param name="AssetID"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="AddData"></param>
        /// <param name="Price"></param>
        /// <param name="PriceDaily"></param>
        /// <param name="PriceWeekly"></param>
        /// <param name="PriceMonthly"></param>
        /// <param name="OwnerID"></param>
        /// <param name="Location"></param>
        /// <param name="Request"></param>
        /// <param name="FeaturedItem"></param>
        public Asset(int AssetID, string Name, string Description, DateTime AddData, decimal Price, bool FeaturedItem)
        {
            assetID = AssetID;
            name = Name;
            description = Description;
            addDate = AddData;
            price = Price;
            featuredItem = FeaturedItem;
        }

        [Key]
        [Display(Name = "Asset ID")]
        public int assetID { get; set; }
        [Required]
        [Display(Name = "Asset Name")]
        public string name { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "Avaliable Date")]
        [DataType(DataType.Date)]
        public DateTime addDate { get; set; }
        [Range(0, int.MaxValue)]
        [Display(Name = "Sale Price")]
        public decimal price { get; set; }
        public bool featuredItem { get; set; }


        /// Navigation properties for one-to-many relationship between
        // Asset and Make.
        // One asset can only have one make, but many makes can be shared by
        // multiple assets.
        public int MakeId { get; set; }
        [ForeignKey("MakeId")]
        public Make Make { get; set; }

        // Navigation properties for many-to-many relationship between
        // Asset and Category.
        // One asset can have many categories (one parent and one child).
        public ICollection<AssetCategory> AssetCategories { get; set; }

        // Navigation properties for one-to-one relationship between Asset and ImageGallery.
        [ForeignKey("ImageGalleryId")]
        public int? ImageGalleryId { get; set; }
        public ImageGallery ImageGallery { get; set; }

    }
}
