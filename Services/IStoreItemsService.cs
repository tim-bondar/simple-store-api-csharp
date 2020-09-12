using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using SimpleStore.DB;
using SimpleStore.Models;

namespace SimpleStore.Services
{
    public interface IStoreItemsService
    {
        Task<IEnumerable<StoreItem>> GetAllItems();
        Task<StoreItem> Get(Guid id);
        Task<StoreItem> Update(Guid id, StoreItemModel model);
        Task<StoreItem> Create(StoreItemModel model);
        Task Delete(Guid id);
    }
}
