using Ecommerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_CommerceSess01.Attributes
{
    public class RedisCachAttribute:ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // what logic i make to make Cach Filter ? 

            // 1 - Get Cach Service From DI Contaier .. hdfha tgbly al Data 
            var CachService = context.HttpContext.RequestServices.GetRequiredService<ICachServices>();
            var CachKey =  CreateCachKey(context.HttpContext.Request);
            // 2 - Check if Cached Data Is Exist 
            var Cached = await CachService.GetAsync(CachKey);
            // 3 - If Exist , Return Cached Data and Skip Excution of Endpoints 
            if (!string.IsNullOrEmpty(Cached)) // y3ne m4 fady y3ne feh item (json) l2no fel Redis Database 
            {
                // return ll front-end al Json da zy mhwa .. wna brg3 fel response hst5dm context 34an hwa aly feh Http request and response
                context.Result = new ContentResult()
                {
                    Content = Cached , 
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK, // l2no l2a 7aga fel Cach 

                };
                return; // de m3naha mtkr24 mn al Endpoint 5las .. l2ina al item fel Cach 
            }
            // 4 - If Not Exist , Excute Endpoint and Store The Result In Cach 
            var Excuted = await next.Invoke(); // nadet 3la al Endoint aly 3leha al Action filter da [RedisAction] 34an ttnfz
            if (Excuted.Result is OkObjectResult { Value: not null } ok)
                await CachService.SetAsync(CachKey, ok.Value, TimeSpan.FromSeconds(1000));
            return;
        }
        #region CreateCachKey Private Function 
        private static string CreateCachKey(HttpRequest request)
        {
            // h3ml an al CachKey hykon al path .. y3ne al path aly al user hyktbo .. zy api/proudact da kda key .. msln api/proudact?BrandId=10 .. da kda key brdo 
            // w 34n a3rf agib al Path b3ml Paramter -> HttpRequest 

            // fa nbne 4kl al Key .. 
            var Key = new StringBuilder();
            Key.Append(request.Path).Append("?");
            //  Key.Append(request.Path) => base url .. api/proudact 
            // Append("?"); .. 34an b3d al url hygily query param fa al path ykmlha f tb2a kda msln .. msln api/proudact? [Query Param lsa h3mla Implemnt] .. bs ana kda l7d dlw2t bnet al Key l7d  api/proudact? .. na2s al Query param

            // build Query Param 
            foreach (var (K, V) in request.Query.OrderBy(q => q.Key))
            {
                Key.Append(K).Append("=").Append(V).Append("&");
            }
            return Key.ToString();
        }
        /*
          api/proudact? .. -> api/proudact?BrandId=10&TypeId=1 -> (BrandId=10&TypeId=1) al goz2 da hwa al Query Param 
          fa ana kda gm3t al path kolo bta3 al Request aly gayly w 5let al CachKey bta3 al Item al 5as bel path da .. hwa al request path 
          y3ne api/proudact?BrandId=10&TypeId=1 hywslne 3la (Hamada Item fel database) .. eh al Cach Key bta3 Hamada item -> api/proudact?BrandId=10&TypeId=1 

          tb leh 3mlt orderBy ? .. 34an msln lw 3mlt request api/proudact?BrandId=10&TypeId=1 .. w b3d kda 3kst bs api/proudact? TypeId=1& BrandId=10 .. my3mle4 new request b new cachkey lnfs al Item
         */ 
        #endregion
    }

    #region Explain Class
    /*
      OnActionExecutionAsync() -> al Function de hdafha al Cach 
      bta5od mne al context aly hwa al Request aly gyle nfso mn al Front-end browser 
      w mn 5lal al attribute da a2dr at7km fel requst w kman al Response 

      next -> da baramter 3obara 3n deleget by4awr 3la next function , al Next Code aly hy run 

      kda al Class da mn 8ir ay 7aga b2a Filter Class 
     */
    #endregion

    #region Logic Of Class 
    /*
      1 - ana 3ayz agib object mn al CashService class 
          fa paramter context hwa aly hysa3dne agibo .. 
          l2no gwah http request and response ... fa hmsk al Request -> RequestServices -> hgib al ICashService interface 
          aly by implemnt mno al CachService class 
       w bkda ana b2a m3aya al Data aly fel Redis Database bta3te 

     2 - 3ayz a search 3la al Data de b2a b3d ma gbtha kolha kda.. 3ayz agib data mo3yna 3n tri2 al CachKey (id) bta3ha : 
         var Cached = CachService.GetAsync(); // error .. an al GetAsync() 3ayza key .. w ana agib al key mnen?? 
                                                  fa hghz PRIVATE function tgbly al Key bta3 data mo3yn gwa al DB
      
       eh aly by7sl ? 
       - ana 3ayz a3ml Cach key ll items aly gwa al database .. b7is any lma atlob item mo3yn agibo bel CachKey bta3o 
       fa 3mlt function bt3ml CachKey lkol al items w 5lto yb2a 3la 7sb al request aly mb3ot 34an ygnha .. 
       
       - b3d kda -> var CachKey =  CreateCachKey(context.HttpContext.Request); .. gbt al cachKey bta3 al Item kda 5las .. fa a2dr any agibo mn al db

      3 - n4of b2a al item da asln mwgod 3nde fel Cach Ram b3d ma gbt al CachKey bta3o wla la 
          wlw l2ito fel Cach fa m4 h5li al Request ykml ll Endpoint .. h5lih ya5do mn al Cach w yrg3 tany fel response mn 8ir ma y3de 3la al database

      4 - lw ml2t4 al Item da fel Cached RAM ..bel Next h3ml Invoke ll code aly b3d al Action Filter [RedisCach]
          aly hya al Endpoint .. ynfzha w ygib al Item mn al Redis database 3la 7sb al Request al mb3ot
          bs b3d ma ygibo .. lw al status OK() .. y3ne l2ah = 5zn al value da f variable asmo ok 
          34an b3d kda a5zno 3ndy fel Cached RAM .. b5zno 3n tri2 al CachService aly bel SetAsync()
         
     */
    #endregion
}
