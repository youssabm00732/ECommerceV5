using Ecommerce.Domain.Common;
using Ecommerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specifications
{
    public abstract class BaseSpecifications<TEntity, Tkey> : ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {

        #region Where()
        // where() Expression
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        protected BaseSpecifications(Expression<Func<TEntity, bool>> cretaria =null)
        {
            Criteria = cretaria; // kda b2oll ll Where ank lazem tbasi ll Parent bta3k Cretaira , w momken mtbasi4 fa h5li defult=null .. momken mb2a4 3ayz a3ml filter bl GetAll 3ady
        }
        #endregion

        #region Include() 
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = [];

        public void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpressions.Add(expression);
        }
        #endregion

        #region OrderBy() & OrderByDesc() 
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
        // make function for Private Set .. h5li al Orderby = orderby of expression 
        public void AddOrderBy(Expression<Func<TEntity, object>>? orderBy)
        => OrderBy = orderBy;

        public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }

        // make function for Private Set .. h5li al OrderByDesc = orderbydesc of expression 
        public void AddOrderByDesc(Expression<Func<TEntity, object>>? orderbydesc)
       => OrderByDesc = orderbydesc;
        #endregion

        #region Pagination 
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        public void ApplayPagination(int PageSize , int PageIndex)
        {
            // 100 =>21- 30
            // 10 3 
            // (3 - 1) * 10 = 20 skipped 

            IsPaginated = true;
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize; // formula sapta for Take() and skip() pagination 
        }
        #endregion
    }
    #region Explain Class 
    /*
      IncludeExpressions -> de Listet al Include bta3te w 5ltha list fadya 
      hbd2 any amlaha .. 3n tri2 any h3ml Function dorha anha t add include
      asmha AddInclude() bta5od mnk fel paramter one Expression bs 
      da hy5line aklm al ListI ncludeExpressions() .. b2nha t add al Expression da feha 
     */
    #endregion

}
