using Epa.Engine.Repository;

namespace EPA_WebAPI
{
    public static class ServiceResolver
    {
        public delegate IRepository RepositoryResolver(RepositoryType type);
    }
}
