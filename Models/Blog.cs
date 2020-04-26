using System.Collections.Generic;

namespace EFCore_Include_Filtering_Benchmark
{
    public class Blog
    {
        public Blog()
        {
            Posts = new List<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
    }
}
