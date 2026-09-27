-- ============================================================================
-- CowShop Database - Script Completo SQL Server
-- ============================================================================
-- Descripción: Script de creación completa de la base de datos CowShop
-- Fecha: 22/06/2026
-- Versión: 1.0
-- Servidor: SQL Server / SQL Server Express
-- ============================================================================

-- ============================================================================
-- PASO 1: CREAR BASE DE DATOS
-- ============================================================================

-- Eliminar BD si existe (descomenta solo si quieres reiniciar)
-- DROP DATABASE IF EXISTS CowShop;

-- Crear la base de datos
CREATE DATABASE CowShop
    COLLATE SQL_Latin1_General_CP1_CI_AS;
GO

-- Usar la base de datos CowShop
USE CowShop;
GO

-- ============================================================================
-- PASO 2: CREAR TABLAS (en orden de dependencias)
-- ============================================================================

-- ============================================================================
-- Tabla 1: ROLES (sin dependencias)
-- ============================================================================
CREATE TABLE Roles (
    ID_Rol INT PRIMARY KEY IDENTITY(1,1),
    NombreRol NVARCHAR(100) NOT NULL UNIQUE,
    DescripcionRol NVARCHAR(500) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,
    Estado BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT CK_Roles_Estado CHECK (Estado IN (0, 1))
);
GO

-- ============================================================================
-- Tabla 2: MEMBRESIAS (sin dependencias)
-- ============================================================================
CREATE TABLE Membresias (
    ID_Membresia INT PRIMARY KEY IDENTITY(1,1),
    Nombre_Membresia NVARCHAR(100) NOT NULL UNIQUE,
    Valor_Membresia DECIMAL(10,2) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NOT NULL DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT CK_Membresias_Valor CHECK (Valor_Membresia >= 0),
    CONSTRAINT CK_Membresias_Estado CHECK (Estado IN (0, 1))
);
GO

