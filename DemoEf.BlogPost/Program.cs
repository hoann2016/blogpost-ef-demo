using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
#region Models
class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<Post> Posts { get; set; }

}
class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }

}
class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1438;Database=BlogDb;User Id=sa;Password=Admin@123;Trust Server Certificate=True");

    }
}

#endregion
class Program
{
    static void Main()
    {
        using (var db = new BloggingContext())
        {
            // ensure the database is created
            db.Database.EnsureCreated();

            Console.WriteLine("Inserting a new blog");
            db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet", Title = "ADO.NET Blog", Content = "This is ADO.NET Blog" });
            db.SaveChanges();

            // Read
            Console.WriteLine("Querying for a blog");
            var blog = db.Blogs
                .OrderBy(b => b.BlogId)
                .First();

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(blog));


            // Update
            Console.WriteLine("Updating the blog and adding a post");
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.Posts = new List<Post>();
            blog.Posts.Add(
                new Post
                {
                    Title = "Hello World",
                    Content = "I wrote an app using EF Core!"
                });
            db.SaveChanges();

            // Searching 
            Console.WriteLine("Searching for a blog");
            var blogSearch = db.Blogs
                .Include(b => b.Posts)
                .AsNoTracking()
                .Where(b => b.Url.Contains("dotnet")).ToList();
            blogSearch.ToList().ForEach(b=>{
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(b.Url));    

            });

            // Delete
            Console.WriteLine("Delete the blog");
            db.Remove(blog);
            db.SaveChanges();
        }
    }
}
