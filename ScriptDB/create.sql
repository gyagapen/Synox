USE [SMS_DB]
GO
/****** Object:  Table [dbo].[Statut]    Script Date: 02/03/2012 15:01:25 ******/
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
SET IDENTITY_INSERT [dbo].[Statut] ON
INSERT [dbo].[Statut] ([idStatut], [libelleStatut]) VALUES (1, N'En attente')
INSERT [dbo].[Statut] ([idStatut], [libelleStatut]) VALUES (2, N'Envoye')
INSERT [dbo].[Statut] ([idStatut], [libelleStatut]) VALUES (3, N'Erreur')
INSERT [dbo].[Statut] ([idStatut], [libelleStatut]) VALUES (4, N'Accuse')
SET IDENTITY_INSERT [dbo].[Statut] OFF
/****** Object:  Table [dbo].[Encodage]    Script Date: 02/03/2012 15:01:25 ******/
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
SET IDENTITY_INSERT [dbo].[Encodage] ON
INSERT [dbo].[Encodage] ([idEncodage], [libelleEncodage]) VALUES (1, N'7bits')
INSERT [dbo].[Encodage] ([idEncodage], [libelleEncodage]) VALUES (2, N'8bits')
INSERT [dbo].[Encodage] ([idEncodage], [libelleEncodage]) VALUES (3, N'16bits')
INSERT [dbo].[Encodage] ([idEncodage], [libelleEncodage]) VALUES (4, N'PDU')
SET IDENTITY_INSERT [dbo].[Encodage] OFF
/****** Object:  Table [dbo].[Message]    Script Date: 02/03/2012 15:01:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[idMessage] [int] IDENTITY(1,1) NOT NULL,
	[messageTexte] [nvarchar](max) NULL,
	[messagePDU] [nvarchar](max) NULL,
	[accuseReception] [int] NULL,
	[noDestinataire] [nvarchar](50) NULL,
	[noEmetteur] [nvarchar](50) NULL,
	[idEncodage] [int] NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Message] ON
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (384, N'SPAM !!!', NULL, 1, N'0675610118', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (385, N'SPAM !!!', NULL, 1, N'0675610118', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (386, N'SPAM !!!', NULL, 1, N'0675610118', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (387, N'	a roule?', NULL, 0, N'+33604655154', N'+33675610118', 4)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (389, N'Test accusation', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (390, N'Test accusation', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (391, N'Alors ?', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (393, N'Je l''ai vu et je t''ai même répondu !!', NULL, 1, N'0622031216', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (394, N'Casses toi !', NULL, 0, N'+33604655154', N'+33625123338', 4)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (399, N'pom pom pom', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (400, N'pom pom pom', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (401, N'pom pom pom', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (402, N'test', NULL, 0, N'33675610118', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (403, N'pom pom pom', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (404, N'pom pom pom', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (405, N'pom pom pom', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (406, N'pom pom pom', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (407, N'pom pom pom', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (408, N'Je peux repondre directement maintenant !!!!', NULL, 1, N'33625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (409, NULL, N'dsgdsgdsgdgsdgdgsdgssgddgsdgsdgs', NULL, NULL, N'+33604655154', 4)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (410, NULL, N'bdazhndio opj  jdknzf', NULL, NULL, N'+33604655154', 4)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (411, NULL, N'bug !!!', NULL, NULL, NULL, 4)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (412, N'spaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (413, N'spaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', NULL, 0, N'+33604655154', N'+33625123338', 4)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (414, N'jj jipv jvzp po  à i ào iv   )v*µ$p o  vr p rjiv i io po jop oprjop', NULL, 1, N'0625123338', N'+33604655154', 3)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (415, N'jj jipv jvzp po  à i ào iv   )v*µ$p o  vr p rjiv i io po jop oprjop', NULL, 1, N'0625123338', N'+33604655154', 3)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (417, N'$^ù$^^^$ùâ* % § *$ +==<>', NULL, 1, N'0625123338', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (418, N'drtdhghgdghgh', NULL, 1, N'0675610118', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (419, N'dftetghygghgh', NULL, 1, N'0675610118', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (422, N'test accuse', NULL, 1, N'0675610118', N'+33604655154', 1)
INSERT [dbo].[Message] ([idMessage], [messageTexte], [messagePDU], [accuseReception], [noDestinataire], [noEmetteur], [idEncodage]) VALUES (423, N'yre', NULL, 1, N'0675610118', N'+33604655154', 1)
SET IDENTITY_INSERT [dbo].[Message] OFF
/****** Object:  Table [dbo].[MessageRecu]    Script Date: 02/03/2012 15:01:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageRecu](
	[idMessage] [int] NOT NULL,
	[dateReception] [datetime] NOT NULL,
	[dateLecture] [datetime] NULL,
 CONSTRAINT [PK_MessageRecu] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MessageEnvoi]    Script Date: 02/03/2012 15:01:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageEnvoi](
	[idMessage] [int] NOT NULL,
	[dateDemande] [datetime] NULL,
	[dateEnvoi] [datetime] NULL,
	[accuseReceptionRecu] [int] NULL,
	[dateReceptionAccuse] [datetime] NULL,
	[dureeValidite] [int] NULL,
	[referenceEnvoi] [nchar](25) NULL,
	[idStatut] [int] NOT NULL,
 CONSTRAINT [PK_MessageEnvoi] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_MessageEnvoi_dureeValidite]    Script Date: 02/03/2012 15:01:25 ******/
ALTER TABLE [dbo].[MessageEnvoi] ADD  CONSTRAINT [DF_MessageEnvoi_dureeValidite]  DEFAULT ((0)) FOR [dureeValidite]
GO
/****** Object:  ForeignKey [FK_Message_Encodage]    Script Date: 02/03/2012 15:01:25 ******/
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Encodage] FOREIGN KEY([idEncodage])
REFERENCES [dbo].[Encodage] ([idEncodage])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Encodage]
GO
/****** Object:  ForeignKey [FK_MessageEnvoi_Message]    Script Date: 02/03/2012 15:01:25 ******/
ALTER TABLE [dbo].[MessageEnvoi]  WITH CHECK ADD  CONSTRAINT [FK_MessageEnvoi_Message] FOREIGN KEY([idMessage])
REFERENCES [dbo].[Message] ([idMessage])
GO
ALTER TABLE [dbo].[MessageEnvoi] CHECK CONSTRAINT [FK_MessageEnvoi_Message]
GO
/****** Object:  ForeignKey [FK_MessageEnvoi_Statut]    Script Date: 02/03/2012 15:01:25 ******/
ALTER TABLE [dbo].[MessageEnvoi]  WITH CHECK ADD  CONSTRAINT [FK_MessageEnvoi_Statut] FOREIGN KEY([idStatut])
REFERENCES [dbo].[Statut] ([idStatut])
GO
ALTER TABLE [dbo].[MessageEnvoi] CHECK CONSTRAINT [FK_MessageEnvoi_Statut]
GO
/****** Object:  ForeignKey [FK_MessageRecu_Message]    Script Date: 02/03/2012 15:01:25 ******/
ALTER TABLE [dbo].[MessageRecu]  WITH CHECK ADD  CONSTRAINT [FK_MessageRecu_Message] FOREIGN KEY([idMessage])
REFERENCES [dbo].[Message] ([idMessage])
GO
ALTER TABLE [dbo].[MessageRecu] CHECK CONSTRAINT [FK_MessageRecu_Message]
GO
