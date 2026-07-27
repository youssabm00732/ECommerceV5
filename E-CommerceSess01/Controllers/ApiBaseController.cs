using Ecommerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSess01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        public static ActionResult<T> ToActionResult<T>(Result<T> result)
        {

            if (result.IsSuccessed)
                return new OkObjectResult(result.data);

            // else 
            return ToProblem(result.Errors);

            #region Explain Function 
            /*
              lma a2olk rg3le fel return ActionResult<T> y3ne Generic ActionResult 
              = kda kda htrg3le 7aga mn no3 Result Result<T> result..  aly htigy asln mn al Service 
              
              fa hy check an al Result aly gya mn al service de 7altha ok ? = yb2a rg3le al Data 
              al data de hya al Goz2 aly 3mlto fel result pattern gdid 
              => public TValue data => IsSuccessed ? _value : default; 

              8ir kda rg3le an feh problem w a3rdle eh al Problem b4kl 3am 
              tb ana 3ndy 7aga btrg3 problem ? = m3nde4 
              fa h3ml Function tany asmha -> ToProblem() .. y3ne rg3le al error da fel response b7alto w tfasilo 
             */
            #endregion
        }

        public static ActionResult<T> ToActionResult<T>(Result result)
        {

            if (result.IsSuccessed)
                return new OkResult();

            // else 
            return ToProblem(result.Errors);

            #region Explain Function 
            /*
              fel ToActionResult() al ola = ana kont b retrun data 
              fa 3mlt al function de 34an f 7alt any m4 h return data just ok() , fail().. 

              fa awl 7aga 4lt Result<t> -> Result 34an m4 hrg3 data 
              tany 7aga return new OkObjectResult(result.data); -> return new OkResult();
             */
            #endregion
        }

        public static ObjectResult ToProblem(IReadOnlyList<Error> Errors)
        {
            var First = Errors[0];

            var status = First.type switch
            {
                ErrorTypes.NotFound => StatusCodes.Status404NotFound,
                ErrorTypes.Validation => StatusCodes.Status400BadRequest,
                ErrorTypes.Conflict => StatusCodes.Status409Conflict,
                ErrorTypes.UnAuthorized => StatusCodes.Status401Unauthorized,
                ErrorTypes.Forbeddin => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var Problem = new ProblemDetails()
            {
                Status = status, 
                Title = First.Code, 
                Detail = First.Description,
                Extensions = { ["errors"] = Errors}
            };

            return new ObjectResult(Problem) { StatusCode = status };

            #region Explain Function 
            /*
              bta5od mne IReadOnlyList<Error> Errors -> aly hya gya mn al Result Pattern .. fa ana m3aya anwa3 al errors
              
              ana 3ayz awl 7aga agib no3 bta3 al First Error .. l2n ward aly ygily fel IReadOnlyList<Error> Errors
              one error or mgmo3a mn al Errors .. fa bgib no3 awl Error bs 
             
              b3d kda a4of 7alt al Response (Status) wzbto 3la 7sb al error aly rag3 fel response da 
              y3ne lw msln rag3 fel resopnse var First = Errors[0]; (el error ): 
              lw al error da no3o notFound => return 404 fel response .. w hkza f ba2i el Errors 
              lw msln var First = Errors[0]; no3o Faild => rg3 fel response 7aga .. whkza 

              b3d ma al hndelt al error aly fel response w3rft no3o 
              zbtle 4kl al problem de b2a aly rg3aly fel response .. a3ml object bel data aly rg3aly 3n al problem da fel response 
             */
            #endregion
        }

        #region Summary
        /*
           yb2a ana kda sbt al response bta3e l kol al EndPoints aly 3nde b4kl 3am 
           w kda kol al functions de ana wrstha f ba2i al Controllers bta3te l2ne 3aml :ApiBaseController 
           y3ne ana 3mlt Controller fel al functions aly bthandle al response aly rag3le mn al Endpoints 
           f 7alt al response da SUCCESS w rag3le data aw success bs m4 rag3 data .. w function hndlt feha kol 7aga t5os al error
           34an b3d kda ast5dm al Controller da .. nfs fkrt al Result Pattern m3 al Services belzbt
         */
        #endregion
    }
}
