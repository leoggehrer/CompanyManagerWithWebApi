using CompanyManager.Logic.Contracts;

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Factory class to create instances of IMusicStoreContext.
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// Creates an instance of IContext.
        /// </summary>
        /// <returns>An instance of IContext.</returns>
        public static IContext CreateContext()
        {
            var result = new CompanyContext();

            return result;
        }

#if DEBUG
        /// <summary>
        /// Creates the database for the CompanyContext.
        /// </summary>
        public static void CreateDatabase()
        {
            var context = new CompanyContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
#endif
    }
}
