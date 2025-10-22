SET IDENTITY_INSERT [dbo].[Compras] ON
INSERT INTO [dbo].[Compras] ([Id], [DirecciónEnvío], [FechaCompra], [PrecioTotal], [MétodoPagoId], [UsuarioId]) VALUES (3, N'C/Fermin 33', N'2030-10-10', 120, 1, N'1')
SET IDENTITY_INSERT [dbo].[Compras] OFF
