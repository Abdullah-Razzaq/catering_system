
Create Table TransactionsLedger (
	LedgerID INT PRIMARY KEY IDENTITY(1, 1),
	OrderID INT,
	CustomerID INT, 
	TransactionType VARCHAR(30) NOT NULL,
	TransactionDate DATETIME DEFAULT GETDATE(),
	Amount DECIMAL(10,2),
	Notes VARCHAR(255)

)

INSERT INTO TransactionsLedger (OrderID, CustomerID, TransactionType, Amount, Notes) VALUES
(1, 1, 'Payment Received', 1150.00, 'Order payment completed by Ali Khan'),
(2, 2, 'Payment Pending', 800.00, 'Order placed by Ayesha Siddiqui, payment not yet received'),
(3, 3, 'Payment Received', 950.00, 'Order payment completed by Omar Farooq'),
(4, 4, 'Order Cancelled', 400.00, 'Order cancelled by Fatima Malik, no payment made'),
(5, 5, 'Payment Received', 650.00, 'Order payment completed by Zainab Ansari'),
(6, 6, 'Payment Received', 1050.00, 'Order payment completed by Ahmad Hussain'),
(7, 7, 'Payment Pending', 900.00, 'Order placed by Maryam Javed, awaiting payment'),
(8, 8, 'Payment Received', 450.00, 'Order payment completed by Bilal Sheikh'),
(9, 9, 'Payment Pending', 600.00, 'Order placed by Hafsa Raza, payment not yet received'),
(10, 10, 'Payment Received', 750.00, 'Order payment completed by Hamza Yousuf');

select *from TransactionsLedger

CREATE Trigger trg_insertLedgerOnOrder
ON Orders
After Insert
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
	Insert INTO TransactionsLedger(OrderID, CustomerID, TransactionType, Amount, Notes)
	Select
		i.OrderID,
		i.CustomerID,
		'Order Payment',
		i.TotalAmount,
		CONCAT('Payment for Order #', i.OrderID)
	FROM
		inserted i
	COMMIT
	END TRY

	BEGIN CATCH
		ROLLBACK
	END CATCH
END

		
CREATE TRIGGER trg_insertLedgerOnOrderUpdate
ON Orders
AFTER Update
AS 
BEGIN
	BEGIN TRANSACTION

	BEGIN TRY
	Insert INTO TransactionsLedger(OrderID, CustomerID, TransactionType, Amount, Notes)
	SELECT
		i.OrderID,
		i.CustomerID,
		'Refund',
		-i.TotalAmount,
		CONCAT('Refund for Order #', i.OrderID)
	
	FROM	
		inserted i
		JOIN deleted d ON i.OrderID = d.OrderID 
	WHERE i.Status = 'Refunded' AND d.Status <> 'Refunded' --so the update only occures for refund
	
	COMMIT
	END	TRY									-- and the second check ensures that if some other column is updated a duplicate entry is not
											-- added to the ledger, in this unnecessary duplication is avoided, thus this check is important
	BEGIN CATCH											-- however for now in our case, there is only one property in orders table that can be changed,
		ROLLBACK										-- even i thats the case if an update is made from Refunded to Refunded, the Ledger would still
	END CATCH										-- add a duplicate row, so better to add the second check
END



CREATE TRIGGER trg_insertLedgerOnOrderBackToPending
ON Orders
AFTER Update
AS
BEGIN
	BEGIN TRANSACTION

	BEGIN TRY
		INSERT INTO TransactionsLedger(OrderID, CustomerID, TransactionType, Amount, Notes)
		SELECT
			i.OrderID,
			i.CustomerID,
			'Completed',
			i.TotalAmount,
			CONCAT('Payment for Order #', d.OrderID)
		FROM 
			inserted i
			JOIN deleted d ON i.OrderID = d.OrderID
		WHERE i.Status = 'Completed' AND d.Status <> 'Completed' --only when status changed to completed from refunded
	COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
	END CATCH
END



CREATE TRIGGER trg_insertLedgerOnOrderDelete
ON Orders
AFTER DELETE
AS
	BEGIN
		INSERT INTO TransactionsLedger(OrderID, CustomerID, TransactionType, Amount, Notes)
		SELECT
		d.OrderID,
		d.CustomerID,
		'Cancelled',
		-d.TotalAmount,
		CONCAT('Canceled Order #', d.OrderID)
		FROM 
		deleted d
		WHERE d.Status <> 'Refunded'
	END

		
	CREATE PROCEDURE GetTransactions
	AS
	BEGIN
		SELECT * FROM TransactionsLedger
	END

