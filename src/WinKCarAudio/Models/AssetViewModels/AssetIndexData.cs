﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinKCarAudio.Models.AssetViewModels
{
    public class AssetIndexData
    {
        public Asset Asset { get; set; }
        public IEnumerable<Asset> Assets { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Image> Images { get; set; }
    }
}
