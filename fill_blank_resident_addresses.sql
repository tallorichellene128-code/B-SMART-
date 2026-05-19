-- BSMART cleanup: fill blank resident addresses
-- Run this after your main BSMART SQL scripts.

CREATE DATABASE IF NOT EXISTS bsmart_db;
USE bsmart_db;

DROP PROCEDURE IF EXISTS sp_FillBlankResidentAddresses;
DELIMITER $$

CREATE PROCEDURE sp_FillBlankResidentAddresses()
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE()
          AND TABLE_NAME = 'users'
          AND COLUMN_NAME = 'address'
    ) THEN
        ALTER TABLE users ADD COLUMN address VARCHAR(255) NULL;
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE()
          AND TABLE_NAME = 'health_records'
          AND COLUMN_NAME = 'address'
    ) THEN
        ALTER TABLE health_records ADD COLUMN address VARCHAR(255) NULL;
    END IF;

    UPDATE users u
    JOIN barangays b ON b.id = u.barangay_id
    SET u.address = CONCAT(
        'Sitio ',
        CASE MOD(u.id, 5)
            WHEN 0 THEN 'Proper'
            WHEN 1 THEN 'Centro'
            WHEN 2 THEN 'Riverside'
            WHEN 3 THEN 'Upper'
            ELSE 'Lower'
        END,
        ', ',
        b.name
    )
    WHERE LOWER(TRIM(COALESCE(u.role, ''))) = 'resident'
      AND (u.address IS NULL OR TRIM(u.address) = '');

    UPDATE health_records h
    JOIN barangays b ON b.id = h.barangay_id
    SET h.address = CONCAT(
        'Sitio ',
        CASE MOD(h.ID, 5)
            WHEN 0 THEN 'Proper'
            WHEN 1 THEN 'Centro'
            WHEN 2 THEN 'Riverside'
            WHEN 3 THEN 'Upper'
            ELSE 'Lower'
        END,
        ', ',
        b.name
    )
    WHERE h.address IS NULL OR TRIM(h.address) = '';
END$$

DELIMITER ;

CALL sp_FillBlankResidentAddresses();
DROP PROCEDURE IF EXISTS sp_FillBlankResidentAddresses;

SELECT 'Blank resident user addresses remaining' AS check_name, COUNT(*) AS total
FROM users
WHERE LOWER(TRIM(COALESCE(role, ''))) = 'resident'
  AND (address IS NULL OR TRIM(address) = '');

SELECT 'Blank health record addresses remaining' AS check_name, COUNT(*) AS total
FROM health_records
WHERE address IS NULL OR TRIM(address) = '';
