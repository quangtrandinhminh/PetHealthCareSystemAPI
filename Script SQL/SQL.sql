USE [PetHealthCareSys]
GO

/* Insert Data to table Service */
INSERT INTO [dbo].[Service] (Name, Description, Duration, Price, CreatedBy, LastUpdatedBy, DeletedBy, CreatedTime, LastUpdatedTime, DeletedTime)
VALUES 
    (N'Kiểm soát bọ chét khi tắm (theo toa)', NULL, 30, 200000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Tư vấn/Đào tạo Hành vi', NULL, 60, 500000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Giấy chứng nhận sức khỏe (Bán hàng & Du lịch)', NULL, 45, 150000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Nội trú và Chăm sóc ban ngày', NULL, 1440, 3000000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Nhập viện', NULL, 1440, 5000000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Hoàn thành các bài kiểm tra đánh giá y tế', NULL, 60, 700000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Phòng thí nghiệm chẩn đoán trong nhà', NULL, 30, 1000000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Vi mạch da liễu', NULL, 30, 800000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Nhận biết', NULL, 15, 50000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Chăm sóc nha khoa', NULL, 60, 1200000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Tư vấn chế độ ăn uống', NULL, 30, 200000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Tiệm thuốc', NULL, 15, 100000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Siêu âm kỹ thuật số', NULL, 45, 1500000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'X-quang kỹ thuật số', NULL, 45, 1200000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Vắc-xin', NULL, 15, 300000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Nội soi sợi quang', NULL, 60, 1800000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL),
    (N'Chương trình chăm sóc sức khỏe', NULL, 1440, 10000000, NULL, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL);
GO

INSERT INTO [dbo].[Cage] ([Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable])
VALUES 
    (4, 'Metal', 101, '123 Pet St', 'Large metal cage for dogs', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (2, 'Plastic', 102, '123 Pet St', 'Small plastic cage for cats', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (3, 'Wood', 103, '123 Pet St', 'Medium wooden cage for rabbits', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (5, 'Metal', 104, '123 Pet St', 'Extra large metal cage for large dogs', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (1, 'Plastic', 105, '123 Pet St', 'Small plastic cage for hamsters', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (3, 'Metal', 106, '123 Pet St', 'Medium metal cage for birds', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (4, 'Wood', 107, '123 Pet St', 'Large wooden cage for small animals', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1),
    (2, 'Plastic', 108, '123 Pet St', 'Small plastic cage for reptiles', NULL, 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1)
GO

-- Helper variables for times
DECLARE @StartTime TIME = '08:00:00';
DECLARE @EndTime TIME = '12:00:00';
DECLARE @Interval INT = 30; -- interval in minutes

-- Insert Book intervals for 08:00 to 12:00
WHILE @StartTime < @EndTime
BEGIN
    INSERT INTO [dbo].[TimeTable] ([DayOfWeeks], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [TimeStart], [TimeEnd])
    VALUES ('Monday', 'Book', 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, @StartTime, DATEADD(MINUTE, @Interval, @StartTime));
    
    SET @StartTime = DATEADD(MINUTE, @Interval, @StartTime);
END

-- Reset the start and end times for the next interval
SET @StartTime = '13:00:00';
SET @EndTime = '17:00:00';

-- Insert Book intervals for 13:00 to 17:00
WHILE @StartTime < @EndTime
BEGIN
    INSERT INTO [dbo].[TimeTable] ([DayOfWeeks], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [TimeStart], [TimeEnd])
    VALUES ('Monday', 'Book', 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, @StartTime, DATEADD(MINUTE, @Interval, @StartTime));
    
    SET @StartTime = DATEADD(MINUTE, @Interval, @StartTime);
END

-- Insert Check intervals for 18:00 to 22:00
DECLARE @CheckIntervals TABLE (StartTime TIME, EndTime TIME)
INSERT INTO @CheckIntervals VALUES ('18:00:00', '19:00:00'), ('19:00:00', '20:00:00'), ('20:00:00', '21:00:00'), ('21:00:00', '22:00:00');

DECLARE @CheckStartTime TIME;
DECLARE @CheckEndTime TIME;

DECLARE CheckCursor CURSOR FOR
SELECT StartTime, EndTime FROM @CheckIntervals;

OPEN CheckCursor;
FETCH NEXT FROM CheckCursor INTO @CheckStartTime, @CheckEndTime;

WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO [dbo].[TimeTable] ([DayOfWeeks], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [TimeStart], [TimeEnd])
    VALUES ('Monday', 'Check', 1, 1, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, @CheckStartTime, @CheckEndTime);
    
    FETCH NEXT FROM CheckCursor INTO @CheckStartTime, @CheckEndTime;
END;

CLOSE CheckCursor;
DEALLOCATE CheckCursor;

GO