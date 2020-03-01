insert into Personne(TypePers, Civilite, Nom, Prenom, Email, Telephone, Datenaissance)
values (1, 'Mr', 'Smith', 'John', 'test@test.com', '0123456789', '1990-10-05'),
(2, 'Mme', 'Smith', 'Jeanne', 'jeanne.smith@gmail.com', '0987654321', '1987-03-28'),
(2, 'Mme', 'Hopkins', 'Marie', 'marie.hopkins@gmail.com', '0567891234', '1979-12-05'),
(1, 'Mme', 'Swim', 'Dory', 'dory.swim@gmail.com', '0654123789', '1991-02-19'),
(2, 'Mr', 'Lefranc', 'Jean', 'jean.lefranc@gmail.com', '0789123456', '1982-12-21'),
(2, 'Mr', 'Depp', 'Johnny', 'Johnny.depp@gmail.com', '9081726354', '1975-07-08')

go

insert into Destination(Nom, Niveau, Description)
values
('Europe ', 1, 'Terre des rois de naguère.'),
('Afrique ', 1, 'De Casablanca à Pretoria'),
('Oceanie ', 1, 'Les plus grands archipels à votre portée.'),
('Asie ', 1, 'De la Chine au Japon, en passant par le Tibet.'),
('Amerique ', 1, 'A la découverte du nouveau-monde.')
go


insert into Destination(Nom, Niveau,IdParente, Description)
values
('France', 2,1, 'Bérêt, baguette et camembert.'),
('Birmanie', 2, 4, 'La Birmanie est un pays passionnant pour tous ceux qui s’intéressent à l''art, aux civilisations, à l''hindouisme. Ce pays s''ouvre et a conservé toute la richesse de son patrimoine culturel. Visitez les temples, les marchés, ...'),
('Canada', 2, 5,'Découvrez la nature généreuse et les grandes villes du Canada en toute saison, grâce aux nombreux circuits que nous avons élaborés.'),
('Etats-Unis', 2, 5,'Découvrez la nature généreuse et les grandes villes du Canada en toute saison, grâce aux nombreux circuits que nous avons élaborés.'),
('Chine', 2, 4, 'Découvrez la république poulaire de Chine, le plus grand pays d''Asie orientale.'),
('Japon', 2, 4,'Le pays des shoguns, de Hokkaido à Kyushu en passant par la région du Shikoku.'),
('Brésil', 2,5, 'Decouvrez la culture locale avec Rio et son carnaval, ainsi que la culture ancestrale grâce au Machu Pichu. '),
('Viêt Nam', 2,4, 'Découvrez  ce pays d''Asie du Sud-Est situé à l''est de la péninsule indochinoise.'),
('Russie', 2, 1, 'Toundra, dictature et vodka.'),
('Perou', 2,5, 'Longez la cordillère des Andes.'),
('Autriche', 2, 1,'Visitez le chateau de l''impératrice Sissi et goutez aux schnitzels!'),
('Mexique', 2,5, 'Ay Caramba!!'),
('Italie', 2,1, 'Parcourez les villes de Bolognes, Rome ou Venise.'),
('Afrique du Sud', 2,2, 'Visitez l''extrémité australe du continent africain.'),
('Sénégal', 2,2, 'Découvrez ce pays d''Afrique de l''Ouest et sa capitale Dakar.'),
('Tunisie', 2,2, 'Héritière de l''ancienne Carthage.'),
('Australie', 2,3, 'Les koalas, les boomerangs et Crocodile Dundee')
go



insert into Destination(Nom, Niveau,IdParente, Description)
values
('Bretagne ', 3, 6, 'Superbe région. Terre de légendes. De nombreux spots pour le surf et le kitesurf.'),
('Guadeloupe', 3, 6,'Dans un site exceptionnel,en bordure d''un petit lagon turquoise, tout est réuni pour un séjour paradisiaque. Découvrez les merveilles de grande terre et de basse terre, les joies des plongées dans la réserve naturelle.'),
('Hokkaido', 3, 11,''),
('belo horizonte', 3, 12,''),
('Toscane', 3, 18,''),
('KwaZulu-Natal', 3, 19,''),
('Québec', 3, 8,''),
('Californie', 3, 9,''),
('Saint-Barthélémy', 3,6,'Imaginez une île où il fait 26 à 28 °C toute l''année. Baignez vous dans une eau turquoise.')
go


