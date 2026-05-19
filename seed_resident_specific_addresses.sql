-- BSMART resident-specific address seed
-- Run this after the resident/health-record seed scripts.

CREATE DATABASE IF NOT EXISTS bsmart_db;
USE bsmart_db;

DROP PROCEDURE IF EXISTS sp_PrepareResidentAddressSeed;
DELIMITER $$

CREATE PROCEDURE sp_PrepareResidentAddressSeed()
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE()
          AND TABLE_NAME = 'health_records'
          AND COLUMN_NAME = 'address'
    ) THEN
        ALTER TABLE health_records ADD COLUMN address VARCHAR(255) NULL;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE()
          AND TABLE_NAME = 'users'
          AND COLUMN_NAME = 'first_name'
    ) THEN
        ALTER TABLE users ADD COLUMN first_name VARCHAR(100) NULL;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE()
          AND TABLE_NAME = 'users'
          AND COLUMN_NAME = 'last_name'
    ) THEN
        ALTER TABLE users ADD COLUMN last_name VARCHAR(100) NULL;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE()
          AND TABLE_NAME = 'users'
          AND COLUMN_NAME = 'birthday'
    ) THEN
        ALTER TABLE users ADD COLUMN birthday DATE NULL;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE()
          AND TABLE_NAME = 'users'
          AND COLUMN_NAME = 'address'
    ) THEN
        ALTER TABLE users ADD COLUMN address VARCHAR(255) NULL;
    END IF;
END$$

DELIMITER ;

CALL sp_PrepareResidentAddressSeed();
DROP PROCEDURE IF EXISTS sp_PrepareResidentAddressSeed;

DROP TEMPORARY TABLE IF EXISTS tmp_resident_addresses;
CREATE TEMPORARY TABLE tmp_resident_addresses (
    barangay VARCHAR(100) NOT NULL,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    birthday DATE NOT NULL,
    address VARCHAR(255) NOT NULL
);

INSERT INTO tmp_resident_addresses (barangay, first_name, last_name, birthday, address) VALUES
('Cabadiangan','Rafael','Abante','1987-04-12','Cabadiangan Public Market'),
('Cabadiangan','Leonora','Buenaventura','1974-09-08','Sitio Proper, Cabadiangan'),
('Cabadiangan','Marcelo','Cabrera','1998-01-17','Sitio Riverside, Cabadiangan'),
('Cabadiangan','Florencia','Dela Pena','1981-06-30','Cabadiangan Chapel Road'),
('Cabadiangan','Gregorio','Escobar','1963-11-22','Upper Cabadiangan'),
('Cabadiangan','Haydee','Fuentes','1992-03-05','Lower Cabadiangan'),
('Cabadiangan','Isidro','Gonzales','1985-12-01','Cabadiangan Elementary School Area'),
('Cabadiangan','Juliana','Hernandez','1996-02-14','Sitio Centro, Cabadiangan'),
('Cabadiangan','Karlo','Ibanez','2002-07-19','Cabadiangan Health Center Road'),
('Cabadiangan','Lourdes','Jacinto','1968-10-03','Cabadiangan Barangay Hall Area'),

('Calero','Jose','Reyes','1980-03-12','Calero Public Market'),
('Calero','Lorna','Dela Cruz','1994-07-25','Sitio Proper, Calero'),
('Calero','Ramon','Bautista','1966-01-05','Calero Chapel Road'),
('Calero','Cristina','Villanueva','1998-09-14','Sitio Riverside, Calero'),
('Calero','Eduardo','Manalo','1976-11-30','Upper Calero'),
('Calero','Rosita','Flores','1988-06-27','Lower Calero'),
('Calero','Samuel','Gomez','1982-01-12','Calero Elementary School Area'),
('Calero','Teresita','Hidalgo','1997-09-30','Sitio Centro, Calero'),
('Calero','Ulysses','Ilagan','1976-12-21','Calero Health Center Road'),
('Calero','Veronica','Jumao-as','1990-02-06','Calero Barangay Hall Area'),

('Catarman','Maria','Garcia','1988-05-20','Catarman Public Market'),
('Catarman','Pedro','Santos','1971-02-14','Sitio Proper, Catarman'),
('Catarman','Anna','Cruz','2004-08-08','Catarman Chapel Road'),
('Catarman','Roberto','Mendoza','1985-12-01','Sitio Riverside, Catarman'),
('Catarman','Teresa','Navarro','1959-04-18','Upper Catarman'),
('Catarman','Felipe','Paredes','1962-05-30','Lower Catarman'),
('Catarman','Gemma','Quiros','1989-09-18','Catarman Elementary School Area'),
('Catarman','Hector','Ramos','1998-12-04','Sitio Centro, Catarman'),
('Catarman','Irene','Sarmiento','1973-06-25','Catarman Health Center Road'),
('Catarman','Joel','Taneo','1986-10-13','Catarman Barangay Hall Area'),

