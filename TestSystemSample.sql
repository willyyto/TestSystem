-- Insert sample data into dbo.[Company]
INSERT INTO dbo.[Company] (Id, Name, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'Tech Solutions Inc.', 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'HealthCare Ltd.', 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'EduWorld Co.', 1, 0, GETDATE(), GETDATE());

-- Insert sample data into dbo.[User]
INSERT INTO dbo.[User] (Id, Name, Username, Password, Email, Role, RefreshToken, TokenCreated, TokenExpires, CompanyId, IsActive, IsArchived, IsLocked, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'John Doe', 'jdoe', 'password1', 'jdoe@techsolutions.com', 'Admin', 'token1', GETDATE(), DATEADD(day, 7, GETDATE()), NULL, 1, 0, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Jane Smith', 'jsmith', 'password2', 'jsmith@healthcare.com', 'User', 'token2', GETDATE(), DATEADD(day, 7, GETDATE()), (SELECT Id FROM dbo.[Company] WHERE Name = 'HealthCare Ltd.'), 1, 0, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Alice Johnson', 'ajohnson', 'password3', 'ajohnson@eduworld.com', 'User', 'token3', GETDATE(), DATEADD(day, 7, GETDATE()), (SELECT Id FROM dbo.[Company] WHERE Name = 'EduWorld Co.'), 1, 0, 0, GETDATE(), GETDATE());

-- Insert sample data into dbo.[Test]
INSERT INTO dbo.[Test] (Id, Name, Description, CompanyId, StartDate, EndDate, Duration, PassMark, IsTimed, ShuffleQuestions, MaximumAttempts, Visibility, TestType, Instructions, Feedback, TestAccessControl, GradingScheme, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'Tech Solutions Assessment', 'Assessment for tech solutions', (SELECT Id FROM dbo.[Company] WHERE Name = 'Tech Solutions Inc.'), GETDATE(), DATEADD(day, 30, GETDATE()), '01:00:00', 50, 1, 1, 3, 1, 1, 'Follow the instructions', 2, 1, 1, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'HealthCare Knowledge Test', 'Knowledge test for healthcare employees', (SELECT Id FROM dbo.[Company] WHERE Name = 'HealthCare Ltd.'), GETDATE(), DATEADD(day, 30, GETDATE()), '00:45:00', 60, 1, 1, 2, 2, 2, 'Answer all questions', 2, 1, 2, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'EduWorld Quiz', 'Quiz for educational purposes', (SELECT Id FROM dbo.[Company] WHERE Name = 'EduWorld Co.'), GETDATE(), DATEADD(day, 30, GETDATE()), '00:30:00', 70, 1, 1, 1, 3, 3, 'Complete the quiz', 2, 1, 3, 1, 0, GETDATE(), GETDATE());

