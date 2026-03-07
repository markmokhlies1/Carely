IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Admins] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NULL,
    [LastName] nvarchar(50) NULL,
    [Email] nvarchar(100) NOT NULL,
    [Password] nvarchar(100) NOT NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [Role] int NOT NULL,
    CONSTRAINT [PK_Admins] PRIMARY KEY ([Id])
);

CREATE TABLE [Mothers] (
    [Id] int NOT NULL IDENTITY,
    [BirthDate] datetime2 NOT NULL,
    [Hight] int NOT NULL,
    [Weight] int NOT NULL,
    [FirstName] nvarchar(25) NOT NULL,
    [LastName] nvarchar(25) NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [Password] nvarchar(50) NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [Role] int NOT NULL,
    CONSTRAINT [PK_Mothers] PRIMARY KEY ([Id])
);

CREATE TABLE [Medications] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Spot] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [Duration] int NOT NULL,
    [MedicationType] int NOT NULL,
    [MotherId] int NOT NULL,
    CONSTRAINT [PK_Medications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Medications_Mothers_MotherId] FOREIGN KEY ([MotherId]) REFERENCES [Mothers] ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'FirstName', N'LastName', N'Password', N'PhoneNumber', N'Role') AND [object_id] = OBJECT_ID(N'[Admins]'))
    SET IDENTITY_INSERT [Admins] ON;
INSERT INTO [Admins] ([Id], [Email], [FirstName], [LastName], [Password], [PhoneNumber], [Role])
VALUES (1, N'super.admin@babycare.com', N'Super', N'Admin', N'Admin@123', N'01000000000', 1),
(2, N'mona.admin@babycare.com', N'Mona', N'Adel', N'Mona@123', N'01011111111', 1),
(3, N'hassan.admin@babycare.com', N'Hassan', N'Tarek', N'Hassan@123', N'01022222222', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'FirstName', N'LastName', N'Password', N'PhoneNumber', N'Role') AND [object_id] = OBJECT_ID(N'[Admins]'))
    SET IDENTITY_INSERT [Admins] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BirthDate', N'Email', N'FirstName', N'Hight', N'LastName', N'Password', N'PhoneNumber', N'Role', N'Weight') AND [object_id] = OBJECT_ID(N'[Mothers]'))
    SET IDENTITY_INSERT [Mothers] ON;
INSERT INTO [Mothers] ([Id], [BirthDate], [Email], [FirstName], [Hight], [LastName], [Password], [PhoneNumber], [Role], [Weight])
VALUES (1, '1998-05-10T00:00:00.0000000', N'sara@example.com', N'Sara', 165, N'Khaled', N'123456', N'01112345678', 0, 62),
(2, '1995-07-15T00:00:00.0000000', N'nada@example.com', N'Nada', 160, N'Mohsen', N'654321', N'01098765432', 0, 58),
(3, '2000-02-20T00:00:00.0000000', N'eman@example.com', N'Eman', 170, N'Ali', N'987654', N'01234567890', 0, 70);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BirthDate', N'Email', N'FirstName', N'Hight', N'LastName', N'Password', N'PhoneNumber', N'Role', N'Weight') AND [object_id] = OBJECT_ID(N'[Mothers]'))
    SET IDENTITY_INSERT [Mothers] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Duration', N'MedicationType', N'MotherId', N'Name', N'Spot', N'StartDate') AND [object_id] = OBJECT_ID(N'[Medications]'))
    SET IDENTITY_INSERT [Medications] ON;
INSERT INTO [Medications] ([Id], [Description], [Duration], [MedicationType], [MotherId], [Name], [Spot], [StartDate])
VALUES (1, N'Daily vitamin supplement for the baby.', 30, 2, 1, N'Vitamin D', 0, '2025-01-01T00:00:00.0000000'),
(2, N'Taken after meals to relieve cough.', 10, 2, 2, N'Cough Syrup', 0, '2025-02-10T00:00:00.0000000'),
(3, N'Prescribed for infection treatment.', 7, 0, 3, N'Antibiotic Injection', 0, '2025-03-05T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Duration', N'MedicationType', N'MotherId', N'Name', N'Spot', N'StartDate') AND [object_id] = OBJECT_ID(N'[Medications]'))
    SET IDENTITY_INSERT [Medications] OFF;

CREATE INDEX [IX_Medications_MotherId] ON [Medications] ([MotherId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251016174218_init feature mother_medication', N'9.0.10');

EXEC sp_rename N'[Mothers].[Password]', N'PasswordHash', 'COLUMN';

EXEC sp_rename N'[Admins].[Password]', N'PasswordHash', 'COLUMN';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251019070624_2nd', N'9.0.10');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Mothers]') AND [c].[name] = N'PasswordHash');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Mothers] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Mothers] ALTER COLUMN [PasswordHash] nvarchar(250) NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251019093334_IncreasePasswordHashLength', N'9.0.10');