('Cotcot','Marco','Aquino','1993-06-15','Cotcot Public Market'),
('Cotcot','Nelia','Soriano','1979-10-22','Sitio Proper, Cotcot'),
('Cotcot','Carlos','Torres','1997-03-30','Cotcot Market Road'),
('Cotcot','Gloria','Ramos','1972-07-11','Cotcot Chapel Road'),
('Cotcot','Dante','Flores','1955-01-28','Upper Cotcot'),
('Cotcot','Paolo','Zamora','1965-11-01','Lower Cotcot'),
('Cotcot','Queenie','Andales','1994-06-20','Cotcot Elementary School Area'),
('Cotcot','Rolando','Bering','1997-08-12','Sitio Centro, Cotcot'),
('Cotcot','Susan','Ceniza','1975-12-29','Cotcot Health Center Road'),
('Cotcot','Teofilo','Duran','1983-10-10','Cotcot Barangay Hall Area'),

('Jubay','Marco','Aquino','1993-06-15','Jubay Public Market'),
('Jubay','Nelia','Soriano','1979-10-22','Sitio Proper, Jubay'),
('Jubay','Carlos','Torres','1997-03-30','Jubay Market'),
('Jubay','Gloria','Ramos','1972-07-11','Jubay Chapel Road'),
('Jubay','Dante','Flores','1955-01-28','Upper Jubay'),
('Jubay','Zandro','Javier','1963-06-12','Lower Jubay'),
('Jubay','Amelia','Kwan','1996-12-05','Jubay Elementary School Area'),
('Jubay','Brando','Lazaro','1992-04-14','Sitio Centro, Jubay'),
('Jubay','Carina','Mendoza','1968-10-31','Jubay Health Center Road'),
('Jubay','Dante','Neri','1981-02-08','Jubay Barangay Hall Area'),

('Lataban','Ricardo','Jimenez','1986-07-08','Lataban Public Market'),
('Lataban','Felicidad','Lim','1996-11-14','Sitio Proper, Lataban'),
('Lataban','Nestor','Maceda','1973-05-21','Lataban Chapel Road'),
('Lataban','Lydia','Navarro','1982-03-09','Sitio Riverside, Lataban'),
('Lataban','Vicente','Ong','1960-09-27','Upper Lataban'),
('Lataban','Jovito','Tabada','1964-12-25','Lower Lataban'),
('Lataban','Kristine','Uy','1995-03-30','Lataban Elementary School Area'),
('Lataban','Lando','Villamor','1986-07-07','Sitio Centro, Lataban'),
('Lataban','Maribel','Wong','1974-09-16','Lataban Health Center Road'),
('Lataban','Nestor','Ybanez','1982-01-29','Lataban Barangay Hall Area'),

('Mulao','Milagros','Pascual','1991-01-16','Mulao Public Market'),
('Mulao','Bonifacio','Quizon','1969-06-04','Sitio Proper, Mulao'),
('Mulao','Corazon','Rebolledo','1983-09-18','Mulao Chapel Road'),
('Mulao','Diego','Salcedo','1997-02-03','Sitio Riverside, Mulao'),
('Mulao','Elvira','Tan','1972-11-11','Upper Mulao'),
('Mulao','Fermin','Ureta','1965-04-24','Lower Mulao'),
('Mulao','Gina','Valdez','1993-08-08','Mulao Elementary School Area'),
('Mulao','Horacio','Yap','1987-12-13','Sitio Centro, Mulao'),
('Mulao','Imelda','Zosa','1976-05-20','Mulao Health Center Road'),
('Mulao','Jasper','Aranas','2002-10-02','Mulao Barangay Hall Area'),

('Poblacion','Danilo','Urbano','1995-02-18','Poblacion Public Market'),
('Poblacion','Erlinda','Valencia','1983-07-07','Poblacion Proper'),
('Poblacion','Fernando','Wenceslao','1971-01-30','Poblacion Chapel Road'),
('Poblacion','Grace','Xavier','1998-09-05','Poblacion Riverside'),
('Poblacion','Henry','Ylanan','1964-06-11','Upper Poblacion'),
('Poblacion','Ivy','Zamora','1990-12-22','Lower Poblacion'),
('Poblacion','Jonathan','Alcantara','1985-03-16','Poblacion Elementary School Area'),
('Poblacion','Katrina','Bautista','2002-08-28','Poblacion Centro'),
('Poblacion','Leo','Castillo','1976-10-14','Poblacion Health Center Road'),
('Poblacion','Myrna','Del Rosario','1980-04-01','Poblacion Barangay Hall Area'),

