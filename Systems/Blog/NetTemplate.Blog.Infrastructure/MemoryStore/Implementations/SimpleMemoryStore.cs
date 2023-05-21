﻿using NetTemplate.Blog.Infrastructure.MemoryStore.Interfaces;

namespace NetTemplate.Blog.Infrastructure.MemoryStore.Implementations
{
    public class SimpleMemoryStore : IMemoryStore
    {
        public Task<T> HashGet<T>(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T[]> HashGetAll<T>(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> KeyExists(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveHash(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
