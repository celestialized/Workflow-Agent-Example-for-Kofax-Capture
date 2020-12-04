using System.Runtime.Serialization;

namespace LoggingWFAgentService
{
	/// <summary>
	/// Represents field information
	/// </summary>
	[DataContract]
	public class FieldInfo
	{
		/// <summary>
		/// Index field name
		/// </summary>
		[DataMember]
		public string IndexFieldName { get; set; }

		/// <summary>
		/// Index field value
		/// </summary>
		[DataMember]
		public string IndexFieldValue { get; set; }
	}
}