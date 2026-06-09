using Issue_Tracker.Data;
using Issue_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Issue_Tracker.Services
{
    public class IssueService : IIssueService
    {
        private readonly AppDbContext _con;

        public IssueService(AppDbContext con)
        {
            _con = con;
        }

        public async Task<Issue> CreateIssueAsync(Issue issue)
        {
            var now = DateTime.UtcNow;

            issue.Status = IssueStatus.TODO;
            issue.CreatedAt = now;
            issue.UpdatedAt = now;

            _con.Issues.Add(issue);
            await _con.SaveChangesAsync();

            return issue;
        }

        /*
         조건 설정 GET과 관련하여 AI[GPT]를 사용해 방법을 모색함.
         */
        public async Task<List<Issue>> GetIssuesAsync(
            IssueStatus? status,
            IssuePriority? priority,
            string? assignee,
            string? sort)
        {
            IQueryable<Issue> query = _con.Issues;
            if (status.HasValue)
            {
                query = query.Where(issue => issue.Status == status.Value);
            }
            if (priority.HasValue)
            {
                query = query.Where(issue => issue.Priority == priority.Value);
            }
            if (!string.IsNullOrWhiteSpace(assignee))
            {
                query = query.Where(issue => issue.Assignee == assignee);
            }
            if(sort == "priority")
            {
                query = query.OrderBy(issue => issue.Priority == IssuePriority.HIGH ? 1 :
                                        issue.Priority == IssuePriority.MEDIUM ? 2 :
                                        3
                                        )
                                        .ThenBy(issue => issue.Id);
            }
            else
            {
                query = query.OrderBy(issue => issue.Id);
            }

            return await query.ToListAsync();
        }

        public async Task<Issue?> GetIssueByIdAsync(int id)
        {
            return await _con.Issues.FindAsync(id);
        }

        public async Task<Issue?> UpdateIssueAssigneeAsync(int id, string assignee)
        {
            var issue = await _con.Issues.FindAsync(id);

            if(issue == null)
            {
                return null;
            }

            issue.Assignee = assignee;
            issue.UpdatedAt = DateTime.UtcNow;

            await _con.SaveChangesAsync();

            return issue;
        }

        public async Task<Issue?> UpdateIssueStatusAsync(int id, IssueStatus status)
        {
            var issue = await _con.Issues.FindAsync(id);

            if (issue == null)
            {
                return null;
            }

            issue.Status = status;
            issue.UpdatedAt = DateTime.UtcNow;

            await _con.SaveChangesAsync();

            return issue;
        }

        public async Task<bool> DeleteIssueAsync(int id)
        {
            var issue = await _con.Issues.FindAsync(id);

            if(issue == null)
            {
                return false;
            }

            _con.Issues.Remove(issue);
            await _con.SaveChangesAsync();

            return true;
        }
        public async Task<object> GetSummaryAsync()
        {
            var todo = await _con.Issues.CountAsync(issue => issue.Status == IssueStatus.TODO);
            var doing = await _con.Issues.CountAsync(issue => issue.Status == IssueStatus.DOING);
            var done = await _con.Issues.CountAsync(issue => issue.Status == IssueStatus.DONE);

            var highOpen = await _con.Issues.CountAsync(issue =>
            issue.Priority == IssuePriority.HIGH &&
            issue.Status != IssueStatus.DONE
            );

            var total = await _con.Issues.CountAsync();


            return new
            {
                todo,
                doing,
                done,
                highOpen,
                total
            };
        }

    }
}
