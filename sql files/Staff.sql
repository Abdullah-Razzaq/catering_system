
CREATE TABLE Staff (
	ID INT PRIMARY KEY IDENTITY(1, 1),
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Position VARCHAR(50),
	HireDate DATE DEFAULT GETDATE()
)

INSERT INTO Staff (FirstName, LastName, Email, Position) VALUES
('Ahmed', 'Khan', 'ahmed.khan@gmail.com', 'Chef'),
('Ayesha', 'Ali', 'ayesha.ali@gmail.com', 'Manager'),
('Bilal', 'Hussain', 'bilal.hussain@gmail.com', 'Waiter'),
('Fatima', 'Sheikh', 'fatima.sheikh@gmail.com', 'Cashier'),
('Omar', 'Yusuf', 'omar.yusuf@gmail.com', 'Delivery Driver'),
('Hassan', 'Raza', 'hassan.raza@gmail.com', 'Sous Chef'),
('Zainab', 'Malik', 'zainab.malik@gmail.com', 'Waitress'),
('Asad', 'Shah', 'asad.shah@gmail.com', 'Kitchen Helper'),
('Maryam', 'Iqbal', 'maryam.iqbal@gmail.com', 'Event Coordinator'),
('Hamza', 'Farooq', 'hamza.farooq@gmail.com', 'Procurement Officer');

select *from Staff

CREATE UNIQUE INDEX IX_Staff_Email
ON Staff (Email);


CREATE PROCEDURE AddStaff
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@Email VARCHAR(100),
	@Position VARCHAR(50)
AS 
BEGIN
	INSERT INTO Staff (FirstName, LastName, Email, Position)
	VALUES (@FirstName, @LastName, @Email, @Position)
END



CREATE PROCEDURE GetStaff	
	@ID INT,
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@Email VARCHAR(100),
	@Position VARCHAR(50)
AS 
BEGIN
	SELECT * FROM Staff
END


CREATE PROCEDURE UpdateStaff
	@StaffID INT,
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@Email VARCHAR(100),
	@Position VARCHAR(50)
AS
BEGIN
	UPDATE Staff
	SET
	FirstName = @FirstName,
	LastName = @LastName,
	Email = @Email,
	Position = @Position
	WHERE ID = @StaffID
END
		
CREATE PROCEDURE DeleteStaff
	@StaffID INT
AS
BEGIN
	DELETE FROM Staff Where ID = @StaffID
END



