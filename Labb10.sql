SELECT products.productname, products.unitprice, Categories.CategoryName 
FROM Products, Categories
ORDER BY Categories.CategoryName, Products.ProductName
GO

SELECT customers.CompanyName, COUNT(orders.orderID) as "Antal Orders"
FROM Customers
JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY customers.CompanyName
ORDER BY [Antal Orders] DESC
GO

SELECT Employees.FirstName, Employees.LastName, Territories.TerritoryDescription
FROM Employees
JOIN EmployeeTerritories ON employees.EmployeeID = employeeterritories.EmployeeID
JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID
ORDER BY Employees.FirstName