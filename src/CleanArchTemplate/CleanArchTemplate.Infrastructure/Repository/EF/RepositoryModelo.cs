using System.Security.Principal;
using CleanArchTemplate.Core.Entities;
using CleanArchTemplate.Core.Interfaces;
using CleanArchTemplate.Infrastructure.Repository.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace CleanArchTemplate.Infrastructure.Repository.EF
{
	public class RepositoryModelo : EFRepositoryAudit<Modelo>, IRepositorioModelo
	{
		public RepositoryModelo(IPrincipal principal, DbContext db) : base(principal, db)
		{
		}
	}
}
