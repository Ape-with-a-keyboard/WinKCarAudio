﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WinKCarAudio.Models.AssetViewModels
{
    /// <summary>
    /// The Image model contains an imagegallery id, because an asset can contain 
    /// more than one image. 
    /// </summary>
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ImageGuid { get; set; }
        public int? ImageGalleryId { get; set; }
        public string Title { get; set; }
        [Required]
        public string FileLink { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool isMain { get; set; }

        // Navigation properties for one-to-many relationship between Image and ImageGallery.
        // One image has only one gallery
        public ImageGallery ImageGallery { get; set; }
    }
}
