using CleanArchTemplate.Core.Entities.Base;
using CleanArchTemplate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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

		public void Insert(T model)
		{
			//Verifica se existe algum item
			if (model == null)
				return;

			//Define os valores padrões do item
			_dbContext.Set<T>().Add(model);
			_dbContext.Entry(model).State = EntityState.Added;

			_dbContext.SaveChanges();
		}

		public void Insert(IEnumerable<T> models)
		{
			//Verifica se existe algum item
			if (models == null || !models.Any())
				return;

			foreach
		}

		public void Atualizar(T model)
		{
			throw new NotImplementedException();
		}

		public void Atualizar(IEnumerable<T> models)
		{
			throw new NotImplementedException();
		}

		public void Excluir(T model)
		{
			throw new NotImplementedException();
		}

		public void Excluir(IEnumerable<T> models)
		{
			throw new NotImplementedException();
		}

		public void Excluir<TProp>(TProp key)
		{
			throw new NotImplementedException();
		}

		public void Excluir<TProp>(List<TProp> keys)
		{
			throw new NotImplementedException();
		}
	}
}