CREATE TABLE [Users]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](500) NOT NULL,
    [Apellido] [nvarchar](max) NULL,
    [Email] [nvarchar](60) NOT NULL,
    [Password] NVARCHAR (50)      NULL,
    CONSTRAINT [PK_User_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);