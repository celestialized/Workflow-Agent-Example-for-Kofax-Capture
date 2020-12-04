using System;
using System.Runtime.Serialization;

namespace LoggingWFAgentService
{
	/// <summary>
	/// Represents batch information
	/// </summary>
	[DataContract]
	public class BatchInfo
	{
		/// <summary>
		/// Current module Name
		/// </summary>
		[DataMember]
		public string CurrentModuleName { get; set; }

		/// <summary>
		/// Batch class name
		/// </summary>
		[DataMember]
		public string BatchClassName { get; set; }

		/// <summary>
		/// Batch name
		/// </summary>
		[DataMember]
		public string BatchName { get; set; }

		/// <summary>
		/// Creation date
		/// </summary>
		[DataMember]
		public DateTime CreationDate { get; set; }

		/// <summary>
		/// Number of loose pages
		/// </summary>
		[DataMember]
		public int NumberOfLoosePages { get; set; }
	}
}