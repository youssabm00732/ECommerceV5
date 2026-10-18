
using E_CommerceSess01.Extentions;
using Ecommerce.Application;
using Ecommerce.Application.Profiles;
using Ecommerce.Application.Services;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace E_CommerceSess01
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // bdl ma a7ot object al Context hna fel DI Container = a7oto fel InfraStructure Services Class 
            //builder.Services.AddDbContext<StoreDbContext>(Options =>
            //{
            //    Options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDbConnection"));
            //});
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlSettings"));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<PaymentGetwaySettings>(builder.Configuration.GetSection("Stripe"));
            var app = builder.Build();
            await app.MigrateAndSeedAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // to read files configurate
            app.UseStaticFiles(new StaticFileOptions { 
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath , "Files")) ,
                RequestPath = "/Files"
            });
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            #region Session01

            #region What Is API ? 
            /*
              Application programming interface -> API 
              APi hwa interface ben al Applications bysa3d anhom ytwaslo m3 b3d 
              al twasol da 3n tri2 anhom yb3to w ya5do data btween each other .. wel data de btro7 wtigy bel API 3la Hy2t Json File 
              for exampel : 
              front-end mobile app (React.js) 3ayz GetALL Member -> yb3t Get request ll back-end app Asp.net App (another app) 
              fa al Backend APP -> Database ygib al Data w yrg3ha fel response as Json file .. nfs al Klamn fel Post Request 
              Put . Batch .. Delete .. nfs al klam 
              al Data b2a whya btro7 mn App ll tany -> by using API's 
              fa al Api bysa3d al Apps ttklm m3 b3dha ayn kant no3ha (Ai model - desktop app - mobile app ) with Back-end APP  

              note : al Back-end is a web service hna .. not web app ..l2n al web app by return view 
                     lakn web service an ana b2dmlk 5dma zy api fa al Apps t2dr tklm b3dha mn 5lalo w ttransfer data behnom 
             */
            #endregion

            #region APi Project Structure 
            /*
              Look At First 6 min .. 

              - Project mfho4 Views l2nk m4 ht return Html View anta ht return Json file 
              - Connected Services 34an lw btklm ay 7aga External -> Database , External Api , Cloud 
              - momken tnzl fel Nuget Backages -> Swagger Extention 34na t test al apis lw m4 3ayz tnzl app .. bs a7na bnst5dm PostMan 
              - LunchSettings.Json -> 34an al protocoles aly bt3aml m3aha Http and Https 
              - AppSetting.json -> for External Configuration zy any aklm kza database provider aw btklm m3 payment get way aw cloud 
                                   b7ot ll 7gat de Configurations fel file da 

              - main program : feh service Container w awl ma ygy y build al app y3de 3la MiddleWares 
              - mfe4 file wwwroot 34an byb2a feh al Assets .. al files aly leha 3llaka bel Css Html Image 
             */
            #endregion

            #region Api Access Types 
            /*
             - Open Api's (Public) :  
             lw anta 48a 3la Application 3ayz t3rd feh data bta3t kol a5bar al Kora .. fa 34an t access 3la al data de
             al 4rka aly 3ndha al Data de byb2a 3ndha apis tgib al Data de wtst5dmha 3ndk fel application .. fa byb2a 3ndk al access lda 
             l2no most7il t3od t3ml al Data de mn al awl for ex: search google (Football api free)

             mlsn brdo an al app bta3k ykon gwah paypal ll df3 .. 4rkt paypal nfsha bt3mlk api as service ll app bta3k 
             34an t2dr tst5dmo w tdf3 w ysm3 fel bank .. wnta tb3n m4 ht3rf t3ml da lw7dk l2nk already m4 mo4trk m3 bank yt3aml m3 al app bta3k 
             fa paypal btdik api y3mlk al 4o8l da w tdf3 anta 7sab al api da 

            internal Api's (Private) : 
            5as bel 4rkat aktr .. ank 3aml api's for al 4rka bs mynf34 ytl3 bra server al 4rka 
             */
            #endregion

            #region SOAP API and GraphQL

            #region intro
            /*
                 feh 3 a4kal bys3done any atl3 apis yb3t data whoma : 
                 Restful , SOAP , GraphQL
                */
            #endregion

            #region SOAP API (A2l ahmya)
            /*
              hwa protocol (y3ne tri2t 7war ben application wel tany)
              al apis de btb3t data .. wel Data de btkon mt5zna 3la 4kl XML 
              Xml de 4bh HTML .. hya 3obara 3n tags <> bs al tags de bt3br 3n al data bta3te 
              for Exampel : 
            <Proudact>
            <id>
            10
            </id>

            <name>
            youssab
            </name>
            </Proudact>
            fa al Api da bygele fel request as XML w ana brg3o brdo fel response 3la hy2t Xml 
            note : al Soap api hwa aktr no3 secure fel apis l2no asln protocol fa byb2a mostly used in banking aw mo2ssat 7komya wel 7gat de 
            mo4klo ano mo3kt fel t3amol 4wya fel request and response 
             */
            #endregion

            #region GraphQL
            /*
              FaceBook hya aly 3amla al Api da l2n kan 3ndha mo4kla -> (Over Fetching): 
              fel awl al front end yb3t request ll back -> 3ayz al id and name of cirten proudact 
              fa al back kan y7drlo al APi feha al data de wyb3tha fel response 
              b3d 4wya al Busniss requirment yt8ir wyb2a 3ayz kman fel request al price of proudact 
              fa al developer kan y8ir fel Controller wel repo wel service 34an yb3t api b 4kl al data al gdid 
              fa de kant mo4kla asmha Over Fetching

              al 7l anna nde al user EndPoint (Action) wa7da asmha -> GetProudact 
              fa al Front end ytlob fel request aly hwa 3ayzo (id ,name) bs .. fa al request ywsl ll EndPoint bta3te 
              btkon gwaha kol 7aga t5os al Proudact .. bs t select mnha al Id , Name w tb3tholk fel response 
              b3dha b yomen tlbt f request tany al price ? -> al graphQl api yro7 ygblk al data mn nfs al Endpoint 
              wyrg3holk b4klo al gdid fel response 
            note : al t3aml ray7 gy fel request and response byb2a Json file 

            fa mezto ank b single Request t2dr t pull al Data mn aktr mn Resource 
            byost5dm kman fel non relational data bases any agib m3lomat mn Kza table mfe4 realtion benhom asln y3ne mfe4 include() 

             */
            #endregion

            #endregion

            #region Restful Api
            /*
              Restful api de btt3aml m3 Endpoints 
              endponit de btbi3t al 7al btb2a 3obara 3n URL 
              al End Point de m4 Action gwa Controller .. bl hya al URL aly btwdek 3la Action gwa Controller .. y3ne eh
              y3ne gylk fel request Get Request bel Url da -> Http/Gym/Member .. fa al Endpoint de htwdine 3la Action gwa al Controller no3ha Get 
              fa aly bywslne ll Action aly hya Function hwa al End Point nfso 
              w feh meza 7lwa Fel Endpoint : 
              An bnfs al Endpoint momkn tkon 4yla Kza URL bmo5tlf al Requests 
              y3ne url for Get .. ywdek 3la Action no3ha Get 
              w fnfs la EndPoint URL Post -> Action Post ..whkza 

              fa ana btf2 m3 al Front-end developer 3la 3dd al EndPoints aly bena fel 4o8l 
              y3ne momken a3ml One Endpoint tkon feha Delete , update , Create , Get 3ady 

              fa aly byb2a dayr : Front yb3tle request url (Get - Post - Put - delete) 
              wna a5od al request anfzo w ab3tlo data fel response as Json file .. w momken arg3lo ok() response msln lw al request no3o post fa hsave 7aga 3nde w m4 modtr arg3 json file 

              fa al Restful api hwa byt3aml m3 Endpoints aly hya 3obara 3n Url btwdene 3la Action mo3yn f Contoller 
              w kman al url da byb2a feh no3 al Request (Get - Post - Put - delete) 
              w brg3 Json file fel response b3d ma bst3bl al URL Request .. w al restful api hwa al 4o8l bnsbt 95% 
             */

            #region Test Restful Api 
            /*
             1 - make Class asmo proudact .. wna 3ayz arg3 fel response one proudact in json file a3ml eh? 
             2 - in Controller -> make (Api Controller) -> ProudactController 
                 gwa al Api Controller (al mo5tlf 3n al MVC Controller) .. 
                 ProudactController : ControllerBase -> m4 bwrs mn al Controller al 3ady 34an y4il al View Support l2ne m4 m7tago fel api 
                 [Route("api/[controller]")] -> Defult Route l Endpoint 
                 fel MVC kan fel Main Program.cs kan feh Configuration 5as bel routing an al Defult route kan -> Controller/Action 
                 dlw2t fel Api mb2a4 feh al Configuration route da fel Main Program l2n al End Point al w7da momken t4il aktr mn Action 
                 fa b2a feh route wa7d ll Endpint api/[controller] -> api/proudact 

                 [ApiController] -> 34an ProudactController yktsb behaviors al Api Controller aly mn meztha an lma y7sl model binding m4 m7tag adwr fel 5 amakn hwa gy mnen zy mkan fel MVC bl hwa bydwr 3leh w bygibo Automatic (at4r7t fel Pre Mvc)

              3 - a3ml al Action aly gwa al Controller bs a7dd al Type bta3ha -> [HttpGet] 
                  w3mlha al Implemntation 

              eh aly hy7sl ? 
              fel url lma ab3t request ll back end -> Get/Api/Proudact 
              fa Api/Proudact -> hy5o4 gwa al Enpoint aly gwa la Controller .. 
              hdwr f ahne Action gwa al Controller -> 7aga no3ha Get .. fa ynfz al Action bta3te aly 3mltha --> GetProudact() 
              trg3le Json Response .. aly hya Proudact id = 10 , name = chai 

              4 - Run 
              5 - Post man -> dos new -> Http -> a5tar no3 al Action aly ray7lha (Get) -> army al Path aly tl3lk lma 3mlt run (https://localhost:7014) -> kmlo : https://localhost:7014/api/proudact 34an a5o4 3la al Endpoint aly gwa al Controller .. w hwa b2a ydwr 3la al Action aly no3o Get
                 hl2ih nfz al Action aly asmha GetProudact() w rg3le Json mnha 
            ===================================================================================================
            tb get 3mlt Action tanya asmha GetProudactById() no3ha -> [HTTPGet]
            lw get 3mlt run w rmet fel post man al url hwa hwa -> https://localhost:7014/api/proudact
            s3tha htgblk error l2n anta 3ndk kda al Endpoint 3ndha two Actions bnfs al no3 -> [HttpGet] fa hwa m4 3arf ynfz anhe Action fehom
            fa al 7l : 
            y2ma a8ir al type wa7da fehom -> [HttpPost] 
            y3ma azwd Segment lwa7da fehom zy kda ->  [HttpGet("Hamada")]
            fa al Url 34an awsl ll Get Action de bdl ma kan kda  https://localhost:7014/api/proudact ->  https://localhost:7014/api/proudact/Hamada 

            note : al two actions no3hom get fa fel post man b5tar get .. b3d kda brme al url .. bs 34an y3rf ywsl l anhe w7da fehom tlma al atnen get -> 7tet segment zyada 34an yfr2

            note : al 3alm al 5arge byt3aml m3 Endpoint (URL) bytb3t wna mn 5lal al URL da bro7 3la controller bta3 al Endpoint de w bdwr 3la al Action al monsba aly gwa la Endpoint de 
             */
            #endregion

            #endregion

            #region Onion Architecture 

            #region Problem
            /*
                  fel MVC ast5dmna N-Tier archeticture w kano 3 layers (PL , DAL , BLL) 
                  kont bst5dm Database wa7da  
                  kman fel BLL kont bst5dm Service w kant btndh mn gwaha 3la UnitOfWork 34an twsl ll Repository wmno ll queries 34an agib al data
                  fa al repo kant tbne al Query w trg3 .. btrg3 eh b2a ?? 
                  kont brg3 object mn al data lma kont bst5dm first or defult aw al 7gat aly ast5dmnaha -> wel data object de kant btst5dmha al service bta3te
                  tb afrd lw fel repo da bdl ma arg3 (data object) = arg3 (IQuarable) y3ne btrg3 Query .. y3ne .where() msln 
                  da m3nah an al service de gtlha al Query w an al service de htktb query gwaha 

                  y3ne dlw2t brg3lk abdl al Data object -> IQuerable.FristOrDefult() .. 
                  htgib awl item , lakn al FristOrDefult() ht5li al Excution bta3t al Query y7sl fel Service = w da 8lt kbir 

                  l2n lw 2oltle al Data base de Sql Server -> wnta get 8irtha f yom l mongo 
                  da m3nah ank modtr t5o4 mot3md gwa al Service w t3dl kol al codes aly 3amlha bel Sql Server into Mongo 

                  fa da kan 3ib al N-tier archetecture , fa a7na 3ayzen Arch. ykon more Scalable b7is a8ir al provider of Database 
                  bra7te asln mn 8ir ma adtr a3dl f Code al service 
                  fa a7na fel Project da hnklm 2 database 8ir al Cashing (data m3molha save fel memory) .. 8ir any hklm m3 external service zy payment getway 
                  w external resources ktir .. fa scale al Ntier m7dod ll project al so8ira aly leha one database provider 

                 */
            #endregion

            #region Onion Architecture 
            /*
              fkrt al basala an al 24ra lma tt8ir albha my7slho4 7aga 
              w hbne bel m3marya de l tnw3 al resources aly al m4ro3 bta3y hyt3aml m3aha (External services - kza Database provider)

             hnbd2 bel Alb (Core) => 2 Layers : 
             layer 1 -> Domain => gwah bussnis mn 7is al models bta3to (Proudact - prand -  customers) + DB Contract => al interfaces aly btst3mlha fel Debendancy Injection bta3t al Databases .. aly feha al Signitures ex : (IGenericRepo) gwah (GetAll - GetById - Add -Ubdate-Delete) not implemntation 
             layer 2 -> Application => gwah al Services aly al Business 2aym 3leha (intrerface + implemnt al service) kman hyb2a feha (Dto's) (AutoMapper)
             w a7na 3arfen an Layer 2 debend on layer 1 (Application must yklm al Domain) .. l2n al Services btst3ml Signature bta3t al Repositorty gwaha 34an tt3aml m3 al data aly fel database bta3tk .. note : kda al service m4 4ayfa ana bklm database eh (sql - mongo ..) hwa by implemnt al interface bs (IGenericRepo or IUnitOfWork) whna at7lt mo4klt al N-tier 

            b3d kda al Layers al 5argya : 
            Layer 3 -> InfraStructure => al Plugens (y3ne 7aga btt8ir) .. 3obara 3n ma y5os al Database l2ne b8ir mn provider l brovider 3ady 
                       fa gwah (Context) (Configurations) (Data Seeding) (External Services payments Cashing) w kol ma y5os al t3amol m3 al External Provider 
                       w fel layer de hykon feh al Implemnt bta3 al Contract of data base aly fel domain layer (wel service m4 bt4of al Implemnt da hya bt4of bs al Interface bta3 al repo.. da bydeny flexability any a8ir fel providers b3d kda bra7te al servces m4 htt2sr)

            Layer 4 -> Presentation layer (APi Layer) => al api bta3e aly feh al API Controllers aly gwah endpoints mgm3a kza action gwaha
             */
            #endregion

            #endregion

            #region Project Archeticture 
            /*
              inner Layers (Class Liberary) -> Domain Layer - Application Layer 
              Outer Layer (Plugins) -> Infrastructure Layer - Api Layer 

              Dependancies Btween Layers : 
              Domain Layer :  
              aly 4ayfo hykon Application Layer 

              infraStructure Layer : 
              htkon 4ayfa al Application Layer fa htkon 4yfa al Domain layer Kman 

              Api Layer : 
              Hykon 4ayf al Infrastructure layer fa bltaly hykon 4ayf kol al layers al tanya 

              yb2a : 
              Domain -> Nothing 
              Application -> Domain 
              Infrastructure -> Domain , Application 
              Api -> Domain , Application , InfraStructure 
             */

            #region How To Implemnt This Layers in Application Project ? 
            /*
              1 - in solution -> right Click -> Add -> NewPorject -> Class Liberary -> Ecommerce.Domain (and Delete their Class)
              2 -  in solution -> right Click -> Add -> NewPorject -> Class Liberary -> Ecommerce.Application (and Delete their Class)
              3 -  in solution -> right Click -> Add -> NewPorject -> Class Liberary -> Ecommerce.Infrastructure (and Delete their Class)

              4 - Add Refrances : 
                  Application -> Reight Click -> Add Refferance -> t4of al Domain 
                  Infrastructure -> Reight Click -> Add Refferance -> t4of al Application 
                  E-CommerceSession01 (API) -> Reight Click -> Add Refferance -> t4of al InfraStructuer  
             */
            #endregion

            #endregion

            #region Proudact Module 
            /*
              Da awl Model fel System bta3na (al montgat b4kl 3am ) : 
              eh al EndPoints aly ht3aml beha fel Module da ? 
             */
            #region Proudact Module EndPoints 
            /*
              kolha 7gat ll 3rd fkt wlys ll t5zin (no post)
              Get -> api/Proudcats -> To Get All Proudacts .. List<Proudacts>
              Get -> api/Proudcats/{id} -> Get Proudact by id 
              Get -> api/proudcats/brands -> return al 4rka al Mosn3a ll montag da
              Get -> api/proudacts/types -> return no3 al proudact nfso eh 
             */
            #endregion

            #region Proudact Entity 
            /*
              we will make 3 tables : 
              proudact , proudactType , proudactBrand 
              1 proudact -> m proudactType
              1 proudact -> m proudactBrand
            so proudact have fk of (proudactType , proudactBrand)
             */
            #endregion

            #region Steps in project 
            /*
              1- Create new Folder in Domain layer -> Entites
              2 - Create new Folder in Domain layer -> Common (34an a3ml gwah class al parent aly ba2i al Classes htwrs mno)
                  3mlt BaseType .. Generic Class feh al Id bs (da aly htwrso ba2i al Entites) .. w 5lito Generic 34an al Id type ben al Entites bta3te hyt8ir mra int mra string .. 

              3 - create New Folder in Entites folder -> Proudcats 
              4 - create inside it Class -> ProudactBrand 
              5 - create inside it Class -> ProudactType 
              6 - create inside it Class -> Proudact
             */
            #endregion

            #endregion

            #region Store Context & Migrations 
            /*
              hnghz al Context fel InfraStructre .. w de al layer aly htt3aml m3 al Provider 
              fa : 
              1 - make new folder in Infrastructure Layer -> Data 
                  gwah Class -> Context and implemnt it 
              note : in ctor of StoreDbContext .. hghz object mn al Dbcontext l2no hyb2a mb3otle mn bra 
                     fa hro7 3nd al AppSetting.json h3ml al Connection string gwah 

              na2s kda a3ml Migrations bs feh 7aga : 
              34an t3ml migrations fel MVC kan lazem tnzl al package Tools .. 
              fa ana hro7 anzl al Package de gwa al Infrastructure layer l2n hya aly feha al Context 
              2lk lw 3mlt kda .. w ft7t al Package manger w 3mlt mogration hydek error .. leh ? 

              l2n al tools gwa al InfraStructure layer m4 3arfa t4of al Connection string bta3 al Database 
              34an t3ml al migration bta3ha .. (da fel api project bs)

             fa kan al 7l any bdl ma anzl al pakage tools fel infrastructure layer = hnzlha fel Api layer 3ltol 
             l2n al layer de feha al Appsetting.json aly feh al Connection string bta3e fa t2ra mno 3ltol 
             w t3ml migration mn al manger Console 3ady 

              2 - install tools backage inside api layer (E-Commerce Session01)


              3 - inside Data Folder -> Make Configurations folder 
              4 - make Class inside Configurations folder 

              5 - open Console Package Manger to add migration 
                  note : update database de ana 3ayz a3mlha awl lma ados run bel DataSeeding not by Console Package Manger

                   hytl3le error l2ne al mfrod a5l2 object mn al Context bta3e w armeh fel Ctor bta3 al DbContext bta3e
              6 - make object from Context in DI container 

                 builder.Services.AddDbContext<StoreDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDbConnection"));
            });

              7 - agr3 ll Package manger (w lazem to2f 3la al infrastructuer layer 34an fhea al Context bta3k) 
                  w t3ml Add Migrations 
             */
            #endregion

            #endregion

            #region Session02

            #region Proudact Data Seed 

            #region Json Folder 
            /*
                  ana 3ndy Json Folder gwah 3 files of Json gwahom data 3n 
                  1 - proudactType 
                  2 - ProudactBrand 
                  3 - Proudcat 
                  wlazem fel Seeding abd2 b proudactType and ProudactBrand l2nhom as fk inside al Proudcat table 

                fa awl 7aga 3mltha hwa nay 7tet al Folder da gwa al path da : 
                D:\RouteTasks\API\Session01\E-CommerceSess01\E-CommerceSess01\bin\Debug\net8.0 
                gwa al Bin folder 
                tb leh ? = 34an at7km fel Url aly htst5dmo (more secure) .. bdl ma kont b7oto fel WWWroot w ay 7d y2dr y acess 3leh 


                 */
            #endregion

            /*
              2olna an al Domain Layer gwaha al Contracts al 32od .. 
              hya aly feha al Interfaces and Signtures aly dorha anha tt3aml m3 Databases ayn kan no3ha 

              fa ana h3ml fel Domain Layer : 
              1 - new folder -> Contracts 
              2 - gwah interface -> IDataSeeder 
            b3d kda hkon 3ayz a3ml Class y implemnt al Interface da hdfo : 
            y3ml Seed 7sb al Path aly ana h7tholo 

              3 - in InfraStructure layer : (l2n de al Layer aly btt3aml b2a m3 al Database) .. wna 3ayz a3ml Implemntation 
                  - make folder inside infrastructure -> Seeding Folder 
                  - make Class inside this folder -> Catalog DataSeeder 
                    leh bganb any a implemnt al interface -> 34an lw 3ayz a seed ay 7aga tanya 5asa bel Context bta3k t3mlo hna 

                 al Class da hy7tag : yt3aml m3 al Context w lw 3ayz t log al Errors htst3ml ILogger 
                 fa h3ml Kda bel Debendancy Injection 

               4 - b3d ma 3mlt Implemntation folder al CatalogeDataSeeder w 4r7t gwah kol 7aga .. 
                   h3ml class for Services for Infrastractuer layer 
                   add calss -> InfrastrucrureServicesRegistrations .. w 4r7t leh gwa al CatalogeDataSeeder brdo 
             */

            #endregion

            #region Program Extention
            /*
              al Extention da b3d ma 7drt al data bta3te aly h3mlha Seeding 
              hy3ml Migrate ll data de fel Database 

              fa hagy f Api Layer (ECommerceSession01) : 
              - make Folder -> Extentions
              - make inside it Class -> ProgramExtentions w implemnt gwha function MigrateAndSeedAsync()
                wziftha t migrate al data then seed it in Database 

              - hro7 3nd al Main Program anady 3la al Function de -> await app.MigrateAndSeedAsync();

              - Run Application 
             */
            #endregion

            #region Generic Repository & Unit Of Work 
            /*
              al Frka any H3ml GenericRepository 
              w UnitOfWork gwaha Function GetRepo() -> aly bt generate lya repositories 

              steps : 
              inside Domain Layer : 
              inside Contracts Folder -> Make Interface (IGenericRepository) 

              in InfraStructure Layer : 
              h implemnt al contract aly kan fel Domaim (IGenericRepository)
              make Folder -> Repositories
              make Class GenericRepository 
              w anday gwah bel Debendancy Injection 3la al Context fel Ctor bta3o 

              in Domain Layer : 
              h3ml b2a al Unit Of work interface .. as Contract .. Domain -> Contract Folder -> Make Interface 
              w b3dden a3mlo Implemnt in Infrastructure Layer 
              InfraStructure -> Repositories -> make UnitOfWork Class to Implemnt 
              w 7ot gwah bel DI object of Context 

              b3d ma 3mlt al UnitOfWork .. ro7 3la al Infrastructure Layer 
             in  InfrastrucrureServicesRegistrations -> ghz object mn al UnitOf work da gwah 
            -> services.AddScoped<IUnitOfWork, UnitOfWork>();

             yb2a ana 3nde unit of work : 
             create new Generic repos
             Save Changes () 
             gwah kol al Repos bta3te aly bt3aml mn 5lalha m3 al Database 
             */
            #endregion

            #region DTO's
            /*
              hna hnbd2 n3ml al Services bta3t al Proudacts  
              w tlma hn4t8l Services = Application layer 
              tb goz2 al Services bnm4eh azay k Sequance yt3ml b4kl kwis y3ne k 4o8l ? 

              => abd2 bel 7aga aly htt3rd al awl (DTO's = VeiwModels)
                  DTO -> is Data Transfare object .. y3ne object hdfo ano yn2l data .. mn al Database ll json bta3k aly htrg3o fel response 
                  y3ne fel Api Project lw 3ayz ab3t 7aga al Front-end y4ofha htkon mb3ota f 4kl Dto w trg3lo k json fel response 

             steps : 
             - in application folder -> new Folder -> Dto's
             - make inside it another folder -> Proudacts 
             - make inside Proudacts folder -> Class -> ProudactDto 

             summary : 
             ana b3ml Dto 34an da 4kl al Data aly hykon rag3 ll Front-end 
             bs hyb2a rag3 k json fel response .. bs al json da y7ml 4kl al data aly ana krrt anha tzhr ll front end
             */
            #endregion

            #region AutoMapper Implemntation 
            /*
              tlma 3mlt al Types aly rag3a fel response .. yb2a kml 3la al Types de wa3mlha mapping bel Auto mapper 
              w de tb2a tany 5atwa fel Services 

              steps : 
              1- in Application Layer -> Nuget backages -> AutoMapper Dawenload (Last version)
              2 - make Profiles Folder 
              3 - make Class inside it -> ProudactProfile bywrs mn : Profile 
                  w gwa al Class fle Ctor bta3o a3ml al Maps aly 3ayzha

              4 - ghz al DI object bta3 al Mapper da 34an ast5dmo 
                  ana dlw2t fel Application Layer .. fa h3ml Services 5asa bel Application layer ..
                  zy ma 3mlt fel Infrastracture layer 
                  y3ne h3ml Class wel Class da hykon gwah kol al services aly bttktb fel Main Program fel DI Container 
                  (bs al 5asa bel Application layer bs) .. w da 34an mz7m4 al donya fel DI Container in Main Program 

                 y3ne : - right click on Application layer -> make class -> ApplicationServicesRegister 

               (yb2a ana kda 3mlt lkol project layer -> registration Services Function)

              5 - hro7 3la al main program anade 3la al function aly gwa Class da as Extention method inside DI Container 
                  => builder.Services.AddApplicationServices();
             */
            #endregion

            #region Result Pattern Implementation
            /*
              b3d ma zbtna 4kl al Type aly rag3 w 3mlnalo kman mapping bel AutoMapper 
              hnsbt 4kl al Result aly hyb2a tal3a mn al Services aly h3mlha b2a b7is al front yb2a fahm al result aly gyalo fel resopne ok wla faild wla not found wla eh 
            .. w de htb2a talt step wna b3ml al services 

            septs : 
            1 - Create folder in Application Layer -> Common
            2 - make Enum -> ErrorTypes inside it 
                - fel Enum da ana bgm3 kol anwa3 al Errors aly momken tkon 5rgaly mn al Service in Response zy any b3ml CRUD opertaions 8lt aw ay 7aga tl3aly mn al Service 8lt
                 fa momken al Service tkon mtl3a one error .. momken array of Errors 

            3 - in Common Folder -> make class -> Error (Record) and Implemnt inside it all Error Functions (from ErrorType)

            4 - make Result Class inside Common Folder 
                34an agm3 feh el Error Functions aly 3mltha f Error Record Class (f7alt lw feh errors rg3aly)

              y3ne ana hst5dm Result Class da gwah Functions al 5asa bel errors + 7gat tanya leha 3laka bel Response aly rag3 
              34an b3d kda al Services wna b3mlha t return (Result Class) 
              aly hwa already feh kol 7aga t5os al response aly rag3 (Errors or 7gat tanya leha 3laka bel Resposne)
              (4r7t al Result Class kolo gwah)
             */
            #endregion

            #region Proudcts Services 
            /*
              b3d ma 3mlt al Dto -> Mapping -> Result Pattern -> h3ml b2a al Services w aghzha wb2a kda 5lst al Cycle 
              bta3t al Services 5als : 

              Steps : 
              in Application Layer -> Add new Folder -> Contracts (al 5asa bel Services )
              - make interface for proudact Services -> IProudactServices (feh al 4 functions bto3e)
              - make folder in application alayer for implemnt al services interface -> Services 
                w gwah h3ml Class asmo ProudactServices w a3ml Implemnt ll 4 Functions 

              - fel a5r aro7 fel Application Layer -> ApplicationServicesRegister (aly feha al Services al 5asa bel Application Layer bdl ma aktbha fel DI Conatainer in main program)
                = hghz bs al Object bta3 al Service da 34an b3d kda hst5dmo bel Debndancy injection fel Controller 
              =>  services.AddScoped<IProudactServices, ProudactServices>();
             */
            #endregion

            #region Base & Proudact Controller 
            /*
              b3d ma 5lasna al Services hbd2 a3ml al Controllers aly httl3le al EndPoints bta3te .. 

              steps : 
              1 - in Api Layer (ECommerce Session01) -> make Folder -> Controllers 
              2 - h add new Controller (api) -> ProudactController

              3 - h3ml Add Controller (api) tany -> ApiBaseController leh ? 
                  34an lma a3ml kza Controller fa feh 7gat htb2a mtkrra fehom .. fa bdl ma akrrha  
                  aktbha mra wa7da fel ApiBaseController da
              b3d kda hbdl ->  public class ProudactController : ControllerBase 
                           to  public class ProudactController : ApiBaseController
              wms7 fel ProudactController : 
                [Route("api/[controller]")]
                [ApiController]
             l2nhom mwgoden fel ApiBaseController


             4- b3d kda hnsbt Return ll Endpoints b4kl 3am aly htkon f kza Controller 3nde 
              zy ma sbt al return bta3 al Services kda bel Result Pattern .. h3ml al pattern da fel ApiBaseController aly 3mlto

             */
            #endregion

            #region Testing The Proudact Controller 
            /*
              A7na hnst5dm al Post Man for Testing Endpoints .. 
              steps : 
              - new -> Collection (b3ml Collection 5as bel Documentation aly hn3mlo) 
              - gwa al Collection -> Make new Folder w smeh session02 
              - gwa al Folder nfso -> Add new request -> smeha GetAllProudacts 

              - run al Application w fel CLR 5od al URL Copy w 7oto fel Postman 
              => https://localhost:7014 (w no3 al Actions is Get zy ma fel Controller l2ne h testhom)
              => https://localhost:7014/api/Proudact (zyada api/name of controller aly feh al Endponits aly 3ayz agrha)
              note : bel URL da hy5o4 3la awl endpoint 3ndy fel Controller ynfzha 
            wel Result 200Ok() .. w json feh kol al Proudcats aly 3nde fel Json 

            kda ana 3mlt Check 3la awl EndPoint aly bt3ml GetAllProudacts
            3ayz a3ml Check 3la al EndPoint aly by GetAllTypes 

            - make gwa al Folder -> new Request tany -> smeh GetAllTypes
            w3ed nfs al 5twat belzbt bs al URL => https://localhost:7014/api/Proudact/Types

            bs abl ma a3ml kda .. al URl da sabt wna m4 kol mara ha5do copy paste .. l2n al URL of Project Solution sabt m4 byt8ir mhma a3mlo new run 
            fa al 7l eh ? 

            - agy 3nd al Collection (aly gwah al folder bta3e) -> agib al Variables -> aktb gwah (BaseUrl) -> w 7ot al value bta3to https://localhost:7014

            fa hagy 3nd al Endpoint aly hya  GetAllTypes tany w hyb2a al URL bta3ha -> {{BaseUrl}}/api/Proudact/Types

            3ayz atest a5r 7aga -> EndPoint Get ALL Brands .. nfs al klam b2a belzbt 
            url -> {{BaseUrl}}/api/Proudact/Brands

            ============================================================================================================
            fel EndPoint -> Get Proudcat by Id .. ana h7ot url b id m4 mwgod for test : 
            eh aly by7sl b2a ? 
            1 - fel Controller inside Endpoint -> hyro7 ll service GetByIdAsync() 
            2 - al service htla2i an al proudact not found -> fa trg3 result of Fail() wyrg3 beha ll Endpoint aly fel Controller 
            3 - b3d kda hyrg3 Function al ToActionResult() wy5o4 gwaha 
            4 - hyla2i al proudact is not success -> fa yb3tne 3la function al Problem() .. w y7dd no3 al Problem belzbt (NotFound)
            5 - fa yrg3 ll Endpoint problem 3la hy2t object Result
            6 - fel 25r alEndpoint t return al Result 
            7 - yban fel Postman b2a object feh details 3n al Problem .. {title , status 404 , details , errors (code , description , type)}

            fel 25r ana 3mlt Collection -> gwah folder -> gwa la Folder feh al 4 endpoints bto3y 3mltelhom test 
            al klam da kolo asmo (Documentation)
             */
            #endregion

            #endregion

            #region Session03

            #region Enhance Program Scope DI(Study Again) 
            /*
              Kona fel Api Layer in Program Extentions : 
              da folder feh al Pending migrations wb3mlha Seeding fel Database  

              fa al fkra anno odam hnt3aml m3 Databases tany zy Identity Database al 5asa bel Security 
              w hykon leha Extention folder brdo by able al migrate al 5asa bel Database de + seeding bta3ha 

              fa al fkra ana h5ly class wa7d bs aly 5as bel Seeding hwa aly y3ml migrate + seeding l 2y databases aly 3ndy 
            fa hn3ml eh ? 
            - Program Extentions -> hna5od goz2 al Pending cut -> w nro7 ll Infrastruture layer -> seeding folder 
              nrme feh al pending paste 
            yb2a f class al seeding da .. y5l2 object mn al DbContext (storeDb) .. w y4of al Pending migrations 
            y3mlha migrate .. b3d kda y3ml seed 
            fa ana 5let class al seed nfso hwa aly y3ml al 3mlyten dol (migrate + seed) 
            f nfs al Class aly hyt5l2 mno object 3la mostwa al Dependancy injection bdl ma kan fel Program Extention
            ==================================================================================================

            fel Class bta3 al Seed (CatalogeDataSeeders) fel Ctor bta3o object mn al Context (storeDb)
            hn4t5l b consept al (Key) .. w da 34an lw 3ndna Database tanya n2dr nkr2 mn nfs al interface (IDataSeeder) 
            mn 8ir ma nfdl nkrr object 3la kol database .. l2 ana bde al interface da key .. ay database t2dr tst3mlo 

            steps : 
            hangy 3nd al  ProgramExtention 

             var DbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
             var CatalogLogger = scope.ServiceProvider.GetRequiredService<ILogger<CatalogDataSeeder>>();
             CatalogDataSeeder catalogDataSeeder = new CatalogDataSeeder(DbContext, CatalogLogger);

             h4il al Objects de 
             w hbdlha b : 
              var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");

              tb lw 3nde class tany 48al b Context mo5tlf h3ml seed 5as beh b key mo5tlf 
              wlakn gwa al Function bta3t al ProgramExtention aly hya de MigrateAndSeedAsync() 
             */
            #endregion

            #region Picture URL Resolver 

            #region First Problem 
            /*
                  zhr m3aya mo4klten fel Response al 5asen bel GetProudcatById() and GetAllProudact()
                  1 - an al path bta3 al Photo gy mn al Database m4 kaml (l2n lazem server/PhatPhoto) 
                      w ana m3nde4 files 5asa bel photo 3nde hna 34an a3rf a2ra al photo nfsha 

                   fa 7l al mo4kla de ana 3ayz server/pathphoto 34an awsl ll sora bta3te 
                   fa ana fel postman hktb al url {{BaseUrl}}/photopath .. aly gyle fel json response .. 34an awsl ll sora btat3e 
                 */
            #endregion

            #region Second Problem 
            /*
              an fel josn Reponse brdo bta3 al GetAllProudact() fel postman 
              an TypeName : null 
                 ProudactName : null 
            w da 34an a7na m3mlna4 ay include .. fa de tany mo4kla 
             */
            #endregion

            #region Solution of First Problem 
            /*
              ana 3ndy folder tb3 al Project da feh kza sora 
              fa ana lazem a5li al swr de 3ndy 3la al server hna (inside solution)
              w 34an a3ml kda 3ndy 7len : 
              - y2ma arme al folder da gwa al Bin zy ma 3mlna m3 al Json bta3 al Data Seeding 
              - y2ma a3ml folder 5as gwa al Server (sloution) a2dr awsl beh bel path bta3o fel project .. w da al afdl

              steps : 
             - api Layer -> righClick -> new Folder -> Files 
              al url bta3 al sora whya rag3a fel response json kda : imges/proudacts/kmittedsweater.jpg

            - fa ana gwa folder File -> create new Folder -> Images 
            - gwa folder Images -> Create NewFolder -> Proudacts 
            - wb3d kda hnrme al swr fel Folder aly asmo Proudcats (al swr m4 mwgoda dlw2t)

             wna 3ayz fel Final Result lma a3ml Request fel Postman 
             h3ml new Request -> w asmeh All Images 
             aktb Al Url da : {{BaseUrl}}/Files/images/products/asm al sora => ygble al sora nfsha 

             fa lazem a3ml kza 5twa 34an da y7sl : 

             1 - fel MiddleWares in Program.cs .. mfe4 service 5asa b kra2t al Files 
                 fa h3mlha =>      app.UseStaticFiles(); .. de btkr2 mn wwwroot by defult bs ana 3ayz akr2 mn folder al files aly 3mlto 
              fa m7tag a8ir al Cinfiguration bta3ha 34an app.UseStaticFiles() t2ra mn Files folder bdl WWWRoot : 

               app.UseStaticFiles(new StaticFileOptions { 
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath , "Files")), 
                RequestPath = "/Files"

              kda lma agy a run w aro7 ll postman tany 34an a3ml request .. hro7 ll new request aly 3mltha (Get Images)
              w7ot al URL : {{BaseUrl}}/Files/images/products/kmittedsweater.jpg 
              => hy3rdle al sora 
             */

            #region Another Problem
            /*
              Problem : 
              feh kda mo4kla bseta .. an ana al mfrod bktb al url da 34an tzhr al sora .. wlakn aly bytb3t ll Front-end
              fel Json response hwa PhotoPath bs (al defult 3ady) gy mn al Database .. fa kda m4 hy4t8l m3ah 
              fa ana dory k back end any ab3tlo al URL bta3 al sora kaml fel response .. m4 hygm3 al Path zy m3mlt 
             */
            #endregion

            #region Solution of Problem 
            /*
              34an agm3 Path al sora fa ana bbd2 mn al Service .. 
              3n tri2 any bb3t al Sora 3la hy2t DTo fel service aly asmha GetAllProudactsAsync()
              fa al service de bt5o4 tklm al Repo -> tgib all data of paroudcts menhom path al sora 
              w b3d kda bt3ml mapp from model -> Dto 

              fa ana momken a3dl f goz al mapping bta3 al sora da .. b7is azbt al Path bta3ha 

              fa h5o4 fel Application Layer -> Profiles : 
              .ForMember(dist => dist.PictureUrl, opt => opt.MapFrom()); 
              ana h3ml Configuration ll PictureUrl 34an agm3 al path .. w da hya5od mne kza 5twa 
              fa m4 hynf3 ast5dm -> opt.MapFrom()); l2nha function bta5od one line bs wna h3ml Configuration 

               fa feh 7aga fel mapping asmha Resolve .. whya any a3ml Class asmo Resolver Class 
               tb al Class da n3mlo azay ? 
              1 - make Class in -> profile folder -> asmo PictrueUrlResolver

               b3d ma 5lst kol 7aga fel Class implemntaion (akr2o) PictrueUrlResolver
               fa lma agy a run fel postman fel new request bta3 GetAllProudactById : 
               hygble fel response bta3 al proudact mo3yn wel path bta3 al Picture kaml 
               {{BaseUrl}}/Files/Proudacts/ImageName.. b7is al Front yktbo k url yzhrlo al sora 

               lakn fel awl kan fel response json al url m4 kaml .. kan kda : "images/products/MensPoloShirt.jpg",

             w kol da 34an al Json lw 3ard Attachment (sora) .. fa lazem a resolve al mo4kla de l2no m4 byrg3 al correct path bta3ha

             */
            #endregion

            #endregion

            #endregion

            #region Spacification Pattern (Include)

            #region Problem 
            /*
              an fel josn Reponse brdo bta3 al GetAllProudact() fel postman 
              an TypeName : null 
              ProudactName : null 
              w da 34an a7na m3mlna4 ay include .. fa de tany mo4kla 
             */
            #endregion

            #region Intro
            /*
              tb men by geb al Data mn al Database asln -> al Generic Rebo aly fel Infrastructure layer 
              hwa aly by Build al Query aly hgib mnha al data 
              tb lw git a3ml fel GetAll() or GetById() => .Include() XXX m4 hyrda 3l4an anta bt4t8l 3la 7aga mn no3 TEntity<>
              y3ne 7aga Generic .. wel Include() m4 bt4t8l m3 al Generic Type 

              fa al TEntity<> 100% mfha4 7aga mn no3 Brand and Type gwa al Proudct msln 

              wkan 7l al mwdo3 da fel Spesefication Repo anna kona bn3ml Repo m5sos ll Type aly 3ayzen n Collect leh al Data 
              bta3to b4kl mo3yn .. w da ka working around m4 a7sn 7aga 
              l2ne kda ana h3ml new Repo b new Service b new Endpoint .. da 34an ana 3ayz a collect al Data l type mo3yn b4kl mo3yn 

              fa aly bndwr 3leh hwa anna n3ml Query Dynamic for GetAllAsync() , GetByIdAsync() .. b7is a3ml behom functions mo5tlfa 
              zy any a serch 3la 7aga .. a sort 7aga aw 7ta any a Filter 7aga 

              w 34an a3ml da => Spacification Design Pattern
             */
            #endregion

            #region Spacification Pattern Continue.
            /*
              Main Goal => an ykon 3ndk Functions fel Generic Repo t2dr t3mlk Dynamic Query w da bona2n 3n al Tlb bta3e 
              34an m4 tbe3e lma a3oz data b4kl mo3yn a3mlha static Query (Function Monfsla)

              tb al Pattern da 48al azay ? 
              ana 48al b LINQ () .. y3ne Methods .. wel Methods de bta5od mne Paramters .. method(param) .. eh r2yk al3b 3la al Param de
              y3ne msln : 
              where (Exepression<Func<Entity,bool>>) 
               Expression y3ne type y2dr y7tfz gwah bel Lampda Expression (=> )
              fa fel Pattern da a7na momken nl3b 3la al Paramters Expression<Func<Entity,bool>> .. aly Linq methods bta5odha mnna
              bm3na
              msln DbContext.Set<T>.Where() 
              ana b2olo b2a fel Pattern .. abd2 a3ml al where() de f 7alt lw m3aya al paramter da Expression<Func<Entity,bool>>
              y3ne lw al Expression da m3aya = andh 3la al where() .. m4 m3aya = m4 hndh 3la al where()

              fkrt al paramter de bnsmeha -> Spasification .. tol ma al Paramter(spasification) gwah value = andh 3la al Linq Method Bta3tk

              w mezet al Pattern da b4kl 3am An ana kan s3b a mintanince al Generic repo bta3e .. w kman s3b fel test wlakn al 
              pattern da 7l al mo4kla 

              fa a7na hntb2 al Pattern da 34an mo4klt la Include() -> Linq Method .. h5liha t2dr tgble al brand + al type m3 al proudact
               fa hya kda bt3ml Dynamic Query w da hdf al Spasification pattern 
             */
            #endregion

            #region Implemnt Spasification Pattern 
            /*
               1 - in Domain Layer -> Cotract -> New Interface -> ISpecifications
                   tb b3d kda h3ml eh ? 
               2 - b3d ma 7drt al Expression .. aly hwa al Paramter aly byt7t gwa al Linq method .. Include(Expression)
                   hro7 b2a 3la al Generic repo l2n da al mkan aly ba build feh al Query asln .. hla2i GetAllAsync() fel 3ady bt return Static Query 
                   fa hro7 3la IGeneric Repo .. ha5od al Signature bta3t GetAllAsync() Copy paste tany fel IGenericRepo bs h3dl 3leha kda : 

                => Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity , Tkey> specifications , CancellationToken ct = default);

                3 - Implemnt Interface IGenericRepo in -> GenericRepo Class in InfraStructure Layer 
                     w h5li al Include(Kza Expression) b7is mra t4t8l 3la Expression 5as belProudactTypes 3ady 
                     w hya hya nfs al Include t4t8l 3la Expression tany for ProudactBrands .. t get all lkza type l2na b2t Dynamic Query

                public Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default)
        {

            if (specifications.IncludeExpressions.Count > 0)
            {

                foreach (var expression in specifications.IncludeExpressions)
                {
                    dbContext.Set<TEntity>().Include(expression);
                }
            }
        }
             bs feh mo4kla ll Function de an ana kda al mwdo3 hykbr mne l2ne ana 3aml Kda bs for Include() Linq Method 
             tb al Where() tb ay Linq method tany 3ayz a3ml nfs al Klam fa al Method kda htkbr mne awe 

             tb ma n3ml msn3 .. wel msn3 da doro ybne al Dynamic Query al mkan da asmo Specification Evaluator 
             y3ne al klam da m4 hyb2a gwa al Function : 

              if (specifications.IncludeExpressions.Count > 0)
            {

                foreach (var expression in specifications.IncludeExpressions)
                {
                    dbContext.Set<TEntity>().Include(expression);
                }
            }
            da hyb2a gwa al Specification Evaluator aly doro ybne Dynamic Queries 

            4 - fa hro7 al Generic Repo tany ams7 aly 3mlto gwa al Function w a5leha kda : 

             public Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default)
        {

        }
            5 - w hagy 3nd al Reposetiors folder in Infrastructure layer -> make new class -> SpecificationEvaluator
               w 4ar7 al Implemnt bta3o feh 

            6 - h3ml gwa al SpecificationEvaluator function asmha CreateQuery() dorha  anha tbne al Query from kza table 
                b linq method Include() aw ay Linq method bra7ti .. w fel a5r bbasi ll Linqmethod(Expression)

             7- b3d ma b5ls implemnt CreateQuery() -> ynady 3leha al function aly fel GenericRepository 
                GetAllWithSpeceficationsAsync() .. 34an t get al Query aly gya mnha w t3rda f toList() 

             8 - men aly byklm al function de GetAllWithSpeceficationsAsync() -> aly byklm al GenericRepository 
                 aly hwa al Service aly Application Layer (ProudactService Class)

             9 - f service GetAllProudacts -> bdl ma btst5dm function GetAllAsync() 
                 h5leha tst5dm GetAllWithSpeceficationsAsync() .. leh ? 
                 34an al proudacts ykon m3aha al Include data bta3t al Brands and Types 
            kda -> var Proudacts = await unitOfWork.GetRepository<Proudact, int>().GetAllWithSpeceficationsAsync(specifications,ct);

            hyb2a feh erorr fel Function de anha htkon 3ayza al specifications .. agbha ll Function azay da interface y3ne 3ayza Class y implemntha

            whna byygy a5r 5twa ank l kol type anta bt3mlo Specifications 5asa beh hwa 
            y3ne kol Type 48al beh t3mlo Specification class y implemnt al Interface 5as beh hwa 

             10 - Fel Application layer -> RightClick -> new Folder -> Specefications 
             11 - h3ml gwa al Folder -> baseClass feh a8lb al Spesification pattern (l2no by Implemnt ISpecifications Interface) .. 34an awaraso mra wa7da bs

                  a3ml gwaha al Implemntation bta3o w hya function bt add Expression gwa al List of Include() aly hya IncludeExpressions

             12 - a3ml Class tany gwa Fel Application layer ->  Specefications -> ProudactSpecification
                   wda class doro ano ywrs mn al BaseClass aly feh mo3zm al Spesifications 

             13 - fel PoudactService in service GetAllProudactsAsync() 
                  hnady 3la al Class da => var spec = new ProudactSpecification();
              wb3den abase al spec 
            var Proudacts = await unitOfWork.GetRepository<Proudact, int>().GetAllWithSpeceficationsAsync(spec, ct);

            fa da m3nah an al specificaions httkwn mn al object bta3 al Class var spec = new ProudactSpecification(); 
             w hrme al spec ll function GetAllWithSpeceficationsAsync() l2nha 3yzaha fel input pramter 
            7sb b2a aly ana 3yzo bmzagy 

             fa kda kol mra h3ml GetAllWithSpeceficationsAsync() fel GetAllProudactsAsync() 
             hygble etnen include aly 3amlhom f ProudactSpecification class 
             AddInclude(p => p.Brand);
             AddInclude(p => p.Type);
             */
            #endregion

            #region Summary Of Specification pattern 
            /*
              - Run App 
              - Open PostMan w aft7 al Request bta3 GetAllProudcats .. w aktb al URl : {{BaseUrl}}/api/Proudact

             1 - hygibk 3nd al ProudactController 3nd al Action al 5asa bel URl 
             2 - hyla2i gwah al Service w ywdine 3ndha GetAllProudactsAsync()
             3 - fa al service tbne Specifications -> var spec = new ProudactSpecification();
             4 - w t5o4 3la Class al proudactSperfications .. y3mle include() ll brand .. w include ll type() 
             5 - fa al var Proudacts = await unitOfWork.GetRepository<Proudact, int>().GetAllWithSpeceficationsAsync(spec, ct);
                 spec -> feha 2 specifecations (brand , type)

             6 - fa al GetAllSpesifications() aly fel Generic Repository tstlm mne al Specifications 
                 fa yla2e an al GetAllSpesifications() bt createQuery() .. 3la 7sb al Spesifications aly gyalha de 
                 fa bt create al Query 3n tri2 al Specification Evaluator 

              7 - y5o4 gwa al Evaluator -> ybne al Query 
                  wybd2 y3ml Aggregate .. include(brand).include(type)
                  w y return fel a5r al Query 

              8 - w yrg3 beha ll Service tany GetAllProudactsAsync() 34an yb3t b2a ll EndPoint tb3t json feh 
                  data of proudacts + include of Type , Include of Brand .. mb2o4 b null wla 7aga 
             */
            #endregion

            #endregion

            #region Specification Pattern (Filter by Id) 

            #region Intro
            /*
                 Al Goal anna fel NewRequest bta3t al GetAllProudact By Id fel Postman anna n filterhom 7sb al Id 
                 bel Specification brdo 
                 fa al mra de hn4t8l 3la Linq Method Where() .. awl so2al hya where bta5od mne Expression no3o eh ? 
                 = Expression Func Of Curren Entity and Return lya Boolen .. where(p=>p.Id)

                */
            #endregion

            #region Steps 
            /*
              1 - hagy 3nd al Interface of Spesification w adif Expression .. hwa gwa al Domain Layer -> Contract .. 34an a3ml al Expression bta3 al Where(Expresison)
              2 - htl3 3la al SpecificationEvaluator fel InfraStructure Layer -> Repositores .. 34an a handle al Query bel Expression aly 3mlto

               note : ana kda fel GeneircRepository f Function 
              public async Task<IReadOnlyList<TEntity>> GetAllWithSpeceficationsAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default)
              ana momken ab3tlha Specification b aktr mn 4kl ll Query bta3te.. momken mra Include() , momken mra Where() .. ana kda kda mtb2 al Two Expression 3ady 

             3 - in IGenericRepo -> in DomainLayer 
                 h3ml Function ll GetById() bs with Specification al mra de .. ana kan 3ndy wa7da without Specification 

              4 - htl3 3la al Generic Repo a3mlha Implemntation .. 34an a creat Query kda tb2a gahza m3aya 

              5 - 5lst al Repository -> atl3 3la al Service fel Application layer 
                    public async Task<Result<ProudactDto>> GetProudactByIdAsync(int id, CancellationToken ct = default)
                 h3dl feha .. bdl ma hya btst5dm gwaha al Static Query -> GetByIdAsync() 
                 hbdlha bel Dynamic Query aly 3mltha GetByIdWithSpecificationAsync(id ,ct) 
            htdene error fel id .. l2nha 3ayza specification .. 
            fa kont 3aml object da (hl2ih f GetAllProudact()) ->  var spec = new ProudactSpecification(); 
            7oto fel service 34an t alpplay al Specification 
            b3d kda 5leha kda : var Proudact = await unitOfWork.GetRepository<Proudact, int>().GetByIdWithSpecificationAsync(spec, ct);
            kda bona2n 3la al Specification aly hbasiha = hybd2 y4t8l 

            bs feh mo4kla .. an al ProudactSpecification -> in Application Layer .. gwah al Spesification bta3t al Include() bs .. m4 m48l al Creataria 

            fa h a5leh y applay al Cretaria : 
            6 - hro7 ll BaseSpecifications -> hro7 crtl + . -> hynzle al Cretaria()

            7 - hrg3 ll ProudactSpecification a3ml ctor tany 5as bel Where() .. yb3t ll BaseSpecification 
              -> al BaseSpecification yst2blha fel Ctor bta3o Cretaira = Cretaria 

             8 - ro7 bs ll Service tany adeha al id 
            => var spec = new ProudactSpecification(id); // yb2a ana kda bkm al Ctor al tany bta3 al GetById() 
             */
            #endregion

            #region Depugging 
            /*
              postman fel NewRequest bta3tGetById() h7ot al Url .. 
              1 - hyro7 3la al EndPoint 
              2 - hy5o4 3la al Service yla2i -> var spec = new ProudactSpecification(id);
              3 - ProudactSpecification htro7 ll ctor da -> public ProudactSpecification(int id) : base(p=>p.Id == id)
              4 - hy3de 3la al Base l2no 3amlo Chaning .. ybasi al Cretaria 

                   protected BaseSpecifications(Expression<Func<TEntity, bool>> cretaria =null)
        {
            Criteria = cretaria; .. p=>p.id == value aly ana mbasiha
        }
              5 - yrg3 tany ll ProudactSpecification y3ml 
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);

              6 - yro7 ll service tany fyla2i ->  var Proudact = await unitOfWork.GetRepository<Proudact, int>().GetByIdWithSpecificationAsync(spec, ct);
                  fa yro7 yklm al Repo 

               34an yro7 ll Generic Repo w ytl3le al Query 
               public async Task<TEntity?> GetByIdWithSpecificationAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default)
        {
            var Result = SpecificationEvaluator.CreateQuery<TEntity, Tkey>(dbContext.Set<TEntity>(), specifications);

            return await Result.FirstOrDefaultAsync(ct); // Query for Get One Item 7sb al Id bta3o 
        }

             7 - fa y create al query bel SoesificationEvaluator Class  (hla2i an al Specifications .. al Creatira feha 1 p=>p.id == value .. wel Include = 2)
             fa y build al Query 3la asas any 3nde 2 include w 1 where f tb2a bel 4kl da : 

             where(p=>p.id == 5).include(p=>p.brand).include(P=>p.Types)

             8 - fa yrg3lk al Query fel GetByIdWithSpecificationAsync() in Generic repo  w t3mlk fel a5r  return await Result.FirstOrDefaultAsync(ct);
             9 - b3d kda yrg3 ll PoudactService ynfz ba2i al GetByIdAsync()

            note : ana lw 3ayz a filter al GetAll() 7ta .. ab3t fel ProudactSpecification 
             public ProudactSpecification():base(null) .. mkan al null hna filter Expression bs 

             */
            #endregion

            #endregion

            #region Specificaion Pattern (Filter by BrandId & TypeId)

            #region Intro
            /*
                3yzen n applay al Specification pattern 3la service GetAllAsync() tany 
                b3d ma zbtna feha 7tt al Include() .. 
                hn5liha tt3aml m3 al BrandId , TypeId .. bel Where() brdo 
                fa h5li al Front-end yb3tle al m3lomten dol  BrandId , TypeId aw wa7da mnhom .. wna hgblo al proudcat da 
                w a3mlo return fel Json

                b7is brdo 22olk lw 3ndy 10 proudcats .. 
                momken agblk aly al prandid bta3hom kza .. fa yb2o 2 proudacts bdl 10 

                al mwdo3 m4 hyb2a feh l3b fel (ISpecification wla Specification Evaluator .. l2n al Where() m3mola kda kda)
                al fkra any h8ir fel Expression bta3 al Creatiara b4kl 3am 
               */
            #endregion

            #region Steps 
            /*
              1 - Fel Proudct controller 
                hzwdlha paramters 
               public async Task<ActionResult<IReadOnlyList<ProudactDto>>> GetAllProudacts(int? brandId, int? tybeId ,CancellationToken ct)
            kda ana hst2bl mn al front mn al url aly hyb3to f postman al two values dol s
            wh5li al Service aly bnady 3leha : 
             var Proudacts = await services.GetAllProudactsAsync(brandId, tybeId,ct);
             w tb3n hydene error l2n al Sevice mfha4 al Klam da 

            2 - fa hro7 3la al Interface -> IproudactService w3dl al asignature 
                 Task<Result<IReadOnlyList<ProudactDto>>> GetAllProudactsAsync(int? brandId ,int? tybeId,CancellationToken ct = default);

            3 - hro7 ll ProudcatService Class brdo adif fel function al two paramters dol 

            4 - b3den f (nfs al service) h5od al two paramters dol zy mhoma ll Specifications 
                var spec = new ProudactSpecification(brandId , tybeId);
                w hydene error l2n mfe4 fel ProudactSpecification al two param dol 
            h2olo bs fel ctor bta3o ast2bl al two pram dol -> public ProudactSpecification(int? brandId, int? tybeId) :base(null)
            bs 5lik fahm an brandId , tybeId = null .. l2n al Base(null)

            fa ana 3ayz ab3tlo ap brandId  , TypeId .. lw h get al proudcat 3la 7sabhom 
            fnfs al w2t ana 3ayz a get all proudact w 5las zy mna (without brandId and typeId)
            fa h5li al Ctor ll base fel ProudcatSpecification kda : 
            :base(P =>(!brandId.HasValue || P.BrandId == brandId) && (!tybeId.HasValue || P.BrandId == tybeId)) 
            da m3nah : 
            true & true = (13 proudact) GetAllProudcat
            value & True = (Brand)
            true & value = (type)
            value & value = (brand and type)

            y3ne ana kda bset ll base al Cretaria bta3te (where(condition)) al Condtion da y hyfilter bel where 3la 7sb al TypeId aw 
                                                                                             hyfilter bel where 3la 7sb al BrandId aw 
                                                                                             hyget all proudacts
            fa b3d kda yro7 ybne al Query t3ml al where w t7ot al klam da gwaha   

            lakn fel Ctor al tany fel ProudactSpecification : 
              public ProudactSpecification(int id) : base(p=>p.Id == id) 
              hna ana bb3t where(condition) ll base .. m3nah any hb3tlk al Id bta3 al Proudcat .. fa ytbne al Query 3la asas da 
             */
            #endregion

            #region Run
            /*
              hft7 Postman 
              - add new Request -> smeh Get Proudacts With Filter 
              wh3ml fel Url -> https://localhost:7014/api/Proudact where(True& True) = Get All proudacts 
              https://localhost:7014/api/Proudact?brandId=5 -> where(p => p.BrandId == brandId && true) = get proudacts by brandId (2 proudacts)
              https://localhost:7014/api/Proudact?tybeId=3 -> where(true && ...) = get proudacts by brandId (2 proudacts)
              https://localhost:7014/api/Proudact?brandId=1&tybeId=1 = (1 proudact only)

              note ana fel postman 3nd section al Paramter dft 
              brandId 
              typeId .. bnfs al syntax kda l2n dol aly b4t8l 3lehom 
             */
            #endregion

            #endregion

            #region Specification Pattern (Search by name)

            #region Intro
            /*
                 hna ana 3ayz a search by proudact name .. fa ygble bynat al Proudcats kolha
                 fa ana brdo lsa 3nd al Where() (Cretaria) .. y3ne m4 h3dl 7aga fel Spercfifcations 5als 
                 fa h3ml options fel where() ano y serach by name of proudact 
                 yb2a overall al weher feha eh : 
                 BrandID , TypeId , Search by name 
                */
            #endregion

            #region Steps 
            /*
              1 - hro7 3nd al Proudact Controller azwd paramter 3nd GetAlProudacts : 
       public async Task<ActionResult<IReadOnlyList<ProudactDto>>> GetAllProudacts(int? brandId, int? tybeId , string? SearchValue ,CancellationToken ct)

             tb ana h3ml nfs al lfa tany bta3t al (Filter by TypeId & BrandId) kol ma azwd paramter ?? 
             = fa hnzwd class bygm3 kol al Paramters .. b7is lw hzwd feh 7aga = ysm3 3ltol 

             2 - in Application Layer -> new folder -> Params
             3 - make Class inside it -> ProudactQueryParams

             4 - fel ProudactController :
              bdl public async Task<ActionResult<IReadOnlyList<ProudactDto>>> GetAllProudacts(int? brandId, int? tybeId , string? SearchValue ,CancellationToken ct)
               h5leha 
                      public async Task<ActionResult<IReadOnlyList<ProudactDto>>> GetAllProudacts(ProudactQueryParams queryParams,CancellationToken ct)
            hytl3le error l2n GetAllProudactsAsync() al service de m4 btst2bl mny ProudactQueryParams

            5 - fa hro7 3la al IproudactService 
              w5le Task<Result<IReadOnlyList<ProudactDto>>> GetAllProudactsAsync(ProudactQueryParams queryParams, CancellationToken ct = default);

            6 - fel implemnt bta3ha b2a fel proudactService nfs al klam 
            public async Task<Result<IReadOnlyList<ProudactDto>>> GetAllProudactsAsync(ProudactQueryParams queryParams, CancellationToken ct = default)

            fnfs al service -> var spec = new ProudactSpecification(brandId , tybeId); mdene error 
            fa h5leh kda var spec = new ProudactSpecification(queryParams);
            brdo mdene error 

             7 - fa h5o4 gwa al Ctor bta3 ProudactSpecification bdl :
            public ProudactSpecification(int? brandId, int? tybeId)
            h5leh 
            public ProudactSpecification(ProudactQueryParams queryParams)

             kda ana lw 3ozt adif ay paramter gdid -> 7oto fel ProudactQueryParams
             wzbto f class al ProudactSpecification .. bs kda bdl al lfa de 
             m4 h3dl b2a wla f service wla end point wla repo 

             hagy a run w aft7 al postMan 3la al  request bta3t -> Get All Proudacts by Filter 
             w ab3t url msln :  https://localhost:7014/api/Proudact?brandId=5  
             hydene error .. ano m4 3arf ywsl ll Endpoint asln aly fel Controller .. tb leh ?? 
             l2nha feha (Complex paramter) y3ne 7aga feha aktr mn paramter aly hwa -> GetAllProudacts(ProudactQueryParams queryParams,CancellationToken ct)
             ProudactQueryParams queryParams -> Complex Param .. fa 34an a7l al mo4kla m7ag a2ol al complex param da ana mgm3o mnen ? = mn al query
             fa hyb2a kda : 
            GetAllProudacts([FromQuery ]ProudactQueryParams queryParams,CancellationToken ct)

              hgrb a run tany -> hy4t8l w y2ole an al endpoint feha proudatId = 5 , tybeId = null , SearchVlaue = null
            ==============================================================================================================================
            kda ana hndelt al Class aly feh kza paramter h4t8l behom 
            na2s azbt al Critaria where() aly fel ProudactSpecifications 34an al SerachValue 
            kda 
             public ProudactSpecification(ProudactQueryParams queryParams)
            : base(P => (!queryParams.barndId.HasValue || P.BrandId == queryParams.barndId)
            && (!queryParams.typeId.HasValue || P.BrandId == queryParams.typeId)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower()))) -> al Goz2 da dfto for SearchValue
            m3nah 
            string.IsNullOrEmpty(queryParams.SearchValue) -> true .. y3ne mfe4 VlaueSerch bel asm da 
             P.Name.ToLower().Contains(queryParams.SearchValue.ToLower()))) -> True .. y3ne search bel Vlaue de tmam 

            run : 
            fel postman in Get All Proudact By Filter 
            - add new param -> SearchValue .. wel value msln Classic 
            url : https://localhost:7014/api/Proudact?SearchValue=Classic -> gable two Proudacts fehom searchvalue name mn asl 13 proudacts
             */
            #endregion

            #endregion

            #region Specification Pattern (Sorting)

            #region Intro
            /*
                 hnst5dm ka Linq Method fel Sorting -> OrderBy() .. w feh mnha 4klen 
                 OrderBy() , OrderBydcending() .. hntb2hom fel Specificaion pattern 
                */
            #endregion

            #region Steps 
            /*
              1 - htl3 3la interface ISpecification fel Domain layer a3ml OrderBy() OrderByDesc()
              2 - atl3 3la al Specification Evaluator 3dl 3la al Query in Infrastructure layer 

                  l7d hna ana dft 2 specefications godad .. w 3delt fel Queries bto3hom 
             note : w fel Genericrepo ana m4 h3dl feh 7aga l2n function GetAllWithSpecificationAsync() kda kda bb3tlha al Spesefications bta3te

              3- kda al mo4kla htb2a fel Service : 
                el Postman ana hyb2a 3nde param asmo sort .. w byb3tlk hwa hy sort b eh (da al ragel al front-end)
                fa ana h sort b 4 a4kal : (nameAsec .. NameDesc .. PriceAsec .. PriceDesc)

                fa htl3 3la al Application Layer -> Params 
                34an a5ls mn fkrt ano lazem yb3tle asm al param (nameAsec,NameDesc..)
              4-  ana momken a3ml Enum Folder gwa al Param Folder .. b7is yb2a odamo ano yb3t string aw int value 3ady 
               y3ne hwa fel post man : param : Sort  value : 1 -> fa y3ml msln nameAsec 
               w hwa hyb2a 3arf an 1 for kza , 2 for kza l2ni hb2a mfhmo fel Documantation aly hb3to

              5 - hro7 fel ProudcatQueryParam a3rf al Enum da b2a as ano Paramter hnak 

              6 - htl3 3la al Ctor bta3 al ProudactSpecifications fel Application Layer t7ot logic al Query 
                  lw al Value aly yalk = NameAsc -> orderby(p=>p.name) whkza fel ba2i 

                  bs al OrderBy() and OrderbyDec() mnzlo4 asln fel ProudactSpecifications 34an ast5dmhom for logic 
                  mfrod akon warshom mn al BaseSpecification .. bs mnzlo4 leh fel Base? 
                  34an ana m3mlt4 implemnt ll ISpecefifcation interface insie BaseSpecification

              7 - aro7 a3ml implemnt ll ISpecefifcation interface insie BaseSpecification
              public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
              public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }
              w3mlhom two Function for Private Set

              8 - 5las ana kda wrst al Functions .. arg3 tany ll Ctor bta3 al ProudactSpecifications fel Application Layer t7ot logic al Query 
             */
            #endregion

            #region Debug 
            /*
              7ot break point 3nd Service GetAllProudactsAsync() fel ProudcatSevice 
              fel PostMan in Get All Proudacts endpoint : 
              - add paramter -> Sort  value : 3 (aly hya priceAsc)

              hla2i GetAllProudactsAsync() feha BrandId = null , TypeId = null , SearchByVlaue = null , Sorting = PriceAsc
              -> var spec = new ProudactSpecification(queryParams); -> y5o4 gwa ProudactSpecification
                 ya5od AddInclude(p => p.Brand);   AddInclude(p => p.Type); + ynfz al Sort switch (ll priceAsc)
              -> yrg3 tany ll GetAllProudactsAsync() f goz2 : 

                var Proudacts = await unitOfWork.GetRepository<Proudact, int>().GetAllWithSpeceficationsAsync(spec, ct);
                Cretirea = kolo = true , IncludeExpression = 2 , OrderBy = (P=>p.price) , OrderByDesc = null 
              -> y5o4 ybne al NewRepo inside al UnitOfWork -> yro7 ll GenericReo ynfz function GetAllWithSpecificationAsync()
              -> tndh 3la function CreateQuery() htndh 3la al where() 34an kolha true + 3la al OrderBy() + hro7 ll OrderByDesc m4 hyla2i feha 7aga + y return al Query
             */
            #endregion

            #endregion

            #endregion

            #region Session04

            #region Specification Pattern (Pagination)

            #region intro 
            /*
                 al Frka lma ykon 3ndk fel Database Records ktir like 1M record w get t3mlhom GetAll() ll browser 
                 fa hy7sl mo4kla l2n al Browser bta3 al Client nfso feh Memory leha Limit  
                 kman m4 hyb3t kol al Data aly 3amlha GetAll() mn al Database fel BackEnd 

                 fa 7l al mwdo3 hwa al Pagination , any 22sm al data de l 2gzah w al Browser lma y3oz goz2 bs ab3tholo 
                 wa5zn al ba2i.. w lw 3az goz tany w hkza .. w da hy3le al Performance ll end user aly hyst5dm al app m4 hyb2a t2il
                */
            #endregion

            #region Proccess of Pagination
            /*
              front-end byb3tle m3lomten : 
              - page size , - page index 
              page size -> 7gm al Data aly anta 3yzo fel page .. msln 10 proudacts .. (y3ne f kol Request = ab3t 10 proudcats)
              page Index -> rkm al Sf7a .. y3ne msln ana 3nde 100 proudact .. wnn b3rd fel page 20 .. y3ne ana 3ndy 5 pages 
              fa btb2a kda (prouact01 .. proudact02 ... proudact03 .. proudact04 .. proudact05) kol wa7da fehom 20  items

               fa al front byb2a ba3t Page size = 10 , page index = 2 .. fa yb2a 3ayz items mn 20 -> 30 
               feh back -end by5lik tb3t al Page Index bs .. lakn hwa aly by7dd al Size aly rag3 (al page size)

              fa al front end byb3t al m3lomten lya fel request .. w da ytb2 3nde fel Database b7aga asmha (offset - featch)
              offset = skip .. featch = take 
             for Exampel : 
             page size = 2 , page index = 2 
             take(2) skip(2) -> proudact 3 and 4


             kman al Front-end y2dr y3ml Pagination ll enduser .. bs al asas mne .. l2no bytlom 100 item .. fa ana bb3tlo 100 
             mn asl 1000 msln , fa already feh 100 item at7mlo 3la al Browser memory .. al front b2a momken y3ml pagination ll 100 
             b7is y3rd kol page = 20 
             lw hwa 5ls al 100 -> byb3tle new request .. fa ana bbd2 a skip(100) wa take(100) ..  100 -> 200 

             fa ana anhrda htb2 al Linq method take() skip() bel Specification Pattern 
             b7is al Quey fel a5r tb2a 4klha kda : 
             _dbcontext.Proudcts.OrderBy(p=>p.name).Skip(2).take(2) 
              fa al two functions dol hystlmo mne bs Int values 
             */
            #endregion

            #region Steps 
            /*
              1 - hft7 al ISpecefication fel Domain Layer a3ml al Expression 
              2 - hro7 3la al Specifiction Evaluator
              3 - m4 hro7 ll GenericRepo a3dl feh 5las .. htl3 3la ProudactService fel Application Layer 
                  whbd2 aghz al Specifications fel GetAllProudactsAsync() .. fa a7na 3ayzen nzwd specification 
                  ll service hna ->  var spec = new ProudactSpecification(queryParams);

              4 - f hnro7 3la al BaseSpecifications al awl .. a implemnt al ISpecification interface aly feh 34an ynzly 
                  al two Functions Take() and Skip() and IsPaginated() 
            note : 
            ana aly b7dd al IsPaginated() de b true wla false .. fa h3ml Function asmha ApplayPaination() 
            dorha anha t5li IsPaginated = True .. w hndh 3leha fel ProudactSpecifications 
            fa al Specification Evaluator ynfz al if(IsPagenated) aly gwah Take() and Skip() 
            bs al Take() and Skip() aly gwah fadyeen .. fa brdo f function ApplayPaination(int pageSize , int PageIndex)
            w dol hbasihom ll evaluator tany .. yb2a take = pageSize wel skip = PageIndex

              5 - kda fel proudactSpecifications =>  ApplayPagination(); erorr .. na2s al Front hwa aly yb3t gwaha 
                  al PageSize and Page Index 
               fa hro7 3la Folder Param -> aly fel Application layer .. w zwd al Two Paramters dol w3ml 3lehom al Validations 

             note : kda al paramters de by defult athndlt fel ProudactController .. GetAllProudacts() l2nha bta5od ProudcatQueryParams
                    nfs al Kalm athndl fel Service GetAllProudactAsync() .. l2nha bta5od mne ProudcatQueryParams

              6 - arg3 b2a ll ProudactSpecification Class 
                  ApplayPagination(); ->  ApplayPagination(queryParams.pageSize , queryParams.PageIndex);
             */
            #endregion

            #region Debug 
            /*
             * break Point in Controller 

             - post man Get All Proudcts 
               lw b3t fel URL ano y get all proudcts -> hygblk fel Controller by defult an 
               PageSize = 5 , PageIndex = 1 
             - al Controller hywde 3la al Service GetAllProudactAsync() 
             - hy5o4 gwa al ProudactSpecification .. y3l AddInclude + swith bta3t al OrderBy + Pagination 

             - fa hla2i an al Spec aly f service -> GetAllProudactAsync() 
                skip = 0 , take = 5 

             - b3d kda ybne alRepo wel repo ybne al Query .. wel result aly htb2a tl3aly = 5 items 
            =================================================================================================
            tb grbt fel post man a add paramter : PageIndex =2 and PageSize = 6 .. (kda al mfrod ykon 3nde 3 sf7at) hygble mn awl item 7 ... item 12 
            tb PageIndex =3 and PageSize = 6 -> hygble 1 item bs aly hwa item 13 
             */
            #endregion

            #endregion

            #region Pagination Standerd Response 

            #region Intro 
            /*
      fkrt al Response aly rag3 fel Josn byb2a data 3ltol .. fa da ll Front-end lma y2ra data 
      m4 hyb2a al mwdo3 mfhom awi .. y3ne msln fel pagination ana rg3t 7 items ka data .. bs mdtho4 any 
      info zyada .. y3ne mn asl kam item msln whkza .. fa kda 4kl al Response m4 a7sn 7aga 
      fa al hdf an al Response bta3e lma a4ofo f postman .. myb2a4 data bas aly rag3a .. tb2a data + info about data
      lazem ab3tlo fel response : 
      page index : ...  page size : ...  total Eelemnt : ... 
     */
            #endregion

            #region Steps 
            /*
              1- in application Layer -> Common -> make new Class -> PaginatedResult 
              wel Class da feh al info aly htt3rd fel Json .. 3mlt fel Ctor bta3o ano lma a5l2 mno object ad5l (data, pageIndex, pageSize, count)

              2 - fel Application Layer htl3 3la al Contract folder aly feh al IProudactService
              kan : Task<Result<IReadOnlyList<ProudactDto>>> GetAllProudactsAsync(ProudactQueryParams queryParams, CancellationToken ct = default);
            <IReadOnlyList<ProudactDto>> -> da kan bytg3 al Data aly fel Json 3ady 
            fa h5leh kda ->  Task<Result<PaginatedResult<ProudactDto>>> GetAllProudactsAsync(ProudactQueryParams queryParams, CancellationToken ct = default);
            <PaginatedResult<ProudactDto>>> -> feh al Info aly 3ayz a3rdha + al data of Proudacts

             3 - htl3 3la al ProudactService 34an htdrb error : 
                 service GetAllProudactAsync() kant kda : 
               public async Task<Result<IReadOnlyList<ProudactDto>>> GetAllProudactsAsync(ProudactQueryParams queryParams, CancellationToken ct = default)
            h5leha kda : 
             public async Task<Result<PaginatedResult<ProudactDto>>> GetAllProudactsAsync(ProudactQueryParams queryParams, CancellationToken ct = default)

             w fel return bta3 al Service ana h3dl 4wyt 7gat .. 
             mbd2yn ana hb3t info about data + data .. lakn ana kont bb3t data bs fa h5li al return kda : 
             return Result<PaginatedResult<ProudactDto>>.Ok(mapperProudcats);

            gable error 3nd mapperProudcats , 34an de ana b return data feha al proudacts bs .. 
            fa ana hb3tlo obj mn al class aly 3mlto PaginatedResult feh al data aly fel mapperProudcats + pageSize + PageIndex + Count 
            (al PageSize and PageIndex fel QueryPram )
            fa al return hyb2a kda : 
            return Result<PaginatedResult<ProudactDto>>.Ok(new PaginatedResult<ProudactDto>(mapperProudcats , queryParams.pageSize , queryParams.PageIndex,0));
            w5li al Count b 0 dlw2t 

             4 - hro7 b3d ma 3dlt fel service -> hro7 ll Controller (ProudactController )
              feh GetAllProudacts() 
              m4 hrg3 <IReadOnlyList<ProudactDto>>> 5las bl -> <PaginatedResult<ProudactDto>>>
             */

            #endregion

            #region When Run Probelm  
            /*
              fel PostMan h3ml new Folder -> Session03 
              - add new Request -> Pagination 
              - add paramters : PageIndex = 3 , PageSize = 6 
             gably kda belzbt 
            data [item 13] bs 
            pageSize : 6 
            pageIndex : 3 
            Count : 0 

            kda feh mo4klten .. awl wa7da hya an ana 3ndy 13 item .. fa index 3 * 6 = 18 item 
            farg3le bs a5r item 3ndy 3ady .. bs aly m4 3ady in al Info pageSize: 6 al mfrod pageSize: 1

            tany mo4kla hya al Count w hn7lha fel region al gy 
             */

            #endregion

            #region Solve First Problem 
            /*
              1 - hro7 3nd al service bta3t GetAllProudactAsync()
                return Result<PaginatedResult<ProudactDto>>.Ok(new PaginatedResult<ProudactDto>(mapperProudcats , queryParams.pageSize , queryParams.PageIndex,0));
            al goz2 da queryParams.pageSize 
            h5leh :  return Result<PaginatedResult<ProudactDto>>.Ok(new PaginatedResult<ProudactDto>(mapperProudcats , Proudacts.Count, queryParams.PageIndex,0));
            -> Proudacts.Count

            fa al result lma agy a run tany fel Postman : 
             data [item 13] bs 
            pageSize : 1
            pageIndex : 3 
            Count : 0 
             */
            #endregion

            #endregion

            #region Count Specification

            #region Intro 
            /*
                 f al Count ana kont 3amlo b 0 w hn7l al mo4kla de hna 
                 .. tb ma fel Count anady 3la GetAll() aly mn 8ir specification t gble kol al Proudacts w yb2a kda ana gbt al Count() 
                 h2olk an feh proudact 5asa bel Type aw al Prand .. s3tha m4 hst5dm GetAll() 
                 l2n ana momken akon 3ayz GetAllProudact with brand "Zara" .. w kzalk fel type 

                 fa al s7 any a3ml al Count 7sb al Filte rbta3y .. 7sb al TypeId aw al BrandId .. SearchVlaue name 
                */
            #endregion

            #region Steps 
            /*
              fel Service -> GetAllProudactAsync() 
              lw 2olt anna 3ayzen n3d aly fel Specification (gwa GetAllProudactAsync() -> GetAllWithSpeceficationsAsync() )
              = hyb2a 8lt leh ? 
              l2n aly anta b3to fel Specification GetAllWithSpeceficationsAsync() .. feh Pagination 
              fa al Count bta3 al  Pagination dymn hykon al rkm aly anta mbasih k size .. msln pagesize = 6 .. y3ne anta max 6 
              wnta momken ykon total al Count 3la al 7aga ka proudcats = 12 msln

              fa al 7l any a3ml Specification gdid mn 8ir Pagination 
              w tlama anta hdfk ank t3d bs .. yb2a 4il mn al Specification da al Include() , sort() , pagination
              anta 3ayz bs al Filter 

             1 - hagy fel Application Layer -> Specifications -> add new Class -> ProudactCountSpecifications
                 w hbasi fel Ctor bta3o zy aly fel ProudactSpecifications .. bs 7gat al Filter bs m4 kol al specifications 

             2 - hro7 3nd al GetAllProudactAsync() service : 
                 var CountSpec = new ProudactCountSpecifications(queryParams);

              3 - htl3 3la al Repo azwd funcion bt3ml Count : 
                - in IGenericRepo w a3ml Function for Count Specification 
              Task<int> GetProudactCountWithSpecificationAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default);
             
               4 - hro7 in GenericRepository -> a3mlha Implemntation

               5 - hrg3 ll GetAllProudactAsync() service :
                   hdif al str da :   var TotalCount = await unitOfWork.GetRepository<Proudact, int>().GetProudactCountWithSpecificationAsync(CountSpec, ct);
                   hdfo ano ygblk al Total Count Bona2n 3n al Filter bta3o 

                w fel a5r  return Result<PaginatedResult<ProudactDto>>.Ok(new PaginatedResult<ProudactDto>(mapperProudcats , Proudacts.Count, queryParams.PageIndex, TotalCount));
               ( 7tet TotalCount bdl ma kan 0 )
             */
            #endregion

            #region Depug 
            /*
              fel pagination request 
              - url : {{BaseUrl}}/api/Proudact?PageIndex=3&PageSize=6 
              b2t al result kda : 
              "pageIndex": 1,
              "pageSize": 3,
               "count": 13 (3rf ano al Item al a5ir 3ndy) .. bdl ma kan 0 

            another test : 
            {{BaseUrl}}/api/Proudact?PageIndex=1&PageSize=3&SearchValue=C

             "pageIndex": 1,
             "pageSize": 3,
              "count": 8
               */
            #endregion

            #endregion

            #region Project Busniss Solution 
            /*
              ana a5trt prooudacts mn kol aly 3nde ..(bel Search by name b2a aw pagination ayn kan) 
              fa bbd2 a7ot al Proudctas de 3ndy fel Shopping Cart (Basket)
              b3ml Confirm ll Basket da .. fa al app by check hl ana 3aml login asln wla la 
              w al login ykml m3aya al Process any a3ml Payment ... w lw ana m4 3aml login ... h3ml Reqister al awl 
              w kml process al 4ra .. l7d ma al Basket da yt7wl l Order 5las 
             
              yb2a hwa flow : 
              1 - Proudact Mangment [Search by value - pagination - sorting]  (a7na 5lsna da..)
              2 - create Shopping Cart [add - remove - update] basket 
              3 - User Authentication [check on user authorizetion and Authentications]
              4 - Order Processing -> any a7wl al Basket bta3y into order w ahtm b tfasil al Order zy twsilo w kda 
              5 - Payment Integration 
              6 - Cashing -> any a5zn data b4kl 3am 

            note : w 7sb al Bussins Role : al Basket momken gdn tms7o abl ma yt7wl l order (Confirmed Basket)
            lakn s3b ank tms7 al Order nfso .. momken ykon mortg3 .. y3ne arg3 al Order lakn m3mlo4 delete 

             kman 3la mostwa al Controllers fel Project hyb2a 3nde : 
             1 - Proudacts Controller (3mlnah)
             2 - Basket Controller [craete - update - remove - get]
             3 - Orders Controller [Create - Get User Orders - Get one Order by Id - Get Delivaey Detatails]
             4 - Authentication Controller [User Login - User Regester - Check Email - Get Logged-in user - Get User Adress - Update User Adress]
             5 - Payment Controller [Create - Update] Payments 
             
            note : al Payment Step mben al Basket wel Order 
                   l2n al User b3d ma by2kd al Payment -> al Basket byt7wl l order b2a 
             */
            #endregion

            #region Basket Entites 
            /*
              h3ml BasketItem model -> kol item gwa al Basket 
             note : al item da m4 proudact .. bl hya data wa5da copy mn al Proudat aly 3nde 
             wel hdf any ab2a 3azl al item aly fel basket wel Proudact aly mwgod 
             
             h3ml CustomerBasket Model -> hyb2a gwah Id , ICollection<BasketItems> .. y3ne mgmo3a mn al Basket Items (aly hya info about proudact)

             steps : 
              in Domain Layer -> on Entity Folder ->  Add New Folder ->Baskets
              - make Class inside Baskets -> BasketItem
              - make Class inside Baskets -> CustomerBasket

             */
            #endregion

            #region In-Memory DB (Redis)
            /*
              a7na m3d ma 3mlna al Models bta3t al Basket al Mford nro7 ll Repos b2a w nkml al Cycle .. 
              lakn m4 hn3ml kda w hna5od Busniss way mo5tlf leh ? 
              = Basket Module gwa system al E-Commarece hwa (Temp Data) .. l2no b3d kda hyt7wl wyb2a Order 
              
              leh al Basket da temp data .. L2n al Client lma msln y add 7aga gwa al Basket .. hwa m4 akid hy4triha 
              fa feh a7tmal ano mygbha4 fa bysbha gwa al Basket wy2fl al Browser 7ta m4 byms7 al Item aly gwa al Basket da 
              w da kda hyzwdly Record zyada fel Database bel Item da 3la al Fady .. w da lw user wa7d mbalk 1000 user !! 
              
             fa al Basket lma yt3aml m3 al Memoery fa hwa byt3aml m3 temp Memory 
             y3ne al Basket bykon gwah Temp data 3n al Proudact (item) .. wel item da byt5zn brdo 3la Temp Memory 

             fa fel 3ady al Sql Server Database m4 htnf3 m3aya fel Basket da .. l2n al Sql Server by5zn al Database records 
             on Hard Disk 3ltol .. fa ana 3ayz database tkon 
             1 - a22dr a access 3leha bsor3a 
             2 - tkon temp mn 7is al T5zin .. y3ne b3d 4wya tms7 al Records aly mt5zna feha de 

              5las ana momken a5zn al Data de 3la al Ram bta3 al Server 3ndy 
              b7is lma t refresh al browser al Data de ttms7 .. aw lma y3de 3leha 3dd ayam mo3yna brdo ttms7 lw7dha 

             fa aly by7sl 3ndy an al User (Browser) => send Request -> ana aly bwgho yro7 (ll HardDisk aly feh data mt5zna || wla yro7 ll Ram .. temp data 3ndy)
             note : al Ram ka Performance agmd bktir fel Read/Write operations l2nha asr3 ll CLR ..W5sosn lw al Data de m4 Complex wfeha Realtions 

             fa ana anhrda hst5dm In-Memory Database  34an t7l al mo4kla de bdl al Sql Server DB w ast5dmtha 34an : 
             1 - bt5line a store al Data 3la al Ram bdl al HardDisk
             2 - al donya fel Read/Write asr3 ka performance 
             3 - adeha timing mo3yn 34an al data de tro7 
             4 - hya bt5zn al Data m4 k table bl ka (Json) object 
             5 - hya mfha4 realtions Zy SqlServer .. hya (No Sql)
             6 - bt722 mwdo3 al Temp Data wel Ram .. anha b3d ftra aw action mo3yn btfdy al Data 34an tb2a tst2bl Data Gdida

             fa ana bdwr 3la tool t5line ast5dm al In-Memory DB .. al tool de hya (Redis tool)
             */

            #region Redis Tool
            /*
              hya tool btsa3dne any a3ml In-Memory Db.. a5zn data 3ndy 3la al Ram 

              why Redis : 
              - l2nha bt5zn gwaha object mn no3 key Value Pair (Json object)  
              - 48ala In-memory Database (FastAcess)
              - Time To Live Object 3la mostwa al Sloution bta3e 
             */
            #endregion

            #region Install Redis 
            /*
              al Redis k server mlha4 Virsion for Windows bl Linux .. 
              fa a7na lma kona bn4t8l Sql .. awl 7aga kont bnzlha hya Server al Sql .. tany 7aga bnzlha hya al Tool aly t5line at3aml m3 al server (ssms (Sql Sever Mangment Studio) )
               
              fa hna nfs al fkra .. ana m7tag Redis Server .. w hwa byst5dm linux 
              fa 7lha any anzl SubSystem 3nde fel Windows .. 34an y2wm al Redis Server da 3la Windows 
              fa hnzlo asmo (WSL2)
              - fel Bower Shell 34an a3mlo install aktb: wsl --install
              - fa hynzlk 7aga asmha (Ubuntu) da Linux SubSystem .. aly h48l 3leh al Redis Server 3ndy fel Windows 
              - fa anzl al Redis Server aly hy4t8l 3la al Ubuntu  

               b3d ma tnzl al Aubentu w tft7o .. 
               - aktb : redis-cli
               - hygblk Port -> 127.0.0.1:6379>  da kda al Server 
               - aktblo Ping hyrod p pong .. kda m3nah al Server 48al 

               kda na2s al Tool aly tt3aml m3 al Server da 
               - hnzl tool asmha Redis Insight tt3aml m3 al Server 

               yb2a ana fel awl b3ml eh : 
               - bft7 Aubetu : 
               - aktb : redis-cli
               - hygblk Port -> 127.0.0.1:6379>  da kda al Server 
               - aktblo Ping hyrod p pong
              
               b3d kda aft7 Redis hla2i nfs al port hnak 127.0.0.1:6379>
               y3ne kda ana 48al 3la al Database server tmam .. tb aflt al Aubentu (al server) .. al tool m4 ht4t8l 

               w yb2a al 5olsa : 
               - bnzl al Aubntu k subSystem 
               - b3den anzl 3leh al Redis Server 3la al Aubentu l2no m4 bynzl Direct on windows 
               - al Redis Inshght w da al GUI tool aly bst5dmha 34an at3aml m3 al Data base bta3t al Redis Server 
               
             */
            #endregion

            #endregion

            #region Basket Repository 

            #region Intro 
            /*
                 b3d ma ghzt al Models bta3t al Basket = aghz al Repo bta3ha 
                 wel Repo da yklm al Database aly mn no3 Redis .. l2n al Generic Repo aly ana 3mlo byklm al Context aly byklm 
                 al Sql Server Database .. fa 
                 ana h3ml Repo m5sos ll Basket b7is al Repo da yklm al Reids database not Sql Server zy al Generic Repo 
                */
            #endregion

            #region Steps 
            /*
              1 - in domain Layer -> Contract -> make new Interface -> IBasketRepository and make 3 function to basket
              2 - in infrastructure Layer -> Repositories -> make new Class BasketRepository :IBasketRepository
              3 - in ctor BasketRepository bel DI m4 hnady 3la al Context (sql database) l2 ana 3ayz object yklm al Redis 
              4 - fel Dependancy of InfraStructure Layer hnzl Package to Redis 
              5 - hrg3 ll Ctor a implemnt feh interface gwa al Package asmo IConnectMultiblexr .. w aghz mno object 
                  34n yklm al Redis database aly ana 3mltha 
               
               kda hwa y2dr ytwasl m3 al Redis Database aly hya f app al Redis Insight al port bta3ha 127.0.0.1:6379
               y3ne ana connected bel Redis Database de .. 
               
               mtnsa4 ana 3mltha azay > ft7t al Aubentu > ktbt al Command l7d ping pong > ft7t al Redis Insight l2tha .. lw 2flt al Aubentu al server yo23 

              6 - b3d ma ghzt obj mn al Di ll Redis databse .. implemnt al 3functions b2a 
              7 - 5lst al Implemtation .. bs m3rft4 public BasketRepository(IConnectionMultiplexer connection) aly fel Ctor 
                  fa htl3 3la al Services Di Container al 5as bel InfraStructure Layer -> InfrastrucrureServicesRegistrations
                  w a3rf al object da fel DI Container Service 

             note : Database of RRedis ht4t8l Singlton m4 scopped zy al Context (sqlServer) .. leh
             l2n al basket hyfdl 3ay4 m3ak tol al life time application .. l7d ma al time bta3o y5ls 
            =>   services.AddSingleton<IConnectionMultiplexer>(opt => 
            {
                return ConnectionMultiplexer.Connect(); 
            });
           
             8 - ha2s a7ot al Connection String of Redis Database gwa al Connect() 
                 fa hro7 a3mlo fel AppSetting File 
                 => "RedisConnection": "localhost"  .. hwa by defult asmo localhost 3ayz a8ir asm al Redis Database hyb2a b command

             9 -   services.AddSingleton<IConnectionMultiplexer>(opt => 
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            });
             
             10 - bel mra mnsa4 a3ml object fel services bta3t al IBsketRepo fel DI container 
              =>  services.AddScoped<IBasketRepository, BasketRepository>();
             
             */
            #endregion

            #endregion

            #region Basket Service 
            /*
              abl ma n3ml al Service of Baskets .. hn3ml al Dto and Mapping 
              1 - in application layer -> Dto's -> Make new Folder -> Baskets 
              2 - make Class inside it -> BasketItemDto 
              3 - make Class inside it -> BasketDto 
              4 - a3ml Mapping : 
                  fel Application Layer 3nd al Profile -> make new Class -> BasketProfile w3ml al mapping bta3e 
              5 - ApplicationServicesRegister class -> h7dr al Object bta3 al Mapping da fel DI Container .. l2 
                  l2n ana 2ylo al Profile al Standerd bta3o hwa ProudactsProfile()
                   wlakn lw 3ndk 7gat t4t8l beha tany bel AddAutoMapper ht2raha mn al Current Assymble (typeof(ApplicationServicesRegister).Assembly)
              6 - n3ml al Service nfsha : 
                - in Application Layer -> Contract -> make new Interface -> IBasketServices
              
              7 - htl3 in application layer -> Services folder -> Create new Class -> BasketServices : IBasketServices
              8 - inject fel ctor of BasketServices al Repositoery (IBasketRepository) bel DI 34an at3aml beh m3 al Services 
                   w kman al IMapper bel mra 
              9 - at2kd ank 3mlt obj mn al BasketService fel DI Cintainer aly fel ApplicationServicesRegister
                   => services.AddScoped<IBasketServices, BasketServices>();
             */
            #endregion

            #region Basket Controller & EndPoints 

            #region Steps 
            /*
                  1 - Add Api Controller (BasketController)
                  2 - fel Ctor of BasketController bel DI klm al BasketService 34an tnday 3la al service inside controller 
                  3 - implemnt 3 Endpoints 
                 */
            #endregion

            #region Debug
            /*
               - 5li balk al BasketController m4 hy4t8l m3ak lw aflt al Aubentu l2no Singlton 
                 fa lazem akon fat7o 
               - f bostman hn3ml New Request no3o Post asmo -> Create Basket 
               - hktb fel URL : {{BaseUrl}}/api/Basket
                 W hb3tlo fel Body : hb3t Row aly hwa Json 
                 {
                  "id":"1",
                  "items":[
                    { 
                       "id":10,
                       "Name":"Proudact01",
                       "PictureUrl":"Test",
                       "Price":150,
                       "Quantity":5
                    }
                  ]  
            da aly byb3to hwa al Front-End fel request .. 34an ana a5do w a5zno 3ndy fel Redis Database 

             tb eh aly hy7sl : 
             - al Request hyro7 ll EndPoint aly hya CreateOrUpedateBasket hyb3tlha basket (aly b3to al Front-end fel request)
             - al EndPoint htklm al Service aly gwaha 
             - fa al service ht7wl al Json da l CustomerBasket bel Mapper 
             - b3d kda a5o4 gwa al Repo .. fa al Repo y3ml Serialize into Json 
             - 34an ytb3t ll Reids .. w lw git tbos 3la al Redis : 
               mn al Redis Insight w a3ml refresh -> htla2i item mtsgl hnak fel Database bta3to .. al Live Time bta3 al item da = 23 sa3a l2ne ana aly 3amlo kda
             - b3d ma t create al Basket htrg3 tany ll Endpoint 3nd : 
               return ToActionResult(basket); -> Trg3 al Basket tany fel response b3d ma 3mltlo save fel Redis Database 
            ================================================================================================================
            - fel PostMan h3ml New Request -> Get Basket
              fel url : {{BaseUrl}}/api/Basket/1 
            - hyto7 ynfz al Enpoint de : [HttpGet("{id}")] .. w htrg3lk brdo al Basket aly at3mlo create fel Redis Database 
             l2n f a5r al Endpoint  -> return ToActionResult(basket);
            ================================================================================================================
             - fel PostMan h3ml New Request -> Delete Basket
              fel url : {{BaseUrl}}/api/Basket/1 -> hyms7 al Basket aly mt5zn fel redis database 
              fa hro7 3la Database al redis (Redis Insight) w a3ml refresh = atms7 
             */
            #endregion

            #endregion

            #endregion

            #region Session05

            #region Caching 

            #region Concept
            /*
                 caching => store data on Temp Memory 
                 tb ana bst5dm fkrt al Caching f eh ? 
                 - any msln fel request tlbt Get All Proudacts (13 items) 
                 fa ana wna b5dhom .. b3ml mnhom copy w b5znhom 3la al Temp Ram 3ndy 
                 w da 34an m4 kol request .. aro7 ll database , ana kda b3ml load 3leha .. bl an al request 
                 yro7 y4of fel ram hl al data aly hwa 3ayzha mwgoda bel f3l abl ma yro7 ll Database ygbha ? 

                 y3ne al request awl mra -> byro7 ygib al data mn al DB -> w yro7 ya5od mnha Copy w y7otha 3la al RAM 
                 34an fel Next Request lw 3ayz nfs al Data .. myro74 ll Database b2a w y load 3leha al Requests
                 bl yro7 ygeb al Data mn al RAM 3ltol 

                 note :fkrt al Caching hnst5dmha fel rides brdo .. b anna n5zn Json 3la al Ram
                 note : al Data aly m3molha Copy w m7tota 3la al RAM (Cached) .. leha TimeToLive w tro7 mn 3la al RAM 

                 Al Cach da by7sl 3la al Clinet (Browser) wla al Server = Server (an al server yt3aml m3 al Ram fel request bdl al Database)
                 w momken al Front-end prdo y3ml Caching fel Browser 
                */
            #endregion

            #region With Caching / Without Caching 
            /*
              Without Caching => Request -> Database -> Response (All time) -> Load on Database 
              With Caching => First Request -> Database -> Resopnse 
                              Another Request -> RAM if data exist (m4 htro7 ll Database) 
                              tb lw al Data not exist .. s3tha al request y ro7 ll database 
                              bs ya5od copy mn al Data w hwa rag3 y7otha fel Ram (Caching)
            w note : al Data bt5tfe mn 3la al RAM 7sb al w2t aly anta bt77do leha (TimeToLive)

              benifits -> Reduse Server and Database Load
                       -> much Faster in Request ana response (sri3 gdn al Response ll End-User)

            note : fkrt al Caching mo5tlfa 3n al Basket .. 
            al Caching => ana ba5od Copy mn al data w a5znha fel Ram bhdf a22ll Requests 3la al Database 
            al Basket => al request nfso byklm al Endpoint -> service -> repository wcycle aly 3arfnha 
             
            w concept al Caching ynf3 y7sl m3 al Database (SQL Server) .. bt5zen objects on RAM 
            w kman ll DB Redis (In-memoery Database) any a5zn Json 3la RAM brdo 
             */
            #endregion

            #endregion

            #region Action Filter Attribute 

            #region Intro 
            /*
                  Action Filter hya tri2a al sequance bta3ha an : 
                  ana gyle Request mn al Browser (Front-end) -> Get/Proudacts .. to get all proudacts 
                  fa al Request da hy5o4 gwa al ProudactController -> GetAllProudact Endpoint yro7 3ndha 

                  l7d hna b2a .. ana b3ml Action Filter 3la al Endpoint de GetAllProudact() 
                  b7is an al Action Filter da gwah Function asmha OnActionExcuting() .. bt2ol ll request ano yro7 ll RAM 
                  ygeb al Data mnha lw mwgoda .. 
                  lw al Data m4 mwgoda fa bysm7 ll Request ano y5o4 gwa al Endpoint .. fa yklm al service wel service tklm 
                  al repo .. wel repo yb3t request ll database 34an ygib al Data .. w b3den yrg3 b2a bel response bel ALL Proudacts
                  da f 7alt an al Data m4 Cached on Memoery 

                  fa al Action filter : 
                 - Runs on Controller Actions 
                 - Excute Before and After an Action Method 
                 - Can Access Action Paramters and the Action Result 
                 */
            #endregion

            #region Steps 
            /*
               1 - in Api Layer -> New Folder -> Attributes 
               2 - make Class -> RedisCachAttribute  (mnsa4 a7ot klmt attribute fel a5r l2n al CLR by3rf an da Filter by defult)
               3 - hn5li al Class da ywrs mn ActionFilterAttribute 
               4 - a3ml override ll Function aly gwah -> OnActionExecutionAsync() 
                - l7d hna ana 3mlt Filter Class .. mn 8ir lsa m Implemnt 7aga feh .. 
               5 - hto7 b2a ll ProudactController -> 3nd al Action aly 3ayz ast5dm 3leha al Filter Class da (filter for Cach)
               kda : 
               [RedisCach] // al Class aly ana 3mlto RedisCachAttribute (hwa m4 byktb Attribute .. de bs 3lama ano Filer ll CLR)
               public async Task<ActionResult<PaginatedResult<ProudactDto>>> GetAllProudacts([FromQuery]ProudactQueryParams queryParams,CancellationToken ct)

               fa al sora ma4ya azay ? 
               al Request ygy ll Endpoint bs abl ma y5o4 3leha yo4 3la al RedisCachAttribute Class .. bsbb [RedisCach]
               wynfz al Logic aly gwaha .. lw al Data mwgoda fel Cach rg3hali .. w al request m4 hy5o4 al Endpoint 
               lw al Data m4 mwgoda hy5o4 gwa al Endpoint ygeb al Data mn al Database w b3d kda y3mlha store fel Cach   
             */
            #endregion

            #endregion

            #region Cach Service 

            #region Intro 
            /*
                  hnrg3 l Class Al Action Filter for Caching () bs m4 dlw2t 
                  dlw2t ana 3ayz a3ml steps any 22ra mn al Redis bta3y k cash w kman a set ...
                 */
            #endregion

            #region Steps 
            /*
              1 - h3ml repo hdfo y3ml Cach 
                 in domain Layer -> Contract -> new Interface -> ICachRepository
                 h3ml gwah two Function .. wa7da for Get Json .. wel tanya set .. b any a5zn al Object bta3y 3la hy2t Json

              2 - n3ml Class Implemnt al Interface 
                  in InfraStructure Layer -> Repositories -> new Class -> CachRepository 

              3 - fel Ctor of CachRepository bel DI nady 3la External Object y access 3la al Redis Database 
                  w 3mlt implemnt .. an function bt get al json mn al Redis database .. wwa7da bt3ml set l json fel redis database

              4 - ro7 ll InfrastrucrureServicesRegistrations w 7ot al Service bta3t al Repo de fel DI Container 
                  y3ne hn48l Object al Cach .. w note : object al Cach byfdl 3ay4 m3aya tol ma al Application is Running 
                  y3ne h3ml al Cach Object da Singlton => services.AddSingleton<ICachRepository, CachRepository>();

               5 - h3ml b2a al Services : 
                   Application Layer -> Contracts -> new Interface -> ICachService 
                   doro any a3ml brdo two functions for Get and Set 

               6 - a3ml Class for service -> CachServices in Services folder inside Application Layer 
               7 - fel ctor bta3 CachServices -> DI hat objcet mn al Repo 34an a3rf at3aml m3 al Redis Datbase in Service 
                   w3ml Implemnt 
               8 - in ApplicationServicesRegister 7ot object al Services fel service of DI Container armeh hnak 
                 =>  services.AddSingleton<ICachServices, CachServices>();

               yb2a ana kda 3mlt Repo .. 34an a get and set in Redis Database
               w 3mlt services 34an lw htb3t json mn al Database .. aw htst2bl object t7wlo l json w b3den t5zno fel database 
              
               al next step b2a nrg3 ll Action filter Class n3mlo Implemntation .. 
             */
            #endregion

            #endregion

            #region Cach Attribute 
            // all things will Explained in RedisCashAttribute Class 

            #region Debug 
            /*
              1 - run Upentu 
              2 - open Postman 
              3 - open redis
              4 - a3ml run ll app hna 

            f Postman Session 2 folder -> Get All Proudcts
            url : {{BaseUrl}}/api/Proudact

            - al request ra7 awl 7aga 3la OnActionExecutionAsync() aly gwa RedisCachAttribute class md5l4 3la al Endpoint
            - hybne al CachKey -> api/proudact? .. hyb2a 4klo kda (zy al request belzbt)
            - hy4of al Cach feha 7aga .. l2 l2n da awl request fa kda kda hyro7 fel a5r ll Endpoint ygib data mn al datbase b3den y5znha fel Cach 
            - f3ln ml2a4 item fel cach fa 3ml invoke() ll Endpoint bel Next .. 
            - al Endpoint trg3lk al Data bta3t al Item .. lw al data ok 
            - hy5o4 tany ll OnActionExecutionAsync() .. w y5zn al Data de fel Cach RAM .. b3den y3ml return 
            - lw d5lt 3la al Redis Database (Redis Insight) -> hla2i al items created fel datbase w leha TimeToLive ana m7ddo (1000 Sec)

            note: kda lw new reqeust gh wkan talb nfs al request -> {{BaseUrl}}/api/Proudact
              m4 hynfz al Endpoint l2no hyto7 3la al Cach aly fel Ram ygib al item mn hnak 

            fa ana kda 2llt Requests 3la al database .. w b2a goz2 byklmha bdl ma kolo kan byb3tlha Request 
             */
            #endregion

            #endregion

            #region Identity Module Overview 

            #region Intro 
            /*
                 al Idntity Package 3mtn kant b5line a3ml Authenticate w Outhorized ll app bta3e 
                 3n tri2 anha gwaha tables wel tables deh hya aly bt2dmle al Services (functions) de
                 zy table al UserManger .. aly mn 5lalo b2dr a addRoleToUser() , AddUser() .. GetRoles().. changePassword()
                 w feh class kan mot7km f 3mlyt al Authentication asmo SignInManger Class .. kol al Functions aly leha 3laka bel Sign in 
                 w feh Class mot7m fel Role of user .. asmo RoleMangerClass 
                 fa ast5dmatna mainly m4 bttl3 bra al 3 classes dol .. 
                */
            #endregion

            #region Identity Uses in our Project 
            /*
              al mfrod anna nh3ml 6 EndPoints ...
              1 - User Login 
              2 - User Regestiration 
              3 - Check Email 
              4 - Current user (Get Logged-in user)
              5 - Get User Adress 
              6 - Update User Adress 
             */
            #endregion

            #region Authentication Flow 
            /*
              lma t3ml Register or Login 
              ana bbd2 a validate al User and Password 
              a create Token .. jwt Token lw al donya 3ndk Valid 
              ab3t al token bta3 al User da ll Front end ka Browser .. y3mlo store 
              34an fel Next request lya a4of hwa mota7 3ndy wla l2 fel Endpoint 
              w fel a5r ab3tlk result 
            nfs flow al MVC bs hn8ir 4kl al Token mn Cookie l MVC 
             */
            #endregion

            #region Endpoints 
            /*
              Login endpoint
              fel request hygilha .. email & password 
              trg3le fel response JWT token + email + display name 

              Regester Endpoint 
              fel request (Email , password ,phone , displayname)
              fel response JWT token + email + display name 

              Check Email Endpoint
              Request = email 
              response = bool true or false 
              
              check on Current user endpoint 
              Request = bb3t al Token JWT 
              Response = Email , Display name , Jwt Token (if include in your DTO)

              Get Adress Endpoint 
              Request = bb3t al Token JWT 
              Response = FirstName , LastName , Street , City 

               Update Adress Endpoint 
              Request = bb3t al Token JWT + adress object
              Response = Updated Adress 
             */
            #endregion

            #endregion

            #region Entites & DbContext 
            /*
              dlw2t ana 3ayz ast5dm al 7 Tables aly gwa al Identity Database 
              kman ana 3ayz a3ml Modfication 3la al Table de .. zy msln 
              ApplicationUser Table -> 3ayz azwdlo Col DisplayName 
             
              kman nhzw table yb2o 8 table .. asmo Address .. w hrbto bel ApplicationUser .. yb2a 3obara 3n 3nwan al user nfso  
             */

            #region Steps 
            /*
              1- in domain layer -> Install Identity Package 
              2 - in domain Layer -> Entites folder -> add new folder = Identity 
              3 - create inside it new Class = ApplicationUser : IdentityUser (feh 7agt al user aly warsha mn al identity table + col al Displayname)
                  hrbot al table da (ApplicationUser) b table tany asmo address .. rbto b navigational property + fk 
              4 - create new Class asmo Address 

                  5lst al Entites aly m7tgha mn al Identity Database .. aro7 b2a adifhom fel Context bta3e 
                  l7za b2a .. ana 3ayz a3ml Database monfsla ll Identity Database 
                  w da lhdf twzi3 7ml al Requests mn 3la al Databases .. y3ne ana 3ndy Bussnis database for bussnis 
                  w database for Caching (Redis) .. w dlw2t database for Security (Identity Datbase)

              5 - infrastructure layer -> Data Folder -> make new Class -> StoreIdentityDbContext : IdentityDbContext 
              6 - ntl3 3la InfrastrucrureServicesRegistrations .. 22lol lma atklm m3 al Dbcontext bta3k a8ir al Connection string 
                  fa b3dl fe options bta3t al Dbcontext 
              7- aro7 a create al Connection string bta3 al IdentityDatabase -> AppSetting File 
              => "IdentityConnection": "server=.;Database=StoreIdentityDb;Trusted_Connection=true;TrustServerCertificate=true"

              8 - a3ml Migration in Console Backage Manger -> not make Update Database .. l2ni h3mlha bel DataSeeding 
              note : 
              wna b3ml mogration lazem a7dd al Context brdo l2n ana b2a 3ndy two context .. 
              PM> Add-Migration "IdentityModule" -Context "StoreIdentityDbContext"
              bganb brdo any lazem fo2 a7dd al Layer aly feha al Context bta3e 
             */
            #endregion

            #endregion

            #region Identity Seeder 

            #region Recap 
            /*
                  ana 3mlt al Entites ... 3mltlha Configurations w dbset .. b3den gwa InfrastrucrureServicesRegistrations 
                  2olnalo 5li 3ndk al Connection String bta3 al IdentotyDatabase .. w ro7na rmena al ConnectionString fa fel AppJson file 
                  w b3den 3mlna migrate .. w w2fna 3nd al Update-databse 
                  l2n ana 3ayz a3ml Class y48l al Pending Migration + y3ml Seed ll database 
                 */
            #endregion

            #region Steps 
            /*
               - in Domain Layer -> Contract -> kont 3al Interface (IDataSeeder)
                  gwaha Function SeedAsync() 34an Multible Classes y3dro yst3mloha 
               
               - in InfraStrcture Layer -> Seeding Folder -> kont 3aml Class asmo CatalogeDataSeeder y seed al Data bta3te
                 w kan gwah al Pending Migrations .. fa ana h3ml nfs al Klam bs ll Identity Databse 

              1 - in InfraStrcture Layer -> Seeding Folder -> create new Class = IdentityDataSeeder : IDataSeeder
              2 - ctor of IdentityDataSeeder external object by DI for Context 
                  hykon 3ndy object mn tables fel Identity database gahzen zy : 
                  UserManger , IdentityRole 
                  w object mn ILogger for errors 
              3 - a3ml Implemntation ll Class 
              4 - InfrastrucrureServicesRegistrations -> services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");
                  34an fel Extention bra fel APi Layer : 
              5 - 
               var Identityseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
               await  Identityseeder.SeedAsync();
             
              6 - kda na2s any a7dr al Objects al ast5dmtha as external objects bel DI f ctor IdentityDataSeeder class
                   fa hghzhom gwa InfrastrucrureServicesRegistrations
            =>          services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();
             */
            #endregion

            #endregion

            #endregion

            #region Session06

            #region Authentication Services 

            #region Intro 
            /*
                3zyzen nghz service dorha anha tt3aml m3 al Classes aly btt3aml m3 al Identity Databse 
                w homa mainly kano 3 Classes (RoleManger - SignInManger - UserManger) 
                fa ana 3ayz a3ml Service yb2a dorha al asasy anha tb2a al source al w7id aly yt3aml m3 al Idintity 
              
                fa ana h3ml Layer zyada tt3aml m3 al Tables bta3t al Identity .. zy any mn 5lal al Tables de a2dr : 
                FindByEmail - CheckPassword - CreateUser .. 3n tri2 any a3ml Interface + class byt3aml m3 al 3 tables dol 
                fa yb2o homa al w7eden aly a2dr aklm mn 3ndhom al Identity 
               */
            #endregion

            #region Steps for This Service 
            /*
              1 - in Application Layer -> Contract -> IIdentityServices .. a3rf 3 functions signature 
              note : ana 3aml al 3 functions dol 34an a3ml two steps (Login - Regestiration)
              
              2- in Application Layer -> Common -> new Class -> IdentityUserResult 
                 h7ot feh al kym aly 3ayzha trg3le mn al 3 functions aly 3mltha (Return of Functions).. FindByEmail - CheckPassword - CreateUser

              3 - f function al CreatUser ana 3ayzha ta5od mne (RegisterDto)
                  fa hro7 gwa al Dto's -> New Folder -> Identity -> RegisterDto 34an t5do function CreatUser as Paramter mn al Front-end 

              4 - htl3 feh Services -> Create Class -> IdentityServices : IIdentityServices

              5 - gwa Ctor IdentityServices bel DI hgib External Object mn UserManger .. 34an da Identity table fel Database 
                  feh kol user info 
              6 - b3d m3ml Implemntation ll IdentityServices aftkr anha dorha al Asasy .. anha tt3aml m3 al Classes al 5asa bel Identity 
                  w 34an tsa3dny a3ml Functions al Login and Register 

              7 - inject al klam da fel ApplicationServicesRegister
              => services.AddScoped<IIdentityServices, IdentityServices>(); 
              w m7tag a3ml DI ll UserManger wel RoleManger .. aly fel Identity 
              => services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>(); // hydene error .. l2n ana 7attha fel Service DI Container bta3 Layer al Application w al StoreIdentityDbContext fel Infrastructure layer fa lazem a 7ot al line da fel DI Container bta3o aly hwa InfrastrucrureServicesRegistrations 
                  bs al line da kan mwgod abl kda fa mktbto4 tany 

              8 - in Application Layer -> Contracts -> New Interface -> IAuthenticationServices 
                  w da hyb2a feh al Services aly btt3aml m3 al Endpoints .. w hyb2a gwaha al Two Methods (Login and Register)
                  l2n yb2a 3ndy Function bt3ml Login w function bt3ml Register .. wel two Function fel a5 r by returno (User) 
                  al user aly rag3 da hykon UserDto (DisplayName - Emial - Token)

              9 - in Dto's -> Identity -> UserDto 
              10 - in Dto's -> Identity -> LoginDto 
              
               gwa al IAuthenticationServices h3ml al Login and Register Functions 
               Login -> ht return UserDto (DisplayName , Email , Token) .. w ta5od fel Param bta3ha LoginDto(Email - Password)
               Register -> ht return UserDto (DisplayName , Email , Token) .. w ta5od fel Param bta3ha RegisterDto(Email - Password - UserName - DisplayName - PhoneNumber)

              11 - make Class Implemnt al Interface -> Services -> AuthenticationServices

              12 - h inject fel Ctor bta3o bel DI external Object .. bs m4 UserManger wla RoleManger .. 
                   h inject IIdentityServices() -> l2no al layer aly btt3aml m3 al  UserManger , RoleManger , ... 

               w da aly gwah al 3 function CreatUser() FindByEmailAsync() CheckPasswordAsync() 
               aly hst5dmhom fel Login and Regester Functions in AuthenticationService 

              13 - ApplicationServicesRegister zwd al IAuthenticationService in DI Container al 5as bel Application Layer

              note : al Authntication service (Login and Register Functions) da b2a hwa aly hy implemnto al Controllers and Endpoints 
              wel Authntication service klm gwah al IdentityService w hya aly btklm al Tables bta3te aly gwa al Identity Database 
              fa bdl ma aklmhom wa7d wa7d ana gm3thom f mkan wa7d w nadet 3leh 
             */
            #endregion

            #endregion

            #region Authentication Endpoints [Login - Register]

            #region Steps 
            /*
              1 - add Controller (api) -> Authentication 
              2 - h7dr External Object in This Ctor mn IAuthenticationServices 
              3 - h3ml 2 Endpoints no3hom post .. y3ne hs2bl 7aga mn al FrontEnd 
             */
            #endregion

            #region Test on PostMan 
            /*
              1 - run app and open postman 
              2 - h3ml new Request for Register -> Register no3o Post 
              3 - URL : {{BaseUrl}}/api/Authentication/Register
              4 - lw 3mlt run m4 hynf3 .. lazem fel body ab3t al Data tro7 hna fel server Reqister tt ctreate b3d kda ttb3t tanu fel Response 
              5 - body -> Raw : 
                 {
    "Email":"youssab@gmail.com",
    "DisplayName":"youssab hany",
    "PhoneNumber":"01222928179",
    "UserName":"youssab22",
    "Password":"P@ssw0rd"
}
            6 - hyrg3ly fel response : 
            {
    "displayName": "youssab hany",
    "email": "youssab@gmail.com",
    "token": "Token"
}
            kda m3nah ano 3ml Register w lma abos 3la al database (Users) hla2i al Raw atsgl 
            ==================================================================================================
            7 - h3ml new Request -> Login no3o Post 
            8 - url : {{BaseUrl}}/api/Authentication/Login 
            9 - hb3t fel body row  bel email and password aly 3mlt behom Register fel endpoint aly fatt:
             "email": "youssab@gmail.com",
                "Password":"P@ssw0rd"
}
            10 - hyb3tle fel resopne 
             {
    "displayName": "youssab hany",
    "email": "youssab@gmail.com",
    "token": "Token"
}
             */
            #endregion

            #endregion

            #region JWT In my Project 

            #region Jwt Implemntation 

            #region Intro 
            /*
                 fel EndPoints swa2 Login or Register kan byb2a emil - username -  token 
                 al token : "Token" .. l2 ana 3ayz a hndlo w yb2a token bgd 
                 w faydet al token da ano al Card aly by5lih y access 3la Endpoints 3ndy fel server 7sb hwa Authorized wla la 
                 y3ne al token bta3 al user bygiby mn al Request from Front-end Brwoser

                 fa ana b create al Token da ll user 34an yb2a uniqe b 7agto hwa bs w y2dr y access on eh w eh l2 

                 fel MVC kona bn3ml "Cookies" anha kant al Token bta3t al User .. kont t3ml login fa al Cookie by defult 
                 tt5zn fel Browser ll user da .. w lma y3ml request l 7aga kan al cookie da ygily fel request fa a3rf mn 5lalo 
                 hl al user da Authorized wla la 

                bs da kan server -> Browser l2n al MVC btt3aml m3 Front-end .. lakn fel api 
                al token m4 byt5zn fel Brwoser by defult .. da ana bt3aml m3 api y3ne momken at3aml m3 mobile app -desktop ..model
                fa al token m4 byt5zn hna f Brwoser .. bl ana b3ml token bta3e mn 5lal Jsons tb azay .. 
                b tool asmha JWT (Json Web Tokens) .. de mktba bt3mlk Tokens Secure ll user 
                kman al mktba de bta5od mnk al Data aly anta 3ayz tkwn beha l token da w tdrbhom f 5lat m3 tzwid 4wyt 7gat tanya w fel a5r y3mlk al token  
                */
            #endregion

            #region Introduction of JWT (Json Web Token)
            /*
             what is Structure of JWT 
             1 - Header 
             2 - PayLoad 
             3 - Signature 

            note : al structure da recommended .. y3ne anta al 7gat de 3obara 3n Key:Value Pair .. fa lw ktbt 
            "Hamada":Hambozo -> hy3mlk al Token bta3k Encrepted 3ady .. bs hwa yofdl al Structuer da 34an y3mlk fel a5r  
             secure Token 

            PayLoad -> y3ne al Data aw al Claims of user w de aly btfr2 al User 3n al Tany 
            w feh no3en mn al Claims : 
            - Registerd Claims -> de Clames btkon 48ala l Kol al Solutions 
            - Private Claims -> de 5asa bs b Claims al user nfso bs 

            al Regesterd Claims -> ank tt3aml m3 data of Issuer and Audance 
            issuer y3ne (molke) .. y3ne data of IP server k publish .. y3ne 7ot al LocalHost//2093.. bt3 al server as ano data 
            wel Audance -> (aly byt3amlo m3 al issure) .. y3ne t7ot al Data bta3t al 7gat aly byy3aml m3 al Sever zy servers tany zy local host of angular project 
            
            private Claims -> Custom Claims ana lay 3amlha bnfsy msln 
            {
              "name" : "youssab"
               "id" : "101383"
            }
            wfel a5r al json daytdrb fel 5lat w ytl3le token encrypted 

            3 - Signature -> any b3ml "SecretKey" .. y3ne momken a3ml l kol Project SecreteKey as ano Sgnature leh 
                fa hya klma beny w ben al Front-end 
                w 34an t make al SecretKey da b4kl Dynamic -> b5o4 3la site JWT Secret w hwa by3mlhole 

            34n fel a5r al Header + payload + Signature ytdrbo fel 5lat -> JWt token 

            fa ana al mfrod abne al Token bta3e fel Solution da 3la tri2t al JWT .. w ny fel Endpoints (Login - Register) 
            bdl ma bb3t Token = "Token" .. ana h3ml Token bgd w ab3to as string 

            fa aly hy7sl any h3ml Service Dorha bs anha t create lya token 
             */
            #endregion

            #region Steps 
            /*
              1 - in Application Layer -> Contracts -> make new Interface -> ITokenServices 
                  hyb2a Gwah Function to Create Token .. bta5od mne al Private Claims of User 
                  w hgib ba2i al Data al Tanya mn 7gat tanya 

              2 - make Class y implemnt al Interface -> services -> TokenServices 
              3 - in application Layer -> install package JWT from Debendancies -> Authentication.JWTBeerer 
              4 - implemnt Function CreateToken() .. 4t7t al class 
              5 - b3d ma ghzt al token .. na2s a48l al service aly bt crately al token  CreateToken() fel two endpoints bto3e (Register - Login)
                - hro7 3la ala AuthenticationServices w a inject external object bel DI -> ITkoenService() gwa al class da 
              6 - hro7 3la ApplicationServicesRegister -> a create gwah al Services bta3t al ITkoenService fel DI Container al 5as bel ApplicationLayer 
              7 - aro7 ll applicationService Class w a7ot al token bta3e f service al LoginAsync()
            => var token = tokenServices.CreateToken(UserResult.data.Id, UserResult.data.Email, UserResult.data.UserName); // error 
               l2n na2s a basy al List of Roles .. h3mlha azay ?? .. fa h create Function t gbly kol al roles : 
               - IIdentityService -> make new funtion segnature -> (GetRolesAsync)
               - hro7 a3mla Implemnt in IdentityServices Class 
               - arg3 tany applicationService class in LoginAsync() 
             =>   var RolesResult = await identityServices.GetRolesAsync(UserResult.data.Email);  // da aly dfto gdid .. 3mlt function btgm3le al roles w naditha fel loginasync 34an CreateToken() m7taga al list of rols as paramter 
            var token = tokenServices.CreateToken(UserResult.data.Id, UserResult.data.Email, UserResult.data.UserName, RolesResult.data);

             8 - nfs al klam fel RegisterAsync()
             */
            #endregion

            #region Debug 
            /*
              1 - in Login Request -> a3ml row fel Body w Send Request 
               hl2ih fel Response 3ml create ll token bta3e m3 al data aly rag3a : 
              
               "displayName": "youssab hany",
               "email": "youssab@gmail.com",
               "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjZmMmM4N2ZhLWViMDQtNDk1My1iM2JmLTIyYTQyODg3ZDg3MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InlvdXNzYWJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InlvdXNzYWIyMiIsImV4cCI6MTc4MzgyNjA1OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAxNC8iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDE0LyJ9.xiFeSgybzCO0eg9LTQJQ1eKon4uSF_pVp_6eOhmhJZ8"
               mokwn mn Header + payload + signature 
            ======================================================================================
             2 - in Register Request -> a3ml Row fel body b data gdida (register gdid) -> w a3ml send Request 
               -  hl2ih fel Response 3ml create ll token bta3e m3 al data aly rag3a : 
            {
              "displayName": "San Saad",
              "email": "San@gmail.com",
              "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Ijc3N2JhYmYzLWJlNzAtNGI0YS04ZmUxLTE3YWJlMDkyMWYyOCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6IlNhbkBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU2FuNDQiLCJleHAiOjE3ODM4MjYyODAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMTQvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAxNC8ifQ.QHeQNQFD3Ajb8J3pGCZdaV3yNncyjo4tY2B7AmqBAEA"
             }  
            a7na kda bnina al Token bs lsa 3ayzen n3ml goz2 an lma ygily al request ana h3ml eh ? 
            a7na 3mlna an al token yt create f mr7lt al login and Register w ytb3t ll Frontend in response 
             */
            #endregion

            #endregion

            #region Authorization [Config JWT] - Part1 
            // azay lma ygily al Request mn al Front-end abd2 a st2bl al token da w avilidate 3leh 
            #region Steps 
            /*
              1 - in Program.cs Check on middlewares 
              hla2i any bst5dm ->  app.UseAuthorization(); bs 
              fa h7ot middleware fo2iha (wel trtib mohem) -> app.UseAuthentication();

              2 - at2kd an al Endpoints fel Controller aly h4t8l beha -> [Authorize]
                 f Controller ProudactController -> GetAllProudacts -> [Authorize]

              3 - hro7 a3rf al CLR bta3e eh tri2t al Authentication aly h4t8l beha l2n al Defult Authentication kan "Cookie"
                 - in Infrastrucutre layer -> InfrastrucrureServicesRegistrations -> h3ml Config ll Securtiy service hnak 
             */
            #endregion

            #endregion

            #region Authorization [Config JWT] - Part2
            #region Debug 
            /*
              1 - Run Code 
              2 - hngib Request bta3 Get All Proudacts 
                - feh url : {{BaseUrl}}/api/Proudact -> hygblk 401 Un-Authorized .. yb2a lazem a3ml Login al awl 
              3 - hro7 3la al Login Request ab3t Row fel Body 
                - {
                "Email":"youssab@gmail.com",
                 "Password":"P@ssw0rd"
                  }   
               4 - ha5od al Token aly tl3le da copy .. w brge ll Get All Proudact -> Authorization -> Bearer token -> paste al token 
               5 - abl matdos send 48l al Aubentu 
               6 - send in Login Reruest 
              = gbly al all proudacts l2ni b2it Authorized 
             y3ni kda al JWT Tool a4t8lt m3aya l2ni 3mlt Config ll Clr ano bdl ma yst2bl Cookie -> JWT token 
             */
            #endregion
            #endregion

            #endregion

            #region Authentication Services [Address - Current User - Check Email]
            /*
              3ayz a3ml al 4 Endpoints b2a bto3 al Security b3d ma 5lst mwdo3 al JWT .. 
              
             */
            #region Steps 
            /*
              1 - a3ml DTO 5as bel Address -> Application Layer -> Dto -> Identity -> AddressDto  
              2 - h3ml Service gwa al IIdentityService asmha -> GetAddressByEmailAsync 
                  w service tanya asmha UpdateAddressAsync() gwa IIdentityService interface brdo 
                  w service kman asmha -> EmailExistAsync() 

              3 - h implemnt al 3 Service dol gwa Class al IdentityServices 
              w kda ana 5lst al 3 functions aly hytklmo m3 al UserManger (Identoty Database)

              4 - h3ml 4 services fel Authentication 34an yklmo al Endpoints 
                - Contract -> IAuthenticationServices -> make 4 Services (CheckEmailAsync - GetUserAddressAsync - UpdateUserAddressAsync - GetCurrent User)
                 dol al serices aly htt3aml m3 al Endpoints aly fel controller 
                 y3ne 3mlt al services bta3t al IdentityServices -> h5li al AuthenicationService tt3aml m3ahom (34an agm3hom f mkan wa7d) -> al Controller yt3aml m3 al AuthenicationService

              5 - aro7 a3mlhom implemnt fel AuthenticationService Class 
             */
            #endregion

            #endregion

            #region Authentication Endpoints [Address - CurrentUser - CheckEmail]

            // 1 - AuthenticationController -> make 4 Endpoints 

            #region Debug 
            /*
              1- run application 
              2- 3nd al Login Request a3ml login w send request 
              3- b3d ma 3mlt login -> a3ml new request asmha CheckEmail 
                - url : {{BaseUrl}}/api/Authentication/emailexists
                - ab3t fel param -> email : youssab@gmail.com 
                fa yb2a al url kda : {{BaseUrl}}/api/Authentication/emailexists?email=youssab@gmail.com
              = hy3mly true .. y3ne al email ealrady mwgod w kda awl endpoint tmam 
            ============================================================================================
            1 - a3ml new request asmha CurrentUser 
                ab3t fel param -> email : youssab@gmail.com
                url : {{BaseUrl}}/api/Authentication/CurrentUser?email=youssab@gmail.com
            al endpoint de m7taga authorization l2nha[Authorize]
            - fa hagy 3nd al Authorization -> bearer Token -> hat mn al Login Request al token copy paste 
            = result : 
            {
             "displayName": "youssab hany",
              "email": "youssab@gmail.com",
               "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjZmMmM4N2ZhLWViMDQtNDk1My1iM2JmLTIyYTQyODg3ZDg3MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InlvdXNzYWJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InlvdXNzYWIyMiIsImV4cCI6MTc4Mzg2MTg4NiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAxNC8iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDE0LyJ9.M5A5Q1UQDIHLwE8HC68hWXQORpzAxM_cXfWXLNE6gjo"
                }
            note : hwa 3mly token gdid 8ir bta3 al Login a4t8l beh 
          ============================================================================================
            1 - a3ml new request asmha Update Address ->  no3ha post 
            url : {{BaseUrl}}/api/Authentication/Address
            - hb3tlo fel param -> email : youssab@gmail.com
             fyb2a al url -> {{BaseUrl}}/api/Authentication/Address?email=youssab@gmail.com

             al endpoint de m7taga authorization l2nha[Authorize]
             - fa hagy 3nd al Authorization -> bearer Token -> hat mn al Current User Request al token copy paste 
             - fel body ab3t 4wyt bynat : 
              {
              "Street":"Taalat",
              "City":"Cairo",
              "Country":"Egypt",
              "FirstName":"youssab",
               "LastName":"Hany"
                 }
           result -> hy3mlk al Updated address aly anta 3mltha .. bygib al Address bta3k al awl mn al Email w b3den y3mlo update
            fa lw bset fel Database hla2i al Adress bta3k m3mol bel bynat al Gdida de 
            ====================================================================================================
             1 - a3ml new request asmha Get User Address ->  no3ha Get
            - hb3tlo fel param -> email : youssab@gmail.com
              url : {{BaseUrl}}/api/Authentication/Address?email=youssab@gmail.com
            al endpoint de m7taga authorization l2nha[Authorize]
             - fa hagy 3nd al Authorization -> bearer Token -> hat mn al Current User Request al token copy paste 
            result = hygblk al 3nwan bta3k ... 
            note : lw 3dlt al 3nwan fel request Update Address aly ablha 3ltol .. htsm3 hna w hyget bel last update 

            note : fel A5r ana b create token 34an a5li al Data fel Database bta3te Secure an .. 34an twsl ll Endpoint w tgib menha data 
                   fa al Endpoint de Authorize .. 34an t5o4lha w tgib al Data mnha fel response .. lazem t create token .. 
                   fa al session de kolha 34an al hdf da 
             */
            #endregion

            #endregion

            #endregion

            #region Session07 

            #region Order Module OverView 
            /*
              al Client by4of al proudacts aly odamo -> h5tar al Proudacts aly 3ayzha -> htt7t fel Basket -> lw ht3ml order 5las byt2kd mn hwytk (make sign in) -> al Basket
              byt7wl l order method 
              fa al Order Module hwa aly ben any b7wl al Basket into order 5las 
              w byb2a fel module da kza tri2t 47n (Delivary Methods) al Client by5tar mnhom 
             */

            #region Order Processing 
            /*
              1 - convert Basket into Order 
              2 - validations on Current Proudact 
              3 - order status tracking (payment recived - Faild - Pending) 
              4 - Delivary Method 
             */
            #endregion

            #region Order Controller 
            /*
              1 - create order -> Post Api/Order (Atwl wa7da) input : BasketId , DelivaryMethodId , Address .. output : Order
              2 - Get User Order -> Get Api/Order .. output List Of Orders bona2n 3la email al user 
              3 - Get Order By iD -> Get Api/Order{Id}
              4 - Get Delivary Methods -> GEt  Api/Order/DelivaryMethods 
             */
            #endregion

            #endregion

            #region Order Entites 

            #region Intro 
            /*
                 3ndy main table -> Order 
                 by3tmd 3la 7gat tanya zy : 
                 Order Status (Enum) - Shepping Address (3nwan al twsil belzbt .. w da bn extracto mn al User Address)
                 delivary Mehtod table (feh anwa3 al Shipping) .. item of orders (an hykon table 1 : m m3 al order table .. an al one order can have many items)
               note : al Item order feh (7gat 5asa bel proudact w 7gat 5asa bel Basket) .. w kol da hytgm3 fel Order Table 
                */
            #endregion

            #region Steps 
            /*
              1 - in domain Layer -> Entites -> new Folder -> Orders and make inside it Order Tables
              
              note in table OrderItem -> hwa feh cols t5os al Proudact nfso .. fa hngm3 al Cols bta3t al Proudact al 5asa bel item f class l27do
              Make ProudactItemOrder Table gwah al Cols bta3t al Porudaact al 5asa bel item .. m4 h3ml Realtion ben al ItemOrderTable and proudact Table 
              w da ProudactItemOrdered hyb2a goz2 mn table aly hwa (OrderItem)
              ana bs gbt nos5a mn al Proudacts from table proudact w 7ttha w gdwl lw7do (ProudactItemOrder)
              b3d kda ndeto gwa al Table nfso OrderItem .. fa b2a m3aya data of proudact + ba2i al logic table 
              tb ana leh m4 3aml relations ? 
              l2n momken al Client yms7 al proudact mn 3ndy fa lw leh order = hdtr ams7 al order m3ah 
              fa m4 3ayzen n3ml relation l2n al Order 7ta lw mlo4 proudact fa hwa bytsgl m3lomato fel 4rka 
              
             note : OrderAddress da hyb2a goz2 mn table brdo gwah 5sa2s al Address bta3t al User 
             */
            #endregion

            #endregion

            #region Order Confguration & Seed 
            /*
               - InfraStructure Layer -> Data -> StoreDbContext 
               1 - hzwd al 3 tables 3nde fel Database (using DbSet)
               2 - h3ml ll 3 tables dol Configurations b2a .. Data -> Configurations -> zwd al Classes w a3ml al Congig.
               3 - a3ml Migration without Update-Database l2ne h3mlha Data seed 
               4 - h add json file feh data gahza 3n al Delivary Methods (hbdl gwa al Json Da al Price -> Cost )
                 - h7ot al Josn da fel pin folder fel project bta3e 34an al Data seed by2ra mn hnak ba2i al Jsons 3ltol
               5 - fel Infrastructure layer -> seeding -> CatalogDataSeeder 
                 =>  await SeedIfEmptyAsync<DelivaryMethod>(SeedPath, "delivery.json", ct);
             */
            #endregion

            #region Order Dto's
            /*
              34an a3ml al service m7tag aghz al Dto , result (gahez) , mapping , b3den al Service 
              1 - in dto folder -> new folder -> Order 
              2 - a3ml al Dto's bta3to b2a 

              note in OrderDto : 
              al order aly hyb3tholy al Front-end hyb2a 8ir al Order aly ana h3mlholo return fel Response 
              fa 3mlt dto ll front -> OrderDto [post]
              order hyrg3 mn hna ll front -> OrderToReturnDto [Get]
             */
            #endregion

            #region Order Mapping Profile 
            /*
              1 - in Application layer -> Profile -> OrderProfile 
              2 - h3ml OrderItemPictureUrlResolver fel Profile brdo 34an agm3 path al Pic 
             */
            #endregion

            #region Order Service [Create]
            /*
              1 - in application layer -> Contracts -> IOrderServices 
              2 - h3ml Class fel Services y implemnt al Interface 
             */
            #endregion

            #region Order Controller [Create Order End Point]
            /*
               1 - In Application Layer -> ApplicationServicesRegister -> hzwd : 
                    services.AddScoped<IOrderServices, OrderServices>();

               2 - Add New Controller (Api) -> OrderController 
               3 - make Create Action 
               4 - Run 
             */

            #region Postman  
            /*
              1- h3ml Basket01 object in Redis Database (lazem a create al awl fel redis abl m3ml al 5twat al gya .. wna m4 fakr btt3ml azay)
              1 - run then open postman 
              2 - hro7 3nd al Login -> h3ml Login b7is yb2a 3nde Token 
              3 - create new Folder -> Session07 
              4 - make inside it New Request No3o Post .. Authrized = beerare .. w 7ot al token aly 3mlto bel login-> Create Order 
              5 - Url : {{BaseUrl}}/api/Order
              6 - Params : hb3t Email ,
              7 - fel Body hb3t OrderDto : 
                  {
   "BasketId":"basket01",
   "DeliveryMethodId":"1",
   "ShippingAddress": {
    "FirstName":"samy",
    "LastName":"hany",
    "Street":"taalat harb",
    "City":"Cairo",
    "Country":"Eygpt"
   }
}

             */
            #endregion

            #region Debugging 
            /*
              hygilk al Request 3la al Action bta3tk "CreateOrder"
              gylk al OrderDto fel request ll Action de wel Email kman m3ak .. 
              hbd2 a5o4 gwa al Service -> orderServices.CreateOrderAsync
              -> ht get al Basket -> basket01 wel Basket da gwah One Item 
              -> hygblk kol al Prouacts ides .. hwa mfe4 8ir wa7d bs bta3 al Basket01 
              -> hla2i al Montg mwgod gwa al List<OrderItem> 
              -> y gblk al Adress 
              -> ygblk al Delivary Method 
              ykml b2i al Services .. w fel a5r y return al order kolo lek bkol al Details bta3to 
             */
            #endregion

            #endregion

            #region Order Services [All Delivary Methods - All Users Orders - Order By Id]
            /*
              All Delivary Methods -> de ll front end 34an y3rdha ll user 
              All Users Orders -> get all orders aly al user talbha mne 
              Order By Id -> get specific order to user 

             steps :
            
             1- in Contract -> IOrderService -> hzwd 3 Endpoints signature 
             2 - H imblemnt al 3 Endpoints dol fel OrderService 
            note : 
            in GetAllOrdersByEmailAsync and GetOrderByIdAndEmailAsync
            GetAllOrdersByEmailAsync -> filter by Email 
            GetOrderByIdAndEmailAsync -> filter by Email and Id 
            fa h3mlhom Filter Specifications : 
            hagy 3nd al Specifications -> Create New Class -> OrderSpecifications
            b3d kda hrg3 ll Service 34an ast5dm al Specifications da fel two Endpoints 
             */
            #endregion

            #region OrderController [All Delivary Methods - All Users Orders - Order By Id] Endpoints 
            /*
              1 - in OrdrController -> hro7 a implemnt al 3 Endpoints bto3i 
             */

            #region Debuging 
            /*
              1 - Run and Open Postman 
              2 - create new request (Get) -> Get Delivary Methods 
               url : {{BaseUrl}}/api/Order/DelivaryMethods
               lw dost send hygbli kol al Dilavey Methods aly 3nde .. bs ana m4 3aml 3la redis object 34an m4 fakr azay 
            ==========================================================================
              1 - create new request (Get) -> Get All Orders 
                url : {{BaseUrl}}/api/Order 
                authorization: berrer token w hgib al token mn al Create Order Request in Postman 
                params : email = youssab@gmail.com 
                fa yb2a al Url : {{BaseUrl}}/api/Order?email=youssab@gmail.com
            lma a3ml Send request = hygib kol al Orders aly 3nde aly t5os al Email da .. bs ana m4 3aml object in Redis brdo 
            ============================================================================
              1 - create new request (Get) -> Get Order By Id 
                  url : {{BaseUrl}}/api/Order/Id Of Proudact 
                   authorization: berrer token from Get All Orders 
                   param :  email = youssab@gmail.com 
            hygblk one order .. bs ana m3mlt4 al Endpoint de 34an m3nde4 data brdo .. hya last two min in vedio 
             */
            #endregion

            #endregion

            #endregion

            #region Session08

            #region Payment Module OverView 
            /*
              a7na kan 3ndna Basket w 3arfen ano hyt7wl l Order .. w 3mlna f3ln Order Module w creatna al Order 
              lakn amta byt7wl al Basket l Order ?? 
              = feh two Cases bntklm fehom lma nigy n3ml Payment Module .. 
              case 1 : lma al Basket ytdf3 kol floso = al Basket da y7slo Creation 

              Case 2 : Payment Intent .. y3ne nyt df3 (once an al Basket 7slo Creation = feh nya ll df3) .. nyt df3 = Hkrr hdf3 b eh 
              al Order lma byt Create m3ak .. byb2a Pending l2no byb2a m3ah Payment Intent .. y3ne order at3ml bs lsa mtdf34 floso .. momken adf3 3nd al astlam m4 kol mara online

              a7na hnm4y b anhe Bussnis Case (al atnen s7) = Order hyt3ml b3d al Basket whyb2a m3ah Bayment Intent bta3to .. wdf3 ay w2t ana 3ayzo (swa2 online aw lma ygily al order)
             */

            #region Payment Flow 
            /*
              fel tbi3e an al Payment Module yb2a feh 3aml 5argi hwa al ms2ol 3n an al User lw hy4tri online 
              fa hyd5l al Card info bta3to 3la al system .. w m4 mn al Tbi3e any a save data al Card de 3nde 34an a3ml b3d kda 3leha Validation l2 
              ana feh 3aml 5argi hwa aly by3ml al Validation de 3la al Card de 3n tri2 ano by4of Account al Banke bta3 al Card de w y4of hl al Bynat mottabka wla la 
              fa ana bst5dm al 3aml al 5argi da fel Payment Module bta3e (Third Party Service).. yt2kd w y3ml Validate an al Flos aly hys7bha mn al Account da mkfya 34an ys7bha w y7otha fel Account bta3na 
              al Third Party Service aly hya al 3aml al 5argy da asmo => Payment GetWay (PayPal - Stripe - Fawry..) wel 5twat f ay Payment GetWay btb2a sabta 

              fa al Sequance bta3e : 
              lma a3ml Pasket -> hro7 ll Payment GetWay h2olha ana m7tag a3ml Payment Intent y3ne ana lsa m4 hdf3 .. bs ana 34an a3ml Object mn al Payment intent hwa al Payment getway (stripe)
              y3ne ana h4tri T-shirt -> da bayment intent .. h4tri sox -> da bayment intent .. fa ana b5l2 List of Payment Object (bel Payment GetWay)
              -> w yrg3li fel A5r al Payment Intent dy yrg3ha gwa al Basket -> b3d kda y5zn al Payment intent de gwa al Order 

              yb2a sammary : 
              h create Basket feh 4wyt items fa al Front-end hybd2 y create lya bel Endpoint = Payment Intent .. hyro7 yklm beha al Payment Getway .. w y5zn al Payment intent de gwa al basekt
             */
            #endregion

            #region Payment Module 
            /*
              3ndk feh 3 agza2 (Frontend - Backend - Third Party Service (Stripe))
              al Frontend lma bn3ml al order hwa lay bykon m3ah al Payment Intent .. fa hwa bykon 3aml Integrate hwa kman 
              m3 stripe fa doro ano y wsl al User ll stripe 34an ydf3 flos al Payment intent .. lma al user ydf3 flos l stripe , stripe yrod 3leh b success w yb3tle ana kman ka backend an al 3mlya tmt .. fa ana k backend a7wl al Basket + payment intent da l Order + payment intent m4 bending b2a = ok 
              note : 
              ana ka backend bb2a 3aml Api bb2a msgl al Payment intent de (al amount bta3tha wel Price w kol 7aga .. ana aly 3aml al payment module )

              note : tb hwa stripe by3rf mnen ano byklm frontend wla Backend ?? 
              = al Strip by3ml Public Key and Private key 
              Private Key -> Backend , Public Key -> Frontend 
              fa stripe lma bt3ml mn 3leh Account .. bydek al Private Key bta3k t7oto fel Api aly bt3mlha k backend 
              w yde ll Frontend al Public Key .. leh = 34an lma aklm al stripe ka front end y3rf mn al Public Key 
             
              note : amta al Api (backend) yklm al Stripe ? 
                    = lma ygy y3ml Payment Intent .. 8ir kda hwa mstne mn stripe ano y2ol ll Backend an feh 7aga atdf3t 
              
              al na7ya al tanya Client (Frontend) -> lma al user y3oz ydf3 flos .. al front end yb3to ll stripe ydf3 floso (bs byb3t al user da m3ah al Public key)

             */
            #endregion

            #endregion

            #region Stripe DashBoard 
            /*
              1 - in google search on stripe 
              2 - a3ml Create Account on stripe 
              3 - fel sandbox -> create -> Create Account (de al Account aly bt3mlha hya aly hta5od 3leha flos)
              fa ana kda b2a 3ndy ACcount asmo ECommerce45 -> ahm 7aga feh hla2i al (Api Keys)
              feh Secrite Key -> da hyb2a bta3y ka backend wna b3ml Configurations 
              Public Key -> bta3 al Frontend 
             */
            #endregion

            #region Update Models & DTO's 
            /*
              1 - in Domain Layer -> Entites -> Baskets -> CustomerBasket 
                  hzwd feh four Properties (ClientSecret , PaymentIntentId , DelivaryMethodId , ShippingPrice)
                  ClientSecret -> da secret 5as bel Payment intent btzhr m3aya (m4 bl api kolo l2n al api leh secret key)
                  PaymentIntentId -> al Id bta3 al payment aly h search beh gwa stripe .. stribe by search 3n al payemnt intent bel PaymentIntentId
                  y3ne stripe by search b 7agten ClientSecret , PaymentIntentId

               2 - nfs al 4 properties h5odom paste fel BasketDto 
               3 - property  public string? PaymentIntentId { get; set; } -> htdaf f entity -> Orders -> Order 
               4 - Add Migration to add thoes cols 

              hla2i fel database PaymentIntentId atdaf l table al Orders .. w hyt3dl fel Dtos w kman al basket 
              bs al basket da m4 table fel database da redis database fa 34an kda al col da sm3 bs fel order table .. l2no table fel database 
             */
            #endregion

            #region Payment GetWay Service 
            /*
              3ayz a3ml Service ttklm m3 al Stripe m4 ttklm m3 al Endpoints 
              fa ana h3ml Service tklm al Payment Getway ayn kant eh (stripe - fawry - paybal...)
              
              steps : 
              1 - in application layer -> Contracts -> Make new Interface -> IPaymentGetWay 
              2 - al return bta3 al Functions de hygy mn stripe -> fa hro7 a3ml result to return from functions : 
                - Common -> new Class -> PaymentIntentResult
                - b3d ma azbt 4kl al return arg3 tany ll IPaymentGetWay
              
              3 - inside Infrastructure layer -> Create New Folder -> Payments
              4 - make inside it Class -> StripePaymentGetway : IPaymentGetWay
              5 - install package of stripe inside Infrastructure layer -> Stripe.net
                - w hst5dm service mn stripe gahza gwa al Class bta3e StripePaymentGetway
              
              6 - hst5dm bynat mn al Stripe fa 34an a5znha h7otha fel AppSetting.Json
            "Stripe": {
                 "SecretKey": "Secret Key .. bs ana ms7o 34an Github m4 by2blo", // gbto mn stripe mfso lma 3mlt Account 
                 "DefultCurrency": "USD"
                         }
             
             7 - t7t class StripePaymentGetway -> h3ml Class tany asmo PaymentGetwaySettings 34an azbt al Configurations bta3t al AppSettings.json 

             8 - 3ayz b2a a5li al class StripePaymentGetway y2ra mn al AppSettings.json n3ml eh : 
               - in Program ->   builder.Services.Configure<PaymentGetwaySettings>(builder.Configuration.GetSection("Stripe"));

              9 - n3ml Implmentation ll services b2a (Create - update)
                bs abl ma a3mlhom implemnt .. bel DI in ctor -> IOptions<PaymentGetwaySettings> options .. de al options aly m3aya mn al AppSettings.json
                b3d kda implemnt al two functions
             */
            #endregion

            #region Payment Api Service 
            /*
              hn3ml al Payment service al 5asa bel Api b2a k backend m4 k t3amolat m3 stripe 
              steps : 
              1- in application Layer -> Contract -> Add new interface -> IPaymentService 
              2- h3ml Class fel Services -> PaymentServices 
              3 - hat class al PaymentGetwaySettings w an2lo mn StripePaymentGetway into PaymentServices
              4 - h7dr 7gat bel DI abl ma a5o4 fel Implemntation of service 
              5 - implemnt al service .. aly b3d kda hst5dmha fel Endpoints bta3te 
             */
            #endregion

            #region Payment Controller and Endpoint 
            /*
              1 - in application Layer -> ApplicationServicesRegister =  services.AddScoped<IPaymentService, PaymentServices>();
              2 - in InfraStructure Layer -> InfrastrucrureServicesRegistrations =  services.AddScoped<IPaymentGetWay, StripePaymentGetway>();
              3 - in Controller -> make new Controller (Api) -> PaymentsController 
              4 - h3ml Endpoint asmha -> CreateOrUpdatePaymentIntent 
             */

            #region Depuging 
            /*
              1 - run and open Ubentu 
              2 - open Postman 
              3 - hro7 3nd al Login Request a3ml Login 34an yb2a m3aya al token 
              4 - hbd2 a create Basket -> fa hro7 3nd al Request bta3 Create Basket .. bs hzwd an al "DeliveryMethodId":1  -> send 
                  hft7 al Redis nfsha hla2i an feh Basket wa7d mwgod fel Database bta3t al Redis 
                  kda 
            {"Id":"1","Items":[{"Id":10,"Name":"Proudact01","PictureUrl":"Test","Price":150,"Quantity":5}],"CleintSecret":null,"PaymentIntentId":null,"DeliveryMethodId":1,"ShippingPrice":null}
            w 5li balk mn al 7gat aly b null de 34an htt8ir b3den 

              5 - kda ana 3mlt login w Create basket .. al 5twa aly b3d kda = 3ayz adf3 flos 
                  fa h3ml New Request in Postman (Post) -> Create Payment Intent
                  URL : {{BaseUrl}}/api/Payments/basket01 
              note : basket01 .. da al object aly mwgod f redis database 
              Authorization : Berer Token -> a7ot al Token aly 3mlt beh Login 
            
            step 5 kda bt2ol ll back end anna 3ayezn n3ml Payment Intent ll Basket aly m3ana da (basket01)

             kda al request hyro7 ll PaymentsController -> 3la al Endpoint CreateOrUpdatePaymentIntent 
             y5o4 gwaha ynfz al Service CreateOrUpdatePaymentIntent => wziftha anha t3mlk Create ll Payment Intent for your Basket
             + bt3mlk al Client secret + payment intent id bta3k aly kano 3ndk b null 

             lma nft7 b2a al Stripe on Google -> Transactions -> Pyamnets .. hla2i fel UI .. All = 1 (bs al State bta3tha InCompleted .. y3ne lsa mdf3t4 .. tb amta bdf3)
             de b2a dor al Front-end .. ano yzhr ll User inputs ymmla bynat -> ywdeha l stripe -> ys7b al flos -> stripe y2olk status is completed ll front + yb3tlk k backend ano Completed 
             
             */

            /*
              Hnzwd 7aga fel Order Service : 
              - Application Layer -> Order Service -> in CreateOrderAsync() 
              ->  PaymentIntentId = Basket.PaymentIntentId
            al order lma ygy yt Create hyb2a m3ak al Basket 
            fa al Basket ana bbasy feha al Payment Intent (lma tro7 3la postman in Create Payment Intent Request .. hla2i al PaymentIntentId and Client Sercret = mwgoden)
            fa al Client secret + payment intent bb3tha ll Order .. 34an yb2a 3arf an al Order da tb3 al Payment intent de 
            34an awl ma ydf3 7alt al Order da tt7wl l success or fail 

            note : ana dori k backend any a create Payment intent ll frontend yst5dmha bs 
             */
            #endregion

            #endregion 

            #endregion
        }
    }
}
