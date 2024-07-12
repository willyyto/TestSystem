namespace TestSystem.Core.Entities
{
    public class Test : IArchivable, IMetaData, IActive
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } // Detailed description of the test
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Duration { get; set; } // Duration of the test
        public int PassMark { get; set; } // Minimum score to pass the test
        public bool IsTimed { get; set; } // Flag indicating if the test is timed
        public bool ShuffleQuestions { get; set; } // Flag indicating if questions should be shuffled
        public int MaximumAttempts { get; set; } // Maximum number of attempts allowed
        public TestVisibility Visibility { get; set; } // Visibility of the test
        public TestType TestType { get; set; } // Type of the test
        public string Instructions { get; set; } // Instructions for the test takers

        // New properties
        public FeedbackType Feedback { get; set; } // Type of feedback provided
        public AccessControl TestAccessControl { get; set; } // Access control for the test
        public GradingScheme GradingScheme { get; set; } // Grading scheme for the test
        public RetakePolicy RetakePolicy { get; set; } // Retake policy for the test

        public ICollection<Question> Questions { get; set; }
        public ICollection<TestResult> TestResults { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public enum TestVisibility
    {
        Public,
        Private,
        Restricted
    }

    public enum TestType
    {
        Quiz,
        Exam,
        Survey
    }

    public enum FeedbackType
    {
        None,
        Immediate,
        AfterCompletion
    }

    public enum AccessControl
    {
        Open,
        InviteOnly,
        PasswordProtected
    }

    public enum GradingScheme
    {
        PassFail,
        Percentage,
        LetterGrade
    }

    public class RetakePolicy
    {
        public bool AllowRetakes { get; set; }
        public int MaxRetakes { get; set; }
        public TimeSpan RetakeInterval { get; set; }
    }
}
