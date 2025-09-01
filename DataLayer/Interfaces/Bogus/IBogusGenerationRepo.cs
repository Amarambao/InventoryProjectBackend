namespace DataLayer.Interfaces.Bogus
{
    public interface IBogusGenerationRepo
    {
        Task CreateAdminAsync();
        Task CreateMainDataAsync();
    }
}
