DELETE FROM Cities
DELETE FROM States
DELETE FROM Countries

SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([Id], [Name]) VALUES (1, N'Colombia')

SET IDENTITY_INSERT [dbo].[Countries] OFF

SET IDENTITY_INSERT [dbo].[States] ON 

INSERT [dbo].[States] ([Id], [Name], [CountryId]) VALUES (1, N'Antioquia', 1)

SET IDENTITY_INSERT [dbo].[States] OFF

SET IDENTITY_INSERT [dbo].[Cities] ON

INSERT [dbo].[Cities] ([Id], [Name], [StateId]) VALUES (1, N'Medellin', 1)

SET IDENTITY_INSERT [dbo].[Cities] OFF