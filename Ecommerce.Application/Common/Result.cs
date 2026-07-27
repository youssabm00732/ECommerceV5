using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common
{
    // al result aly tal3 fel response mn al service bta3te  ll Front-end 
    public class Result
    {
        public bool IsSuccessed { get; }

        // make a list of Errors 
        public IReadOnlyList<Error> Errors { get; } // (Notfound , validation , failer ... )

        // lw btrg3 7aga mn no3 result = yb2a al Ctor bta3k yst2bl nfs al kym de 
        protected Result(bool isSuccess, IReadOnlyList<Error> error) 
        {
            IsSuccessed = isSuccess;
            Errors = error;
        }

        // feh 3 functions m7taghom lma andh 3la Class al Result 

        // Ok()
        public static Result Ok() => new Result(true, Array.Empty<Error>()); // true, Array.Empty<Error>() -> dol aly gwa al Ctor bta3 al Result l2n al result 3ndo paramter constractor fa lma a3ml mno object lazem ad5l al kym de 

        // Fail()
        public static Result Fail(Error error) => new Result(false, new[] { error}); // hstlm error wb3den a return this error ll front end 

        // Fail() .. bta5od mgmo3a mn al errors 
        public static Result Fail(IReadOnlyList<Error> errors) => new Result(false, errors); // hstlm mgmo3a mn al errors wb3den a return this errors ll front end 

        // note : 3mlt al ctor protected 34an wsoli leh hyb2a 3n tri2 al 3 functions dol bs

        #region Important Explain 
        /*
          note :
         al Class Result da for lw ana b3ml 7aga mn no3 (Post) y3ne msln (Create - update - delete) .. fa byb3tlk message mn al 3 functions aly 3mlnahom..
         lakn m4 ll 7aga aly no3ha (GET) .. an y7sl error msln wna b get al Proudacts ll Front-end in responce 

         y3ne .. al service bta3tk f 7alt al get .. al service btgib mn al Repo Data fa ana 3ayz a return al result data de + oK() lw hya success 
         aw lw feh error fel data aly ana gbtha mn al repo de -> fail() y3ne m4 h3rf a3rdha fel response json 
         lw al data aly gbtha mn al repo feha aktr mn error -> fail() aly feha array of error w brdo m3naha an m4 h3rf a3rdha fel response json 

         fa ana bganb Class al Result aly 3mlto for Post() .. (al services bt retrun result mthandel an al service de b3d ma t3ml CRUD -> t return fel response ok() , fail() , fail() group of errors)
         h3ml b2a class tany Result brdo bs for Get() .. (Al service by get data mn al repo w tb3tha fel Response .. fa w hya btb3t Class al result bta3 al get da byhandle al response .. hl al data aly atb3tt ok() , fail() , fail() group of errors)
        */ 
        #endregion

    }
    // make generic class Result ano ya5od al Data aly rag3a mn al repo <TValue>
    public class Result<TValue>:Result
    {
        private readonly TValue _value;

        public TValue data => IsSuccessed ? _value : default; 

        //Case 1 -> ok() retreve data from repo without errors
        private Result(TValue value) : base(true, Array.Empty<Error>())
        {
            _value = value;
        }
        //Case 2 -> Fail() bs error wa7d wna b retreve al Data 
        private Result(Error error) : base(false, new[] { error})
        {
            _value = default!;
        }

        //Case 3 -> Fail() bs group of errors wna b retreve al Data 
        private Result(IReadOnlyList<Error> errors) : base(false, errors)
        {
            _value = default!;
        }

        // make 3 Functions 
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value); // htro7 tnady 3la al Ctor bta3ha.. Case 1
        public new static Result<TValue> Fail(Error error) => new Result<TValue>(error); // htro7 tnady 3la al Ctor bta3ha .. Case 2
        public new static Result<TValue> Fail(IReadOnlyList<Error> errors) => new Result<TValue>(errors); // htro7 tnady 3la al Ctor bta3ha .. Case 3

        public static implicit operator Result<TValue>(TValue value) => Ok(value); // m4 fahm leh 
        public static implicit operator Result<TValue>(Error error) => Fail(error); // m4 fahm leh 


        #region Explains in Class 
        /*
          1 - h5li al Class ywrs mn Result bta3 al Post = l2no nfs al Sequance bta3 al class hwa hwa .. aly hyzed any 
              hzwd al TValue bs (al Data aly gyale mn al repo )

          2 - tlma wrst fa lazem a3ml Ctor Chaning 
              fel Ctor da Result(TValue value) -> tlama ana ndht 3la al result w adtlk value => yb2a kda ana Success any agib al data mn al repo 
              => yb2a anta tbase ll Base Class => true , error empty .. w de tb2a Case 1 -> ok() retreve data from repo without errors 

          3 - 3mlt Ctor l Case 2.. w hya al Fail() .. fa bset al Error fel Ctor .. da m3nha an fel Base Class hb3t false => al 3mlya m7slt4 + hb3t el error da  
         
          4 - nfs al klam Case 3 bs ana mrg3 mgmo3a mn al Errors 

          5 - ana m4 b return al Result 3n tri2 al Class nfso 
              ana b returno 3n tri2 any b3ml 3 functions .. fa ana h5li al Ctor bta3t al Class = private 
              34an aly y2dr y access 3la al Ctors dol homa al 3 functions aly h3mlhom .. aly b3d kda hst5dmhom 3ady 
            
              fa aly hy7sl an malsn ndet 3la function ok() fa tro7 al Funcion de 3la al Ctor bta3ha tnfzo : 

             public Result(TValue value) : base(true, Array.Empty<Error>())
        {
            _value = value;
        }
             whkza fel bai 

          6 - lma t3ml 3 functions ... 7ot keyword (new) gmb al Static 
              34an anta wars al Functions de 34an y3rf anhom Functions mo5tlfen w my3ml4 3lehom override 
         */
        #endregion

        #region Sammary 
        /*
           al Service btgib mn al Repo Data .. al Data gt .. yb2a t return al result al Generic  Result<TValue> wlakn andh 3la function Ok()
           Function Ok() tgblk al Ctor bta3ha aly hwa bya5od mnk value .. wel Ctor da yb3t ll Base aly fel Result Class (Post)
           an Success b true w error mfe4 

           nfs al lfa lw feh error .. bs byro7 yde ll Base Class IsSuccessd = false , w yb3tlo el error 

           w yb2a al Service btndh 3la al Result aly bt3mlo Return da 7sb al Bussnis Case 
           al service no3ha get .. yb2a ht4t8l v class la Result al Generic 
            3mlt Crud .. result of post Class  
         */
        #endregion
    }
}
