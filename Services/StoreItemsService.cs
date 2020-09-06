using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SimpleStore.DB;
using SimpleStore.Models;

namespace SimpleStore.Services
{
    public class StoreItemsService : IStoreItemsService
    {
        private readonly SimpleStoreContext _context;

        public StoreItemsService(SimpleStoreContext context) => _context = context;

        public async Task<IEnumerable<StoreItem>> GetAllItems()
        {
            return await _context.StoreItems.ToListAsync();
        }

        public async Task<StoreItem> Get(Guid id)
        {
            return await _context.StoreItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<StoreItem> Update(Guid id, JsonPatchDocument<AddStoreItem> model)
        {
            var item = await _context.StoreItems.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return null;

            // Preventing of unintentional override existing values
            var newItem = new AddStoreItem
            {
                Description = item.Description,
                Price = item.Price,
                Title = item.Title,
                IsAvailable = item.IsAvailable
            };

            model.ApplyTo(newItem);

            // Modify item in DB only through temp secure object to prevent unacceptable modifications
            item.Description = newItem.Description;
            item.Price = newItem.Price;
            item.Title = newItem.Title;
            item.IsAvailable = newItem.IsAvailable;

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<StoreItem> Create(AddStoreItem model)
        {
            var item = new StoreItem
            {
              Description = model.Description,
              Price = model.Price,
              Title = model.Title,
              IsAvailable = model.IsAvailable,
              Id = Guid.NewGuid()
            };

            await _context.StoreItems.AddAsync(item);

            return item;
        }

        public async Task Delete(Guid id)
        {
            var item = await _context.StoreItems.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
                _context.StoreItems.Remove(item);
        }
    }
}
