CREATE database AlamCaterers


CREATE TABLE Menus (
	ID INT IDENTITY(1,1) Primary KEY,
	MenuName VARCHAR(30),
	Category VARCHAR(30),
	Price decimal (10, 2),
	Description varchar(15)
)

INSERT INTO Menus (MenuName, Category, Price, Description) VALUES
('Biryani', 'Rice', 350.00, 'Spicy rice'),
('Karahi', 'Curry', 800.00, 'Spicy dish'),
('Haleem', 'Curry', 250.00, 'Lentil mix'),
('Nihari', 'Curry', 400.00, 'Slow-cooked'),
('Seekh Kebab', 'BBQ', 300.00, 'Grilled meat'),
('Paya', 'Curry', 450.00, 'Bone stew'),
('Chicken Handi', 'Curry', 600.00, 'Creamy curry'),
('Tikka', 'BBQ', 350.00, 'Spiced meat'),
('Chapli Kebab', 'BBQ', 300.00, 'Flat kebab'),
('Kheer', 'Dessert', 150.00, 'Rice pudding');

select *from Menus

ALTER TABLE Menus
ADD NewID INT IDENTITY(1, 1)

ALTER TABLE Menus
DROP Constraint PK__Menus__3214EC277DFBC796

EXEC sp_rename 'Menus.NewID', 'ID', 'COLUMN'

ALTER table Menus
add constraint PK_Menus Primary Key (ID)

INSERT INTO Menus (MenuName, Category, Price, Description)
VALUES 
('Cheese Pizza', 'Pizza', 9.99, 'Delicious')


CREATE PROCEDURE GetAllMenus
AS
BEGIN
SELECT * FROM Menus
END	

exec GetAllMenus

CREATE PROCEDURE AddMenu
	@MenuName VARCHAR(30),
	@Category VARCHAR(30),
	@Price float,
	@Description VARCHAR(15)
AS
BEGIN
	BEGIN TRANSACTION
	
	BEGIN TRY
		INSERT INTO Menus (MenuName, Category, Price, Description)
		VALUES (@MenuName, @Category, @Price, @Description)
	
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
	END CATCH
END

CREATE PROCEDURE UpdateMenu
	@ID INT,
	@MenuName VARCHAR(30),
	@Category VARCHAR(30),
	@Price float,
	@Description VARCHAR(15)
AS
BEGIN
	BEGIN TRANSACTION

	BEGIN TRY
		UPDATE Menus
		SET 
		MenuName = @MenuName,
		Category = @Category,
		Price = @Price,
		Description = @Description
		WHERE Menus.ID = @ID

		COMMIT
	END TRY

	BEGIN CATCH
		ROLLBACK
	END CATCH
END

CREATE PROCEDURE DeleteMenu
	@ID INT
AS 
BEGIN
	DELETE FROM Menus WHERE ID = @ID
END



