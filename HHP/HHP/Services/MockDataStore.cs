using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HHP.Models;
using HHP.Models.Page;

namespace HHP.Services
{
    public class MockDataStore : IDataStore<PageItem>
    {
        readonly List<PageItem> items;

        public MockDataStore()
        {
            items = new List<PageItem>()
            {
                new PageItem { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new PageItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new PageItem { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new PageItem { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new PageItem { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new PageItem { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddItemAsync(PageItem item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(PageItem item)
        {
            var oldItem = items.Where((PageItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((PageItem arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<PageItem> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<PageItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}