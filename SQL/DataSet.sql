insert into Personne(TypePers, Civilite, Nom, Prenom, Email, Telephone, Datenaissance)
values (1, 'Mr', 'Smith', 'John', 'john.smith@gmail.com', '0123456789', '1990-10-05'),
(2, 'Mme', 'Smith', 'Jeanne', 'jeanne.smith@gmail.com', '0987654321', '1987-03-28'),
(2, 'Mme', 'Hopkins', 'Marie', 'marie.hopkins@gmail.com', '0567891234', '1979-12-05'),
(1, 'Mme', 'Swim', 'Dory', 'dory.swim@gmail.com', '0654123789', '1991-02-19'),
(2, 'Mr', 'Lefranc', 'Jean', 'jean.lefranc@gmail.com', '0789123456', '1982-12-21'),
(2, 'Mr', 'Depp', 'Johnny', 'Johnny.depp@gmail.com', '9081726354', '1975-07-08')

go

insert into Destination(Nom, Niveau, Description)
values ('Guadeloupe', 3, 'Dans un site exceptionnel,en bordure d''un petit lagon turquoise, tout est réuni pour un séjour paradisiaque. Découvrez les merveilles de grande terre et de basse terre, les joies des plongées dans la réserve naturelle.'),
('Saint-Barthélémy', 3, 'Imaginez une île où il fait 26 à 28 °C toute l''année. Baignez vous dans une eau turquoise.'),
('Birmanie', 2, 'La Birmanie est un pays passionnant pour tous ceux qui s’intéressent à l''art, aux civilisations, à l''hindouisme. Ce pays s''ouvre et a conservé toute la richesse de son patrimoine culturel. Visitez les temples, les marchés, ...'),
('Canada ', 2, 'Découvrez la nature généreuse et les grandes villes du Canada en toute saison, grâce aux nombreux circuits que nous avons élaborés.'),
('Bretagne ', 3, 'Superbe région. Terre de légendes. De nombreux spots pour le surf et le kitesurf.')

go

insert into Photo(NomFichier , IdDestination)
values ('~/Images/birmanie_1.jpg', 3),
('~/Images/birmanie_2.jpg', 3),
('~/Images/birmanie_3.jpg', 3),
('~/Images/bretagne_1.jpg', 5),
('~/Images/canada_1.jpg', 4),
('~/Images/canada_2.jpg', 4),
('~/Images/guadeloupe_1.jpg', 1),
('~/Images/guadeloupe_2.jpg', 1),
('~/Images/saint-barth_1.jpg', 2),
('~/Images/saint-barth_2.jpg', 2)

go

insert into Voyage(IdDestination, DateDepart, DateRetour, PlacesDispo, PrixHT, Reduction, Descriptif)
values (1, '2020-03-05', '2020-03-15', 10, 800.00, 0.20, 'Superbe voyage en Guadeloupe - Parcours 1'),
(1, '2020-03-15', '2020-03-25', 12, 820.00, 0.20, 'Superbe voyage en Guadeloupe - Parcours 2'),
(2, '2020-03-05', '2020-03-15', 10, 700.00, 0.20, 'Superbe voyage à Saint-Barthélémy - Parcours 1'),
(2, '2020-03-15', '2020-03-25', 11, 720.00, 0.20, 'Superbe voyage à Saint-Barthélémy - Parcours 2'),
(3, '2020-03-05', '2020-03-15', 10, 600.00, 0.20, 'Superbe voyage en Birmanie - Parcours 1'),
(3, '2020-03-15', '2020-03-25', 10, 620.00, 0.20, 'Superbe voyage en Birmanie - Parcours 2'),
(3, '2020-03-25', '2020-04-05', 10, 630.00, 0.20, 'Superbe voyage en Birmanie - Parcours 3'),
(4, '2020-03-05', '2020-03-15', 10, 800.00, 0.20, 'Superbe voyage au Canada - Parcours 1'),
(4, '2020-03-15', '2020-03-25', 10, 820.00, 0.20, 'Superbe voyage au Canada - Parcours 2'),
(5, '2020-03-05', '2020-03-15', 10, 100.00, 0.20, 'Superbe voyage en Bretagne')
