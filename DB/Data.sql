USE [Stock]
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([Id], [Name]) VALUES (1, N'аспирин')
GO
INSERT [dbo].[Products] ([Id], [Name]) VALUES (2, N'распиредон')
GO
INSERT [dbo].[Products] ([Id], [Name]) VALUES (3, N'аналгин')
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[PurchaseInvoices] ON 
GO
INSERT [dbo].[PurchaseInvoices] ([Id], [BroughtId], [When], [ShippedId], [CheckedId], [IsOnStock]) VALUES (1, 3, CAST(N'2020-04-21T18:35:00.0000000' AS DateTime2), 2, 4, 0)
GO
SET IDENTITY_INSERT [dbo].[PurchaseInvoices] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductInfos] ON 
GO

SET IDENTITY_INSERT [dbo].[ProductInfos] OFF
GO
INSERT [dbo].[Users] ([Email], [Password], [FullName], [RoleId]) VALUES (N'loader@mail.com', N'123456', NULL, 5)
GO
INSERT [dbo].[Users] ([Email], [Password], [FullName], [RoleId]) VALUES (N'driver@mail.com', N'123456', NULL, 3)
GO
INSERT [dbo].[Users] ([Email], [Password], [FullName], [RoleId]) VALUES (N'storeKeeper@mail.com', N'123456', NULL, 4)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