-- Insert sample data into dbo.[Question]
INSERT INTO dbo.[Question] (Id, Text, Type, TestId, Weight, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'What is the capital of France?', 0, (SELECT Id FROM dbo.[Test] WHERE Name = 'Tech Solutions Assessment'), 1.0, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Which organ is responsible for pumping blood?', 1, (SELECT Id FROM dbo.[Test] WHERE Name = 'HealthCare Knowledge Test'), 1.0, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'What is 2 + 2?', 0, (SELECT Id FROM dbo.[Test] WHERE Name = 'EduWorld Quiz'), 1.0, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Explain the theory of relativity.', 3, (SELECT Id FROM dbo.[Test] WHERE Name = 'Tech Solutions Assessment'), 2.0, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Complete the following sentence: "The quick brown fox jumps over the _____ dog."', 4, (SELECT Id FROM dbo.[Test] WHERE Name = 'HealthCare Knowledge Test'), 1.5, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Match the countries with their capitals.', 5, (SELECT Id FROM dbo.[Test] WHERE Name = 'Tech Solutions Assessment'), 1.0, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'What is the boiling point of water?', 0, (SELECT Id FROM dbo.[Test] WHERE Name = 'HealthCare Knowledge Test'), 1.2, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'What is the powerhouse of the cell?', 1, (SELECT Id FROM dbo.[Test] WHERE Name = 'EduWorld Quiz'), 1.3, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Who developed the theory of evolution?', 1, (SELECT Id FROM dbo.[Test] WHERE Name = 'EduWorld Quiz'), 1.4, 1, 0, GETDATE(), GETDATE());

-- Insert sample data into dbo.[Answer]
INSERT INTO dbo.[Answer] (Id, Text, IsCorrect, IsFillInTheBlank, QuestionId, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'Paris', 1, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the capital of France?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Berlin', 0, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the capital of France?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Heart', 1, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'Which organ is responsible for pumping blood?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Lungs', 0, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'Which organ is responsible for pumping blood?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), '4', 1, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is 2 + 2?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), '3', 0, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is 2 + 2?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'lazy', 1, 1, (SELECT Id FROM dbo.[Question] WHERE Text = 'Complete the following sentence: "The quick brown fox jumps over the _____ dog."'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), '100°C', 1, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the boiling point of water?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Mitochondria', 1, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the powerhouse of the cell?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Nucleus', 0, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the powerhouse of the cell?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Charles Darwin', 1, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'Who developed the theory of evolution?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Albert Einstein', 0, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'Who developed the theory of evolution?'), 1, 0, GETDATE(), GETDATE());

-- Insert sample data into dbo.[MatchPair]
INSERT INTO dbo.[MatchPair] (Id, LeftItem, RightItem, LeftItemId, RightItemId, QuestionId, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'France', 'Paris', NEWID(), NEWID(), (SELECT Id FROM dbo.[Question] WHERE Text = 'Match the countries with their capitals.'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Germany', 'Berlin', NEWID(), NEWID(), (SELECT Id FROM dbo.[Question] WHERE Text = 'Match the countries with their capitals.'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Italy', 'Rome', NEWID(), NEWID(), (SELECT Id FROM dbo.[Question] WHERE Text = 'Match the countries with their capitals.'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Spain', 'Madrid', NEWID(), NEWID(), (SELECT Id FROM dbo.[Question] WHERE Text = 'Match the countries with their capitals.'), 1, 0, GETDATE(), GETDATE());

-- Insert additional sample data into dbo.[Test]
INSERT INTO dbo.[Test] (Id, Name, Description, CompanyId, StartDate, EndDate, Duration, PassMark, IsTimed, ShuffleQuestions, MaximumAttempts, Visibility, TestType, Instructions, Feedback, TestAccessControl, GradingScheme, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'Advanced Tech Solutions Assessment', 'Advanced assessment for tech solutions', (SELECT Id FROM dbo.[Company] WHERE Name = 'Tech Solutions Inc.'), GETDATE(), DATEADD(day, 45, GETDATE()), '01:30:00', 70, 1, 1, 3, 1, 1, 'Read each question carefully', 2, 1, 1, 1, 0, GETDATE(), GETDATE());

-- Insert additional sample data into dbo.[Question]
INSERT INTO dbo.[Question] (Id, Text, Type, TestId, Weight, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'What is the speed of light?', 0, (SELECT Id FROM dbo.[Test] WHERE Name = 'Advanced Tech Solutions Assessment'), 1.5, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Define Newtons First Law.', 3, (SELECT Id FROM dbo.[Test] WHERE Name = 'Advanced Tech Solutions Assessment'), 2.5, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'What is the capital of Japan?', 0, (SELECT Id FROM dbo.[Test] WHERE Name = 'Advanced Tech Solutions Assessment'), 1.0, 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Match the programming languages with their creators.', 5, (SELECT Id FROM dbo.[Test] WHERE Name = 'Advanced Tech Solutions Assessment'), 1.5, 1, 0, GETDATE(), GETDATE());

-- Insert additional sample data into dbo.[Answer]
INSERT INTO dbo.[Answer] (Id, Text, IsCorrect, IsFillInTheBlank, QuestionId, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), '299,792,458 ms', 1, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the speed of light?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), '9.8 ms', 0, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the speed of light?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Tokyo', 1, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the capital of Japan?'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Kyoto', 0, 0, (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the capital of Japan?'), 1, 0, GETDATE(), GETDATE());

