using AutoMapper;
using AutoMapper.QueryableExtensions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Z.EntityFramework.Plus;

namespace EFCore_Include_Filtering_Benchmark
{
    [ShortRunJob]
    //[SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    public class Benchmark
    {
        private readonly IMapper _mapper;

        public Benchmark()
        {
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Blog, BlogDto>().ForMember(p => p.Posts, opt => opt.MapFrom(q => q.Posts.Where(p => p.Title.Contains("Cheese"))));//.ReverseMap();
                config.Advanced.BeforeSeal(configProvicer => configProvicer.CompileMappings());
            }).CreateMapper();
        }

        [Benchmark]
        public void NonFilteredInclude()
        {
            var dbContext = new BlogDbContext();
            var list = dbContext.Blogs
                .AsNoTracking()
                .Include(e => e.Posts)
                .ToList();
            list.ForEach(p => p.Posts = p.Posts.Where(p => p.Title.Contains("test title")).ToList());

            //var trackedObjects = dbContext.ChangeTracker.Entries().ToList();
        }

        [Benchmark]
        public void FilteredInclude_EFCore5()
        {
            var dbContext = new BlogDbContext();
            var list = dbContext.Blogs
                .AsNoTracking()
                .Include(p => p.Posts.Where(p => p.Title.Contains("Cheese")))
                .ToList();

            //var trackedObjects = dbContext.ChangeTracker.Entries().ToList();
        }

        [Benchmark]
        public void Projection_Manually()
        {
            var dbContext = new BlogDbContext();
            var list = dbContext.Blogs
                .AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    Posts = p.Posts.Where(p => p.Title.Contains("test title")).ToList()
                }).ToList();

            //var trackedObjects = dbContext.ChangeTracker.Entries().ToList();
        }

        [Benchmark]
        public void Projection_Automapper()
        {
            var dbContext = new BlogDbContext();
            var list = dbContext.Blogs
                .AsNoTracking()
                .ProjectTo<BlogDto>(_mapper.ConfigurationProvider)
                .ToList();

            //var aaaa = dbContext.ChangeTracker.Entries().ToList();
        }

        [Benchmark]
        public void IncludeFilter_EFCorePlus()
        {
            var dbContext = new BlogDbContext();
            var list = dbContext.Blogs
                .AsNoTracking()
                .IncludeFilter(e => e.Posts.Where(p => p.Title.Contains("test title")))
                .ToList();

            //var trackedObjects = dbContext.ChangeTracker.Entries().ToList();
        }
    }
}
