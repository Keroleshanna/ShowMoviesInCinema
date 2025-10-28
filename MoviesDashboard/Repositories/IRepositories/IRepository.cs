﻿using System.Linq.Expressions;

namespace MoviesDashboard.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        void Update(T entity);
        void Delete(T entity);

        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);
        Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
