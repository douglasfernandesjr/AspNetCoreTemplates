using System;

namespace CleanArchTemplate.Core.Interfaces
{
	public interface IEntityAudit : IEntity
	{
		///<summary>Login do usuário que efetuou a inclusao </summary>
		 string LoginInclusao { get; set; }

		///<summary>Data de inclusão do registro </summary>
		 DateTime DataHoraInclusao { get; set; }

		///<summary>Login do usuário que efetuou a última alteração </summary>
		 string LoginAlteracao { get; set; }

		///<summary>Data da última alteração do registro </summary>
		 DateTime? DataHoraAlteracao { get; set; }

		/// <summary>
		/// Indica se o registro foi excluido logicamente
		/// </summary>
		 bool Excluido { get; set; }
	}
}