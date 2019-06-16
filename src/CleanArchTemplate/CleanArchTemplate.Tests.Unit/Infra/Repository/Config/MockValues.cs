using System;
using System.Security.Claims;
using System.Security.Principal;

namespace CleanArchTemplate.Tests.Unit.Infra.Repository.Config
{
	public static class MockValues
	{
		public const string NomeGenericoA = "Produto ABC";
		public const string NomeGenericoB = "Nome ABC";

		public const double ValorGenericoA = 123;
		public const double ValorGenericoB = 123456;

		public const string MockUserName = "MockUser";
		public const string MockUserId = "MockUser@email.com";
		public const string MockUserAuthorityType = "TOKEN";

		public static IPrincipal GetMockUser()
		{
			return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, MockUserName),
				new Claim(ClaimTypes.NameIdentifier, MockUserId),
			}, MockUserAuthorityType));
		}
	}
}
