using BusinessLayer.Interfaces.Bogus;
using DataLayer.Interfaces.Bogus;

namespace BusinessLayer.Services.Bogus
{
    public class BogusGenerationSrv : IBogusGenerationSrv
    {
        private readonly IBogusGenerationRepo _bogusGenerationRepo;
        
        public BogusGenerationSrv(IBogusGenerationRepo bogusGenerationRepo)
        {
            _bogusGenerationRepo = bogusGenerationRepo;
        }

        public Task CreateAdminAsync()
            => _bogusGenerationRepo.CreateAdminAsync();

        public Task CreateMainDataAsync()
            => _bogusGenerationRepo.CreateMainDataAsync();
    }
}
