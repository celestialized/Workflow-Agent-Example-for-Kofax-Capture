using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LoggingWFAgentService
{
	/// <summary>
	/// Logging service implementation
	/// </summary>
	public class LoggingService : ILoggingService
	{
		/// <summary>
		/// Connection string name in Web.config
		/// </summary>
		private const string c_strConnectionStringName = "LoggingWFAgentDB";

		public int LogBatchInfo(BatchInfo oBatchInfo)
		{
			if (oBatchInfo == null)
			{
				return 0;
			}

			// Execute batch insert command
			return (int)ExecuteCommand(BuildBatchInsertCommand(), CreateBatchInsertParameters(oBatchInfo));
		}

		public void LogDocumentInfo(List<DocumentInfo> oDocumentInfoList, int nBatchId)
		{
			if (oDocumentInfoList == null)
			{
				return;
			}

			foreach (var oDocumentInfo in oDocumentInfoList)
			{
				// For each document, execute document insert command
				int nDocumentId = (int)ExecuteCommand(BuildDocumentInsertCommand(), CreateDocumentInsertParameters(oDocumentInfo, nBatchId));


				if (oDocumentInfo.FieldInfoList == null)
				{
					continue;
				}

				foreach (var oFieldInfo in oDocumentInfo.FieldInfoList)
				{
					// For each field, execute field insert command
					ExecuteCommand(BuildFieldInsertCommand(), CreateFieldInsertParameters(oFieldInfo, nDocumentId));
				}
			}
		}

		/// <summary>
		/// Execute a command with command text and parameters
		/// </summary>
		/// <param name="strCommandText">Command text</param>
		/// <param name="arrParameters">Parameters</param>
		/// <returns>First value in the result set or null if the result set is empty</returns>
		private object ExecuteCommand(string strCommandText, SqlParameter[] arrParameters)
		{
			using (var oConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[c_strConnectionStringName].ConnectionString))
			{
				using (var oCommand = new SqlCommand(strCommandText, oConnection))
				{
					oCommand.Parameters.AddRange(arrParameters);

					try
					{
						// Open connection for the command to execute
						oConnection.Open();

						return oCommand.ExecuteScalar();
					}
					finally
					{
						// Close connection after executing commands
						oConnection.Close();
					}
				}
			}
		}

		/// <summary>
		/// Create insert command for batch information
		/// </summary>
		/// <returns>an insert command to batch log table</returns>
		private string BuildBatchInsertCommand()
		{
			var oBuilder = new StringBuilder();
			oBuilder.AppendLine("INSERT INTO BATCH_LOG (");
			oBuilder.AppendLine("    CURRENT_MODULE_NAME");
			oBuilder.AppendLine("    ,BATCH_CLASS_NAME");
			oBuilder.AppendLine("    ,BATCH_NAME");
			oBuilder.AppendLine("    ,CREATION_DATE");
			oBuilder.AppendLine("    ,NUMBER_OF_LOOSE_PAGES");
			oBuilder.AppendLine("    ,REGISTER_DATE");
			oBuilder.AppendLine(") OUTPUT Inserted.ID VALUES (");
			oBuilder.AppendLine("    @CurrentModuleName");
			oBuilder.AppendLine("    ,@BatchClassName");
			oBuilder.AppendLine("    ,@BatchName");
			oBuilder.AppendLine("    ,@CreationDate");
			oBuilder.AppendLine("    ,@NumberOfLoosePages");
			oBuilder.AppendLine("    ,GETDATE()");
			oBuilder.AppendLine(")");
			return oBuilder.ToString();
		}

		/// <summary>
		/// Create an array of parameters for batch insert command
		/// </summary>
		/// <param name="oBatchInfo">Batch information</param>
		/// <returns>An array of command parameters</returns>
		private SqlParameter[] CreateBatchInsertParameters(BatchInfo oBatchInfo)
		{
			var oCreationDateParameter = new SqlParameter("@CreationDate", SqlDbType.DateTime2);
			oCreationDateParameter.Value = oBatchInfo.CreationDate;

			return new SqlParameter[]
			{
				new SqlParameter("@CurrentModuleName", oBatchInfo.CurrentModuleName),
				new SqlParameter("@BatchClassName", oBatchInfo.BatchClassName),
				new SqlParameter("@BatchName", oBatchInfo.BatchName),
				oCreationDateParameter,
				new SqlParameter("@NumberOfLoosePages", oBatchInfo.NumberOfLoosePages),
			};
		}

		/// <summary>
		/// Create insert command for document information
		/// </summary>
		/// <returns>an insert command to document log table</returns>
		private string BuildDocumentInsertCommand()
		{
			var oBuilder = new StringBuilder();
			oBuilder.AppendLine("INSERT INTO DOCUMENT_LOG (");
			oBuilder.AppendLine("    BATCH_ID");
			oBuilder.AppendLine("    ,FORM_TYPE_NAME");
			oBuilder.AppendLine("    ,REGISTER_DATE");
			oBuilder.AppendLine(") OUTPUT Inserted.ID VALUES (");
			oBuilder.AppendLine("    @BatchId");
			oBuilder.AppendLine("    ,@FormTypeName");
			oBuilder.AppendLine("    ,GETDATE()");
			oBuilder.AppendLine(")");
			return oBuilder.ToString();
		}

		/// <summary>
		/// Create an array of parameters for document insert command
		/// </summary>
		/// <param name="oDocumentInfo">Document information</param>
		/// <param name="batchId">Batch Id</param>
		/// <returns>An array of command parameters</returns>
		private SqlParameter[] CreateDocumentInsertParameters(DocumentInfo oDocumentInfo, int batchId)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@BatchId", batchId),
				new SqlParameter("@FormTypeName", oDocumentInfo.FormTypeName)
			};
		}

		/// <summary>
		/// Create insert command for field information
		/// </summary>
		/// <returns>an insert command to field log table</returns>
		private string BuildFieldInsertCommand()
		{
			var oBuilder = new StringBuilder();
			oBuilder.AppendLine("INSERT INTO FIELD_LOG (");
			oBuilder.AppendLine("    DOCUMENT_ID");
			oBuilder.AppendLine("    ,INDEX_FIELD_NAME");
			oBuilder.AppendLine("    ,INDEX_FIELD_VALUE");
			oBuilder.AppendLine("    ,REGISTER_DATE");
			oBuilder.AppendLine(") VALUES (");
			oBuilder.AppendLine("    @DocumentId");
			oBuilder.AppendLine("    ,@IndexFieldName");
			oBuilder.AppendLine("    ,@IndexFieldValue");
			oBuilder.AppendLine("    ,GETDATE()");
			oBuilder.AppendLine(")");
			return oBuilder.ToString();
		}

		/// <summary>
		/// Create an array of parameters for field insert command
		/// </summary>
		/// <param name="oFieldInfo">Field information</param>
		/// <param name="documentId">Document Id</param>
		/// <returns>An array of command parameters</returns>
		private SqlParameter[] CreateFieldInsertParameters(FieldInfo oFieldInfo, int documentId)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@DocumentId", documentId),
				new SqlParameter("@IndexFieldName", oFieldInfo.IndexFieldName),
				new SqlParameter("@IndexFieldValue", oFieldInfo.IndexFieldValue)
			};
		}
	}
}
