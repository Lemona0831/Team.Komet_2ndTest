using System.ComponentModel.DataAnnotations;

namespace Issue_Tracker.Models
{
    public class Issue  
    {
        public int Id { get; set; } //Issue 고유 ID  / Primary Key?
        public string Title { get; set; } = string.Empty; //Issue 제목
        public string? Description { get; set; } //Issue 설명
        [Required]
        public IssuePriority Priority { get; set; } //Issue 중요도
        public IssueStatus Status { get; set; } = IssueStatus.TODO; //Issue 상태
        public string Assignee { get; set; } = string.Empty; //Issue 담당자
        public DateTime CreatedAt { get; set; } //작성된 시간
        public DateTime UpdatedAt { get; set; } //수정된 시간



    }
}
