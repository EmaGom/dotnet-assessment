INSERT INTO Category
	([Name],
	LastBuildDate)
VALUES 
	(@Name,
	@LastBuildDate)

SELECT * FROM Category WHERE Id = @@IDENTITY;
