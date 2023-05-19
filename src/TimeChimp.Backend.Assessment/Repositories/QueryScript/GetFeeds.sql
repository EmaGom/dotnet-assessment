DROP TABLE IF EXISTS #FeedsTmp;

CREATE TABLE #FeedsTmp (
	Id INT,
	Title VARCHAR(100),
	PublishDate DATETIME,
	[Url] VARCHAR(1500),
	CategoryId INT,
	[Name] VARCHAR(50)
);

DECLARE @sortCondition VARCHAR(50) = LOWER(ISNULL(@SortBy, 'PublishDate')) + ' ' + LOWER(ISNULL(@SortDirection, 'desc'));
DECLARE @size INT = LOWER(ISNULL(@PageSize, 10));
DECLARE @index INT = LOWER(ISNULL(@PageIndex, 0));

INSERT INTO #FeedsTmp
SELECT 
	F.Id,
	F.Title,
	F.PublishDate,
	F.[Url],
	C.Id AS CategoryId,
	C.[Name] 
FROM 
	Feed F 
		INNER JOIN Category C ON F.CategoryId = C.Id
WHERE 
	(@PublishDate IS NULL OR CAST(PublishDate AS DATE) = @PublishDate)
	AND (@Title IS NULL OR LOWER(@Title) like LOWER('%' + @Title + '%'));


SELECT 
	*
FROM 
	#FeedsTmp
ORDER BY 
	-- PublishDate DateTime   
    CASE WHEN @sortCondition = 'PublishDate asc' THEN PublishDate END ASC,
    CASE WHEN @sortCondition = 'PublishDate desc' THEN PublishDate END DESC,
	-- Title
    CASE WHEN @sortCondition = 'Title asc' THEN Title END ASC,
    CASE WHEN @sortCondition = 'Title desc' THEN Title END DESC
OFFSET (@index * @size) ROWS
FETCH NEXT @size ROWS ONLY;

DROP TABLE IF EXISTS #TestingWorksheetTmp;
