USE [LoggingWFAgentDB]
GO

/****** Object: Table [dbo].[BATCH_LOG] Script Date: 3/10/2020 10:09:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BATCH_LOG] (
    [ID]                    INT           IDENTITY (1, 1) NOT NULL,
    [CURRENT_MODULE_NAME]   VARCHAR (100) NULL,
    [BATCH_CLASS_NAME]      VARCHAR (32)  NULL,
    [BATCH_NAME]            VARCHAR (122) NULL,
    [CREATION_DATE]         DATETIME2 (7) NULL,
    [NUMBER_OF_LOOSE_PAGES] INT           NULL,
    [REGISTER_DATE]         DATETIME2 (7) NOT NULL
);

CREATE TABLE [dbo].[DOCUMENT_LOG] (
    [ID]                  INT           IDENTITY (1, 1) NOT NULL,
    [BATCH_ID]            INT           NULL,
    [DOCUMENT_CLASS_NAME] VARCHAR (32)  NULL,
    [REGISTER_DATE]       DATETIME2 (7) NOT NULL
);

CREATE TABLE [dbo].[FIELD_LOG] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [DOCUMENT_ID]       INT           NULL,
    [INDEX_FIELD_NAME]  VARCHAR (32)  NULL,
    [INDEX_FIELD_VALUE] VARCHAR (MAX) NULL,
    [REGISTER_DATE]     DATETIME2 (7) NOT NULL
);
