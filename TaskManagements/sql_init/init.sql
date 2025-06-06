IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'Jean')
BEGIN
    CREATE LOGIN seu_usuario WITH PASSWORD = 'P@ssw0rd!sStr0ng';
END;
GO

USE [master]; -- Ou o nome do seu banco de dados, se já existir
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'UserProTasksDB')
BEGIN
    CREATE DATABASE UserProTasksDB;
END;
GO

USE [UserProTasksDB];
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'Jean')
BEGIN
    CREATE USER seu_usuario FOR LOGIN seu_usuario;
    ALTER ROLE db_owner ADD MEMBER seu_usuario; -- Ou outro papel de acesso adequado
END;
GO