using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore_Include_Filtering_Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeDbContext();

            //Warmup and test
            var x = new Benchmark();
            x.NonFilteredInclude();
            x.FilteredInclude_EFCore5();
            x.Projection_Manually();
            x.Projection_Automapper();
            x.IncludeFilter_EFCorePlus();

            BenchmarkRunner.Run<Benchmark>();
        }

        static void InitializeDbContext()
        {
            var context = new BlogDbContext();
            context.Database.EnsureCreated();

            if (context.Blogs.Any() == false)
            {
                var blogs = new List<Blog>();

                for (int i = 0; i < 100; i++)
                {
                    var blog = new Blog
                    {
                        Name = Guid.NewGuid().ToString()
                    };

                    for (int j = 0; j < 100; j++)
                    {
                        blog.Posts.Add(new Post
                        {
                            Title = Guid.NewGuid().ToString(),
                            Description = "Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description Description "
                        });
                    }

                    blogs.Add(blog);
                }

                blogs[99].Posts.ElementAt(99).Title = "Cheese";

                context.Blogs.AddRange(blogs);
                context.SaveChanges();
            }
        }
    }
}
