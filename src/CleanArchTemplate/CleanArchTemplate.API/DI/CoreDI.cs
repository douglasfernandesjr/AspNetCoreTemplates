using CleanArchTemplate.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CleanArchTemplate.API.DI
{
	public class CoreDI
	{
		public static void Config(IServiceCollection servicesContainer)
		{
			ConfigBusinessService(servicesContainer);
		}

		private static void ConfigBusinessService(IServiceCollection servicesContainer)
		{
			IEnumerable<Type> allTypes = typeof(IBusinessService).Assembly.GetExportedTypes();

			var services = allTypes.Where(type =>
							type.GetInterfaces().Contains(typeof(IBusinessService))
							&& !type.IsAbstract && !type.IsInterface
						);

			foreach (Type service in services)
			{
				servicesContainer.AddTransient(service);

			}
		}
	}
}
