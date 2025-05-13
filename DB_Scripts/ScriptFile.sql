USE BasketDB
GO

--Joins
--Inner Join
--Left Join
--Right Join
--Outer Apply/Cross Appy


SELECT
	*
FROM Customers
SELECT
	*
FROM Orders

UPDATE Orders
SET CustomerId = 9
WHERE id = 3

SELECT
	*
FROM OrderDetails
SELECT
	*
FROM Items



--Left Join
SELECT
	c.*
   ,o.*
FROM Customers c
LEFT JOIN Orders o
	ON c.id = o.CustomerId


--Right Join
SELECT
	c.*
   ,o.*
FROM Customers c
RIGHT JOIN Orders o
	ON c.id = o.CustomerId

--Inner join
SELECT
	c.id
   ,c.Name AS CustomerName
   ,c.MobileNUmber
   ,o.OrderDateTime
   ,o.OrderTotalPrice
   ,os.Status AS OrderStatus
   ,i.ItemName
   ,od.UnitPrice
   ,od.OrderQuantity
   ,(od.UnitPrice * od.OrderQuantity) ItemTotal
FROM Customers c
INNER JOIN Orders o
	ON c.id = o.CustomerId
INNER JOIN OrderDetails od
	ON o.Id = od.OrderId
INNER JOIN OrderStatus os
	ON o.OrderStatus = os.Id
INNER JOIN Items i
	ON od.ItemId = i.Id

	go
	--Sp:Start
          --Create / Alter Stored Proc in SQL DB
            Alter Proc ValidateUser
            (
            @UserName nvarchar(100),
            @Password nvarchar(100)
            )
            As
            BEGIN


            IF(( SELECT
		COUNT(*)
	FROM Users
	WHERE UserName = @UserName
	AND Password = @Password)
> 1)
BEGIN

SELECT
	'0' AS Result
   ,'morethan one user exists, its not valid request.' AS Message
RETURN;
END

ELSE
IF ((SELECT
			COUNT(*)
		FROM Users
		WHERE UserName = @UserName
		AND Password = @Password)
	= 0)
BEGIN

SELECT
	'0' AS Result
   ,'No user found' AS Message
RETURN;
END
ELSE
IF EXISTS (SELECT TOP 1
			*
		FROM Users
		WHERE UserName = @UserName
		AND Password = @Password)
BEGIN
SELECT
	'1' AS Result
   ,'Valid User' AS Message;
RETURN;
END
END
--SP:End
--Exec SP Structure
EXEC ValidateUser 'naresh3'
				 ,'admin123'


SELECT
	*
FROM Customers

CREATE CLUSTERED INDEX Customers_Name_Mobilenumber
ON Customers (Name, MobileNUmber)

CREATE NONCLUSTERED INDEX Customers_Mobilenumber_IsActive
ON Customers (MobileNUmber, IsActive)