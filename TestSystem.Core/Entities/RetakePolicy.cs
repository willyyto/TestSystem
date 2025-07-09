namespace TestSystem.Core.Entities;

public class RetakePolicy
{
    public bool AllowRetakes { get; set; }
    public int MaxRetakes { get; set; }
    public TimeSpan RetakeInterval { get; set; }
    public bool RequirePasswordForRetake { get; set; } = false;
    public bool ResetProgressOnRetake { get; set; } = true;
    public bool ShowPreviousResults { get; set; } = false;
    public decimal RetakePenalty { get; set; } = 0;
}