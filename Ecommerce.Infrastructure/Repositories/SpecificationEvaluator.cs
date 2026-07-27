using Ecommerce.Domain.Common;
using Ecommerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, Tkey>(IQueryable<TEntity> InputQuery , ISpecifications<TEntity , Tkey> specifications) 
             where TEntity : BaseEntity<Tkey>
        {
            var  query = InputQuery;

            // check an feh Expressions wla la 
            if (specifications.IncludeExpressions.Count > 0)
            {
                query = specifications.IncludeExpressions.Aggregate(query, (Current, Expression) => Current.Include(Expression));
                // hnd2 arme al Query (mgmo3t al tables dbcontext.set<t>() .. y7otha fel Current .. w ymsk awl Expression gwa al list bta3te(IncludeExpressions).. a7ot al Expression da f anhe function ? = al include())
                // ybd2 y5zn  => Current.Include(Expression) fel Current .. b3d kda y4of al next Expression aly 3nde fel List(IncludeExpressions).. a7ot al Expression da fel Include()
                // fyb2a al 4kl al nha2i .. DbContext.set<T>.Include(brand).Include(Type)  
            }

            // specification for Where() 
            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria); // y3ne al query bta3te hytb2 3leha al Where(Func<TEntity , bool>)

            // specification for Sorting 
            // for OrderBy()
            if (specifications.OrderBy is not null) // feha object
            {
                query = query.OrderBy(specifications.OrderBy);
            }
            // for OrderByDesc()
            if (specifications.OrderByDesc is not null) // feha object
            {
                query = query.OrderByDescending(specifications.OrderByDesc);
            }

            // for Pagination Take() .. Skip() 
            if (specifications.IsPaginated)
            {
                query = query.Skip(specifications.Skip /*Skip-> int value*/).Take(specifications.Take);
            }
            return query;
        }
    }

    #region Explain Class 
    /*
      CreateQuery() -> de function btrg3le Query mn al type aly b4t8l 3leh (TEntity<>)
      dor al function t build al Query 
      w 34an a Build al Query for type m7tag 7agten fel paramter bto3 al Function de : 
      Input Query .. y3ne ygily query mn no3 al TEntity aly 48al 3leh -> IQueryable<TEntity> InputQuery
      tany 7aga tgily fel Paramter hya ISpesification ll TEntity .. y3ne Expressions leha -> ISpecifications<TEntity , Tkey> specifications
      kda ana 22dr a build al Query al5asa b type mo3yn <TEntity>

      hdf al SpecificationEvaluator ano y return query
     */
    #endregion
}
