SET IDENTITY_INSERT [dbo].[Alquileres] ON
INSERT INTO [dbo].[Alquileres] ([Id], [DireccionEnvio], [FechaAlquiler], [FechaInicio], [FechaFin], [PrecioTotal], [Correo], [MétodoPagoId], [UsuarioId]) VALUES (1, N'C/Dimas 33', N'2090-10-10 00:00:00', N'2010-10-10 00:00:00', N'3190-10-10 00:00:00', 1221212, N'dimas@gmail.com', 1, N'1')
SET IDENTITY_INSERT [dbo].[Alquileres] OFF
