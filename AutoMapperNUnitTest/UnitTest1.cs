using NUnit.Framework;
using AutoMapper;
using Model;
using System;

namespace AutoMapperNUnitTest
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
                // Projection Transform
                cfg.CreateMap<CalendarEvent, CalendarEventForm>()
                    .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Date.Date))
                    .ForMember(dest => dest.EventHour, opt => opt.MapFrom(src => src.Date.Hour))
                    .ForMember(dest => dest.EventMinute, opt => opt.MapFrom(src => src.Date.Minute));
                // Nested Mappings
                cfg.CreateMap<OuterSource, OuterDest>();
                cfg.CreateMap<InnerSource, InnerDest>();
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

        [Test]
        public void ModelMapperProjectionTransform()
        {
            var source = new CalendarEvent
            {
                Date = new DateTime(2008, 12, 15, 20, 30, 0),
                Title = "Company Holiday Party"
            };
            
            CalendarEventForm form = mapper.Map<CalendarEvent, CalendarEventForm>(source);

            Assert.AreEqual(form.EventHour, 20);
            Assert.AreEqual(form.EventMinute, 30);
            Assert.AreEqual(form.Title, "Company Holiday Party");
            Assert.AreEqual(form.EventDate, new DateTime(2008, 12, 15));
        }

        [Test]
        public void ModelMapperNested()
        {
            var source = new OuterSource
            {
                Value = 5,
                Inner = new InnerSource { OtherValue = 15 }
            };

            var dest = mapper.Map<OuterSource, OuterDest>(source);

            Assert.AreEqual(dest.Value, 5);
            Assert.IsNotNull(dest.Inner);
            Assert.AreEqual(dest.Inner.OtherValue, 15);
        }
    }
}