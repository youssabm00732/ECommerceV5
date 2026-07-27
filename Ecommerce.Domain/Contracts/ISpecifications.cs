using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface ISpecifications<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        // Expression for Include() -> include(p=>p.brand) 
        List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } // y3ne htst2bl type mn no3 TEntity w ht retrun 7aga mn no3 object
                                                                     // w da hyb2a al Expression (paramter) aly htst2blo mny Function al Linq aly hya Include() 
                                                                     // wna hst5dm al Include() aktr mn mra l2n fel Proudact feh mra for type w mra for brand .. fa ana al paramter (Expression aly 3amlo hna da) mra hykon by return (object) type , w mra (object) Brands .. 34an kda 7tet al Expression signature de f list<>
                                                                     // w 5ltha get mn 8ir set .. 34an ana 3ayz a3mlha set f mkan mo3yn b4kl Private 

        // Expression for Where() -> where(p=>p.Id)
        Expression<Func<TEntity, bool>> Criteria { get; }

        //  Expression for Sorting 
        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDesc { get; }

        // Expression For Pagenation 
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; } // for any m4 dymn hst5dm al Pagination .. f7gat zy al Brand wel Type asln 3ddhom m4 kbir ka data .. fa lw b true = make Take() and Skip() 
    }
}
