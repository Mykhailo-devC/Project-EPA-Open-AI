using Epa.Engine.Models.Logic_Models;

namespace Epa.Engine.Repository
{
    public static class ServiceResolver
    {
        public delegate IRepository RepositoryResolver(RepositoryType type);
    }
}
