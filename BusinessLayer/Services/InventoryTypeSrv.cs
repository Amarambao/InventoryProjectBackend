using BusinessLayer.Interfaces;
using BusinessLayer.Services.Generic;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class InventoryTypeSrv : GenericIdAndNameService<InventoryTypeEntity>, IInventoryTypeSrv
    {
        public InventoryTypeSrv(IInventoryTypeRepo repo) : base(repo, name => new InventoryTypeEntity(name))
        {

        }
    }
}
