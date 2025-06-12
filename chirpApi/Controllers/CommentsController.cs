using chirpApi.Models;
using chirpApi.Services.Models.DTOs;
using chirpApi.Services.Models.Filters;
using chirpApi.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chirpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentsService commentService, ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        // GET: api/Comments
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
        {
            _logger.LogInformation("Received request to get all comments, no filter");
            var result = await _commentService.GetAllComments();
            return Ok(result);
        }

        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByFilter(CommentFilter filter)
        {
            _logger.LogInformation("Received request to get comments with filter: {@Filter}", filter);
            var result = await _commentService.GetCommentsByFilter(filter);

            if (result == null || !result.Any())
            {
                _logger.LogInformation("No comments found for the given filter: {@Filter}", filter);
                return NoContent();
            }
            else
            {
                _logger.LogInformation("Found {Count} comments for the given filter: {@Filter}", result.Count(), filter);
                return Ok(result);
            }
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(int id)
        {
            _logger.LogInformation("Received request to get comment with ID {Id}", id);
            var result = await _commentService.GetCommentById(id);

            if (result == null)
            {
                _logger.LogWarning("Comment with ID {Id} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Successfully retrieved comment with ID {Id}", id);
            return Ok(result);
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentUpdateDTO comment)
        {
            _logger.LogInformation("Received request to update comment with ID {Id}", id);
            var result = await _commentService.UpdateComment(id, comment);

            if (!result)
            {
                _logger.LogWarning("Comment with ID {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully updated comment with ID {Id}", id);
            return Ok(result);
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(CommentCreateDTO comment)
        {
            _logger.LogInformation("Received request to create a new comment: {@Comment}", comment);
            var result = await _commentService.CreateComment(comment);

            if (result == null)
            {
                _logger.LogWarning("Comment creation failed due to Text: {@Comment}", comment.Text);
                return BadRequest("Invalid Text [Max Length: 140]");
            }
            if(result == -1)
            {
                _logger.LogWarning("Comment creation failed due to ChirpId: {@Comment}", comment.ChirpId);
                return BadRequest("Invalid ChirpId or Chirp does not exist");
            }
            if (result == -2)
            {
                _logger.LogWarning("Comment creation failed due to ParentId: {@Comment}", comment.ParentId);
                return BadRequest("Invalid ParentId or ParentId does not exist");
            }
            return Ok(result);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            _logger.LogInformation("Received request to delete comment with ID {Id}", id);
            var result = await _commentService.DeleteComment(id);

            if (result == null)
            {
                _logger.LogWarning("Comment with ID {Id} not found", id);
                return NotFound();
            }
            if(result == -1)
            {
                _logger.LogWarning("Comment deletion failed due to other comments depending on comment: {Id}", id);
                return BadRequest("Other comments depend on comment");
            }

            _logger.LogInformation("Successfully deleted comment with ID {Id}", id);
            return Ok(result);
        }
    }
}
