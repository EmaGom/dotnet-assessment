INSERT INTO Feed
	(Title,
	PublishDate,
	[Url])
VALUES 
	(@Title,
	@PublishDate,
	@Url)

SELECT * FROM FEED WHERE Id = @@IDENTITY;
