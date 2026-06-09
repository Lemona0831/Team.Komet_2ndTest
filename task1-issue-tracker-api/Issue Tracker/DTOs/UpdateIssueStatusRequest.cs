using Issue_Tracker.Models;

namespace Issue_Tracker.DTOs
{
    public class UpdateIssueStatusRequest
    {
        public IssueStatus Status { get; set; }
    }
}
