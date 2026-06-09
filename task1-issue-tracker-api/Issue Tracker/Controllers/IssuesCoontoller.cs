using Issue_Tracker.Models;
using Issue_Tracker.Services;
using Issue_Tracker.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Issue_Tracker.Controllers
{
    [ApiController]
    [Route("issues")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Title은 비어 있을 수 없다.");
            }
            if (string.IsNullOrWhiteSpace(request.Assignee))
            {
                return BadRequest("Assingnee는 비어 있을 수 없다.");
            }

            var issue = new Issue
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Assignee = request.Assignee
            };

            var createdIssue = await _issueService.CreateIssueAsync(issue);

            return Created($"/issues/{createdIssue.Id}", createdIssue);
        }
        [HttpGet]
        public async Task<IActionResult> GetIssues(
        [FromQuery] IssueStatus? status,
        [FromQuery] IssuePriority? priority,
        [FromQuery] string? assignee,
        [FromQuery] string? sort)
        {
            if (!string.IsNullOrWhiteSpace(sort) && sort != "priority")
            {
                return BadRequest("sort는 priority만 사용할 수 있습니다.");
            }

            var issues = await _issueService.GetIssuesAsync(
                status,
                priority,
                assignee,
                sort
            );

            return Ok(issues);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetIssueById(int id)
        {
            var issue = await _issueService.GetIssueByIdAsync(id);

            if (issue == null)
            {
                return NotFound();
            }

            return Ok(issue);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateIssueStatus(
        int id,
        [FromBody] UpdateIssueStatusRequest request)
        {
            var issue = await _issueService.GetIssueByIdAsync(id);

            if (issue == null)
            {
                return NotFound();
            }

            if (issue.Status == IssueStatus.DONE && request.Status != IssueStatus.DONE)
            {
                return BadRequest("이미 DONE 상태인 이슈는 TODO 또는 DOING으로 되돌릴 수 없습니다.");
            }

            var updatedIssue = await _issueService.UpdateIssueStatusAsync(id, request.Status);

            return Ok(updatedIssue);
        }

        [HttpPatch("{id:int}/assignee")]
        public async Task<IActionResult> UpdateIssueAssignee(
            int id,
            [FromBody] UpdateIssueAssigneeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Assignee))
            {
                return BadRequest("assignee는 비어 있을 수 없습니다.");
            }

            var issue = await _issueService.GetIssueByIdAsync(id);

            if (issue == null)
            {
                return NotFound();
            }

            if (issue.Status == IssueStatus.DONE)
            {
                return BadRequest("이미 DONE 상태인 이슈는 담당자를 변경할 수 없습니다.");
            }

            var updatedIssue = await _issueService.UpdateIssueAssigneeAsync(
                id,
                request.Assignee
            );

            return Ok(updatedIssue);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteIssue(int id)
        {
            var deleted = await _issueService.DeleteIssueAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _issueService.GetSummaryAsync();

            return Ok(summary);
        }

    }
    
}
