
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
	CustomerID INT NOT NULL,
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(ID) ON DELETE CASCADE,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    OrderDate DATETIME DEFAULT GETDATE(),
);

INSERT INTO Orders (CustomerID, TotalAmount, Status) VALUES
(1, 1150.00, 'Completed'), 
(2, 800.00, 'Pending'),    
(3, 950.00, 'Completed'),  
(4, 400.00, 'Cancelled'),  
(5, 650.00, 'Completed'),  
(6, 1050.00, 'Completed'), 
(7, 900.00, 'Pending'),    
(8, 450.00, 'Completed'),  
(9, 600.00, 'Pending'),    
(10, 750.00, 'Completed'); 

select *from Orders

Alter Table Orders
ADD CONSTRAINT DF__Orders__Status DEFAULT 'Completed' FOR Status

select * from Orders


CREATE PROCEDURE GetOrders
AS
BEGIN
	SELECT * FROM Orders
END

CREATE PROCEDURE AddOrder
	@CustomerId INT ,
	@TotalAmount DECIMAL(10, 2),
	@OrderID INT OUTPUT
AS 
BEGIN
	
		INSERT INTO Orders (CustomerID, TotalAmount)
		Values (@CustomerId, @TotalAmount)

		SET @OrderID = SCOPE_IDENTITY();
END
		

CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
	CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    MenuID INT NOT NULL,
    Quantity INT NOT NULL,
    PriceAtTimeOfOrder DECIMAL(10, 2) NOT NULL,
    TotalItemPrice AS (Quantity * PriceAtTimeOfOrder) PERSISTED
);

ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_Orders
FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE

	
CREATE PROCEDURE AddOrderDetails
	@OrderID INT,
	@MenuID INT,
	@Quantity INT,
	@PriceAtTimeOfOrder DECIMAL(10, 2)
AS
BEGIN
	INSERT INTO OrderDetails (OrderID, MenuID, Quantity, PriceAtTimeOfOrder)
	Values (@OrderID, @MenuID, @Quantity, @PriceAtTimeOfOrder)
END


CREATE PROCEDURE GetOrderDetails
AS
BEGIN
	SELECT * FROM OrderDetails
END

CREATE PROCEDURE DeleteOrder
	@OrderID INT
AS 
BEGIN
	DELETE FROM Orders WHERE OrderID = @OrderID
END

CREATE PROCEDURE UpdateOrderStatus
	@Status VARCHAR(20),
	@OrderID INT
AS
BEGIN
	Update Orders
	Set Status = @Status
	WHERE OrderID = @OrderID
END