-- Insert additional sample data into dbo.[MatchPair]
INSERT INTO dbo.[MatchPair] (Id, LeftItem, RightItem, LeftItemId, RightItemId, QuestionId, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
    (NEWID(), 'Python', 'Guido van Rossum', NEWID(), NEWID(), (SELECT Id FROM dbo.[Question] WHERE Text = 'Match the programming languages with their creators.'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Java', 'James Gosling', NEWID(), NEWID(), (SELECT Id FROM dbo.[Question] WHERE Text = 'Match the programming languages with their creators.'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'C', 'Dennis Ritchie', NEWID(), NEWID(), (SELECT Id FROM dbo.[Question] WHERE Text = 'Match the programming languages with their creators.'), 1, 0, GETDATE(), GETDATE()),
    (NEWID(), 'Ruby', 'Yukihiro Matsumoto', NEWID(), NEWID(), (SELECT Id FROM dbo.[Question] WHERE Text = 'Match the programming languages with their creators.'), 1, 0, GETDATE(), GETDATE());

-- Insert sample data into dbo.[TestResult]
INSERT INTO dbo.[TestResult] (Id, UserId, TestId, CompletedDate, Score, IsArchived, UpdatedOn, CreatedOn, IsActive)
VALUES 
    (NEWID(), (SELECT Id FROM dbo.[User] WHERE Username = 'jdoe'), (SELECT Id FROM dbo.[Test] WHERE Name = 'Tech Solutions Assessment'), GETDATE(), 85, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[User] WHERE Username = 'jsmith'), (SELECT Id FROM dbo.[Test] WHERE Name = 'HealthCare Knowledge Test'), GETDATE(), 75, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[User] WHERE Username = 'ajohnson'), (SELECT Id FROM dbo.[Test] WHERE Name = 'EduWorld Quiz'), GETDATE(), 90, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[User] WHERE Username = 'jdoe'), (SELECT Id FROM dbo.[Test] WHERE Name = 'Advanced Tech Solutions Assessment'), GETDATE(), 95, 0, GETDATE(), GETDATE(), 1);

-- Insert sample data into dbo.[QuestionResult]
INSERT INTO dbo.[QuestionResult] (Id, TestResultId, QuestionId, Answer, IsCorrect, IsArchived, UpdatedOn, CreatedOn, IsActive)
VALUES 
    (NEWID(), (SELECT Id FROM dbo.[TestResult] WHERE Score = 85), (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the capital of France?'), 'Paris', 1, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[TestResult] WHERE Score = 75), (SELECT Id FROM dbo.[Question] WHERE Text = 'Which organ is responsible for pumping blood?'), 'Heart', 1, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[TestResult] WHERE Score = 90), (SELECT Id FROM dbo.[Question] WHERE Text = 'What is 2 + 2?'), '4', 1, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[TestResult] WHERE Score = 85), (SELECT Id FROM dbo.[Question] WHERE Text = 'Complete the following sentence: "The quick brown fox jumps over the _____ dog."'), 'lazy', 1, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[TestResult] WHERE Score = 75), (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the boiling point of water?'), '100°C', 1, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[TestResult] WHERE Score = 90), (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the powerhouse of the cell?'), 'Mitochondria', 1, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[TestResult] WHERE Score = 95), (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the speed of light?'), '299,792,458 m/s', 1, 0, GETDATE(), GETDATE(), 1),
    (NEWID(), (SELECT Id FROM dbo.[TestResult] WHERE Score = 95), (SELECT Id FROM dbo.[Question] WHERE Text = 'What is the capital of Japan?'), 'Tokyo', 1, 0, GETDATE(), GETDATE(), 1);
