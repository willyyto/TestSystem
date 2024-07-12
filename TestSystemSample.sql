-- Insert companies
INSERT INTO Company (Id, Name, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
(NEWID(), 'Tech Solutions', 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Health Innovations', 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Edu World', 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Finance Group', 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Marketing Masters', 1, 0, GETDATE(), GETDATE());

-- Insert users
INSERT INTO [User] (Id, Name, Username, Password, Email, Role, RefreshToken, TokenCreated, TokenExpires, CompanyId, IsActive, IsArchived, IsLocked, UpdatedOn, CreatedOn)
VALUES 
(NEWID(), 'John Doe', 'jdoe', 'password123', 'jdoe@techsolutions.com', 'Admin', '', GETDATE(), DATEADD(YEAR, 1, GETDATE()), NULL, 1, 0, 0, GETDATE(), GETDATE()),
(NEWID(), 'Jane Smith', 'jsmith', 'password123', 'jsmith@healthinnovations.com', 'User', '', GETDATE(), DATEADD(YEAR, 1, GETDATE()), (SELECT TOP 1 Id FROM Company WHERE Name = 'Health Innovations'), 1, 0, 0, GETDATE(), GETDATE()),
(NEWID(), 'Emily Johnson', 'ejohnson', 'password123', 'ejohnson@eduworld.com', 'User', '', GETDATE(), DATEADD(YEAR, 1, GETDATE()), (SELECT TOP 1 Id FROM Company WHERE Name = 'Edu World'), 1, 0, 0, GETDATE(), GETDATE()),
(NEWID(), 'Alice Brown', 'abrown', 'password123', 'abrown@techsolutions.com', 'User', '', GETDATE(), DATEADD(YEAR, 1, GETDATE()), (SELECT TOP 1 Id FROM Company WHERE Name = 'Tech Solutions'), 1, 0, 0, GETDATE(), GETDATE()),
(NEWID(), 'Bob Martin', 'bmartin', 'password123', 'bmartin@healthinnovations.com', 'Admin', '', GETDATE(), DATEADD(YEAR, 1, GETDATE()), NULL, 1, 0, 0, GETDATE(), GETDATE()),
(NEWID(), 'Sarah Connor', 'sconnor', 'password123', 'sconnor@financegroup.com', 'User', '', GETDATE(), DATEADD(YEAR, 1, GETDATE()), (SELECT TOP 1 Id FROM Company WHERE Name = 'Finance Group'), 1, 0, 0, GETDATE(), GETDATE()),
(NEWID(), 'Michael Jordan', 'mjordan', 'password123', 'mjordan@marketingmasters.com', 'Admin', '', GETDATE(), DATEADD(YEAR, 1, GETDATE()), (SELECT TOP 1 Id FROM Company WHERE Name = 'Marketing Masters'), 1, 0, 0, GETDATE(), GETDATE());

-- Insert tests
INSERT INTO Test (Id, Name, Description, CompanyId, StartDate, EndDate, Duration, PassMark, IsTimed, ShuffleQuestions, MaximumAttempts, Visibility, TestType, Instructions, Feedback, TestAccessControl, GradingScheme, IsActive, IsArchived, UpdatedOn, CreatedOn, RetakePolicy_AllowRetakes, RetakePolicy_MaxRetakes, RetakePolicy_RetakeInterval)
VALUES 
(NEWID(), 'Tech Basics Quiz', 'A quiz to test basic tech knowledge', (SELECT TOP 1 Id FROM Company WHERE Name = 'Tech Solutions'), GETDATE(), DATEADD(DAY, 30, GETDATE()), '01:00:00', 60, 1, 1, 3, 0, 0, 'Answer all questions to the best of your knowledge.', 1, 0, 1, 1, 0, GETDATE(), GETDATE(), 1, 3, '01:00:00'),
(NEWID(), 'Health Safety Exam', 'An exam on health safety protocols', (SELECT TOP 1 Id FROM Company WHERE Name = 'Health Innovations'), GETDATE(), DATEADD(DAY, 30, GETDATE()), '02:00:00', 75, 1, 0, 2, 1, 1, 'Follow all instructions carefully.', 2, 1, 0, 1, 0, GETDATE(), GETDATE(), 1, 2, '02:00:00'),
(NEWID(), 'Education Strategies Survey', 'A survey on effective education strategies', (SELECT TOP 1 Id FROM Company WHERE Name = 'Edu World'), GETDATE(), DATEADD(DAY, 30, GETDATE()), '00:30:00', 50, 0, 1, 1, 2, 2, 'Provide your feedback honestly.', 0, 2, 2, 1, 0, GETDATE(), GETDATE(), 0, 1, '01:00:00'),
(NEWID(), 'Advanced Tech Exam', 'An advanced exam to test deep tech knowledge', (SELECT TOP 1 Id FROM Company WHERE Name = 'Tech Solutions'), GETDATE(), DATEADD(DAY, 30, GETDATE()), '02:00:00', 70, 1, 1, 2, 0, 1, 'Read each question carefully.', 1, 0, 1, 1, 0, GETDATE(), GETDATE(), 1, 2, '01:30:00'),
(NEWID(), 'Finance Fundamentals Quiz', 'A quiz on basic finance knowledge', (SELECT TOP 1 Id FROM Company WHERE Name = 'Finance Group'), GETDATE(), DATEADD(DAY, 30, GETDATE()), '01:00:00', 60, 1, 1, 3, 0, 0, 'Answer all questions to the best of your knowledge.', 1, 0, 1, 1, 0, GETDATE(), GETDATE(), 1, 3, '01:00:00'),
(NEWID(), 'Marketing Essentials Exam', 'An exam on fundamental marketing concepts', (SELECT TOP 1 Id FROM Company WHERE Name = 'Marketing Masters'), GETDATE(), DATEADD(DAY, 30, GETDATE()), '01:30:00', 70, 1, 0, 2, 1, 1, 'Follow all instructions carefully.', 2, 1, 0, 1, 0, GETDATE(), GETDATE(), 1, 2, '01:30:00');

-- Insert questions
INSERT INTO Question (Id, Text, Type, TestId, Weight, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
(NEWID(), 'What is the capital of France?', 0, (SELECT TOP 1 Id FROM Test WHERE Name = 'Tech Basics Quiz'), 1.0, 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Is the earth round?', 1, (SELECT TOP 1 Id FROM Test WHERE Name = 'Health Safety Exam'), 1.5, 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Describe the process of photosynthesis.', 2, (SELECT TOP 1 Id FROM Test WHERE Name = 'Education Strategies Survey'), 2.0, 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'What is the chemical symbol for water?', 0, (SELECT TOP 1 Id FROM Test WHERE Name = 'Advanced Tech Exam'), 1.0, 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Is fire hot?', 1, (SELECT TOP 1 Id FROM Test WHERE Name = 'Advanced Tech Exam'), 1.0, 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Match the following items: (1) CPU - (a) Central Processing Unit, (2) RAM - (b) Random Access Memory', 5, (SELECT TOP 1 Id FROM Test WHERE Name = 'Advanced Tech Exam'), 2.0, 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'What is the formula for calculating compound interest?', 2, (SELECT TOP 1 Id FROM Test WHERE Name = 'Finance Fundamentals Quiz'), 2.0, 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'What is the main goal of marketing?', 0, (SELECT TOP 1 Id FROM Test WHERE Name = 'Marketing Essentials Exam'), 1.5, 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'Explain the importance of customer segmentation.', 2, (SELECT TOP 1 Id FROM Test WHERE Name = 'Marketing Essentials Exam'), 2.0, 1, 0, GETDATE(), GETDATE());

-- Insert answers
INSERT INTO Answer (Id, Text, IsCorrect, IsFillInTheBlank, QuestionId, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
(NEWID(), 'Paris', 1, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the capital of France?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'London', 0, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the capital of France?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'True', 1, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'Is the earth round?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'False', 0, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'Is the earth round?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'H2O', 1, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the chemical symbol for water?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'CO2', 0, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the chemical symbol for water?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'True', 1, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'Is fire hot?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'False', 0, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'Is fire hot?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'To maximize profits', 0, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the main goal of marketing?'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'To meet customer needs', 1, 0, (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the main goal of marketing?'), 1, 0, GETDATE(), GETDATE());

-- Insert match pairs
INSERT INTO MatchPair (Id, LeftItem, RightItem, QuestionId, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
(NEWID(), 'CPU', 'Central Processing Unit', (SELECT TOP 1 Id FROM Question WHERE Text LIKE 'Match the following items%'), 1, 0, GETDATE(), GETDATE()),
(NEWID(), 'RAM', 'Random Access Memory', (SELECT TOP 1 Id FROM Question WHERE Text LIKE 'Match the following items%'), 1, 0, GETDATE(), GETDATE());

-- Insert test results
INSERT INTO TestResult (Id, UserId, TestId, CompletedDate, Score, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
(NEWID(), (SELECT TOP 1 Id FROM [User] WHERE Username = 'jsmith'), (SELECT TOP 1 Id FROM Test WHERE Name = 'Health Safety Exam'), GETDATE(), 80, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM [User] WHERE Username = 'ejohnson'), (SELECT TOP 1 Id FROM Test WHERE Name = 'Education Strategies Survey'), GETDATE(), 90, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM [User] WHERE Username = 'abrown'), (SELECT TOP 1 Id FROM Test WHERE Name = 'Tech Basics Quiz'), GETDATE(), 70, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM [User] WHERE Username = 'bmartin'), (SELECT TOP 1 Id FROM Test WHERE Name = 'Advanced Tech Exam'), GETDATE(), 85, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM [User] WHERE Username = 'sconnor'), (SELECT TOP 1 Id FROM Test WHERE Name = 'Finance Fundamentals Quiz'), GETDATE(), 88, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM [User] WHERE Username = 'mjordan'), (SELECT TOP 1 Id FROM Test WHERE Name = 'Marketing Essentials Exam'), GETDATE(), 92, 1, 0, GETDATE(), GETDATE());