('San Roque','Josefa','Aguilar','1989-04-14','San Roque Public Market'),
('San Roque','Lorenzo','Batungbakal','1974-08-22','Sitio Proper, San Roque'),
('San Roque','Marina','Cortes','1997-05-06','San Roque Chapel Road'),
('San Roque','Nicanor','Dela Cruz','1981-11-19','Sitio Riverside, San Roque'),
('San Roque','Ophelia','Estrada','1970-02-25','Upper San Roque'),
('San Roque','Patricio','Fernandez','1963-07-03','Lower San Roque'),
('San Roque','Querida','Gaviola','1992-12-09','San Roque Elementary School Area'),
('San Roque','Roberto','Hernando','1986-06-21','Sitio Centro, San Roque'),
('San Roque','Salome','Ilustrisimo','1975-01-15','San Roque Health Center Road'),
('San Roque','Tomas','Jaranilla','2000-09-27','San Roque Barangay Hall Area'),

('San Vicente','Teodoro','Fajardo','1992-03-27','San Vicente Public Market'),
('San Vicente','Ursula','Galang','1978-10-15','Sitio Proper, San Vicente'),
('San Vicente','Vicente','Hilario','1968-05-05','San Vicente Chapel Road'),
('San Vicente','Wendy','Ibarra','1999-12-11','Sitio Riverside, San Vicente'),
('San Vicente','Xavier','Jimenez','1984-07-02','Upper San Vicente'),
('San Vicente','Yvette','Kangleon','1990-02-19','Lower San Vicente'),
('San Vicente','Zaldy','Ledesma','1964-08-30','San Vicente Elementary School Area'),
('San Vicente','Analyn','Magsaysay','1993-04-17','Sitio Centro, San Vicente'),
('San Vicente','Benedicto','Nolasco','1980-11-23','San Vicente Health Center Road'),
('San Vicente','Clarita','Ong','1973-06-06','San Vicente Barangay Hall Area'),

('Santa Cruz','Yolanda','Katipunan','1987-08-11','Santa Cruz Public Market'),
('Santa Cruz','Zosimo','Lacson','1970-03-29','Sitio Proper, Santa Cruz'),
('Santa Cruz','Aurora','Mendoza','1982-12-13','Santa Cruz Chapel Road'),
('Santa Cruz','Benjamin','Natividad','1996-05-04','Sitio Riverside, Santa Cruz'),
('Santa Cruz','Carmela','Ople','2001-10-08','Upper Santa Cruz'),
('Santa Cruz','Dario','Palma','1962-01-20','Lower Santa Cruz'),
('Santa Cruz','Estrella','Quilaton','1991-06-28','Santa Cruz Elementary School Area'),
('Santa Cruz','Felix','Rosales','1983-09-09','Sitio Centro, Santa Cruz'),
('Santa Cruz','Geraldine','Santos','1974-02-02','Santa Cruz Health Center Road'),
('Santa Cruz','Herminio','Tolentino','1998-07-30','Santa Cruz Barangay Hall Area'),

('Tabla','Dionisio','Padilla','1982-09-13','Tabla Public Market'),
('Tabla','Esther','Quijano','1995-04-01','Sitio Proper, Tabla'),
('Tabla','Fausto','Rivera','1971-11-15','Tabla Chapel Road'),
('Tabla','Glenda','Sagun','2000-02-26','Sitio Riverside, Tabla'),
('Tabla','Homer','Tampus','1964-06-17','Upper Tabla'),
('Tabla','Imee','Ureta','1988-01-05','Lower Tabla'),
('Tabla','Jerson','Valencia','1992-08-20','Tabla Elementary School Area'),
('Tabla','Kessa','Wenceslao','1976-03-12','Sitio Centro, Tabla'),
('Tabla','Leandro','Xerez','1985-10-04','Tabla Health Center Road'),
('Tabla','Monica','Yape','1997-12-18','Tabla Barangay Hall Area'),

('Tayud','Lisa','Bautista','1991-06-10','Tayud Public Market'),
('Tayud','Kate','Mae','2001-01-01','Pagutlan, Tayud'),
('Tayud','Anna','Garcia','1980-03-15','Tayud Chapel Road'),
('Tayud','Pedro','Santos','1996-07-22','Sitio Riverside, Tayud'),
('Tayud','Maria','Reyes','1971-09-05','Upper Tayud'),
('Tayud','Rico','Magno','1997-11-18','Lower Tayud'),
('Tayud','Sofia','Villanueva','1985-02-17','Tayud Elementary School Area'),
('Tayud','Miguel','Torres','1988-12-19','Sitio Centro, Tayud'),
('Tayud','Elena','Cruz','1993-04-26','Tayud Health Center Road'),
('Tayud','Jonas','Dela Torre','1966-09-14','Tayud Barangay Hall Area'),

