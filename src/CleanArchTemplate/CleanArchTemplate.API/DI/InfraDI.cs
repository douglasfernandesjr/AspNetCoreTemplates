using CleanArchTemplate.Core.Interfaces.Data;
using CleanArchTemplate.Core.Interfaces.DataAccess;
using CleanArchTemplate.Infrastructure.Repository.EF.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchTemplate.API.DI
{
	public class InfraDI
	{
		public static void Config(IServiceCollection servicesContainer)
		{
			ConfigEFRepository(servicesContainer);
		}

		private static void ConfigEFRepository(IServiceCollection servicesContainer)
		{
			var infraAssembly = typeof(EFRepository<>).Assembly;
			var respositoryBase = typeof(IRepository<>);

			//Busca todos os repositórios implementados e as interfaces customizadas.
			//Estes são os repositório implementados pelos devs
			var customRepositories =
				from type in infraAssembly.GetExportedTypes()
				where type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == respositoryBase)
				&& type.IsClass && !type.IsAbstract && !type.IsGenericType
				select new { Service = type.GetInterfaces(), Implementation = type };

			//Efetua o registro das implementações para resolver por
			//as requisições de (IRepository<T> e IRepositoryExemplo)
			foreach (var reg in customRepositories)
			{
				foreach (var service in reg.Service)
				{
					servicesContainer.AddScoped(service, reg.Implementation);
				}
			}


			//Busca o assembly do modelo
			var dbModelAssembly = typeof(IEntity).Assembly;
			//Busca lista de classe que são entidade dos banco

			var dbModelListAll = dbModelAssembly.GetExportedTypes()
				.Where(type => type.GetInterfaces().Contains(typeof(IEntity)) && type.IsClass && !type.IsAbstract);

			var dbModelList = new List<Type>();
			var dbAuditModelList = new List<Type>();

			//Separa em modelo IEntityAudit e IEntity
			foreach (Type type in dbModelListAll)
			{
				if (type.GetInterfaces().Contains(typeof(IEntityAudit)))
					dbAuditModelList.Add(type);
				else
					dbModelList.Add(type);
			}


			//Implementações base para repositórios não customizados

			var implementationEFRepository = typeof(EFRepository<>);
			var implementationEFAuditRepository = typeof(EFRepositoryAudit<>);

			//Busca todas as classes da Modelo que ainda não foram registradas em repositório para serem registradas de forma genérica.
			//Isso é feito para que não seja necessário criar um repositório, caso não existam métodos customizados
			var modelAuditRegistrations = from type in dbAuditModelList
											  // Remove as implementações existentes de Repositórios especificos
										  where !customRepositories.ToList()
										  .Exists(i => i.Service.Any(s => s.GenericTypeArguments.FirstOrDefault() == type))
										  select new
										  {
											  Service = respositoryBase.MakeGenericType(type),
											  Implementation = implementationEFAuditRepository.MakeGenericType(type)

										  };

			var modelRegistrations = from type in dbModelList
										 // Remove as implementações existentes de Repositórios especificos
									 where !customRepositories.ToList()
										  .Exists(i => i.Service.Any(s => s.GenericTypeArguments.FirstOrDefault() == type))
									 select new
									 {
										 Service = respositoryBase.MakeGenericType(type),
										 Implementation = implementationEFRepository.MakeGenericType(type)

									 };
			foreach (var reg in modelAuditRegistrations)
				servicesContainer.AddScoped(reg.Service, reg.Implementation);
			foreach (var reg in modelRegistrations)
				servicesContainer.AddScoped(reg.Service, reg.Implementation);
		}
	}
}