-- Insert question results
INSERT INTO QuestionResult (Id, TestResultId, QuestionId, Answer, IsCorrect, IsActive, IsArchived, UpdatedOn, CreatedOn)
VALUES 
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'jsmith') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Health Safety Exam')), (SELECT TOP 1 Id FROM Question WHERE Text = 'Is the earth round?'), 'True', 1, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'ejohnson') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Education Strategies Survey')), (SELECT TOP 1 Id FROM Question WHERE Text = 'Describe the process of photosynthesis.'), 'It is the process by which green plants use sunlight to synthesize foods with the help of chlorophyll.', 1, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'abrown') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Tech Basics Quiz')), (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the capital of France?'), 'Paris', 1, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'bmartin') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Advanced Tech Exam')), (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the chemical symbol for water?'), 'H2O', 1, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'bmartin') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Advanced Tech Exam')), (SELECT TOP 1 Id FROM Question WHERE Text = 'Is fire hot?'), 'True', 1, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'bmartin') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Advanced Tech Exam')), (SELECT TOP 1 Id FROM Question WHERE Text LIKE 'Match the following items%'), 'CPU - Central Processing Unit, RAM - Random Access Memory', 1, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'sconnor') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Finance Fundamentals Quiz')), (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the formula for calculating compound interest?'), 'Compound interest formula', 1, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'mjordan') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Marketing Essentials Exam')), (SELECT TOP 1 Id FROM Question WHERE Text = 'What is the main goal of marketing?'), 'To meet customer needs', 1, 1, 0, GETDATE(), GETDATE()),
(NEWID(), (SELECT TOP 1 Id FROM TestResult WHERE UserId = (SELECT TOP 1 Id FROM [User] WHERE Username = 'mjordan') AND TestId = (SELECT TOP 1 Id FROM Test WHERE Name = 'Marketing Essentials Exam')), (SELECT TOP 1 Id FROM Question WHERE Text = 'Explain the importance of customer segmentation.'), 'It helps in targeting specific groups more effectively.', 1, 1, 0, GETDATE(), GETDATE());
