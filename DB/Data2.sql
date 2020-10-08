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
INSERT [dbo].[ProductInfos] ([Id], [PurchaseInvoiceId], [ProductId], [Count], [Cost], [Expiration]) VALUES (1, 1, 1, 50, CAST(100.00 AS Decimal(18, 2)), CAST(N'2020-04-23T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[ProductInfos] ([Id], [PurchaseInvoiceId], [ProductId], [Count], [Cost], [Expiration]) VALUES (2, 1, 2, 15, CAST(120.00 AS Decimal(18, 2)), CAST(N'2021-04-22T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[ProductInfos] ([Id], [PurchaseInvoiceId], [ProductId], [Count], [Cost], [Expiration]) VALUES (3, 1, 3, 60, CAST(50.00 AS Decimal(18, 2)), CAST(N'2020-05-22T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[ProductInfos] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Id], [Email], [Password], [FullName], [RoleId]) VALUES (2, N'loader@mail.com', N'123456', NULL, 5)
GO
INSERT [dbo].[Users] ([Id], [Email], [Password], [FullName], [RoleId]) VALUES (3, N'driver@mail.com', N'123456', NULL, 3)
GO
INSERT [dbo].[Users] ([Id], [Email], [Password], [FullName], [RoleId]) VALUES (4, N'storeKeeper@mail.com', N'123456', NULL, 4)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
