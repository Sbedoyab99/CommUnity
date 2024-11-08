DELETE FROM ResidentialUnits
DELETE FROM Apartments
DELETE FROM CommonZones
DELETE FROM News
DELETE FROM Pets
DELETE FROM Vehicles

SET IDENTITY_INSERT [dbo].[ResidentialUnits] ON 

INSERT [dbo].[ResidentialUnits] ([Id], [Name], [Address], [CityId], [HasAdmin]) VALUES (1, N'Cyprus', N'Cra 46 76 sur 69', 1, 0)

SET IDENTITY_INSERT [dbo].[ResidentialUnits] OFF

SET IDENTITY_INSERT [dbo].[Apartments] ON

INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (1, N'101', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (2, N'102', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (3, N'103', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (4, N'104', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (5, N'201', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (6, N'202', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (7, N'203', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (8, N'204', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (9, N'301', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (10, N'302', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (11, N'303', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (12, N'304', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (13, N'401', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (14, N'402', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (15, N'403', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (16, N'404', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (17, N'501', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (18, N'502', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (19, N'503', 1)
INSERT [dbo].[Apartments] ([Id], [Number], [ResidentialUnitId]) VALUES (20, N'504', 1)

SET IDENTITY_INSERT [dbo].[Apartments] OFF

SET IDENTITY_INSERT [dbo].[CommonZones] ON 

INSERT INTO [dbo].[CommonZones] ([Id], [Name], [Capacity], [ResidentialUnitId]) VALUES (1, N'Salon Social', 50, 1);
INSERT INTO [dbo].[CommonZones] ([Id], [Name], [Capacity], [ResidentialUnitId]) VALUES (2, N'Gimnasio', 10, 1);
INSERT INTO [dbo].[CommonZones] ([Id], [Name], [Capacity], [ResidentialUnitId]) VALUES (3, N'Piscina', 20, 1);
INSERT INTO [dbo].[CommonZones] ([Id], [Name], [Capacity], [ResidentialUnitId]) VALUES (4, N'Cancha', 15, 1);
INSERT INTO [dbo].[CommonZones] ([Id], [Name], [Capacity], [ResidentialUnitId]) VALUES (5, N'Zona Bbq', 25, 1);

SET IDENTITY_INSERT [dbo].[CommonZones] OFF

SET IDENTITY_INSERT [dbo].[News] ON

INSERT INTO [dbo].[News] ([Id], [Title], [Content], [Date], [ResidentialUnitId]) VALUES (1, N'Nueva Aplicacion', N'Nos hemos registrado en la nueva aplicacion CommUnity', GETDATE(), 1);

SET IDENTITY_INSERT [dbo].[News] OFF

SET IDENTITY_INSERT [dbo].[Vehicles] ON

INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (1, N'SGM688', N'Camioneta', N'Vehiculo tipo Camioneta de color Rojo', 1);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (2, N'ZRO976', N'Motocicleta', N'Vehiculo tipo Motocicleta de color Blanco', 2);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (3, N'AUV136', N'Automóvil', N'Vehiculo tipo Automóvil de color Azul', 3);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (4, N'ZSU290', N'Camioneta', N'Vehiculo tipo Camioneta de color Verde', 4);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (5, N'GYJ257', N'Camioneta', N'Vehiculo tipo Camioneta de color Verde', 5);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (6, N'XON491', N'Automóvil', N'Vehiculo tipo Automóvil de color Verde', 6);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (7, N'HIB550', N'Camioneta', N'Vehiculo tipo Camioneta de color Verde', 7);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (8, N'ADK998', N'Automóvil', N'Vehiculo tipo Automóvil de color Rojo', 8);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (9, N'DNY300', N'Motocicleta', N'Vehiculo tipo Motocicleta de color Verde', 9);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (10, N'UCB577', N'Motocicleta', N'Vehiculo tipo Motocicleta de color Rojo', 10);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (11, N'CAL064', N'Camioneta', N'Vehiculo tipo Camioneta de color Azul', 11);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (12, N'OVA851', N'Motocicleta', N'Vehiculo tipo Motocicleta de color Rojo', 12);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (13, N'HZX385', N'Camioneta', N'Vehiculo tipo Camioneta de color Gris', 13);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (14, N'IEN465', N'Automóvil', N'Vehiculo tipo Automóvil de color Rojo', 14);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (15, N'CTF189', N'Automóvil', N'Vehiculo tipo Automóvil de color Negro', 15);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (16, N'QYW107', N'Motocicleta', N'Vehiculo tipo Motocicleta de color Azul', 16);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (17, N'KZP534', N'Camioneta', N'Vehiculo tipo Camioneta de color Blanco', 17);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (18, N'LLO036', N'Motocicleta', N'Vehiculo tipo Motocicleta de color Negro', 18);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (19, N'ZUR964', N'Automóvil', N'Vehiculo tipo Automóvil de color Verde', 19);
INSERT INTO [dbo].[Vehicles] ([Id], [Plate], [Type], [Description], [ApartmentId]) VALUES (20, N'NJE399', N'Automóvil', N'Vehiculo tipo Automóvil de color Negro', 20);

SET IDENTITY_INSERT [dbo].[Vehicles] OFF

SET IDENTITY_INSERT [dbo].[Pets] ON

INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (1, N'Bailey', N'Bulldog', 1);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (2, N'Toby', N'Rottweiler', 2);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (3, N'Teo', N'Pug', 3);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (4, N'Milo', N'Rottweiler', 4);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (5, N'Lucy', N'Rottweiler', 5);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (6, N'Bella', N'Yorkshire Terrier', 6);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (7, N'Oscar', N'Labrador', 7);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (8, N'Toby', N'Beagle', 8);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (9, N'Buddy', N'Rottweiler', 9);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (10, N'Charlie', N'Golden Retriever', 10);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (11, N'Charlie', N'Labrador', 11);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (12, N'Bailey', N'Poodle', 12);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (13, N'Jack', N'Golden Retriever', 13);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (14, N'Teo', N'Rottweiler', 14);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (15, N'Simba', N'Cocker Spaniel', 15);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (16, N'Rocky', N'Beagle', 16);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (17, N'Sophie', N'Bulldog', 17);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (18, N'Teddy', N'Bulldog', 18);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (19, N'Teo', N'Bulldog', 19);
INSERT INTO [dbo].[Pets] ([Id], [Name], [Breed], [ApartmentId]) VALUES (20, N'Charlie', N'Poodle', 20);

SET IDENTITY_INSERT [dbo].[Pets] OFF









