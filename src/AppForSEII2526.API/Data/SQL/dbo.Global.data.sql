/*
update-database -migration 0
borras la base de datos marcando la casilla de cerrar conexiones (click derecho y eliminar "aspnet-AppForSEII2526.Web..." que está en la carpeta "Bases de datos")
remove-migration
add-migration CreateIdentitySchema
update-database
ejecutar SQL (Local -> MSSQLLocalDB, nombre de la base de datos -> "aspnet-AppForSEII2526.Web...")
*/

INSERT INTO [dbo].[AspNetUsers] ([Id], [Nombre], [Apellidos], [CorreoElectronico], [NumeroTelefono], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'1', N'Yoel', N'CS', N'Hola@gmail.com', N'000000000', N'YS', N'YS', N'Hola@gmail.com', N'Hola@gmail.com', 0, N'0', N'0', N'0', N'0', 0, 0, N'10/10/2020 0:00:00 +02:00', 0, 1)

SET IDENTITY_INSERT [dbo].[Fabricantes] ON
INSERT INTO [dbo].[Fabricantes] ([Id], [Nombre]) VALUES (1, N'EMPRESA1')
INSERT INTO [dbo].[Fabricantes] ([Id], [Nombre]) VALUES (2, N'EMPRESA2')
INSERT INTO [dbo].[Fabricantes] ([Id], [Nombre]) VALUES (3, N'EMPRESA3')
INSERT INTO [dbo].[Fabricantes] ([Id], [Nombre]) VALUES (4, N'LWL')
SET IDENTITY_INSERT [dbo].[Fabricantes] OFF

SET IDENTITY_INSERT [dbo].[Herramientas] ON
INSERT INTO [dbo].[Herramientas] ([Id], [FechaFabricacion], [Nombre], [Material], [Precio], [Stock], [TiempoReparacion], [FabricanteId]) VALUES (1, N'2026-10-10 00:00:00', N'Martillo', N'Madera', 10, 99, 10, 1)
INSERT INTO [dbo].[Herramientas] ([Id], [FechaFabricacion], [Nombre], [Material], [Precio], [Stock], [TiempoReparacion], [FabricanteId]) VALUES (2, N'2027-10-10 00:00:00', N'Llave', N'Hierro', 15, 99, 15, 2)
SET IDENTITY_INSERT [dbo].[Herramientas] OFF

SET IDENTITY_INSERT [dbo].[MetodosPagos] ON
INSERT INTO [dbo].[MetodosPagos] ([Id], [Nombre], [TipoDePago]) VALUES (1, N'Paypal', N'PayPal')
SET IDENTITY_INSERT [dbo].[MetodosPagos] OFF

SET IDENTITY_INSERT [dbo].[Compras] ON
INSERT INTO [dbo].[Compras] ([Id], [DireccionEnvio], [FechaCompra], [PrecioTotal], [MetodoPagoId], [UsuarioId]) VALUES (3, N'C/Fermin 33', N'2030-10-10', 120, 1, N'1')
SET IDENTITY_INSERT [dbo].[Compras] OFF

INSERT INTO [dbo].[CompraItems] ([CompraId], [HerramientaId], [Cantidad], [Descripcion], [Precio]) VALUES (3, 1, 2, N'Hola', 10)
INSERT INTO [dbo].[CompraItems] ([CompraId], [HerramientaId], [Cantidad], [Descripcion], [Precio]) VALUES (3, 2, 3, N'Adios', 20)

SET IDENTITY_INSERT [dbo].[Ofertas] ON
INSERT INTO [dbo].[Ofertas] ([Id], [FechaFinal], [FechaInicio], [FechaOferta], [TipoDirigida], [MetodosPagoId]) VALUES (3, N'2026-10-10 00:00:00', N'2027-10-10 00:00:00', N'2026-11-10 00:00:00', 1, 1)
SET IDENTITY_INSERT [dbo].[Ofertas] OFF

INSERT INTO [dbo].[OfertaItems] ([OfertaId], [HerramientaId], [Porcentaje], [PrecioFinal]) VALUES (3, 1, 10, 18)
INSERT INTO [dbo].[OfertaItems] ([OfertaId], [HerramientaId], [Porcentaje], [PrecioFinal]) VALUES (3, 2, 20, 15)

SET IDENTITY_INSERT [dbo].[Alquileres] ON
INSERT INTO [dbo].[Alquileres] ([Id], [DireccionEnvio], [FechaAlquiler], [FechaInicio], [FechaFin], [PrecioTotal], [MetodoPagoId], [UsuarioId]) VALUES (1, N'C/Dimas 33', N'2090-10-10 00:00:00', N'2010-10-10 00:00:00', N'3190-10-10 00:00:00', 1221212, 1, N'1')
SET IDENTITY_INSERT [dbo].[Alquileres] OFF

INSERT INTO [dbo].[AlquilarItems] ([AlquilerId], [HerramientaId], [Precio], [Cantidad]) VALUES (1, 1, 100, 10)
INSERT INTO [dbo].[AlquilarItems] ([AlquilerId], [HerramientaId], [Precio], [Cantidad]) VALUES (1, 2, 10, 10)
