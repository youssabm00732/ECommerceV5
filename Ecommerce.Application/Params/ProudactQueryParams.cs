using Ecommerce.Application.Params.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Params
{
    public class ProudactQueryParams
    {
        public int ? barndId{ get; set; } // praramter 1 
        public int ? typeId{ get; set; } // paramter 2 
        public string ? SearchValue { get; set; } // paramter 3 
        public ProudactSortingOptions sort { get; set; } // Paramter 3

        #region Pagination Params 
        public int PageIndex { get; set; } = 1; // l2n mfe4 pageIndex 0 .. bbd2 mn 1 

        // kman fel Pagination mn 7is al Size of data .. ana 3ayz a2ol ll front-end anta lek range mo3yn 34n mytlob4 data kbira aw tdene rkm bel salb msln 
        private const int DefultPageSize = 5; // lw mb3tle4 5als => hb3tlk 5 items 
        private const int MaxPageSize = 10; // lw zwdt 3n 10 m4 hb3tlk 10 bs

        private int _pageSize = DefultPageSize;

        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? DefultPageSize : value);
            // page size .. lw al value aly anta mbasiha akbr mn max = 7ot max = 10 
            // tl3t al value de 22l mn al max .. at2kd hya 22l mn 1 ? lw ah rg3le 5 items bs .. lw l2 rg3le al value 3ady (l2nha htb2a 22l mn 10)
        } 
        #endregion
    }
}
