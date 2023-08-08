SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[Id] [uniqueidentifier] NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[Category] [nvarchar](255) NULL,
	[Title] [nvarchar](255) NULL,
	[Rating] [nvarchar](50) NULL,
	[ReviewsCount] [nvarchar](50) NULL,
	[Price] [nvarchar](50) NULL,
	[ShopName] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[RemoteId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Products] ADD  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
