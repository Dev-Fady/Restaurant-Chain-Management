-- START: سكريبت لإدخال بيانات وهمية (SQL Server)

-- (1) Cities
INSERT INTO Cities(Name) VALUES
('Cairo'),
('Giza'),
('Alexandria');

-- (2) Branches
-- نضع قيم StockId مبدئياً (سننشئ الـ Stocks بنفس الـ Ids لاحقًا)
INSERT INTO Branches (Name, CityId, ManagerId, StockId) VALUES
('Main Branch - Downtown', 1, NULL, 1),
('West Branch', 2, NULL, 2),
('North Branch', 3, NULL, 3);

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
