SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Files]
(
	[FileId]       bigint IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[FileSize]     bigint        NULL,
	[FileName]     nvarchar(500) NULL,
	[ContentType]  nvarchar(500) NULL,
	[RegisterDate] smalldatetime NULL
) ON [PRIMARY];
GO
