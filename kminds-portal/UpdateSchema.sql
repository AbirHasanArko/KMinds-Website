-- Script to update the existing KMindsPortal database schema
-- Run this script in SQL Server Management Studio (SSMS) against your [KMindsPortal] database

ALTER TABLE Users
ADD 
    Department NVARCHAR(50) NULL,
    RollNumber NVARCHAR(20) NULL,
    YearTerm NVARCHAR(10) NULL;

GO