insert into Photo(NomFichier , IdDestination)
values 
('/Images/europe.jpg', 1),
('/Images/afrique.jpg', 2),
('/Images/Oceanie.jpg', 3),
('/Images/Asie.jpg', 4),
('/Images/Amerique.jpg', 5),
('/Images/france_1.jpg', 6),
('/Images/birmanie_1.jpg', 7),
('/Images/birmanie_2.jpg', 7),
('/Images/birmanie_3.jpg', 7),
('/Images/canada_1.jpg', 8),
('/Images/canada_2.jpg', 8),
('/Images/Etats_unis.jpg', 9),
('/Images/chine.jpg', 10),
('/Images/japon.jpg', 11),
('/Images/Bresil.jpg', 12),
('/Images/VietNam.jpg', 13),
('/Images/russie.jpg', 14),
('/Images/perou.jpg', 15),
('/Images/autriche.jpg', 16),
('/Images/Mexique.jpg', 17),
('/Images/Italie.jpg', 18),
('/Images/AfriqueDuSud.jpg', 19),
('/Images/Senegal.jpg', 20),
('/Images/Tunisie.jpg', 21),
('/Images/Australie.jpg', 22),
('/Images/bretagne_1.jpg', 23),
('/Images/guadeloupe_1.jpg', 24),
('/Images/guadeloupe_2.jpg', 24),
('/Images/Hokkaido.jpg', 25),
('/Images/BeloHorizonte.jpg', 26),
('/Images/Toscane.jpg', 27),
('/Images/Kwazulu.jpg', 28),
('/Images/Quebec.jpg', 29),
('/Images/californie.jpg', 30),
('/Images/saint-barth_1.jpg', 31),
('/Images/saint-barth_2.jpg', 31)


go

