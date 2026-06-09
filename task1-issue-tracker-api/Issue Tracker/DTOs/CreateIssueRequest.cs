using Issue_Tracker.Models;

namespace Issue_Tracker.DTOs
{
    public class CreateIssueRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IssuePriority Priority { get; set; }
        public string Assignee { get; set; } = string.Empty;

    }
}
