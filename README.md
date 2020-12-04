### A simple example Workflow Agent for Kofax Capture
This workflow collects detail information of a batch and sends them to a web service where that information is logged into a database

Information from workflow is stored in 3 tables (Refer [Database script](Scripts/DB Scripts.sql))

    - BATCH_LOG     Store batch information
    - DOCUMENT_LOG  Store document information
    - FIELD_LOG     Store field information

The workflow is divided into 3 parts as below
1. **Project LoggingWFAgent**  (Class library VB.NET)

   Represents the workflow and must be registered to KC, it has an entry point of the workflow where we put main logging logic
2. **Project LoggingWFAgentConfiguration** (Class library VB.NET)

   This is where the logging workflow can be configured.
   There will be 2 options for the workflow
     - *service endpoint*:    A service that the workflow connects to
     - *document threshold*:  Number of document records that the workflow sends to the service at once
                            By increasing the threshold we can reduce number of requests to the service and this will help improve performance in case of there is a lot of logging records
3. **LoggingWFAgentService** (WCF Application C#)

   A service that receives data from the workflow and in turn logs that data to a database

There is another Unit test project **LoggingWFAgentServiceTest** which contains testing content for LoggingWFAgentService

In order to add the workflow to KC, we use an [install script](Scripts/LoggingWFAgentSetup.aex)
