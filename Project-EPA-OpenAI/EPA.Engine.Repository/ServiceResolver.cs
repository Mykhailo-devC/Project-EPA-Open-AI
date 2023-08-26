using EPA.Engine.Models.Logic_Models;

namespace EPA.Engine.Repository
{
    public static class ServiceResolver
    {
        public delegate IRepository RepositoryResolver(RepositoryType type);
    }
}
