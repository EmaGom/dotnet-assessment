INSERT INTO Feed
	(Title,
	PostedDateTime,
	[Url])
VALUES 
	(@Title,
	@PostedDateTime,
	@Url)

SELECT @@IDENTITY;