insert into Voyage(IdDestination, DateDepart, DateRetour, PlacesDispo, PrixHT, Reduction, Descriptif)
values (24, '2020-03-05', '2020-03-15', 10, 800.00, 0.20, 'Superbe voyage en Guadeloupe - Parcours 1'),
(24, '2020-03-15', '2020-03-25', 12, 820.00, 0.20, 'Superbe voyage en Guadeloupe - Parcours 2'),
(31, '2020-03-05', '2020-03-15', 10, 700.00, 0.20, 'Superbe voyage à Saint-Barthélémy - Parcours 1'),
(31, '2020-03-15', '2020-03-25', 11, 720.00, 0.20, 'Superbe voyage à Saint-Barthélémy - Parcours 2'),
(7, '2020-03-05', '2020-03-15', 10, 600.00, 0.20, 'Superbe voyage en Birmanie - Parcours 1'),
(7, '2020-03-15', '2020-03-25', 10, 620.00, 0.20, 'Superbe voyage en Birmanie - Parcours 2'),
(7, '2020-03-25', '2020-04-05', 10, 630.00, 0.20, 'Superbe voyage en Birmanie - Parcours 3'),
(8, '2020-03-05', '2020-03-15', 10, 800.00, 0.20, 'Superbe voyage au Canada - Parcours 1'),
(8, '2020-03-15', '2020-03-25', 10, 820.00, 0.20, 'Superbe voyage au Canada - Parcours 2'),
(23, '2020-03-05', '2020-03-15', 10, 100.00, 0.20, 'Superbe voyage en Bretagne'),
(2, '2020-06-02', '2020-06-30', 13, 1250.00, 0.20, 'Superbe voyage en Afrique - Parcours 1'),
(1, '2020-04-06', '2020-04-18', 7, 1450.00, 0.20, 'Road-Trip en Europe - Parcours 2'),
(1, '2020-03-01', '2020-03-7', 15, 800.00, 0.20, 'Road-Trip en Europe - Parcours 1'),
(3, '2020-08-01', '2020-08-7', 6, 650.00, 0.20, 'Superbe voyage en Océanie - Parcours 1'),
(4, '2020-07-19', '2020-07-30', 4, 550.00, 0.20, 'Superbe voyage en Asie - Parcours 1'),
(5, '2020-04-16', '2020-04-25', 10, 560.00, 0.20, 'Superbe voyage en Amérique - Parcours 1'),
(5, '2020-05-01', '2020-05-14', 10, 750.00, 0.20, 'Superbe voyage en Amérique - Parcours 2'),
(6, '2020-08-02', '2020-08-14', 12, 950.00, 0.20, 'Superbe voyage en France - Parcours 1'),
(9, '2020-04-02', '2020-05-01', 11, 1050.00, 0.20, 'Superbe voyage aux USA - Parcours 1'),
(9, '2020-05-09', '2020-05-18', 89, 830.00, 0.20, 'Superbe voyage aux USA - Parcours 1'),
(9, '2020-06-04', '2020-06-09', 6, 610.00, 0.20, 'Superbe voyage aux USA - Parcours 1'),
(10, '2020-05-28', '2020-06-09',3, 250.00, 0.20, 'Superbe voyage en Chine - Parcours 1'),
(11, '2020-04-04', '2020-04-11', 7, 800.00, 0.20, 'Superbe voyage au Japon - Parcours 1'),
(12, '2020-06-05', '2020-06-09', 15, 280.00, 0.20, 'Superbe voyage au Brésil - Parcours 1'),
(13, '2020-07-01', '2020-07-08', 14, 380.00, 0.20, 'Superbe voyage au Viêt Nam - Parcours 1'),
(14, '2020-03-25', '2020-04-02', 6, 640.00, 0.20, 'Superbe voyage en Russie - Parcours 1'),
(15, '2020-05-04', '2020-05-09', 10, 490.00, 0.20, 'Superbe voyage au Perou - Parcours 1'),
(16, '2020-07-02', '2020-07-11', 10, 350.00, 0.20, 'Superbe voyage en Autriche - Parcours 1'),
(17, '2020-08-01', '2020-08-06', 8, 280.00, 0.20, 'Superbe voyage au Mexique - Parcours 1'),
(18, '2020-05-18', '2020-05-29', 5, 750.00, 0.20, 'Superbe voyage en Italie - Parcours 1'),
(19, '2020-03-20', '2020-03-27', 14, 820.00, 0.20, 'Superbe voyage au Afrique du Sud - Parcours 1'),
(20, '2020-05-12', '2020-05-18', 10, 360.00, 0.20, 'Superbe voyage au Sénégal - Parcours 1'),
(21, '2020-06-01', '2020-06-04', 6, 250.00, 0.20, 'Superbe voyage en Tunisie - Parcours 1'),
(22, '2020-04-05', '2020-04-09', 9, 510.00, 0.20, 'Superbe voyage en Australie - Parcours 1'),
(25, '2020-06-11', '2020-06-17', 7, 790.00, 0.20, 'Superbe voyage en Hokkaido - Parcours 1'),
(25, '2020-05-18', '2020-05-29', 10, 1090.00, 0.20, 'Superbe voyage en Hokkaido - Parcours 2'),
(25, '2020-06-09', '2020-06-13', 8, 690.00, 0.20, 'Superbe voyage en Hokkaido - Parcours 3'),
(26, '2020-08-02', '2020-08-07', 6, 420.00, 0.20, 'Superbe voyage à belo horizonte - Parcours 1'),
(27, '2020-05-16', '2020-05-25', 8, 660.00, 0.20, 'Superbe voyage en Toscane - Parcours 1'),
(28, '2020-06-01', '2020-06-06', 10, 450.00, 0.20, 'Superbe voyage en KwaZulu-Natal - Parcours 1'),
(29, '2020-05-04', '2020-05-10', 10, 960.00, 0.20, 'Superbe voyage au Québec - Parcours 1'),
(30, '2020-06-04', '2020-06-09', 5, 840.00, 0.20, 'Superbe voyage en Californie - Parcours 1')




