﻿namespace ItsyBitseList.Core.Interfaces.Persistence
{
    public interface IAsyncRepository<T> where T : IRootEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
