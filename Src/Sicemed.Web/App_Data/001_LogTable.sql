CREATE TABLE [dbo].[Log] (
    [Id] [int] IDENTITY (1, 1) NOT NULL,
    [Date] [datetime] NOT NULL,
    [Thread] [varchar] (255) NOT NULL,
    [UserId] [varchar] (255) NOT NULL,
    [UserIp] [varchar] (50) NOT NULL,
    [SessionId] [varchar] (255) NOT NULL,
    [Level] [varchar] (50) NOT NULL,
    [Logger] [varchar] (255) NOT NULL,
    [Message] [varchar] (MAX) NOT NULL,
    [Exception] [varchar] (MAX) NULL	
)