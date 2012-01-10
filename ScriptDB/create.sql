USE [SMS_DB]
GO
/****** Object:  Table [dbo].[Statut]    Script Date: 01/10/2012 17:04:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Statut](
	[idStatut] [int] IDENTITY(1,1) NOT NULL,
	[libelleStatut] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Statut] PRIMARY KEY CLUSTERED 
(
	[idStatut] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Encodage]    Script Date: 01/10/2012 17:04:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encodage](
	[idEncodage] [int] IDENTITY(1,1) NOT NULL,
	[libelleEncodage] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Encodage] PRIMARY KEY CLUSTERED 
(
	[idEncodage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 01/10/2012 17:04:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[idMessage] [int] IDENTITY(1,1) NOT NULL,
	[messageTexte] [nvarchar](max) NULL,
	[messagePDU] [nvarchar](max) NULL,
	[accuseReception] [int] NOT NULL,
	[noDestinataire] [nvarchar](50) NOT NULL,
	[noEmetteur] [nvarchar](50) NULL,
	[idEncodage] [int] NOT NULL,
	[idStatut] [int] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MessageRecu]    Script Date: 01/10/2012 17:04:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageRecu](
	[idMessage] [int] NOT NULL,
	[dateReception] [date] NOT NULL,
	[dateLecture] [date] NULL,
 CONSTRAINT [PK_MessageRecu] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MessageEnvoi]    Script Date: 01/10/2012 17:04:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageEnvoi](
	[idMessage] [int] NOT NULL,
	[dureeValidite] [date] NULL,
	[dateDemande] [date] NOT NULL,
	[dateEnvoi] [date] NULL,
 CONSTRAINT [PK_MessageEnvoi] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Message_Encodage]    Script Date: 01/10/2012 17:04:03 ******/
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Encodage] FOREIGN KEY([idEncodage])
REFERENCES [dbo].[Encodage] ([idEncodage])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Encodage]
GO
/****** Object:  ForeignKey [FK_Message_Statut]    Script Date: 01/10/2012 17:04:03 ******/
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Statut] FOREIGN KEY([idStatut])
REFERENCES [dbo].[Statut] ([idStatut])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Statut]
GO
/****** Object:  ForeignKey [FK_MessageEnvoi_Message]    Script Date: 01/10/2012 17:04:03 ******/
ALTER TABLE [dbo].[MessageEnvoi]  WITH CHECK ADD  CONSTRAINT [FK_MessageEnvoi_Message] FOREIGN KEY([idMessage])
REFERENCES [dbo].[Message] ([idMessage])
GO
ALTER TABLE [dbo].[MessageEnvoi] CHECK CONSTRAINT [FK_MessageEnvoi_Message]
GO
/****** Object:  ForeignKey [FK_MessageRecu_Message]    Script Date: 01/10/2012 17:04:03 ******/
ALTER TABLE [dbo].[MessageRecu]  WITH CHECK ADD  CONSTRAINT [FK_MessageRecu_Message] FOREIGN KEY([idMessage])
REFERENCES [dbo].[Message] ([idMessage])
GO
ALTER TABLE [dbo].[MessageRecu] CHECK CONSTRAINT [FK_MessageRecu_Message]
GO
