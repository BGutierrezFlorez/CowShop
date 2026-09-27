-- Script: Crear tabla Pagos para registro de transacciones Wompi (pruebas)
IF OBJECT_ID('dbo.Pagos', 'U') IS NOT NULL
    DROP TABLE dbo.Pagos;

CREATE TABLE dbo.Pagos (
    ID_Pago INT IDENTITY(1,1) PRIMARY KEY,
    Reference VARCHAR(200) NOT NULL,
    TransactionId VARCHAR(200) NULL,
    UserId INT NULL,
    AmountInCents BIGINT NULL,
    Currency VARCHAR(10) NULL,
    Estado VARCHAR(50) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    RawResponse NTEXT NULL
);

-- Indice por referencia para búsquedas rápidas
CREATE INDEX IX_Pagos_Reference ON dbo.Pagos(Reference);
