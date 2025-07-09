namespace TestSystem.Core.Entities;

public class Test : IArchivable, IMetaData, IActive
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Instructions { get; set; }
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan Duration { get; set; }
    public int PassMark { get; set; }
    public bool IsTimed { get; set; }
    public bool ShuffleQuestions { get; set; }
    public int MaximumAttempts { get; set; }
    public TestVisibility Visibility { get; set; }
    public TestType TestType { get; set; }
    public FeedbackType Feedback { get; set; }
    public AccessControl TestAccessControl { get; set; }
    public GradingScheme GradingScheme { get; set; }
    public RetakePolicy RetakePolicy { get; set; }
    
    // Enhanced features
    public bool ShowProgressBar { get; set; } = true;
    public bool AllowBackNavigation { get; set; } = true;
    public bool ShowQuestionNumbers { get; set; } = true;
    public bool AutoSubmit { get; set; } = false; // Auto-submit when time expires
    public bool RequirePassword { get; set; } = false;
    public string? Password { get; set; }
    public bool ShowResultsImmediately { get; set; } = true;
    public bool ShowCorrectAnswers { get; set; } = true;
    public bool ShowScorePercentage { get; set; } = true;
    public bool EmailResults { get; set; } = false;
    public string? CustomCss { get; set; } // Custom styling
    public string? WelcomeMessage { get; set; }
    public string? CompletionMessage { get; set; }
    public string? FailureMessage { get; set; }
    public bool IsPublic { get; set; } = false;
    public string? InviteCode { get; set; } // For invite-only tests
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableUntil { get; set; }
    
    // Randomization settings
    public int? RandomQuestionCount { get; set; } // Show only N random questions
    public bool RandomizeFromPool { get; set; } = false;
    
    // Security settings
    public bool DisableCopyPaste { get; set; } = false;
    public bool FullScreenMode { get; set; } = false;
    public bool DisableRightClick { get; set; } = false;
    public bool TrackTabSwitches { get; set; } = false;
    public int MaxTabSwitches { get; set; } = 3;
    
    // Proctoring
    public bool RequireWebcam { get; set; } = false;
    public bool RequireMicrophone { get; set; } = false;
    public bool EnableScreenRecording { get; set; } = false;
    
    // Scheduling
    public bool IsScheduled { get; set; } = false;
    public List<TestSchedule> Schedules { get; set; } = new List<TestSchedule>();

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    public ICollection<TestAttempt> TestAttempts { get; set; } = new List<TestAttempt>();
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
    Completion
}

public enum AccessControl
{
    Open,
    Invite,
    Password
}

public enum GradingScheme
{
    PassFail,
    Percentage,
    LetterGrade
}