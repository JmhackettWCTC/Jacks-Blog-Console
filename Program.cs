using NLog;

string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

while (true)
{
    Console.Clear();
    Console.WriteLine("====================================");
    Console.WriteLine("    Welcome to the Blog Manager!");
    Console.WriteLine("====================================");
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1) Display all Blogs");
    Console.WriteLine("2) Add Blog");
    Console.WriteLine("3) Create Post");
    Console.WriteLine("4) Display Posts");
    Console.WriteLine("q) Exit");
    Console.Write("Your choice: ");

    var input = Console.ReadLine();

    if (input == "1")
    {
        Console.Clear();
        DisplayAllBlogs();
    }
    else if (input == "2")
    {
        Console.Clear();
        CreateBlog();
    }
    else if (input == "3")
    {
        Console.Clear();
        CreatePost();
    }
    else if (input == "4")
    {
        Console.Clear();
        DisplayPosts();
    }
    else if (input.Trim().ToLower() == "q")
    {
        Console.Clear();
        Console.WriteLine("Exiting the Blog Manager. Goodbye!");
        Thread.Sleep(2000);
        break;
    }
    else
    {
        Console.Clear();
        Console.WriteLine("Invalid option. Please try again.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

static void DisplayAllBlogs()
{
    // Display all Blogs from the database
    using var db = new DataContext();
    var query = db.Blogs.OrderBy(b => b.Name);

    Console.WriteLine("All blogs in the database:");
    foreach (var item in query)
    {
        Console.WriteLine(item.Name);
    }
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

static void CreateBlog()
{
    // Create and save a new Blog
    Console.Write("Enter a name for a new Blog: ");
    var name = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Blog name cannot be empty.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        return;
    }

    var blog = new Blog { Name = name };

    using var db = new DataContext();
    db.AddBlog(blog);
    logger.Info("Blog added - {name}", name);
    Console.WriteLine("Blog added successfully.");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

static void CreatePost()
{
    // Create and save a new Post associated with a Blog, displaying the list of Blogs to choose from
    using var db = new DataContext();
    var blogs = db.Blogs.OrderBy(b => b.Name).ToList();

    if (!blogs.Any())
    {
        Console.WriteLine("No blogs available. Please add a blog first.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("Select a Blog for the new Post:");
    for (int i = 0; i < blogs.Count; i++)
    {
        Console.WriteLine($"{i + 1}) {blogs[i].Name}");
    }

    Console.Write("Enter the number of the blog: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out int choice) || choice < 1 || choice > blogs.Count)
    {
        Console.WriteLine("Invalid selection.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        return;
    }

    var selectedBlog = blogs[choice - 1];

    Console.Write("Enter the post title: ");
    var title = Console.ReadLine();

    Console.Write("Enter the post content: ");
    var content = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
    {
        Console.WriteLine("Title and content cannot be empty.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        return;
    }

    var post = new Post { Title = title, Content = content, BlogId = selectedBlog.BlogId };

    db.Posts.Add(post);
    db.SaveChanges();

    logger.Info("Post added - {title} to blog {blog}", title, selectedBlog.Name);
    Console.WriteLine("Post added successfully.");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

static void DisplayPosts()
{
    // Display all Posts for a selected Blog
    using var db = new DataContext();
    var blogs = db.Blogs.OrderBy(b => b.Name).ToList();

    if (!blogs.Any())
    {
        Console.WriteLine("No blogs available.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("Select a Blog to display posts:");
    for (int i = 0; i < blogs.Count; i++)
    {
        Console.WriteLine($"{i + 1}) {blogs[i].Name}");
    }

    Console.Write("Enter the number of the blog: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out int choice) || choice < 1 || choice > blogs.Count)
    {
        Console.WriteLine("Invalid selection.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        return;
    }

    var selectedBlog = blogs[choice - 1];

    var posts = db.Posts.Where(p => p.BlogId == selectedBlog.BlogId).ToList();

    Console.WriteLine($"Posts for {selectedBlog.Name} ({posts.Count} posts):");
    if (!posts.Any())
    {
        Console.WriteLine("No posts found.");
    }
    else
    {
        foreach (var post in posts)
        {
            Console.WriteLine($"Blog: {selectedBlog.Name}");
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Content: {post.Content}");
            Console.WriteLine("---");
        }
    }
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

logger.Info("Program ended");
