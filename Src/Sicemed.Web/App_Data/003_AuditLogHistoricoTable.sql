CREATE TABLE [AuditLogHistorico](
	[Id] [uniqueidentifier] NOT NULL,
	[Entidad] [nvarchar](255) NULL,
	[EntidadId] [bigint] NULL,
	[Usuario] [nvarchar](255) NULL,
	[Accion] [nvarchar](255) NULL,
	[Fecha] [datetime] NULL,
	[EntidadAntes] [nvarchar](max) NULL,
	[EntidadDespues] [nvarchar](max) NULL
) 