namespace BusinessLayer.Interfaces.Bogus
{
    public interface IBogusGenerationSrv
    {
        Task CreateAdminAsync();
        Task CreateMainDataAsync();
    }
}
