USE BasketDB
GO

--SQL Query for same above linq query            
select TOP 1 * from Users order by UserName--ASC
select TOP 1 * from Users order by UserName desc--DESc
select DISTINCT UserName from Users

 Select   u.Gender from Users u
 GROUP By u.Gender
 ORDER by u.UserName ASC

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



IF NOT EXISTS(Select * from Users where UserName='Raman')
BEGIN 
Insert INTO Users(UserName, Password, MobileNUmber, IsActive, CreatedDate, UpdatedDate, Gender)
VALUES('Raman', 'admin@123', '09876', 1, GETDATE(), GETDATE(), 'Male')
END

IF EXISTS(Select * from Users where UserName='Raman' and IsActive=0)
BEGIN
Update Users set IsActive=1 where UserName='Raman'
END

IF EXISTS(Select * from Users where UserName='Raman' and IsActive=1)
BEGIN
delete from Users where UserName='Raman'
END


--Create Table
drop table if exists #TEST
--Temp table Approch 1
CREATE TABLE #TEST
(
Id Int,
Name nvarchar(50),
DOB DATETIME
)

INSERT into #TEST(Id, Name, DOB)
VALUES(1,'Test1', '2025-01-01'),
(2,'Test2', '2025-01-01'),
(3,'Test3', '2025-01-01'),
(4,'Test5', '2025-01-01'),
(5,'Test6', '2025-01-01')

select * from #TEST
--Approch 1 End

--Approch 2
select * into #TEST1 from Users where UserName='TEST1'
--Temp table Approch 2 End


--CTE Start

;WITH CTE_Test as (

Select 'CTE_Test' as test,UserName, Password, IsActive from Users where UserName='Test1'

)
select * from CTE_Test
--CTE END

--Merge
MERGE INTO Users AS Target
USING #TEST AS Source
ON Target.UserName = Source.Name

WHEN MATCHED THEN
    UPDATE SET Target.IsActive = 1

WHEN NOT MATCHED BY TARGET THEN
    INSERT (UserName, Password, MobileNumber, IsActive, CreatedDate)
    VALUES (Source.Name, '123', '123', 1, GETDATE())

WHEN NOT MATCHED BY SOURCE THEN
    DELETE;
--Merge End