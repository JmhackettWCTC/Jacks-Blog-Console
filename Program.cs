using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

While (true)
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
    Console.Wait(2000);
    break;
  }else
  {
    Console.Clear();
    Console.WriteLine("Invalid option. Please try again.");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
  }
}

function DisplayAllBlogs() {
// Display all Blogs from the database
var query = db.Blogs.OrderBy(b => b.Name);

Console.WriteLine("All blogs in the database:");
foreach (var item in query)
{
  Console.WriteLine(item.Name);
}
}

function CreateBlog() {
// Create and save a new Blog
Console.Write("Enter a name for a new Blog: ");
var name = Console.ReadLine();

var blog = new Blog { Name = name };

var db = new DataContext();
db.AddBlog(blog);
logger.Info("Blog added - {name}", name);
}

function CreatePost() {
// Create and save a new Post
}

function DisplayPosts() {
// Display all Posts from the database
}


logger.Info("Program ended");
