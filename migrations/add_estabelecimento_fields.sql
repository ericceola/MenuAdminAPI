-- Migration: Add new fields to Estabelecimentos table for hierarchy and additional info
-- Date: 2026-02-12
-- Description: Add RazaoSocial, NomeResponsavel, TelefoneResponsavel, EhMatriz, TemFiliais, MatrizId columns

BEGIN TRANSACTION;

-- Add new columns to Estabelecimentos table
ALTER TABLE Estabelecimentos
ADD 
    RazaoSocial NVARCHAR(255) DEFAULT '',
    NomeResponsavel NVARCHAR(255) DEFAULT '',
    TelefoneResponsavel NVARCHAR(20) DEFAULT '',
    EhMatriz BIT DEFAULT 0,
    TemFiliais BIT DEFAULT 0,
    MatrizId UNIQUEIDENTIFIER NULL;

-- Add foreign key constraint for MatrizId
ALTER TABLE Estabelecimentos
ADD CONSTRAINT FK_Estabelecimentos_Matriz
FOREIGN KEY (MatrizId) REFERENCES Estabelecimentos(Id);

-- Create index on MatrizId for better query performance
CREATE INDEX IX_Estabelecimentos_MatrizId ON Estabelecimentos(MatrizId);

COMMIT TRANSACTION;
