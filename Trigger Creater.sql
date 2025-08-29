DECLARE @TableName NVARCHAR(128);
DECLARE @SQL NVARCHAR(MAX);

-- جدول بأسماء الجداول المستهدفة
DECLARE @TargetTables TABLE (TableName NVARCHAR(128));
INSERT INTO @TargetTables (TableName)
VALUES 
    ('Orders'),
    ('Products'),
    ('ImportOrders');

DECLARE TableCursor CURSOR FOR
SELECT TableName FROM @TargetTables;

OPEN TableCursor;
FETCH NEXT FROM TableCursor INTO @TableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- حذف التريجرات إن وجدت
    SET @SQL = '
    IF OBJECT_ID(''trg_' + @TableName + '_Insert'', ''TR'') IS NOT NULL
        DROP TRIGGER trg_' + @TableName + '_Insert;
    IF OBJECT_ID(''trg_' + @TableName + '_Update'', ''TR'') IS NOT NULL
        DROP TRIGGER trg_' + @TableName + '_Update;
    IF OBJECT_ID(''trg_' + @TableName + '_Delete'', ''TR'') IS NOT NULL
        DROP TRIGGER trg_' + @TableName + '_Delete;';
    EXEC sp_executesql @SQL;

    -- Trigger Insert
    SET @SQL = '
    CREATE TRIGGER trg_' + @TableName + '_Insert
    ON [' + @TableName + ']
    AFTER INSERT
    AS
    BEGIN
        SET NOCOUNT ON;
        INSERT INTO LogRegister (
            TableName,
            NewData,
            OldData,
            ActionDate,
            ActoinByUser,
            ActionType,
            Version
        )
        SELECT 
            ''' + @TableName + ''',
            (SELECT * FROM inserted FOR JSON PATH),
            NULL,
            MIN(ActionDate),
            MIN(ActionByUser),
            ''Insert'',
            1
        FROM inserted;
    END';
    EXEC(@SQL);

    -- Trigger Update
    SET @SQL = '
    CREATE TRIGGER trg_' + @TableName + '_Update
    ON [' + @TableName + ']
    AFTER UPDATE
    AS
    BEGIN
        SET NOCOUNT ON;
        INSERT INTO LogRegister (
            TableName,
            NewData,
            OldData,
            ActionDate,
            ActoinByUser,
            ActionType,
            Version
        )
        SELECT 
            ''' + @TableName + ''',
            (SELECT * FROM inserted FOR JSON PATH),
            (SELECT * FROM deleted FOR JSON PATH),
            MIN(ActionDate),
            MIN(ActionByUser),
            ''Update'',
            1
        FROM inserted;
    END';
    EXEC(@SQL);

    -- Trigger Delete
    SET @SQL = '
    CREATE TRIGGER trg_' + @TableName + '_Delete
    ON [' + @TableName + ']
    AFTER DELETE
    AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @UserID NVARCHAR(100) = CONVERT(NVARCHAR(100), SESSION_CONTEXT(N''UserID''));

        INSERT INTO LogRegister (
            TableName,
            NewData,
            OldData,
            ActionDate,
            ActoinByUser,
            ActionType,
            Version
        )
        SELECT 
            ''' + @TableName + ''',
            NULL,
            (SELECT * FROM deleted FOR JSON PATH),
            GETDATE(),
            ISNULL(@UserID, ''Unknown''),
            ''Delete'',
            1
        FROM deleted;
    END';
    EXEC(@SQL);

    FETCH NEXT FROM TableCursor INTO @TableName;
END;

CLOSE TableCursor;
DEALLOCATE TableCursor;
