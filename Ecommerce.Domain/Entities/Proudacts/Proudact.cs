using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Proudacts
{
    public class Proudact:BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        #region Navigational Property 
        public ProudactBrand Brand { get; set; } = null;
        public ProudactTypes Type { get; set; } = null;
        #endregion

        #region Forgin Keys 
        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        [ForeignKey("Type")]
        public int TypeId { get; set; }
        #endregion
    }
}