CREATE TABLE [Doctors] (
    [Id] int NOT NULL IDENTITY,
    [Gender] int NOT NULL,
    [Age] int NOT NULL,
    [Link] nvarchar(max) NOT NULL,
    [Specification] int NOT NULL,
    [FirstName] nvarchar(25) NOT NULL,
    [LastName] nvarchar(25) NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [PasswordHash] nvarchar(250) NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [Role] int NOT NULL,
    CONSTRAINT [PK_Doctors] PRIMARY KEY ([Id])
);

CREATE TABLE [Clinics] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Address] nvarchar(200) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [DoctorId] int NOT NULL,
    CONSTRAINT [PK_Clinics] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Clinics_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Meetings] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(500) NULL,
    [MeetingType] int NOT NULL,
    [Date] datetime2 NOT NULL,
    [Address] nvarchar(200) NULL,
    [DoctorId] int NOT NULL,
    CONSTRAINT [PK_Meetings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Meetings_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ClinicWorkTimes] (
    [Id] int NOT NULL IDENTITY,
    [Day] int NOT NULL,
    [From] time NOT NULL,
    [To] time NOT NULL,
    [ClinicId] int NOT NULL,
    CONSTRAINT [PK_ClinicWorkTimes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClinicWorkTimes_Clinics_ClinicId] FOREIGN KEY ([ClinicId]) REFERENCES [Clinics] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Feedbacks] (
    [Id] int NOT NULL IDENTITY,
    [Stars] int NOT NULL,
    [Comment] nvarchar(500) NULL,
    [MotherId] int NOT NULL,
    [MeetingId] int NOT NULL,
    CONSTRAINT [PK_Feedbacks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Feedbacks_Meetings_MeetingId] FOREIGN KEY ([MeetingId]) REFERENCES [Meetings] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Feedbacks_Mothers_MotherId] FOREIGN KEY ([MotherId]) REFERENCES [Mothers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [MotherMeetings] (
    [MeetingsId] int NOT NULL,
    [MothersId] int NOT NULL,
    CONSTRAINT [PK_MotherMeetings] PRIMARY KEY ([MeetingsId], [MothersId]),
    CONSTRAINT [FK_MotherMeetings_Meetings_MeetingsId] FOREIGN KEY ([MeetingsId]) REFERENCES [Meetings] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MotherMeetings_Mothers_MothersId] FOREIGN KEY ([MothersId]) REFERENCES [Mothers] ([Id]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Age', N'Email', N'FirstName', N'Gender', N'LastName', N'Link', N'PasswordHash', N'PhoneNumber', N'Role', N'Specification') AND [object_id] = OBJECT_ID(N'[Doctors]'))
    SET IDENTITY_INSERT [Doctors] ON;
INSERT INTO [Doctors] ([Id], [Age], [Email], [FirstName], [Gender], [LastName], [Link], [PasswordHash], [PhoneNumber], [Role], [Specification])
VALUES (1, 40, N'ahmed.samir@clinic.com', N'Ahmed', 0, N'Samir', N'bbbb', N'Doctor@123', N'01033333333', 2, 1),
(2, 35, N'mariam.magdy@clinic.com', N'Mariam', 1, N'Magdy', N'gergre', N'Mariam@123', N'01044444444', 2, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Age', N'Email', N'FirstName', N'Gender', N'LastName', N'Link', N'PasswordHash', N'PhoneNumber', N'Role', N'Specification') AND [object_id] = OBJECT_ID(N'[Doctors]'))
    SET IDENTITY_INSERT [Doctors] OFF;

CREATE INDEX [IX_Clinics_DoctorId] ON [Clinics] ([DoctorId]);

CREATE INDEX [IX_ClinicWorkTimes_ClinicId] ON [ClinicWorkTimes] ([ClinicId]);

CREATE INDEX [IX_Feedbacks_MeetingId] ON [Feedbacks] ([MeetingId]);

CREATE INDEX [IX_Feedbacks_MotherId] ON [Feedbacks] ([MotherId]);

CREATE INDEX [IX_Meetings_DoctorId] ON [Meetings] ([DoctorId]);

CREATE INDEX [IX_MotherMeetings_MothersId] ON [MotherMeetings] ([MothersId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251120152739_f2 added', N'9.0.10');

COMMIT;
GO

