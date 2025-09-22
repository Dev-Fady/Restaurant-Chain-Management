-- START: سكريبت لإدخال بيانات وهمية (SQL Server)

-- (1) Cities
INSERT INTO Cities(Name) VALUES
('Cairo'),
('Giza'),
('Alexandria');

-- (2) Branches
-- نضع قيم StockId مبدئياً (سننشئ الـ Stocks بنفس الـ Ids لاحقًا)
INSERT INTO Branches (Name, CityId, ManagerId, StockId) VALUES
(N'فرع القاهرة',1,1,1)

--('Main Branch - Downtown', 1, NULL, 1),
--('West Branch', 2, NULL, 2),
--('North Branch', 3, NULL, 3);

-- (3) Stocks
-- Stock.BranchId تشير للـ Branchs التي أُنشئت أعلاه
INSERT INTO Stocks (Name, BranchId) VALUES
('Main Stock', 1),
('West Stock', 2),
('North Stock', 3);

-- (4) Employees
INSERT INTO Employees (Name, Title, Role, BranchId) VALUES
('Ahmed Ali', 'Branch Manager', 1, 1),
('Mona Salah', 'Sales', 2, 1),
('Omar Youssef', 'Stock Keeper', 3, 1),
('Hanan Mostafa', 'Sales', 2, 2),
('Khaled Hamed', 'Cashier', 2, 3);

-- (تحديث ManagerId)
UPDATE Branches SET ManagerId = 1 WHERE Id = 1;
UPDATE Branches SET ManagerId = 4 WHERE Id = 2;

-- (5) Products
INSERT INTO Products (Name, Des, Price, IsFavorite) VALUES
('Coffee Beans 500g', 'Premium roasted coffee beans', 120.00, 0),
('Electric Kettle', '1.7L stainless steel', 350.00, 1),
('Tea Pack', 'Green tea 200g', 45.50, 0),
('Mug Ceramic', '350ml mug', 35.00, 1);

-- (6) ImageProduct
INSERT INTO ImageProducts (ImageUrl, ProductId) VALUES
('/images/products/coffee500.jpg', 1),
('/images/products/kettle.jpg', 2),
('/images/products/tea200.jpg', 3),
('/images/products/mug.jpg', 4);

-- (7) StockProduct
INSERT INTO StockProducts (StockId, ProductId, Quantity) VALUES
(1, 1, 50),
(1, 2, 10),
(2, 2, 5),
(2, 4, 30),
(3, 3, 100);

-- (8) FavoriteProduct
INSERT INTO FavoriteProducts (ProductId, AddedDate) VALUES
(2, GETDATE()),
(4, GETDATE());

------------------------

INSERT INTO Offers (Name, Des, Price, StartDate, EndDate)
VALUES 
(N'عرض رمضان', N'بيتزا كبيرة + بيبسي', 120.00, '2025-03-01', '2025-03-31'),
(N'عرض الصيف',N'برجر + بطاطس + مشروب', 85.00, '2025-06-01', '2025-06-30'),
(N'عرض العيلة', N'2 بيتزا وسط + 1 لتر بيبسي', 200.00, '2025-07-01', '2025-07-15');

-- ربط العروض بالفروع (OfferStock)
INSERT INTO OfferStocks (Name, OfferId, BranchId, Quantity)
VALUES 
(N'عرض رمضان - فرع القاهرة', 1, 1, 50),
(N'عرض رمضان - فرع الجيزة', 1, 2, 30),
(N'عرض الصيف - فرع الاسكندرية', 2, 3, 40),
(N'عرض العيلة - فرع القاهرة', 3, 1, 20);

-- صور العروض
INSERT INTO ImageOffers (OfferId, ImageUrl)
VALUES 
(1, 'https://example.com/images/ramadan_offer1.jpg'),
(1, 'https://example.com/images/ramadan_offer2.jpg'),
(2, 'https://example.com/images/summer_offer.jpg'),
(3, 'https://example.com/images/family_offer.jpg');
-----------
-- Insert Customers
INSERT INTO Customers (Name, Title, Phone, Email, BranchId)
VALUES 
('Ahmed Ali', 'Mr.', '01012345678', 'ahmed.ali@example.com', 1),
('Sara Mohamed', 'Ms.', '01098765432', 'sara.mohamed@example.com', 1),
('Omar Hassan', 'Dr.', '01122334455', 'omar.hassan@example.com', 2),
('Mona Samir', 'Mrs.', '01233445566', 'mona.samir@example.com', 2),
('Youssef Adel', 'Mr.', '01566778899', 'youssef.adel@example.com', 3),
('Laila Nabil', 'Ms.', '01044556677', 'laila.nabil@example.com', 3);


-- تأكد من البيانات
SELECT * FROM Cities;
SELECT * FROM Branches;
SELECT * FROM Stocks;
SELECT * FROM Employees;
SELECT * FROM Products;
SELECT * FROM StockProducts;
SELECT * FROM FavoriteProducts;

-- END سكريبت

SELECT b.Id, b.Name AS BranchName, c.Name AS CityName
FROM Branches b
JOIN Cities c ON b.CityId = c.Id;

SELECT *
FROM Branches
WHERE CityId = 1;
---------------

SELECT o.Name AS OfferName, b.Name AS BranchName, os.Quantity
FROM OfferStocks os
JOIN Offers o ON os.OfferId = o.Id
JOIN Branches b ON os.BranchId = b.Id;

SELECT o.Name, o.Des, i.ImageUrl
FROM Offers o
JOIN ImageOffers i ON o.Id = i.OfferId
WHERE o.Name = N'عرض رمضان';

SELECT o.Name, os.Quantity
FROM OfferStocks os
JOIN Offers o ON os.OfferId = o.Id
JOIN Branches b ON os.BranchId = b.Id
WHERE b.Id = 1

SELECT o.Name, os.Quantity
FROM OfferStocks os
JOIN Offers o ON os.OfferId = o.Id
JOIN Branches b ON os.BranchId = b.Id
WHERE b.Name = N'فرع القاهرة';
