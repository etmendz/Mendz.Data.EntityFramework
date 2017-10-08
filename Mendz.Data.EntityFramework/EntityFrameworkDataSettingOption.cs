namespace Mendz.Data.EntityFramework
{
    /// <summary>
    /// Provides the data setting options for DbContext instance access.
    /// </summary>
    public static class EntityFrameworkDataSettingOption
    {
        /// <summary>
        /// Gets or sets the DbContext instance connection string name.
        /// </summary>
        public static string Name { get; set;  } = "EntityFrameworkConnectionString";
    }
}
