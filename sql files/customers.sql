
CREATE Table Customers (
	ID INT IDENTITY(1, 1) PRIMARY KEY,
	Name Varchar(20),
	Phone VARCHAR(15),
	Email Varchar(50),
	Address Varchar(100)
)

INSERT INTO Customers (Name, Phone, Email, Address) VALUES
('Ali Khan', '0321-1234567', 'ali.khan@gmail.com', '123 Gulberg Road, Lahore'),
('Ayesha Siddiqui', '0301-9876543', 'ayesha.siddiqui@gmail.com', '45 Ferozepur Street, Karachi'),
('Omar Farooq', '0333-4567890', 'omar.farooq@gmail.com', '78 Mall Road, Islamabad'),
('Fatima Malik', '0345-7654321', 'fatima.malik@gmail.com', '56 Defence Avenue, Lahore'),
('Zainab Ansari', '0302-3456789', 'zainab.ansari@gmail.com', '89 Clifton Block, Karachi'),
('Ahmad Hussain', '0314-5678901', 'ahmad.hussain@gmail.com', '67 Canal View, Lahore'),
('Maryam Javed', '0320-9870123', 'maryam.javed@gmail.com', '34 Blue Area, Islamabad'),
('Bilal Sheikh', '0336-8901234', 'bilal.sheikh@gmail.com', '12 University Town, Peshawar'),
('Hafsa Raza', '0307-7896543', 'hafsa.raza@gmail.com', '23 DHA Phase 5, Karachi'),
('Hamza Yousuf', '0312-6547890', 'hamza.yousuf@gmail.com', '90 Satellite Town, Rawalpindi');



CREATE PROCEDURE GetCustomers
AS
BEGIN
	Select * FROM Customers
END

CREATE PROCEDURE AddCustomer
		@CustomerName VARCHAR(20),
		@Phone VARCHAR(15),
		@Email VarChar(50),
		@Address VarChar(100)
AS
BEGIN
	BEGIN TRANSACTION

	BEGIN TRY
		INSERT INTO Customers (Name, Phone, Email, Address)
		Values(@CustomerName, @Phone, @Email, @Address)
	
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
	END CATCH
END

CREATE PROCEDURE UpdateCustomer
	@ID INT,
	@CustomerName VARCHAR(20),
	@Phone VARCHAR(15),
	@Email VarChar(50),
	@Address VarChar(100)
AS
BEGIN
	BEGIN TRANSACTION
	
	BEGIN TRY
	UPDATE Customers
	SET
	Name = @CustomerName,
	Phone = @Phone,
	Email = @Email,
	Address = @Address
	Where ID = @ID

	COMMIT 
	END TRY
	BEGIN CATCH
		ROLLBACK
	END CATCH
END

CREATE PROCEDURE DeleteCustomer
	@ID INT
AS
BEGIN
	Delete FROM Customers Where ID = @ID
END

