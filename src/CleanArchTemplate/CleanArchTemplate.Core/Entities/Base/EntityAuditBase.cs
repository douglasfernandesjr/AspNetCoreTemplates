using CleanArchTemplate.Core.Interfaces;
using System;

namespace CleanArchTemplate.Core.Entities.Base
{
	/// <summary>
	/// Classe de exemplo para ser usada como entidade básica para definir campos padrões.
	/// </summary>
	public abstract class EntityAuditBase : EntityBase, IEntityAudit
	{
		///<summary>Login do usuário que efetuou a inclusao </summary>
		public string LoginInclusao { get; set; }

		///<summary>Data de inclusão do registro </summary>
		public DateTime DataHoraInclusao { get; set; }

		///<summary>Login do usuário que efetuou a última alteração </summary>
		public string LoginAlteracao { get; set; }

		///<summary>Data da última alteração do registro </summary>
		public DateTime? DataHoraAlteracao { get; set; }

		/// <summary>
		/// Indica se o registro foi excluido logicamente
		/// </summary>
		public bool Excluido { get; set; }
	}
}