('Yati','Imelda','Umali','1984-05-28','Yati Public Market'),
('Yati','Julian','Velasco','1973-10-16','Sitio Proper, Yati'),
('Yati','Katherine','Wong','2002-03-03','Yati Chapel Road'),
('Yati','Leopoldo','Xavier','1965-12-29','Sitio Riverside, Yati'),
('Yati','Marissa','Ybanez','1990-07-11','Upper Yati'),
('Yati','Nelson','Zamora','1986-01-24','Lower Yati'),
('Yati','Ofelia','Abella','1969-08-08','Yati Elementary School Area'),
('Yati','Primo','Basilio','1981-04-15','Sitio Centro, Yati'),
('Yati','Queenie','Cabanlit','1996-11-19','Yati Health Center Road'),
('Yati','Renato','Dacles','1999-06-05','Yati Barangay Hall Area');

UPDATE health_records h
JOIN barangays b ON b.id = h.barangay_id
JOIN tmp_resident_addresses a
  ON a.barangay COLLATE utf8mb4_unicode_ci = b.name COLLATE utf8mb4_unicode_ci
 AND LOWER(TRIM(a.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(h.first_name)) COLLATE utf8mb4_unicode_ci
 AND LOWER(TRIM(a.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci
 AND DATE(a.birthday) = DATE(h.birthday)
SET h.address = a.address
WHERE h.address IS NULL OR TRIM(h.address) = '' OR h.address LIKE 'Sitio %, %';

UPDATE users u
JOIN barangays b ON b.id = u.barangay_id
JOIN tmp_resident_addresses a
  ON a.barangay COLLATE utf8mb4_unicode_ci = b.name COLLATE utf8mb4_unicode_ci
 AND LOWER(TRIM(a.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(u.first_name)) COLLATE utf8mb4_unicode_ci
 AND LOWER(TRIM(a.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(u.last_name)) COLLATE utf8mb4_unicode_ci
 AND (u.birthday IS NULL OR DATE(a.birthday) = DATE(u.birthday))
SET u.address = a.address
WHERE LOWER(TRIM(COALESCE(u.role, ''))) = 'resident'
  AND (u.address IS NULL OR TRIM(u.address) = '' OR u.address LIKE 'Sitio %, %');

-- Fill any remaining resident addresses that were not part of the named seed list.
-- This catches older barangay LGU sample records such as Norma Torres, Herminia Yap,
-- Perita Castillo, and other records from the individual barangay scripts.
UPDATE health_records h
JOIN barangays b ON b.id = h.barangay_id
SET h.address = CONCAT(
    CASE MOD(h.ID, 6)
        WHEN 0 THEN 'Public Market Area'
        WHEN 1 THEN 'Barangay Hall Area'
        WHEN 2 THEN 'Health Center Road'
        WHEN 3 THEN 'Chapel Road'
        WHEN 4 THEN 'Elementary School Area'
        ELSE 'Sitio Riverside'
    END,
    ', ',
    b.name
)
WHERE h.address IS NULL OR TRIM(h.address) = '';

UPDATE users u
JOIN barangays b ON b.id = u.barangay_id
SET u.address = CONCAT(
    CASE MOD(u.id, 6)
        WHEN 0 THEN 'Public Market Area'
        WHEN 1 THEN 'Barangay Hall Area'
        WHEN 2 THEN 'Health Center Road'
        WHEN 3 THEN 'Chapel Road'
        WHEN 4 THEN 'Elementary School Area'
        ELSE 'Sitio Riverside'
    END,
    ', ',
    b.name
)
WHERE LOWER(TRIM(COALESCE(u.role, ''))) = 'resident'
  AND (u.address IS NULL OR TRIM(u.address) = '');

-- If a resident account still has a blank address but has a matching health record,
-- copy the health-record address into the account.
UPDATE users u
JOIN barangays b ON b.id = u.barangay_id
JOIN health_records h
  ON h.barangay_id = u.barangay_id
 AND LOWER(TRIM(h.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(u.first_name)) COLLATE utf8mb4_unicode_ci
 AND LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(u.last_name)) COLLATE utf8mb4_unicode_ci
 AND (u.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE(u.birthday))
SET u.address = h.address
WHERE LOWER(TRIM(COALESCE(u.role, ''))) = 'resident'
  AND (u.address IS NULL OR TRIM(u.address) = '')
  AND h.address IS NOT NULL
  AND TRIM(h.address) <> '';

SELECT b.name AS Barangay, h.first_name, h.last_name, h.address
FROM health_records h
JOIN barangays b ON b.id = h.barangay_id
WHERE h.is_archived = 0
ORDER BY b.name, h.first_name, h.last_name;
