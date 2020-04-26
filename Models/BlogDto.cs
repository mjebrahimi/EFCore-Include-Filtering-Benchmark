using System.Collections.Generic;

namespace EFCore_Include_Filtering_Benchmark
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
