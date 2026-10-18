using AutoMapper;
using Ecommerce.Application.DTO_s.Proudacts;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Proudacts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Tests.CoreTest.Products
{
    public class ProudactServicesTest
    {
        #region Test Case 1

        [Fact]
        public async Task GetProudactByIdAsync_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IGenericRepository<Proudact, int>>();
            var mapperMock = new Mock<IMapper>();

            unitOfWorkMock
                .Setup(u => u.GetRepository<Proudact, int>())
                .Returns(repositoryMock.Object);

            repositoryMock
                .Setup(r => r.GetByIdWithSpecificationAsync(
                    It.IsAny<ISpecifications<Proudact, int>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Proudact?)null);

            var service = new ProudactServices(
                unitOfWorkMock.Object,
                mapperMock.Object);

            // Act
            var result = await service.GetProudactByIdAsync(100);

            // Assert
            Assert.False(result.IsSuccessed);
            Assert.Contains(
                result.Errors,
                error => error.Code == "Proudact.NotFound");
        }

        #region Explain Test Function 

        #region Intro
        /*
          de test Function 3n Service asmha GetProudactByIdAsync(int id) dorha eh al Function de 
          anha tgib al proudact bel Id bta3o mn al DB .. wel Test function de bt test an al Proudact da 
          lw al Id bta3o fel DB m4 mwgod hy return Not Found f3ln zy ma amktob fel service kda ? 
          if (Proudact is null)
          return Result<ProudactDto>.Fail(Error.NotFound("Proudact.NotFound", $"Proudact With Id :{id} is not found"));

          bn3ml Test function 34an mn3ml4 request an response manually for test al service tmam wla eh .. ana aly b3ml Test function t3ml kda directly 

          Test Funtion mt2sma 3 Parts:
          Arrange -> de al ms2ola 3n tghiz al Dependances aly ht7tagha al Test Function de 34an t4t8l 
          Act -> de bt48l al Function aly 3ayzen n5tbrha 
          Assert -> bnt2kd an al Expected result = Actual Result ? 
         */
        #endregion

        #region Arrange 
        /*
           test Function Syntax : 
          - ast5dmna feha Task 34an al Service al asasya btst5dm Async and Await 
          - leh bn3ml Mock ? 
             var unitOfWorkMock = new Mock<IUnitOfWork>();
             var repositoryMock = new Mock<IGenericRepository<Proudact, int>>();
             var mapperMock = new Mock<IMapper>();
          = hya nfs fkrt al Dependancy aly al Class by7tagha .. y3ne msln al Class service by7tag object mn al Repositores 34an yghz al data .. w hkza msln object mn al Mapper 
            tb a7na 34an n5tbr al Service m7tagen nghz eh ? 
            m4 lazem n48l al Database wnghz kol al Dependances al 72i2ya aly al Class service by7tagha .. fa bnst5dm al Mock 
            bs note .. ana b7dd al Behavior bta3 al mock da bel Setup() 
            y3ne ana momken anady reposetory .. bs al behavior bta3o ano lma atlob montg .. rg3le null bdl ma yro7 msln ll Database 
            y3ne ana b7dr al Debendancy bta3te + bt7km fel behavior bta3o = Mock (w kol da gwa al Test Function)
            ex : 
            var unitOfWorkMock = new Mock<IUnitOfWork>(); // 7ddt al Dependancy 
            unitOfWorkMock.Setup(u => u.GetRepository<Proudact, int>()).Returns(repositoryMock.Object); // (ghzt al Reop al whmy) .. na2s a7dd al Bihavior bta3o
            y3ne eh ? 
            a7na bn7dd ll mock bta3na (unitOfWorkMock) lma y7sl tsrof mo3yn = httsrf azay ? 
            al GetRepository btrg3 Generic Repository 
            y3ne lma atlob mnk repository = rg3lha al repository al whmy aly ghznah abl kda .. tb fen al repository al whmy da => repositoryMock.Object
            da aly al service tt3aml m3ah ano repository 3ady 
            leh al step de mohma ? 
            34an fel Service al aslya by3ml -> unitOfWork.GetRepository<Proudact, int>()
            fa lazem nghz al Mock 34an yrg3 al Repository al whmy aly hn7dd al Behavior bta3o b3d kda 
             y3ne ana kda bel line da : 
           unitOfWorkMock.Setup(u => u.GetRepository<Proudact, int>()).Returns(repositoryMock.Object); = ghzt al repository bta3y al whmy .. b2ol whmy l2no m3 hy2om bel function bta3to ano y5o4 gwa al Database .. bl ana aly h7dd al Behavior bta3o b3d kda 
            
            al Behavior hwa anna n5li al Repository da y return null .. w hnfhm 7sl azay 
            repositoryMock.Setup(r => r.GetByIdWithSpecificationAsync(It.IsAny<ISpecifications<Proudact, int>>(),It.IsAny<CancellationToken>())).ReturnsAsync((Proudact?)null);
            y3ne eh ? 
            - r.GetByIdWithSpecificationAsync -> 34an tgib al Proudact bel Specification al 5as beh 
            - it.isAny .. m3naha anha mthtm4 bel 7aga de dlw2t (Spesification aw cancellation token)
            leh 3mlna kda .. 34an nt2kd mn al Service lma al proudact mykon4 mwgod .. m4 anna n5tbr al Specification nfsha
            - .ReturnsAsync((Proudact?)null); .. y3ne hy return null mn no3 al proudact da 

            fa kda a7na 3mlna service bttkob repo , wel repo hy return null bdl ma ynfz query 722ya mn al DB 

            -  var service = new ProudactServices(
                unitOfWorkMock.Object,
                mapperMock.Object);

            = a7na 3mlna mock ll Dependances .. bs lazem n3ml Mock ProudactServices nfsha .. da al Class aly feh al service al aslya bta3tna aly 3ayzen n3mlha test 
              tb leh 3mlna kda ? 
              l2n de al service aly m7tagen n test al Logic bta3ha al 72i2e wel Ctor bt3ha al 2sly : 
               public ProudactServices(IUnitOfWork unitOfWork,IMapper mapper)
               lakn a7na 7tena gwah al Dependancies bta3t al Test Function bta3tna al whmya .. m4 al Dependancies al aslya 
               y3ne al service ht4t8l bel Code al 72i2e bta3ha .. bs bel Dependances aly 3mlnaha 
         */
        #endregion

        #region Act 
        /*
          var result = await service.GetProudactByIdAsync(100);
          hna ana bnady 3la al service al aslya aly h3mlha test .. fa tro7 tnfz aly gwaha 
          al service t3ml ProudactSpecification by using id = 100 -> ttlob repo mn unitOfWorkMock fa yrg3 al repositoryMock aly ghznah 
          -> tnady 3la GetByIdWithSpecificationAsync wel mock y return null -> al service al asly td5ol f conditon if (Proudact is null).
          -> ft return Proudact.NotFound

          a7na m2gbrna4 al Service anha t return not found Directly .. a7na 5lina al Dependancy t return null w sbna al Logic bta3 al Service y4t8l
         */
        #endregion

        #endregion

        #region Sammary Note :  
        /*
          1. Ya3ny eh Dependency Injection (DI)?

        Dependency Injection hya en el Service ta5od el dependencies elly me7tagaha men barra, zay IUnitOfWork w IMapper, badal ma te3melhom gowa el Service nafsha.

        2. Ya3ny eh Mock?

        El Mock howa object we7my b3melo 3ashan a5aly el behavior beta3o ta7t se6rty fel testing.

        El Service el 7a2e2eya hya elly ba5tarha a3melha test.

        El dependencies beta3etha momken a3melhom Mock.

        Ana ba7added el Mock hayeraga3 eh lama el Service testa5demo.

        El far2 el mohem:

        DI: ezay el dependencies betetpassar lel Service.

        Mocking: ezay a3mel dependencies wehmeya w a7aded el behavior beta3ha fel test.

        3. Leih besta5dem Mock fel Unit Testing?

        3ashan a3zel el Service elly ba5tarha 3an el dependencies beta3etha.

        Masalan, law ba5tar ProudactServices, mesh mo7tag a-connect bel Database aw a3mel request 7a2e2y lel DB. Ana 3ayez a5tar el logic beta3 el Service lwa7do.

        El benefits:

        El test yeb2a asra3.

        Ma-yeb2ash mo3tamed 3ala Database aw external services.

        A2dar a7aded el scenario elly ba5tbero.

        El test yeb2a more predictable.

        4. Eh ma3na Setup() fel Moq?

        El Setup() ba7added behavior mo3ayan lel Mock.

        Example:

        repositoryMock
            .Setup(r => r.GetByIdWithSpecificationAsync(
                It.IsAny<ISpecifications<Proudact, int>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Proudact?)null);

        Ma3na el code:

        Lama el Service testa5dem GetByIdWithSpecificationAsync().

        W teb3at ay Specification mn el no3 ISpecifications<Proudact, int>.

        W teb3at ay CancellationToken.

        El Mock yeraga3 null.

        Keda ana ba7aded el behavior beta3 el Repository el we7meya.

        5. Ezay a5tar scenario en el Product mesh mawgod?

        El Service 3andaha el logic da:

        var product = await repository.GetByIdWithSpecificationAsync(spec, ct);

        if (product is null)
            return Result<ProudactDto>.Fail(
                Error.NotFound(
                    "Proudact.NotFound",
                    $"Proudact With Id :{id} is not found"));

        3ashan a5tar el scenario elly el Product mesh mawgod, ba3mel:

        .ReturnsAsync((Proudact?)null);

        Keda el Mock hayeraga3 null, wel Service el 7a2e2eya hatestakhdem el result da, w ted5ol fel if (product is null).

        Ba3d keda ba-check en el Service raga3et NotFound.

        6. Tab law shelt el Setup()?

        Law shelt el Setup() da, el Mock mesh hayeb2a 3ando el behavior elly ana 7addedto enno yeraga3 null.

        Fel Moq, lama method beteraga3 Task<Proudact?> w ma3andhash Setup, el default behavior ghaleban hayeraga3 null lel reference type 3and el awaited result. Fa momken el test yefdal ye3ady w yeraga3 NotFound, bas mesh 3ashan ana sa3etha 7addedt el scenario saree7.

        3ashan keda el Setup() mohem: beywaDa7 el intention beta3 el test, w bey5aliny controlled fel behavior elly ba5tbero.

        Note: El natiga el default momken te5talef 7asab no3 el method wel return type. Ma-yenfa3sh a3تمد 3ala default behavior badal ma a7aded el scenario elly 3ayez a5tbero.

        7. Hal keda e5tbarna el Repository el 7a2e2eya?

        La2. E7na ma-e5tbarna-sh el Repository el 7a2e2eya, w ma-e5tbarna-sh el Database.

        E7na e5tbarna el Service el 7a2e2eya, bas 3azalnaha 3an el Repository el 7a2e2eya, w esta5demna Mock benarga3 meno null.

        Da esmo Unit Test
         */
        #endregion

        #endregion

        #region Test Case 2 
        [Fact]
        public async Task GetProudactByIdAsync_ProductExists_ReturnsSuccess()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IGenericRepository<Proudact, int>>();
            var mapperMock = new Mock<IMapper>();

            unitOfWorkMock.Setup(u => u.GetRepository<Proudact, int>()).Returns(repositoryMock.Object);

            var product = new Proudact
            {
                Id = 100,
                Name = "Test Product",
                Description = "Test Description",
                PictureUrl = "test.jpg",
                Price = 500
            };

            repositoryMock.Setup(r => r.GetByIdWithSpecificationAsync(It.IsAny<ISpecifications<Proudact, int>>(),It.IsAny<CancellationToken>())).ReturnsAsync(product);

            var expectedDto = new ProudactDto
            {
                Id = 100,
                Name = "Test Product",
                Description = "Test Description",
                PictureUrl = "test.jpg",
                Price = 500,
                BrandName = "Test Brand",
                TypeName = "Test Type"
            };

            mapperMock.Setup(m => m.Map<Proudact, ProudactDto>(product)).Returns(expectedDto);

            var service = new ProudactServices(unitOfWorkMock.Object,mapperMock.Object);

            // Act
            var result = await service.GetProudactByIdAsync(100);

            // Assert
            Assert.True(result.IsSuccessed);
            Assert.NotNull(result.data);
            Assert.Equal(100, result.data.Id);
            Assert.Equal("Test Product", result.data.Name);
        }

        #region Explain Test Function 
        /*
          a7na 48alen 3la same Service aly a4t8lna 3leha in Test Case 1 -> GetProudactByIdAsync()
          w hya kan al Logic bta3ha 3obara 3n Two Senario : 
          1 - an f 7alt al Repo retun null = service return not found (w da 3mlna leh Test .. in test Case 1)
          2 - an al Repo rg3 proudact .. s3tha al Service bt7wl al Proudact da into ProudactDto w b3dha bt retun Ok() 
              w da aly 3ayzen n3mlo Test in This Test Function 

          note : 
          m4 hn7tag DB 72i2ya .. hn5li al Repository Mock trg3 Proudact a7na mghzino + hn5li al Mapper Mock yrg3 DTO motwk3

          Explain Test Function : 
          fel Arrange Ghzna al Mockes bta3tna .. aly homa al UnitOfWork , Generic Repository , Mapper 
          bs lsa mzbtna4 al Behaviour bto3hom by Using Setup()
          
          fel Behaviour bta3 al Repositorty .. an f 7alt an al Repo feh Proudact .. fa h5li al Proudact da 
          bnfs al Property pat3t al project bta3e (bs ana aly 3aml al Proudat da for test bdl ma agibo mn al DB .. l2n atf2na an fel test ana b test al Service Logic Without Using al Dependncies bta3tha)
          + kman an lw Feh Any Specifications (agib data mn al Proudact b way mo3yna) fa ast5dm al Spesification w return lya al Proudact fel a5r 
          fa b3d al Setup of Repository da al Service al aslya ht5tbr an feh Proudact f3ln 3ndha .. fa al Repository mock hna b2olo lw feh proudact fel service al aslya tmam a3ml proudact ana 3mlto for test y3ne 
          
          goz2 al Mapper : 
          - fel service al aslya bel Auto Mapper by7wl al Proudact into ProudactDto 
            w 34an n5tbr al Service witout using al Mapping al 72i2e bta3ha = 5lina al Mapper Mock .. w y return al ExpextedDto
            kda : mapperMock .Setup(m => m.Map<Proudact, ProudactDto>(product)).Returns(expectedDto);
   
          fel a5r : 
        var service = new ProudactServices(unitOfWorkMock.Object,mapperMock.Object); which mean anna bnst5dm al Service al asyaya bs bel dependances bta3t al Test Function de (aly hya al Mockes aly 7drnaha)

          Act : 
         var result = await service.GetProudactByIdAsync(100);
         hna al service ht4t8l w ttlop mn al Repository Mock ygblha Proudact .. w htstlm al Proudact bdl ma hwa b null 
         w htkml Mapping (Aly 3amleno hna brdo my Mock) w t return Ok() 
        da m3nah > anna bn test al Service f3ln fel logic bta3ha bs .. a7na bnb3tlha al Matrial (Proudact + expectedDto)
        wel service al 72i2ya t4t8l 3lehom w tnfz al Logic bta3ha 3leh .. w kol da mn 8ir ma a5li al Service al aslya tst5dm al Dependancies bta3tha w tklm al DB wkol da l2ni b test Logic bs 

        Assert : 
        ana b test b2a al Expected an al Test Function 
        IsSuccessed -> hl al 3mlya ng7t 
        NotNull -> hl feh data rag3a ? 
        Id -> hl al DTO byrg3 al Id al mtwk3 .. aly hwa = 100 .. w hyrg3 true l2ni 3aml proudact hna fel Test Fun. Id=100
        Name -> hl al DTO byrg3 al Name bta3 al Proudact al Mtwk3 ? 
    
        fel assert : 
        a7na b3d ma 3mlna al Steps de .. 
        GetProudactByIdAsync(100) -> al service ttlop al Proudact aly id = 100 .. fa tst5dm al Mocks aly 3mlnaha 
        fel test Function -> tla2i al Proudact aly ana 3mlo fel test function l2n al Id=100 -> t3mlo Mapping (yrg3 brdo al Mapping mock aly 3mlnah)
        fa kda asb7 an feh result ana m5znha .. na2s n test b2a fa da by7sl bel Assert .. bs2l 4wyt as2la hl aly ana m5zno fel result da feh kza w kza .. 
         */
        #endregion

        // note .. lma b3ml Run Test fa lma byrg3le al test Success or Field da depend on al (Assert) .. y3ne msln lw 5lit  Assert.Equal(100, result.data.Id); ->  Assert.Equal(200, result.data.Id); hyrg3le Field fel Test Funcion de

        #endregion

        #region Test Case 3 

        [Fact]
        public async Task GetProudactByIdAsync_UsesCorrectIdInSpecification()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IGenericRepository<Proudact, int>>();
            var mapperMock = new Mock<IMapper>();

            unitOfWorkMock.Setup(u => u.GetRepository<Proudact, int>()).Returns(repositoryMock.Object);

            var product = new Proudact
            {
                Id = 100,
                Name = "Test Product",
                Description = "Test Description",
                PictureUrl = "test.jpg",
                Price = 500
            };

            repositoryMock.Setup(r => r.GetByIdWithSpecificationAsync(It.IsAny<ISpecifications<Proudact, int>>(),It.IsAny<CancellationToken>())).ReturnsAsync(product);

            // Act
            await new ProudactServices(unitOfWorkMock.Object,mapperMock.Object).GetProudactByIdAsync(100);

            // Assert
            repositoryMock.Verify( r => r.GetByIdWithSpecificationAsync(It.Is<ISpecifications<Proudact, int>>(spec => spec.Criteria.Compile()(product)&& !spec.Criteria.Compile()(new Proudact {Id = 200})),It.IsAny<CancellationToken>()),Times.Once);
        }

        #region Explain Test Function 
        /*
          a7na dlw2t a5tbrna 2 test Function 3la al GetProudactByIdAsync() 
          Test 1 -> lw al repo m4 byrg3 Proudact -> return Not Found (DONE)
          Test 2 -> lw al Repo byrg3 Proudact -> Return Ok() (DONE)
          Test 3 -> 3ayzen n test an al service btst5dm al Id al mtlob fel search 3la al Proudact 
                    w da test mohem l2n al service tb3t Specification ll Repository an al Proudact Id = 100 , m4 Id=200
                    l2n momken al service trg3 Success r8m an al Id 8lt .. fa de Test Function mohma 

        Important To Know .. 
        a7na 3ayzen n test an al specification aly feh Service al 72i2ya by search by id s7 7sb al 4art aly feh Spacifiaction da 
        y3ne ana 3aml class for specification 3nde fel project 
        public ProudactSpecification(int id) : base(p => p.Id == id) 
        wzift al Specification da ano ya5od Id mne ll Prpudact .. w y search 3leh 3n tri2 al 4rt da p => p.Id == id
        y3ne lw 2olt -> var spec = new ProudactSpecification(100); 
        then .. p => p.Id == 100 .. wel 4rt da mt5zn 3nde fel spec.Criteria
        al Criteria bt7fz 4rt al Search nfso .. 34an al Repository t2dr tst5dmo 
        fa fel test Function de a7na 3ayzen n5tbr al 4rt da 48al tmam wla l2 

        Explain Function : 
        - fel Arrange a7na ghzna al mockes al whmya aly al Service al aslya m7tgaha 
        
        -  unitOfWorkMock.Setup(u => u.GetRepository<Proudact, int>()).Returns(repositoryMock.Object);
           = y3ne al service ttlob repo 5as bel proudact .. fa al unit of work hydeha al Repository mock of Proudact aly ghznah 

        - w3mlt proudact for test hna hyt7t gwa al mock repo aly 3mlto -> repositoryMock
          bs nfhm aktr eh aly 7sl .. 
            repositoryMock.Setup(r => r.GetByIdWithSpecificationAsync(It.IsAny<ISpecifications<Proudact, int>>(),It.IsAny<CancellationToken>())).ReturnsAsync(product);
          al line da 3la b3do by2ol .. lma al service al aslya tnady 3la GetByIdWithSpecificationAsync .. a3mly return ll Proudact aly ghzto mhma kan al specificattion aly atb3t
          leh 3mlna kda = 34an a7na hn check 3la al specification b3d kda using Varify 
          
        - in Act : 
           await new ProudactServices(unitOfWorkMock.Object,mapperMock.Object).GetProudactByIdAsync(100);
           = hna hnklm al Service al aslya bs bel Debendancies aly 3mlnaha .. fa a7na astd3ena al Service al aslya 
             w adnaha ID=100
             fa al service ht3ml Specification w tb3tha ll Mock repo .. w a7na m4 mohtmen fel test de bel result 
             ad ma mohtmen bel Specification nfso aly atb3t 

        - In Assert : 
        It.Is<ISpecifications<Proudact, int>>(spec => ...) -> m4 kfaya an al service tb3t Specification .. a7na 3ayzen al specification by722 4rt mo3yn 
        fa mot8ir al Spec => .. bymsl al Specification aly atb3t f3ln ll Repository 

        spec.Criteria.Compile()(product) -> da al 4rt al awl -> search 3la Proudact al Id = 100 
        !spec.Criteria.Compile()(new Proudact { Id = 200 }) -> lw ml2t4 Id = 200 
        Times.Once -> m3naha anna mtwk3en nnady 3la al repo al motabk ll 4rot de mra wa7da bs 

         lw al Test ng7t da m3nah an Repository mock ast2blt al Specification aly by722 al 4rt aly a7na mtwk2ino fel Assert 
         lakn note mohma : 
         al Setup() -> btrg3 lena al Proudact nfso mhma kant al specifications .. 34an kda lw al Service rg3t proudact fa da m4 
         dlel an al service 3la s7t an al condetion bat3 al Search s7 .. aly by5tbr Condition al search hwa al Varify 

         al Point al Gdida fel test case de hya anna bnrkz 3la tnfiz al Specification m4 3la al Final Result zy Test 1 and 2 
         
         */
        #endregion

        #region Important Notes (Read Again)
        /*
          1. Leih besta5dem Verify?

            Verify men Moq besta5demha 3ashan at2aked en el Mock etnada bel taree2a elly ana metwa22e3ha.

            Masalan, fel GetProudactByIdAsync(100):

            El Service bet3mel ProudactSpecification(100).

            Beteb3at el Specification lel Repository.

            Verify bet2aked en el Repository etnada bel method el sa7.

            Example:

            repositoryMock.Verify(
                r => r.GetByIdWithSpecificationAsync(
                    It.IsAny<ISpecifications<Proudact, int>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            Ma3na el code: el Repository Mock lazim tekon etnada bel method de mara wa7da.

            Mohem:

            Setup() = ba7added el Mock hay3mel eh.

            Verify() = bat2aked en ested3a2 mo3ayan 7asal fel test.

            2. Leih Verify mesh Assert.Equal?

            Kol wa7da leha est5dam mo5talef:

            Assert.Equal()

            Bet2aren ben 2 values.

            Assert.Equal(100, result.data.Id);

            Ma3naha: hal el ID elly rag3 men el Service byesawy 100?

            Verify()

            Bet2aked en interaction ma3 Mock 7asal.

            repositoryMock.Verify(
                r => r.GetByIdWithSpecificationAsync(
                    It.IsAny<ISpecifications<Proudact, int>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            Ma3naha: hal el Service nadet el Repository bel method de mara wa7da?

            El kholasa:

            Assert.Equal = ba-check value.

            Assert.True = ba-check en condition sa7.

            Assert.NotNull = bat2aked en el value mesh null.

            Verify = bat2aked en method etnadet bel arguments wel number elly ana 3ayezhom.

            Law 3ayez at2aked en el Repository estalmet Specification mo3ayana, momken asta5dem Verify ma3 It.Is<...> badal It.IsAny<...>.
         */
        #endregion

        #endregion
    }
}
