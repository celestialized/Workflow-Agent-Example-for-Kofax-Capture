using LoggingWFAgentService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LoggingWFAgentServiceTest
{
	/// <summary>
	/// Test class for LoggingService
	/// </summary>
	[TestClass]
	public class LoggingServiceTest
	{
		/// <summary>
		/// Batch log table name
		/// </summary>
		private const string cm_strBATCH_LOG_TABLE_NAME = "BATCH_LOG";

		/// <summary>
		/// Document log table name
		/// </summary>
		private const string cm_strDOCUMENT_LOG_TABLE_NAME = "DOCUMENT_LOG";

		/// <summary>
		/// Field log table name
		/// </summary>
		public const string cm_strFIELD_LOG_TABLE_NAME = "FIELD_LOG";

		/// <summary>
		/// Connection string name in App.config
		/// </summary>
		private const string cm_strConnectionStringName = "LoggingWFAgentDB";

		/// <summary>
		/// Pass null to batch logging service
		/// </summary>
		[TestMethod]
		public void TestLogBatchInfo1()
		{
			TruncateTable(cm_strBATCH_LOG_TABLE_NAME);

			LoggingService oLoggingService = new LoggingService();
			Assert.AreEqual(0, oLoggingService.LogBatchInfo(null));

			var oTable = GetTable(cm_strBATCH_LOG_TABLE_NAME);
			Assert.AreEqual(0, oTable.Rows.Count);
		}

		/// <summary>
		/// Pass an instance to batch logging service
		/// </summary>
		[TestMethod]
		public void TestLogBatchInfo2()
		{
			TruncateTable(cm_strBATCH_LOG_TABLE_NAME);

			var oBatchInfo = new BatchInfo()
			{
				BatchClassName = "Personal info batch class",
				BatchName = "S4NACE8:Sess 3_3/6/2020_1:56:48 PM",
				CreationDate = new DateTime(2020, 3, 6, 12, 15, 1, 1),
				CurrentModuleName = "fp.exe",
				NumberOfLoosePages = 9
			};

			LoggingService oLoggingService = new LoggingService();
			Assert.IsTrue(oLoggingService.LogBatchInfo(oBatchInfo) > 0);

			var oTable = GetTable(cm_strBATCH_LOG_TABLE_NAME);
			AssertBatchDataTableEqual(new List<BatchInfo> { oBatchInfo }, oTable);
		}

		/// <summary>
		/// Pass null to document logging service
		/// </summary>
		[TestMethod]
		public void TestLogDocumentInfo1()
		{
			TruncateTable(cm_strDOCUMENT_LOG_TABLE_NAME);
			TruncateTable(cm_strFIELD_LOG_TABLE_NAME);

			List<DocumentInfo> oDocumentInfoList = null;
			LoggingService oLoggingService = new LoggingService();
			oLoggingService.LogDocumentInfo(oDocumentInfoList, 1);

			var oDocumentLogTable = GetTable(cm_strDOCUMENT_LOG_TABLE_NAME);
			var oFieldLogTable = GetTable(cm_strFIELD_LOG_TABLE_NAME);
			AssertDocumentDataTableEqual(oDocumentInfoList, oDocumentLogTable, oFieldLogTable);
		}

		/// <summary>
		/// Document list has only 1 document and this document does not have any fields
		/// </summary>
		[TestMethod]
		public void TestLogDocumentInfo2()
		{
			TruncateTable(cm_strDOCUMENT_LOG_TABLE_NAME);
			TruncateTable(cm_strFIELD_LOG_TABLE_NAME);

			var oDocumentInfoList = new List<DocumentInfo>()
			{
				new DocumentInfo()
				{
					FormTypeName = "Nothwest Products Class",
					FieldInfoList = null
				}
			};
			LoggingService oLoggingService = new LoggingService();
			oLoggingService.LogDocumentInfo(oDocumentInfoList, 1);

			var oDocumentLogTable = GetTable(cm_strDOCUMENT_LOG_TABLE_NAME);
			var oFieldLogTable = GetTable(cm_strFIELD_LOG_TABLE_NAME);
			AssertDocumentDataTableEqual(oDocumentInfoList, oDocumentLogTable, oFieldLogTable);
		}

		/// <summary>
		/// Document list has only 1 document and this document has only 1 field
		/// </summary>
		[TestMethod]
		public void TestLogDocumentInfo3()
		{
			TruncateTable(cm_strDOCUMENT_LOG_TABLE_NAME);
			TruncateTable(cm_strFIELD_LOG_TABLE_NAME);

			var oDocumentInfoList = new List<DocumentInfo>()
			{
				new DocumentInfo()
				{
					FormTypeName = "Nothwest Products Class",
					FieldInfoList = new List<FieldInfo>()
					{
						new FieldInfo()
						{
							IndexFieldName = "Address",
							IndexFieldValue = "365 Planter Street"
						}
					}
				}
			};
			LoggingService oLoggingService = new LoggingService();
			oLoggingService.LogDocumentInfo(oDocumentInfoList, 1);

			var oDocumentLogTable = GetTable(cm_strDOCUMENT_LOG_TABLE_NAME);
			var oFieldLogTable = GetTable(cm_strFIELD_LOG_TABLE_NAME);
			AssertDocumentDataTableEqual(oDocumentInfoList, oDocumentLogTable, oFieldLogTable);
		}

		/// <summary>
		/// Document list has many documents and each document has may fields
		/// </summary>
		[TestMethod]
		public void TestLogDocumentInfo4()
		{
			TruncateTable(cm_strDOCUMENT_LOG_TABLE_NAME);
			TruncateTable(cm_strFIELD_LOG_TABLE_NAME);

			List<DocumentInfo> oDocumentInfoList = new List<DocumentInfo>()
			{
				new DocumentInfo()
				{
					FormTypeName = "Nothwest Products Class",
					FieldInfoList = new List<FieldInfo>()
					{
						new FieldInfo()
						{
							IndexFieldName = "Address",
							IndexFieldValue = "365 Planter Street"
						},
						new FieldInfo()
						{
							IndexFieldName = "Address",
							IndexFieldValue = "",
						},
						new FieldInfo()
						{
							IndexFieldName = "Zip",
							IndexFieldValue = "07326"
						}
					}
				},
				new DocumentInfo()
				{
					FormTypeName = "Tri-Spectrum Solutions",
					FieldInfoList = new List<FieldInfo>()
					{
						new FieldInfo()
						{
							IndexFieldName = "ShipToName",
							IndexFieldValue = "Kay Brummel"
						},
						new FieldInfo()
						{
							IndexFieldName = "State",
							IndexFieldValue = "NY",
						}
					}
				},
				new DocumentInfo()
				{
					FormTypeName = "Nothwest Products Class",
					FieldInfoList = new List<FieldInfo>()
					{
						new FieldInfo()
						{
							IndexFieldName = "ShipToName",
							IndexFieldValue = "Barb Harvey"
						},
						new FieldInfo()
						{
							IndexFieldName = "State",
							IndexFieldValue = "IL",
						},
						new FieldInfo()
						{
							IndexFieldName = "Name",
							IndexFieldValue = "Janet Harvey"
						}
					}
				},
			};

			LoggingService oLoggingService = new LoggingService();
			oLoggingService.LogDocumentInfo(oDocumentInfoList, 1);

			var oDocumentLogTable = GetTable(cm_strDOCUMENT_LOG_TABLE_NAME);
			var oFieldLogTable = GetTable(cm_strFIELD_LOG_TABLE_NAME);
			AssertDocumentDataTableEqual(oDocumentInfoList, oDocumentLogTable, oFieldLogTable);
		}

		/// <summary>
		/// Test to make sure batch info and batch log table have the same data
		/// </summary>
		/// <param name="arrBatchInfo">Batch information</param>
		/// <param name="oBatchLogTable">Batch log data</param>
		private void AssertBatchDataTableEqual(List<BatchInfo> arrBatchInfo, DataTable oBatchLogTable)
		{
			Assert.AreEqual(arrBatchInfo.Count, oBatchLogTable.Rows.Count);
			for (int i = 0; i < oBatchLogTable.Rows.Count; i++)
			{
				Assert.AreEqual(arrBatchInfo[i].BatchClassName, oBatchLogTable.Rows[i]["BATCH_CLASS_NAME"]);
				Assert.AreEqual(arrBatchInfo[i].BatchName, oBatchLogTable.Rows[i]["BATCH_NAME"]);
				Assert.AreEqual(arrBatchInfo[i].CreationDate, (DateTime)oBatchLogTable.Rows[i]["CREATION_DATE"]);
				Assert.AreEqual(arrBatchInfo[i].CurrentModuleName, oBatchLogTable.Rows[i]["CURRENT_MODULE_NAME"]);
				Assert.AreEqual(arrBatchInfo[i].NumberOfLoosePages, oBatchLogTable.Rows[i]["NUMBER_OF_LOOSE_PAGES"]);
				Assert.IsNotNull(oBatchLogTable.Rows[i]["REGISTER_DATE"]);
			}
		}

		/// <summary>
		/// Test to make sure document info, document log table and field log table have the same data
		/// </summary>
		/// <param name="oDocumentInfoList"></param>
		/// <param name="oDocumentLogTable"></param>
		/// <param name="oFieldLogTable"></param>
		private void AssertDocumentDataTableEqual(List<DocumentInfo> oDocumentInfoList, DataTable oDocumentLogTable, DataTable oFieldLogTable)
		{
			if (oDocumentInfoList == null)
			{
				Assert.AreEqual(0, oDocumentLogTable.Rows.Count);
				Assert.AreEqual(0, oFieldLogTable.Rows.Count);
				return;
			}

			Assert.AreEqual(oDocumentInfoList.Count, oDocumentLogTable.Rows.Count);
			for (int i = 0; i < oDocumentLogTable.Rows.Count; i++)
			{
				Assert.AreEqual(oDocumentInfoList[i].FormTypeName, oDocumentLogTable.Rows[i]["FORM_TYPE_NAME"]);
				Assert.IsNotNull(oDocumentLogTable.Rows[i]["REGISTER_DATE"]);

				// Get fields that belong to current document
				var oRelatedFieldList = oFieldLogTable.Rows.Cast<DataRow>().Where(r => (int)r["DOCUMENT_ID"] == (int)oDocumentLogTable.Rows[i]["ID"]).ToList();
				if (oDocumentInfoList[i].FieldInfoList == null)
				{
					Assert.AreEqual(0, oRelatedFieldList.Count);
				}
				else
				{
					Assert.AreEqual(oDocumentInfoList[i].FieldInfoList.Count, oRelatedFieldList.Count);
					for (int j = 0; j < oDocumentInfoList[i].FieldInfoList.Count; j++)
					{
						Assert.AreEqual(oDocumentInfoList[i].FieldInfoList[j].IndexFieldName, oRelatedFieldList[j]["INDEX_FIELD_NAME"]);
						Assert.AreEqual(oDocumentInfoList[i].FieldInfoList[j].IndexFieldValue, oRelatedFieldList[j]["INDEX_FIELD_VALUE"]);
						Assert.IsNotNull(oRelatedFieldList[j]["REGISTER_DATE"]);
					}
				}
			}
		}

		/// <summary>
		/// Get data of a table given its name
		/// </summary>
		/// <param name="strTableName">table name</param>
		/// <returns>a DataTable containing all data of the table</returns>
		private DataTable GetTable(string strTableName)
		{
			string strConnectionString = ConfigurationManager.ConnectionStrings[cm_strConnectionStringName].ConnectionString;
			using (var oDataAdapter = new SqlDataAdapter($"SELECT * FROM {strTableName}", strConnectionString))
			{
				DataTable oDataTable = new DataTable();
				oDataAdapter.Fill(oDataTable);
				return oDataTable;
			}
		}

		/// <summary>
		/// Truncate a table
		/// </summary>
		/// <param name="strTableName">table name</param>
		private void TruncateTable(string strTableName)
		{
			string strConnectionString = ConfigurationManager.ConnectionStrings[cm_strConnectionStringName].ConnectionString;
			using (var oConnection = new SqlConnection(strConnectionString))
			{
				using (var oCommand = new SqlCommand($"TRUNCATE TABLE {strTableName}", oConnection))
				{
					try
					{
						oConnection.Open();
						oCommand.ExecuteNonQuery();
					}
					finally
					{
						oConnection.Close();
					}
				}
			}
		}
	}
}
