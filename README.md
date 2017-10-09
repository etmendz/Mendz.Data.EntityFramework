# Mendz.Data.EntityFramework
Provides Mendz.Data-aware classes and types for creating Entity Framework-compatible contexts and repositories. [Wiki](https://github.com/etmendz/Mendz.Data.EntityFramework/wiki)
## Namespace
Mendz.Data.EntityFramework
### Contents
Name | Description
---- | -----------
EntityFrameworkDataSettingOption | Provides the data setting options for DbContext instance access.
EntityFrameworkDbRepositoryBase | The base class for Entity Framework-compatible repositories.
#### EntityFrameworkDataSettingOption
EntityFrameworkDataSettingOption assumes that appsettings.json contains an entry/section for DataSettings.
```JSON
{
    "DataSettings": {
        "ConnectionStrings": {
            "EntityFrameworkConnectionString" : "connection string for entity framework DbContext instance"
        }
    }
}
```
In the application startup or initialization routine, the DataSettings should be loaded into DataSettingOptions as follows:
```C#
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DataSettingOptions.Initialize(Configuration.GetSection("DataSettings").Get<DataSettings>());
        }
```
Let's say for example that you have a DbContext class library project. 
To make it Mendz.Data-aware, just reference Mendz.Data and implement the OnConfiguring override as follows:
```C#
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DataSettingOptions.ConnectionStrings[EntityFrameworkDataSettingOption.Name]);
        }
```
This approach makes the DbContext instance semi-self-initializing.
It also removes the need for the DbContext instance to be registered in the application's Startup.ConfigureServices -- 
usually done via call to IServiceCollection.AddDbContext().
Thus, the application does not even need to be aware of the DbContext instance itself. 
#### EntityFrameworkDbRepositoryBase
The purpose of EntityFrameworkDbRepositoryBase is to let developers create repositories
that resemble how other Mendz.Data-aware repositories are coded. In a sense, this is an overkill bastardization of EF usage.
It is still recommended that you use EF directly in your applications.
Note that EF already implements the Unit of Work and Repository patterns internally.

A repository skeleton using EntityFrameworkDbRepositoryBase can start as follows:
```C#
    public class TestRepository : EntityFrameworkDbRepositoryBase<TestContext>
    {
        ...
    }
```
An advantage to using EntityFrameworkDbRepositoryBase to create repositories
is that it allows for consistency in coding repository calls in your application.
Mendz.Data-based repositories, regardless of what the actual data access technology is used,
allows for the same coding pattern to be applied in the application code.
For example, in an ASP.NET MVC application's controller code:
```C#
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index(int id)
        {
            using (TestRepository testRepository = new TestRepository())
            {
                return View(testRepository.Search<int, object>(id, null));
            }
        }
    }
```
Regardless if the repository is using ADO.Net, Dapper, Mendz.Data.SqlServer.SqlServerDbDataContext or EF DbContext, 
the controller code can look the same as shown above.
The application does not need to "know" what data access, source or target the repository uses.
All the application needs to know are the models and the repositories.
## NuGet It...
https://www.nuget.org/packages/Mendz.Data.EntityFramework/
