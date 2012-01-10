USE [SMS_DB]
GO
/****** Object:  ForeignKey [FK_Message_Encodage]    Script Date: 01/10/2012 13:51:30 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_Encodage]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message] DROP CONSTRAINT [FK_Message_Encodage]
GO
/****** Object:  ForeignKey [FK_Message_Statut]    Script Date: 01/10/2012 13:51:30 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_Statut]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message] DROP CONSTRAINT [FK_Message_Statut]
GO
/****** Object:  ForeignKey [FK_MessageEnvoi_Message]    Script Date: 01/10/2012 13:51:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageEnvoi_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageEnvoi]'))
ALTER TABLE [dbo].[MessageEnvoi] DROP CONSTRAINT [FK_MessageEnvoi_Message]
GO
/****** Object:  ForeignKey [FK_MessageRecu_Message]    Script Date: 01/10/2012 13:51:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageRecu_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageRecu]'))
ALTER TABLE [dbo].[MessageRecu] DROP CONSTRAINT [FK_MessageRecu_Message]
GO
/****** Object:  Table [dbo].[MessageEnvoi]    Script Date: 01/10/2012 13:51:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageEnvoi_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageEnvoi]'))
ALTER TABLE [dbo].[MessageEnvoi] DROP CONSTRAINT [FK_MessageEnvoi_Message]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MessageEnvoi]') AND type in (N'U'))
DROP TABLE [dbo].[MessageEnvoi]
GO
/****** Object:  Table [dbo].[MessageRecu]    Script Date: 01/10/2012 13:51:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageRecu_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageRecu]'))
ALTER TABLE [dbo].[MessageRecu] DROP CONSTRAINT [FK_MessageRecu_Message]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MessageRecu]') AND type in (N'U'))
DROP TABLE [dbo].[MessageRecu]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 01/10/2012 13:51:30 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_Encodage]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message] DROP CONSTRAINT [FK_Message_Encodage]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_Statut]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message] DROP CONSTRAINT [FK_Message_Statut]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Message]') AND type in (N'U'))
DROP TABLE [dbo].[Message]
GO
/****** Object:  Table [dbo].[Encodage]    Script Date: 01/10/2012 13:51:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Encodage]') AND type in (N'U'))
DROP TABLE [dbo].[Encodage]
GO
/****** Object:  Table [dbo].[Statut]    Script Date: 01/10/2012 13:51:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Statut]') AND type in (N'U'))
DROP TABLE [dbo].[Statut]
GO
/****** Object:  Table [dbo].[Statut]    Script Date: 01/10/2012 13:51:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Statut]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Statut](
	[idStatut] [int] IDENTITY(1,1) NOT NULL,
	[libelleStatut] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Statut] PRIMARY KEY CLUSTERED 
(
	[idStatut] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Encodage]    Script Date: 01/10/2012 13:51:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Encodage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Encodage](
	[idEncodage] [int] IDENTITY(1,1) NOT NULL,
	[libelleEncodage] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Encodage] PRIMARY KEY CLUSTERED 
(
	[idEncodage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Message]    Script Date: 01/10/2012 13:51:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Message]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Message](
	[idMessage] [int] IDENTITY(1,1) NOT NULL,
	[messageTexte] [nvarchar](max) NULL,
	[messagePDU] [nvarchar](max) NULL,
	[accuseReception] [int] NOT NULL,
	[noDestinataire] [nvarchar](50) NOT NULL,
	[noEmetteur] [nvarchar](50) NOT NULL,
	[idEncodage] [int] NOT NULL,
	[idStatut] [int] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MessageRecu]    Script Date: 01/10/2012 13:51:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MessageRecu]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MessageRecu](
	[idMessage] [int] NOT NULL,
	[dateReception] [date] NOT NULL,
	[dateLecture] [date] NULL,
 CONSTRAINT [PK_MessageRecu] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MessageEnvoi]    Script Date: 01/10/2012 13:51:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MessageEnvoi]') AND type in (N'U'))
BEGIN
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
END
GO
/****** Object:  ForeignKey [FK_Message_Encodage]    Script Date: 01/10/2012 13:51:30 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_Encodage]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Encodage] FOREIGN KEY([idEncodage])
REFERENCES [dbo].[Encodage] ([idEncodage])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_Encodage]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Encodage]
GO
/****** Object:  ForeignKey [FK_Message_Statut]    Script Date: 01/10/2012 13:51:30 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_Statut]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Statut] FOREIGN KEY([idStatut])
REFERENCES [dbo].[Statut] ([idStatut])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Message_Statut]') AND parent_object_id = OBJECT_ID(N'[dbo].[Message]'))
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Statut]
GO
/****** Object:  ForeignKey [FK_MessageEnvoi_Message]    Script Date: 01/10/2012 13:51:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageEnvoi_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageEnvoi]'))
ALTER TABLE [dbo].[MessageEnvoi]  WITH CHECK ADD  CONSTRAINT [FK_MessageEnvoi_Message] FOREIGN KEY([idMessage])
REFERENCES [dbo].[Message] ([idMessage])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageEnvoi_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageEnvoi]'))
ALTER TABLE [dbo].[MessageEnvoi] CHECK CONSTRAINT [FK_MessageEnvoi_Message]
GO
/****** Object:  ForeignKey [FK_MessageRecu_Message]    Script Date: 01/10/2012 13:51:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageRecu_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageRecu]'))
ALTER TABLE [dbo].[MessageRecu]  WITH CHECK ADD  CONSTRAINT [FK_MessageRecu_Message] FOREIGN KEY([idMessage])
REFERENCES [dbo].[Message] ([idMessage])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MessageRecu_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[MessageRecu]'))
ALTER TABLE [dbo].[MessageRecu] CHECK CONSTRAINT [FK_MessageRecu_Message]
GO
