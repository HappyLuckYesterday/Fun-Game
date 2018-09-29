﻿using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Abstract implementation of a repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : IRepository<T>
        where T : DbEntity
    {
        private readonly DbContext _context;

        /// <summary>
        /// Initializes the repository <see cref="DbContext"/>.
        /// </summary>
        /// <param name="context"></param>
        protected RepositoryBase(DbContext context)
        {
            this._context = context;
        }

        /// <inheritdoc />
        public T Create(T entity)
        {
            this._context.Set<T>().Add(entity);

            return entity;
        }

        /// <inheritdoc />
        public async Task<T> CreateAsync(T entity)
        {
            var trackedEntity = await this._context.Set<T>().AddAsync(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Update(T entity)
        {
            var trackedEntity = this._context.Set<T>().Update(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Delete(T entity)
        {
            var trackedEntity = this._context.Set<T>().Remove(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Get(int id) => this.Get(x => x.Id == id);

        /// <inheritdoc />
        public T Get(Func<T, bool> func)
        {
            return this.GetQueryable(this._context).FirstOrDefault(func);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll()
        {
            return this.GetQueryable(this._context).AsEnumerable();
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll(Func<T, bool> func)
        {
            return this.GetQueryable(this._context).Where(func).AsEnumerable();
        }

        /// <inheritdoc />
        public int Count()
        {
            return this.GetQueryable(this._context).Count();
        }

        /// <inheritdoc />
        public int Count(Func<T, bool> func)
        {
            return this.GetQueryable(this._context).Count(func);
        }

        /// <inheritdoc />
        protected virtual IQueryable<T> GetQueryable(DbContext context) => context.Set<T>().AsQueryable();
    }
}
