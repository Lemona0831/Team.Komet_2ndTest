using Issue_Tracker.Models;


namespace Issue_Tracker.Services
{
    public interface IIssueService
    {
        Task<Issue> CreateIssueAsync(Issue issue);

        Task<List<Issue>> GetIssuesAsync( 
            IssueStatus? status,
            IssuePriority? priority,
            string? assignee,
            string? sort
            );
        Task<Issue?> GetIssueByIdAsync(int id);
        Task<Issue?> UpdateIssueStatusAsync(int id, IssueStatus status);
        Task<Issue?> UpdateIssueAssigneeAsync(int id, string assignee);
        Task<bool> DeleteIssueAsync(int id);
        Task<object> GetSummaryAsync();
    }
}
