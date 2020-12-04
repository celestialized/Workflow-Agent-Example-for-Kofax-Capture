using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LoggingWFAgentService
{
	/// <summary>
	/// Represents document information
	/// </summary>
	[DataContract]
	public class DocumentInfo
	{
		/// <summary>
		/// Form type name
		/// </summary>
		[DataMember]
		public string FormTypeName { get; set; }

		/// <summary>
		/// A list of field information
		/// </summary>
		[DataMember]
		public List<FieldInfo> FieldInfoList { get; set; }
	}
}