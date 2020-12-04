using System.Collections.Generic;
using System.ServiceModel;

namespace LoggingWFAgentService
{
	/// <summary>
	/// Interface of logging service which writes all data passed by a workflow to database
	/// </summary>
	[ServiceContract]
	public interface ILoggingService
	{
		/// <summary>
		/// Writes batch information to database
		/// </summary>
		/// <param name="oBatchInfo">Batch information</param>
		/// <returns>Batch Id</returns>
		[OperationContract]
		int LogBatchInfo(BatchInfo oBatchInfo);

		/// <summary>
		/// Writes a list of document information to database
		/// </summary>
		/// <param name="oDocumentInfoList">Document information</param>
		/// <param name="nBatchId">Batch Id</param>
		[OperationContract]
		void LogDocumentInfo(List<DocumentInfo> oDocumentInfoList, int nBatchId);
	}
}
