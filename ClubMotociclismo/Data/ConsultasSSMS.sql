-- Consultas para verificar la base de datos en SQL Server Management Studio
-- Servidor: localhost\SQLEXPRESS   Base de datos: ClubMotociclismoDb
USE ClubMotociclismoDb;
GO

-- 1. Tablas creadas por las migraciones de Code First
SELECT name AS Tabla FROM sys.tables ORDER BY name;

-- 2. Migraciones aplicadas
SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory;

-- 3. Claves primarias
SELECT tc.TABLE_NAME AS Tabla, kcu.COLUMN_NAME AS Columna, tc.CONSTRAINT_NAME AS Restriccion
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY';

-- 4. Clave foránea Miembros.PlanId -> Planes.Id (relación 1:N)
SELECT fk.name AS ClaveForanea,
       OBJECT_NAME(fk.parent_object_id) AS TablaHija,
       COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS Columna,
       OBJECT_NAME(fk.referenced_object_id) AS TablaPadre,
       fk.delete_referential_action_desc AS AlEliminar
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id;

-- 5. Registros de cada tabla
SELECT * FROM Planes;
SELECT * FROM Miembros;
SELECT * FROM Eventos;

-- 6. Datos relacionados: cada miembro con su plan (JOIN)
SELECT m.Id, m.Nombre, m.TipoMoto, m.Cilindraje, m.Placa,
       p.Nombre AS PlanNombre, p.CuotaMensual AS PrecioPlan, m.CuotaMensual AS CuotaMiembro
FROM Miembros m
INNER JOIN Planes p ON p.Id = m.PlanId
ORDER BY m.Nombre;

-- 7. Cantidad de miembros por plan (lado "uno" de la relación)
SELECT p.Nombre AS PlanNombre, COUNT(m.Id) AS Miembros, ISNULL(SUM(m.CuotaMensual), 0) AS TotalCuotas
FROM Planes p
LEFT JOIN Miembros m ON m.PlanId = p.Id
GROUP BY p.Nombre
ORDER BY Miembros DESC;
