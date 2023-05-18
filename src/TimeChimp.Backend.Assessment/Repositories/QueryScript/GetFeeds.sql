DROP TABLE IF EXISTS #FeedsTmp;

CREATE TABLE #FeedsTmp (
	Id INT,
	Title VARCHAR(100),
	PostedDateTime DATETIME,
	[Url] VARCHAR(1500)
);

DECLARE @sortCondition VARCHAR(50) = LOWER(ISNULL(@SortBy, 'PostedDateTime')) + ' ' + LOWER(ISNULL(@SortDirection, 'desc'));
DECLARE @size INT = LOWER(ISNULL(@PageSize, 10));
DECLARE @index INT = LOWER(ISNULL(@PageIndex, 0));

INSERT INTO #FeedsTmp
SELECT 
	Id,
	Title,
	PostedDateTime,
	[Url]
FROM 
	Feed
WHERE 
	(@PostedDate IS NULL OR CAST(PostedDateTime AS DATE) = @PostedDate)
	AND (@Title IS NULL OR LOWER(@Title) like LOWER('%' + @Title + '%'));


SELECT 
	*
FROM 
	#FeedsTmp
ORDER BY 
	-- PostedDate DateTime   
    CASE WHEN @sortCondition = 'PostedDateTime asc' THEN PostedDateTime END ASC,
    CASE WHEN @sortCondition = 'PostedDateTime desc' THEN PostedDateTime END DESC,
	-- Title
    CASE WHEN @sortCondition = 'Title asc' THEN Title END ASC,
    CASE WHEN @sortCondition = 'Title desc' THEN Title END DESC
OFFSET (@index * @size) ROWS
FETCH NEXT @size ROWS ONLY;

DROP TABLE IF EXISTS #TestingWorksheetTmp;
