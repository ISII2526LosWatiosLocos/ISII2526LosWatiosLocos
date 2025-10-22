SET IDENTITY_INSERT [dbo].[Herramientas] ON
INSERT INTO [dbo].[Herramientas] ([Id], [Nombre], [Material], [Precio], [TiempoReparacion], [FabricanteId]) VALUES (1, N'Martillo', N'Madera', 10, 10, 1)
INSERT INTO [dbo].[Herramientas] ([Id], [Nombre], [Material], [Precio], [TiempoReparacion], [FabricanteId]) VALUES (2, N'Llave', N'Hierro', 15, 15, 2)
SET IDENTITY_INSERT [dbo].[Herramientas] OFF
