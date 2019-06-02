using CleanArchTemplate.Core.Entities.Base;
using CleanArchTemplate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchTemplate.Infrastructure.Repository.EF.Base
{
	public class Repository<T> : IRepository<T>
	where T : EntityBase, new()
	{
		private DbContext _dbContext;

		public Repository(DbContext db)
		{
			_dbContext = db;
		}

		protected virtual void Set(T model, EntityState state)
		{
			if (state == EntityState.Added)
				_dbContext.Set<T>().Add(model);
			else if (state == EntityState.Modified)
				_dbContext.Set<T>().Update(model);
			else if (state == EntityState.Deleted)
				_dbContext.Set<T>().Remove(model);

		}

		protected virtual void Set(IEnumerable<T> models, EntityState state)
		{
			if (state == EntityState.Added)
				foreach (var model in models)
				{
					if (model != null)
						_dbContext.Set<T>().Add(model);
				}
			else if (state == EntityState.Modified)
				foreach (var model in models)
				{
					if (model != null)
						_dbContext.Set<T>().Update(model);
				}
			else if (state == EntityState.Deleted)
				foreach (var model in models)
				{
					if (model != null)
						_dbContext.Set<T>().Remove(model);
				}
		}

		protected virtual void SingleOperation(T model, EntityState state)
		{
			//Verifica se existe algum item
			if (model == null)
				return;

			_dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

			//Define os valores padrões do item
			Set(model, state);

			_dbContext.SaveChanges();
		}

		protected virtual void MultipleOperations(IEnumerable<T> models, EntityState state)
		{
			//Verifica se existe algum item
			if (models == null || !models.Any())
				return;

			_dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

			Set(models, state);

			_dbContext.SaveChanges();
		}

		public virtual void Insert(T model) => SingleOperation(model, EntityState.Added);

		public virtual void Insert(IEnumerable<T> models) => MultipleOperations(models, EntityState.Added);

		public virtual void Update(T model) => SingleOperation(model, EntityState.Modified);

		public virtual void Update(IEnumerable<T> models) => MultipleOperations(models, EntityState.Modified);

		public virtual void Delete(T model) => SingleOperation(model, EntityState.Deleted);

		public virtual void Delete(IEnumerable<T> models) => MultipleOperations(models, EntityState.Deleted);

		public virtual void Delete<TKeyProp>(TKeyProp key)
		{
			Delete(_dbContext.Set<T>().Find(key));
		}

		public virtual void Delete<TKeyProp>(IEnumerable<TKeyProp> keys)
		{
			var models = new List<T>();

			foreach (TKeyProp key in keys)
			{
				var tempModel = _dbContext.Set<T>().Find(key);
				if (tempModel != null)
					models.Add(_dbContext.Set<T>().Find(key));
			}

			Delete(models);
		}
	}
}