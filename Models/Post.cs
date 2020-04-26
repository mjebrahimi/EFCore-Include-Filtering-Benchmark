using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore_Include_Filtering_Benchmark
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; }
    }
}
