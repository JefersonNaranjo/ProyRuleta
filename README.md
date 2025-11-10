URL: https://localhost:7037/swagger

Scrip para BD
CREATE DATABASE DBRuleta;
GO
USE DBRuleta;
GO

CREATE TABLE Ruletas (
  IdRuleta INT IDENTITY(1,1) PRIMARY KEY,
  Nombre NVARCHAR(100) NULL,
  Estado NVARCHAR(20) NOT NULL,
  FechaCreacion DATETIME NOT NULL,
  FechaApertura DATETIME NULL,
  FechaCierre DATETIME NULL,
  NumeroGanador INT NULL,
  ColorGanador NVARCHAR(10) NULL
);

CREATE TABLE Apuestas (
  IdApuesta INT IDENTITY(1,1) PRIMARY KEY,
  IdRuleta INT NOT NULL FOREIGN KEY REFERENCES Ruletas(IdRuleta),
  IdUsuario NVARCHAR(50) NOT NULL,
  TipoApuesta NVARCHAR(10) NOT NULL,
  ValorNumero INT NULL,
  ValorColor NVARCHAR(10) NULL,
  Monto DECIMAL(18,2) NOT NULL,
  FechaApuesta DATETIME NOT NULL,
  Resultado NVARCHAR(10) NULL,
  Pago DECIMAL(18,2) NULL
);

Json de Prueba con idruleta 1

{
  "idRuleta": 1,
  "tipoApuesta": "NUMERO",
  "valorNumero": 10,
  "monto": 100
}

{
  "idRuleta": 1,
  "tipoApuesta": "COLOR",
  "valorColor": "ROJO",
  "monto": 50
}
