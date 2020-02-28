
CREATE DATABASE BoVoyage
Go

USE BoVoyage
Go

CREATE TABLE Client (
	Id INTEGER NOT NULL
)

Go

ALTER TABLE Client ADD Constraint Client_Pk PRIMARY KEY (Id)
Go

CREATE TABLE Destination 
    ( Id INTEGER NOT NULL IDENTITY , 
     IdParente INTEGER, 
     Nom NVARCHAR (100) NOT NULL , 
     Niveau TINYINT NOT NULL , 
     Description NVARCHAR (500) 
    )
GO 



EXEC sp_addextendedproperty 'MS_Description' , '1 : Continent
2 : Pays
3 : Région' , 'USER' , 'dbo' , 'TABLE' , 'Destination' , 'COLUMN' , 'Niveau'
Go

ALTER TABLE Destination ADD Constraint Destination_Pk PRIMARY KEY (Id)
 Go

CREATE TABLE Dossierresa 
    ( Id INTEGER NOT NULL IDENTITY , 
     NumeroCB VARCHAR (16) , 
	 PrixTotal DECIMAL (16,4) NOT NULL DEFAULT 0 ,
     IdClient INTEGER NOT NULL , 
     IdEtatDossier TINYINT NOT NULL , 
     IdVoyage INTEGER NOT Null ) 
Go

ALTER TABLE DossierResa ADD Constraint Dossierresa_Pk PRIMARY KEY (Id)

Go

CREATE TABLE Etatdossier 
    (
	Id        Tinyint NOT NULL,
	Libelle   Nvarchar (30) NOT Null )
Go

ALTER TABLE EtatDossier ADD Constraint Etatdossier_Pk PRIMARY KEY (Id)

Go

CREATE TABLE Personne 
 ( Id INTEGER NOT NULL IDENTITY , 
	TypePers TINYINT NOT NULL , 
	Civilite VARCHAR (3) NOT NULL , 
	Nom NVARCHAR (30) NOT NULL , 
	Prenom NVARCHAR (30) NOT NULL , 
	Email NVARCHAR (30) NOT	Null,
	Telephone VARCHAR ( 16 ),
	Datenaissance DATE
)
GO

ALTER TABLE Personne ADD Constraint Personne_Pk PRIMARY KEY (Id)
Go

CREATE TABLE Photo 
    ( Id INTEGER NOT NULL IDENTITY , 
     NomFichier NVARCHAR (100) NOT NULL , 
     IdDestination INTEGER NOT Null )
Go

ALTER TABLE Photo ADD Constraint Photo_Pk PRIMARY KEY (Id)
Go

CREATE TABLE Voyage 
    ( Id INTEGER NOT NULL IDENTITY , 
     IdDestination INTEGER NOT NULL , 
     DateDepart DATE NOT NULL , 
     DateRetour DATE NOT NULL , 
     PlacesDispo INTEGER NOT NULL , 
     PrixHT DECIMAL (16,4) NOT NULL DEFAULT 0 , 
     Reduction DECIMAL (3,2) NOT NULL , Descriptif Nvarchar(500) ) 
Go

ALTER TABLE Voyage ADD Constraint Voyage_Pk PRIMARY KEY (Id)
 
Go

CREATE TABLE Voyageur (
	Id         INTEGER NOT NULL,
	Idvoyage   INTEGER NOT NULL
)

Go

ALTER TABLE Voyageur ADD Constraint Voyageur_Pk PRIMARY KEY (Id, Idvoyage)
GO

ALTER TABLE Client
	ADD CONSTRAINT Client_Personne_Fk FOREIGN KEY ( Id )
		REFERENCES Personne ( Id )
Go

ALTER TABLE Destination
	ADD CONSTRAINT Destination_Destination_Fk FOREIGN KEY ( Idparente )
		REFERENCES Destination ( Id )
Go

ALTER TABLE DossierResa
	ADD CONSTRAINT Dossierresa_Client_Fk FOREIGN KEY ( Idclient )
		REFERENCES Client ( Id )
Go

ALTER TABLE DossierResa
	ADD CONSTRAINT Dossierresa_Etatdossier_Fk FOREIGN KEY ( Idetatdossier )
		REFERENCES Etatdossier ( Id )
Go

ALTER TABLE DossierResa
	ADD CONSTRAINT Dossierresa_Voyage_Fk FOREIGN KEY ( Idvoyage )
		REFERENCES Voyage ( Id )
Go

ALTER TABLE Photo
	ADD CONSTRAINT Photo_Destination_Fk FOREIGN KEY ( Iddestination )
		REFERENCES Destination ( Id )
Go

ALTER TABLE Voyage
	ADD CONSTRAINT Voyage_Destination_Fk FOREIGN KEY ( Iddestination )
		REFERENCES Destination ( Id )
Go

ALTER TABLE Voyageur
	ADD CONSTRAINT Voyageur_Personne_Fk FOREIGN KEY ( Id )
		REFERENCES Personne ( Id )
Go

ALTER TABLE Voyageur
	ADD CONSTRAINT Voyageur_Voyage_Fk FOREIGN KEY ( Idvoyage )
		REFERENCES Voyage ( Id )
Go
