using NUnit.Framework;
using AutoMapper;
using Model;

namespace NUnitTestProject1
{
    public partial class Tests
    {
        IMapper mapper;
        [SetUp]
        public void Setup()
        {
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.ClearPrefixes();
                // Recognize Prefixes
                cfg.RecognizePrefixes("frm");
                // Recognize Postfixes
                cfg.RecognizePostfixes("Data");
                cfg.CreateMap<Foo, FooDto>();
                // prefix postfix
                cfg.CreateMap<Source, Dest>();
                cfg.CreateMap<SourcePos, DestPos>();
                // Naming Convention 
                cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
                cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
                cfg.CreateMap<SourceNameConventions, DestNameConventions>();
            });

            configuration.AssertConfigurationIsValid();
            mapper = configuration.CreateMapper();
        }

        [Test]
        public void ModelMapper()
        {
            var testfoo = new Foo
            {
                Name = "BCA",
                Description = "ABCD123"
            };
            var fooDto = mapper.Map<FooDto>(testfoo);

            Assert.AreEqual(testfoo.Name, fooDto.Name);
            Assert.AreEqual(testfoo.Description, fooDto.Description);
        }

        [Test]
        public void ModelMapperPrefix()
        {
            var source = new Source
            {
                frmValue = 123,
                frmValue2 = 456
            };
            var dest = mapper.Map<Dest>(source);

            Assert.AreEqual(dest.Value, source.frmValue);
            Assert.AreEqual(dest.Value2, source.frmValue2);
        }

        [Test]
        public void ModelMapperPosfix()
        {
            var source = new SourcePos
            {
                ValueData = 999,
                Value2Data = 6666
            };
            var dest = mapper.Map<DestPos>(source);

            Assert.AreEqual(dest.Value, source.ValueData);
            Assert.AreEqual(dest.Value2, source.Value2Data);
        }

        [Test]
        public void ModelMapperNamingConventions()
        {
            var source = new SourceNameConventions
            {
                product_id = 22999,
                product_name = "筆記型電腦"
            };
            var dest = mapper.Map<DestNameConventions>(source);

            Assert.AreEqual(dest.ProductId, source.product_id);
            Assert.AreEqual(dest.ProductName, source.product_name);
        }
    }
}