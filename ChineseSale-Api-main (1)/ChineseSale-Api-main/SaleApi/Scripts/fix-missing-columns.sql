-- Run once if add-to-bag fails with "Invalid column name 'Quantity'"
IF COL_LENGTH('Bags', 'Quantity') IS NULL
BEGIN
    ALTER TABLE Bags ADD Quantity int NOT NULL CONSTRAINT DF_Bags_Quantity DEFAULT 1;
END
GO

IF COL_LENGTH('Orders', 'OrderGroupId') IS NULL
BEGIN
    ALTER TABLE Orders ADD OrderGroupId int NOT NULL CONSTRAINT DF_Orders_OrderGroupId DEFAULT 0;
END
GO
