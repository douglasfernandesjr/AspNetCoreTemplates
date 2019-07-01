using CleanArchTemplate.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace CleanArchTemplate.Infrastructure.Repository.EF.Base
{
	/// <summary>
	/// Exemplo de repositório, considerando um entidade com campos padrões para auditoria.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EFAuditRepository<T> : EFRepository<T>
		where T : EntityAuditBase, new()
	{
		private IPrincipal _currentPrincipal;
		public EFAuditRepository(IPrincipal principal, DbContext db) : base(db)
		{
			_currentPrincipal = principal;
		}

		private string currentUserName()
		{
			return _currentPrincipal.Identity.Name;
		}

		private void UpdateAuditInfo(IEnumerable<T> models, EntityState state)
		{
			if (models != null && models.Any())
			{
				foreach (T model in models)
					UpdateAuditInfo(model, state);
			}
		}

		private void UpdateAuditInfo(T model, EntityState state)
		{
			if (model != null)
			{
				if (state == EntityState.Added)
				{
					model.LoginInclusao = currentUserName();
					model.DataHoraInclusao = DateTime.Now;
				}
				else if (state == EntityState.Modified || state == EntityState.Deleted)
				{
					model.LoginAlteracao = currentUserName();
					model.DataHoraAlteracao = DateTime.Now;

					if (state == EntityState.Deleted)
						model.Excluido = true;

				}
			}
		}

		public override void Insert(T model)
		{
			UpdateAuditInfo(model, EntityState.Added);
			base.Insert(model);
		}

		public override void Insert(IEnumerable<T> model)
		{
			UpdateAuditInfo(model, EntityState.Added);
			base.Insert(model);
		}

		public override void Update(T model)
		{
			UpdateAuditInfo(model, EntityState.Modified);
			base.Update(model);
		}

		public override void Update(IEnumerable<T> model)
		{
			UpdateAuditInfo(model, EntityState.Modified);
			base.Update(model);
		}

		public override void Delete(T model)
		{
			UpdateAuditInfo(model, EntityState.Deleted);
			base.Update(model);
		}

		public override void Delete(IEnumerable<T> model)
		{
			UpdateAuditInfo(model, EntityState.Deleted);
			base.Update(model);
		}
	}
}