-- ============================================================================
-- Tabla 3: USUARIOS (depende de Roles y Membresias)
-- ============================================================================
CREATE TABLE Usuarios (
    ID_Usuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(80) NOT NULL,
    Cedula NVARCHAR(20) NOT NULL UNIQUE,
    Fecha_Nacimiento DATETIME NOT NULL,
    Correo NVARCHAR(320) NOT NULL UNIQUE,
    Celular NVARCHAR(10) NOT NULL,
    Tipo_Usuario NVARCHAR(20) NOT NULL,
    Contrasena NVARCHAR(255) NOT NULL,
    ID_Membresia INT NOT NULL,
    ID_Rol INT NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NOT NULL DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1,
    
    -- Restricciones de validación
    CONSTRAINT CK_Usuarios_Celular CHECK (LEN(Celular) = 10 AND Celular LIKE '[0-9]%'),
    CONSTRAINT CK_Usuarios_TipoUsuario CHECK (Tipo_Usuario IN ('Vendedor', 'Comprador', 'Ambos')),
    CONSTRAINT CK_Usuarios_Estado CHECK (Estado IN (0, 1)),
    
    -- Claves foráneas
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (ID_Rol) 
        REFERENCES Roles(ID_Rol) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_Usuarios_Membresias FOREIGN KEY (ID_Membresia) 
        REFERENCES Membresias(ID_Membresia) ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO

-- ============================================================================
-- Tabla 4: VACAS (depende de Usuarios)
-- ============================================================================
CREATE TABLE Vacas (
    ID_Vaca INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Raza NVARCHAR(50) NOT NULL,
    Edad INT NOT NULL,
    Peso DECIMAL(10,2) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Estado_Salud NVARCHAR(50) NOT NULL,
    Fecha_Ingreso DATETIME NOT NULL DEFAULT GETDATE(),
    ID_Vendedor INT NOT NULL,
    
    -- Restricciones de validación
    CONSTRAINT CK_Vacas_Edad CHECK (Edad > 0),
    CONSTRAINT CK_Vacas_Peso CHECK (Peso > 0),
    CONSTRAINT CK_Vacas_Precio CHECK (Precio > 0),
    
    -- Clave foránea
    CONSTRAINT FK_Vacas_Usuarios FOREIGN KEY (ID_Vendedor) 
        REFERENCES Usuarios(ID_Usuario) ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO

-- ============================================================================
-- Tabla 5: VENTAS (depende de Usuarios)
-- ============================================================================
CREATE TABLE Ventas (
    ID_Venta INT PRIMARY KEY IDENTITY(1,1),
    ID_Comprador INT NOT NULL,
    Fecha_Venta DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(12,2) NOT NULL,
    
    -- Restricciones de validación
    CONSTRAINT CK_Ventas_Total CHECK (Total >= 0),
    
    -- Clave foránea
    CONSTRAINT FK_Ventas_Usuarios FOREIGN KEY (ID_Comprador) 
        REFERENCES Usuarios(ID_Usuario) ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO

-- ============================================================================
-- Tabla 6: DETALLESVENTA (depende de Ventas y Vacas)
-- ============================================================================
CREATE TABLE DetallesVenta (
    ID_DetalleVenta INT PRIMARY KEY IDENTITY(1,1),
    ID_Venta INT NOT NULL,
    ID_Vaca INT NOT NULL,
    
    -- Claves foráneas
    CONSTRAINT FK_DetallesVenta_Ventas FOREIGN KEY (ID_Venta) 
        REFERENCES Ventas(ID_Venta) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT FK_DetallesVenta_Vacas FOREIGN KEY (ID_Vaca) 
        REFERENCES Vacas(ID_Vaca) ON DELETE NO ACTION ON UPDATE NO ACTION,
    
    -- Constraint para evitar duplicados en la misma venta
    CONSTRAINT UQ_DetallesVenta_Unique UNIQUE (ID_Venta, ID_Vaca)
);
GO

-- ============================================================================
-- PASO 3: CREAR ÍNDICES PARA OPTIMIZACIÓN
-- ============================================================================

-- Índices en Usuarios
CREATE NONCLUSTERED INDEX IX_Usuarios_IDRol ON Usuarios(ID_Rol, Estado);
CREATE NONCLUSTERED INDEX IX_Usuarios_IDMembresia ON Usuarios(ID_Membresia);
CREATE NONCLUSTERED INDEX IX_Usuarios_FechaCreacion ON Usuarios(FechaCreacion);
CREATE NONCLUSTERED INDEX IX_Usuarios_TipoUsuario ON Usuarios(Tipo_Usuario, Estado);

-- Índices en Vacas
CREATE NONCLUSTERED INDEX IX_Vacas_IDVendedor ON Vacas(ID_Vendedor);
CREATE NONCLUSTERED INDEX IX_Vacas_Raza ON Vacas(Raza);
CREATE NONCLUSTERED INDEX IX_Vacas_EstadoSalud ON Vacas(Estado_Salud);
CREATE NONCLUSTERED INDEX IX_Vacas_Precio ON Vacas(Precio);

-- Índices en Ventas
CREATE NONCLUSTERED INDEX IX_Ventas_IDComprador ON Ventas(ID_Comprador);
CREATE NONCLUSTERED INDEX IX_Ventas_FechaVenta ON Ventas(Fecha_Venta);
CREATE NONCLUSTERED INDEX IX_Ventas_Total ON Ventas(Total);

-- Índices en DetallesVenta
CREATE NONCLUSTERED INDEX IX_DetallesVenta_IDVenta ON DetallesVenta(ID_Venta);
CREATE NONCLUSTERED INDEX IX_DetallesVenta_IDVaca ON DetallesVenta(ID_Vaca);

-- ============================================================================
-- PASO 4: INSERTAR DATOS INICIALES
-- ============================================================================

-- ============================================================================
-- DATOS INICIALES: ROLES
-- ============================================================================
INSERT INTO Roles (NombreRol, DescripcionRol, FechaCreacion, Estado) 
VALUES 
    ('Administrador', 'Acceso completo al sistema y gestión de usuarios', GETDATE(), 1),
    ('Vendedor', 'Usuario que puede vender vacas en la plataforma', GETDATE(), 1),
    ('Comprador', 'Usuario que puede comprar vacas en la plataforma', GETDATE(), 1),
    ('Moderador', 'Usuario con permisos de moderación y auditoría', GETDATE(), 1);
GO

-- ============================================================================
-- DATOS INICIALES: MEMBRESIAS
-- ============================================================================
INSERT INTO Membresias (Nombre_Membresia, Valor_Membresia, FechaCreacion, FechaModificacion, Estado)
VALUES 
    ('Básica', 0.00, GETDATE(), GETDATE(), 1),
    ('Premium', 99.99, GETDATE(), GETDATE(), 1),
    ('Platino', 249.99, GETDATE(), GETDATE(), 1);
GO

-- ============================================================================
-- DATOS INICIALES: USUARIOS
-- ============================================================================
-- Administrador: admin@cowshop.com / Contraseña: Admin123
-- Vendedores: juan@cowshop.com / maria@cowshop.com
-- Compradores: carlos@cowshop.com / ana@cowshop.com
-- Nota: Las contraseñas deben ser hasheadas con BCrypt antes de usar en producción

INSERT INTO Usuarios (Nombre, Cedula, Fecha_Nacimiento, Correo, Celular, Tipo_Usuario, Contrasena, ID_Membresia, ID_Rol, FechaCreacion, FechaModificacion, Estado)
VALUES 
    -- Administrador
    ('Administrador CowShop', '1000000001', '1985-01-15', 'admin@cowshop.com', '3001234567', 'Ambos', '$2a$11$TG9zIHByb3RIBBUWFRoN8eHZLZqI4H8mN2P5Q6R7S8T9U0V1W2X3Y4', 1, 1, GETDATE(), GETDATE(), 1),
    -- Vendedor 1
    ('Juan Carlos Mendoza', '1001234567', '1988-03-22', 'juan@cowshop.com', '3005551234', 'Vendedor', '$2a$11$QW9lMi9NZW5kb3phIHR0dHR0dHR0dHR0dHR0dHR0dHR0dHR0dHR0dH', 2, 2, GETDATE(), GETDATE(), 1),
    -- Vendedor 2
    ('María García López', '1002345678', '1990-07-10', 'maria@cowshop.com', '3009876543', 'Vendedor', '$2a$11$SXQwMDAgR2FyY2lhIHR0dHR0dHR0dHR0dHR0dHR0dHR0dHR0dHR0dH', 2, 2, GETDATE(), GETDATE(), 1),
    -- Comprador 1
    ('Carlos Alberto Rodríguez', '1003456789', '1992-05-18', 'carlos@cowshop.com', '3004445555', 'Comprador', '$2a$11$Q29tcHJhZG9yIFJvZHJpZ3VleiB0dHR0dHR0dHR0dHR0dHR0dHR0dH', 3, 3, GETDATE(), GETDATE(), 1),
    -- Comprador 2
    ('Ana Martínez Ruiz', '1004567890', '1995-11-25', 'ana@cowshop.com', '3008887777', 'Comprador', '$2a$11$QW5hIE1hcnRpbmV6IFJ1aXogdHR0dHR0dHR0dHR0dHR0dHR0dHR0dH', 3, 3, GETDATE(), GETDATE(), 1);
GO

-- ============================================================================
-- DATOS INICIALES: VACAS
-- ============================================================================
-- Vacas de Juan (ID_Vendedor = 2)
INSERT INTO Vacas (Nombre, Raza, Edad, Peso, Precio, Estado_Salud, Fecha_Ingreso, ID_Vendedor)
VALUES 
    -- Vacas del Vendedor 1 (Juan)
    ('Blanca Luna', 'Brahman', 3, 850.50, 8500.00, 'Excelente', DATEADD(DAY, -30, GETDATE()), 2),
    ('Negra Fuerte', 'Angus', 4, 920.00, 9200.00, 'Bueno', DATEADD(DAY, -25, GETDATE()), 2),
    ('Dorada Hermosa', 'Simmental', 3, 780.75, 7800.00, 'Excelente', DATEADD(DAY, -20, GETDATE()), 2),
    ('Café Oscuro', 'Charolais', 5, 1020.00, 10200.00, 'Bueno', DATEADD(DAY, -15, GETDATE()), 2),
    ('Rubia Clara', 'Hereford', 2, 650.25, 6500.00, 'Excelente', DATEADD(DAY, -10, GETDATE()), 2),
    -- Vacas del Vendedor 2 (María)
    ('Manchada Bella', 'Holstein', 4, 950.00, 9500.00, 'Excelente', DATEADD(DAY, -28, GETDATE()), 3),
    ('Gris Plateado', 'Brahman', 6, 1100.50, 11000.00, 'Bueno', DATEADD(DAY, -22, GETDATE()), 3),
    ('Pinta Roja', 'Guernsey', 3, 720.75, 7200.00, 'Excelente', DATEADD(DAY, -18, GETDATE()), 3),
    ('Blanca Pura', 'Brahman', 5, 890.00, 8900.00, 'Bueno', DATEADD(DAY, -12, GETDATE()), 3),
    ('Oscura Premium', 'Angus', 7, 1050.25, 10500.00, 'Excelente', DATEADD(DAY, -5, GETDATE()), 3);
GO

-- ============================================================================
-- DATOS INICIALES: VENTAS
-- ============================================================================
-- Venta 1: Carlos compra 2 vacas
-- Venta 2: Ana compra 2 vacas
-- Venta 3: Carlos compra 1 vaca

INSERT INTO Ventas (ID_Comprador, Fecha_Venta, Total)
VALUES 
    (4, DATEADD(DAY, -5, GETDATE()), 17700.00),   -- Carlos compra 2 vacas (8500 + 9200)
    (5, DATEADD(DAY, -3, GETDATE()), 26700.00),   -- Ana compra 2 vacas (9500 + 7200 + 10000)
    (4, DATEADD(DAY, -1, GETDATE()), 8900.00);    -- Carlos compra 1 vaca
GO

-- ============================================================================
-- DATOS INICIALES: DETALLES DE VENTA
-- ============================================================================
-- Venta 1 (ID_Venta = 1, Carlos): Blanca Luna + Negra Fuerte
INSERT INTO DetallesVenta (ID_Venta, ID_Vaca)
VALUES 
    (1, 1),      -- Venta 1: Blanca Luna (Juan)
    (1, 2),      -- Venta 1: Negra Fuerte (Juan)
    (2, 6),      -- Venta 2: Manchada Bella (María)
    (2, 8),      -- Venta 2: Pinta Roja (María)
    (3, 9);      -- Venta 3: Blanca Pura (María)
GO

-- ============================================================================
-- RESUMEN DE DATOS INICIALES INSERTADOS
-- ============================================================================
-- Roles: 4
-- Membresias: 3
-- Usuarios: 5 (1 Admin, 2 Vendedores, 2 Compradores)
-- Vacas: 10 (5 de Juan, 5 de María)
-- Ventas: 3
-- Detalles de Venta: 5
-- ============================================================================
GO

-- ============================================================================
-- PASO 5: CREAR VISTAS ÚTILES PARA REPORTES
-- ============================================================================

-- Vista para listar usuarios con detalles de rol y membresía
CREATE VIEW v_UsuariosDetalle AS
SELECT 
    u.ID_Usuario,
    u.Nombre,
    u.Cedula,
    u.Correo,
    u.Celular,
    u.Tipo_Usuario,
    r.NombreRol AS Rol,
    m.Nombre_Membresia AS Membresia,
    u.FechaCreacion,
    u.FechaModificacion,
    CASE WHEN u.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
FROM Usuarios u
INNER JOIN Roles r ON u.ID_Rol = r.ID_Rol
INNER JOIN Membresias m ON u.ID_Membresia = m.ID_Membresia;
GO

-- Vista para vendedores activos con cantidad de vacas
CREATE VIEW v_VendedoresActivos AS
SELECT 
    u.ID_Usuario,
    u.Nombre,
    u.Correo,
    COUNT(v.ID_Vaca) AS CantidadVacas,
    SUM(v.Precio) AS ValorTotalInventario
FROM Usuarios u
LEFT JOIN Vacas v ON u.ID_Usuario = v.ID_Vendedor
WHERE u.Tipo_Usuario IN ('Vendedor', 'Ambos') 
    AND u.Estado = 1
GROUP BY u.ID_Usuario, u.Nombre, u.Correo;
GO

-- Vista para historial de ventas detallado
CREATE VIEW v_VentasDetalle AS
SELECT 
    v.ID_Venta,
    v.Fecha_Venta,
    u.Nombre AS Comprador,
    u.Correo AS CorreoComprador,
    vac.Nombre AS NombreVaca,
    vac.Raza,
    vac.Edad,
    vac.Precio,
    v.Total
FROM Ventas v
INNER JOIN Usuarios u ON v.ID_Comprador = u.ID_Usuario
INNER JOIN DetallesVenta dv ON v.ID_Venta = dv.ID_Venta
INNER JOIN Vacas vac ON dv.ID_Vaca = vac.ID_Vaca;
GO

-- Vista para vacas disponibles
CREATE VIEW v_VacasDisponibles AS
SELECT 
    v.ID_Vaca,
    v.Nombre,
    v.Raza,
    v.Edad,
    v.Peso,
    v.Precio,
    v.Estado_Salud,
    u.Nombre AS VendedorNombre,
    u.Correo AS VendedorCorreo
FROM Vacas v
INNER JOIN Usuarios u ON v.ID_Vendedor = u.ID_Usuario
WHERE v.ID_Vaca NOT IN (
    SELECT DISTINCT ID_Vaca FROM DetallesVenta
);
GO

-- ============================================================================
-- PASO 6: CREAR PROCEDIMIENTOS ALMACENADOS (BÁSICOS)
-- ============================================================================

-- Procedimiento para registrar usuario
CREATE PROCEDURE sp_RegistrarUsuario
    @Nombre NVARCHAR(80),
    @Cedula NVARCHAR(20),
    @Fecha_Nacimiento DATETIME,
    @Correo NVARCHAR(320),
    @Celular NVARCHAR(10),
    @Tipo_Usuario NVARCHAR(20),
    @Contrasena NVARCHAR(255),
    @ID_Membresia INT,
    @ID_Rol INT,
    @ID_Usuario INT OUTPUT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Usuarios (
            Nombre, Cedula, Fecha_Nacimiento, Correo, Celular,
            Tipo_Usuario, Contrasena, ID_Membresia, ID_Rol,
            FechaCreacion, FechaModificacion, Estado
        )
        VALUES (
            @Nombre, @Cedula, @Fecha_Nacimiento, @Correo, @Celular,
            @Tipo_Usuario, @Contrasena, @ID_Membresia, @ID_Rol,
            GETDATE(), GETDATE(), 1
        );
        
        SET @ID_Usuario = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedimiento para actualizar usuario
CREATE PROCEDURE sp_ActualizarUsuario
    @ID_Usuario INT,
    @Nombre NVARCHAR(80),
    @Cedula NVARCHAR(20),
    @Correo NVARCHAR(320),
    @Celular NVARCHAR(10),
    @Tipo_Usuario NVARCHAR(20),
    @ID_Membresia INT,
    @ID_Rol INT,
    @Estado BIT
AS
BEGIN
    BEGIN TRY
        UPDATE Usuarios
        SET 
            Nombre = @Nombre,
            Cedula = @Cedula,
            Correo = @Correo,
            Celular = @Celular,
            Tipo_Usuario = @Tipo_Usuario,
            ID_Membresia = @ID_Membresia,
            ID_Rol = @ID_Rol,
            Estado = @Estado,
            FechaModificacion = GETDATE()
        WHERE ID_Usuario = @ID_Usuario;
        
        SELECT @@ROWCOUNT AS FilasActualizadas;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedimiento para listar usuarios activos
CREATE PROCEDURE sp_ListarUsuariosActivos
AS
BEGIN
    SELECT 
        u.ID_Usuario,
        u.Nombre,
        u.Cedula,
        u.Correo,
        u.Celular,
        u.Tipo_Usuario,
        r.NombreRol,
        m.Nombre_Membresia,
        u.FechaCreacion,
        u.Estado
    FROM Usuarios u
    INNER JOIN Roles r ON u.ID_Rol = r.ID_Rol
    INNER JOIN Membresias m ON u.ID_Membresia = m.ID_Membresia
    WHERE u.Estado = 1
    ORDER BY u.FechaCreacion DESC;
END;
GO

-- Procedimiento para registrar vaca
CREATE PROCEDURE sp_RegistrarVaca
    @Nombre NVARCHAR(100),
    @Raza NVARCHAR(50),
    @Edad INT,
    @Peso DECIMAL(10,2),
    @Precio DECIMAL(10,2),
    @Estado_Salud NVARCHAR(50),
    @ID_Vendedor INT,
    @ID_Vaca INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Verificar que el usuario es vendedor
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE ID_Usuario = @ID_Vendedor 
                       AND Tipo_Usuario IN ('Vendedor', 'Ambos') AND Estado = 1)
        BEGIN
            RAISERROR('El usuario no es un vendedor válido', 16, 1);
        END;
        
        INSERT INTO Vacas (Nombre, Raza, Edad, Peso, Precio, Estado_Salud, ID_Vendedor)
        VALUES (@Nombre, @Raza, @Edad, @Peso, @Precio, @Estado_Salud, @ID_Vendedor);
        
        SET @ID_Vaca = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedimiento para registrar venta
CREATE PROCEDURE sp_RegistrarVenta
    @ID_Comprador INT,
    @Total DECIMAL(12,2),
    @ID_Venta INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Verificar que el usuario es comprador
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE ID_Usuario = @ID_Comprador 
                       AND Tipo_Usuario IN ('Comprador', 'Ambos') AND Estado = 1)
        BEGIN
            RAISERROR('El usuario no es un comprador válido', 16, 1);
        END;
        
        INSERT INTO Ventas (ID_Comprador, Fecha_Venta, Total)
        VALUES (@ID_Comprador, GETDATE(), @Total);
        
        SET @ID_Venta = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedimiento para registrar detalle de venta
CREATE PROCEDURE sp_RegistrarDetalleVenta
    @ID_Venta INT,
    @ID_Vaca INT
AS
BEGIN
    BEGIN TRY
        -- Verificar que la vaca existe
        IF NOT EXISTS (SELECT 1 FROM Vacas WHERE ID_Vaca = @ID_Vaca)
        BEGIN
            RAISERROR('La vaca no existe', 16, 1);
        END;
        
        -- Verificar que no existe ya este detalle
        IF EXISTS (SELECT 1 FROM DetallesVenta WHERE ID_Venta = @ID_Venta AND ID_Vaca = @ID_Vaca)
        BEGIN
            RAISERROR('Esta vaca ya está en la venta', 16, 1);
        END;
        
        INSERT INTO DetallesVenta (ID_Venta, ID_Vaca)
        VALUES (@ID_Venta, @ID_Vaca);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- ============================================================================
-- PASO 7: INFORMACIÓN DE SEGURIDAD Y AUDITORÍA
-- ============================================================================

-- Crear rol de base de datos para aplicación
CREATE ROLE db_cowshop_app;

-- Permisos básicos para la aplicación
GRANT SELECT ON dbo.Usuarios TO db_cowshop_app;
GRANT SELECT ON dbo.Roles TO db_cowshop_app;
GRANT SELECT ON dbo.Membresias TO db_cowshop_app;
GRANT SELECT ON dbo.Vacas TO db_cowshop_app;
GRANT SELECT ON dbo.Ventas TO db_cowshop_app;
GRANT SELECT ON dbo.DetallesVenta TO db_cowshop_app;
GRANT INSERT, UPDATE ON dbo.Usuarios TO db_cowshop_app;
GRANT INSERT ON dbo.Vacas TO db_cowshop_app;
GRANT INSERT, UPDATE ON dbo.Vacas TO db_cowshop_app;
GRANT INSERT ON dbo.Ventas TO db_cowshop_app;
GRANT INSERT ON dbo.DetallesVenta TO db_cowshop_app;
GRANT EXECUTE ON dbo.sp_RegistrarUsuario TO db_cowshop_app;
GRANT EXECUTE ON dbo.sp_ActualizarUsuario TO db_cowshop_app;
GRANT EXECUTE ON dbo.sp_ListarUsuariosActivos TO db_cowshop_app;
GRANT EXECUTE ON dbo.sp_RegistrarVaca TO db_cowshop_app;
GRANT EXECUTE ON dbo.sp_RegistrarVenta TO db_cowshop_app;
GRANT EXECUTE ON dbo.sp_RegistrarDetalleVenta TO db_cowshop_app;

GO

-- ============================================================================
-- PASO 8: VERIFICACIÓN DE INTEGRIDAD
-- ============================================================================

-- Listar todas las tablas creadas
SELECT 
    TABLE_NAME,
    'Tabla' AS Tipo
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_NAME;

-- Listar todas las vistas creadas
SELECT 
    TABLE_NAME,
    'Vista' AS Tipo
FROM INFORMATION_SCHEMA.VIEWS
WHERE TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_NAME;

-- Listar todos los procedimientos almacenados
SELECT 
    ROUTINE_NAME,
    'Procedimiento' AS Tipo
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_NAME;

GO

-- ============================================================================
-- FIN DEL SCRIPT
-- ============================================================================
-- 
-- INSTRUCCIONES FINALES:
--
-- 1. Ejecutar este script completo en SQL Server Management Studio
-- 2. La base de datos CowShop se creará con todas las tablas
-- 3. Se insertarán datos iniciales (Roles y Membresias)
-- 4. Se crearán vistas para consultas frecuentes
-- 5. Se crearán procedimientos almacenados básicos
-- 6. Se configurará permisos de seguridad
--
-- PRÓXIMOS PASOS:
--
-- 1. Ajustar la cadena de conexión en Web.config:
--    Data Source=DESKTOP-3418HVO\SQLEXPRESS;Initial Catalog=CowShop;Integrated Security=True;
--
-- 2. Crear/Actualizar los procedimientos almacenados según el código en UsuarioData.cs,
--    VacaData.cs, VentaData.cs, etc.
--
-- 3. Ejecutar migraciones de datos si hay BD antigua
--
-- 4. Hacer backup de la BD:
--    BACKUP DATABASE CowShop TO DISK = 'C:\Backups\CowShop_Initial.bak';
--
-- ============================================================================
