IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Account] (
    [AccountId] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY ([AccountId])
);

GO

CREATE TABLE [Permission] (
    [PermissionId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [AccountId] int NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY ([PermissionId]),
    CONSTRAINT [FK_Permission_Account_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Account] ([AccountId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AccountPermission] (
    [AccountPermissionId] int NOT NULL IDENTITY,
    [AccountId] int NOT NULL,
    [PermissionId] nvarchar(max) NULL,
    [PermissionId1] int NULL,
    CONSTRAINT [PK_AccountPermission] PRIMARY KEY ([AccountPermissionId]),
    CONSTRAINT [FK_AccountPermission_Account_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Account] ([AccountId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccountPermission_Permission_PermissionId1] FOREIGN KEY ([PermissionId1]) REFERENCES [Permission] ([PermissionId]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AccountPermission_AccountId] ON [AccountPermission] ([AccountId]);

GO

CREATE INDEX [IX_AccountPermission_PermissionId1] ON [AccountPermission] ([PermissionId1]);

GO

CREATE INDEX [IX_Permission_AccountId] ON [Permission] ([AccountId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200416124251_InitialMigration', N'2.2.6-servicing-10079');

GO

