using chirpApi.Models;
using chirpApi.Services.Models.DTOs;
using chirpApi.Services.Models.Filters;
using chirpApi.Services.Models.ViewModels;
using chirpApi.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Services
{
    public class JereCommentsService : ICommentsService
    {
        private readonly CinguettioContext _context;

        public JereCommentsService(CinguettioContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CommentViewModel>> GetAllComments()
        {
            var result = await _context.Comments.ToListAsync();

            return result.Select(c => new CommentViewModel
            {
                Id = c.Id,
                ChirpId = c.ChirpId,
                ParentId = c.ParentId,
                Text = c.Text,
                CreationDate = c.CreationDate
            });
        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentsByFilter(CommentFilter filter)
        {
            IQueryable<Comment> query = _context.Comments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Text))
            {
                query = query.Where(c => c.Text == filter.Text);
            }

            var result = await query.Select(c => new CommentViewModel
            {
                Id = c.Id,
                ChirpId = c.ChirpId,
                ParentId = c.ParentId,
                Text = c.Text,
                CreationDate = c.CreationDate
            }).ToListAsync();

            return result;
        }

        public async Task<CommentViewModel?> GetCommentById(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null) return null;

            return new CommentViewModel
            {
                Id = comment.Id,
                ChirpId = comment.ChirpId,
                ParentId = comment.ParentId,
                Text = comment.Text,
                CreationDate = comment.CreationDate
            };
        }

        public async Task<bool> UpdateComment(int id, CommentUpdateDTO comment)
        {
            var entity = await _context.Comments.FindAsync(id);
            if (entity == null) return false;

            if (!string.IsNullOrWhiteSpace(comment.Text)) entity.Text = comment.Text;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int?> CreateComment(CommentCreateDTO comment)
        {
            if (!string.IsNullOrWhiteSpace(comment.Text) && comment.Text.Length > 140) return null;

            var chirpEntity = await _context.Chirps.FindAsync(comment.ChirpId);
            if (comment.ChirpId <= 0 || chirpEntity == null) return -1; // Invalid ChirpId or Chirp does not exist

            var commentEntity = await _context.Comments.FindAsync(comment.ParentId);
            if ((comment.ParentId.HasValue && comment.ParentId <= 0) || commentEntity == null) return -2; // Invalid ParentId or ParentId does not exist

            var entity = new Comment
            {
                ChirpId = comment.ChirpId,
                ParentId = comment.ParentId,
                Text = comment.Text
                //CreationDate = DateTime.UtcNow
            };

            _context.Comments.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int?> DeleteComment(int id)
        {
            var entity = await _context.Comments.Include(x => x.InverseParent)
                                                .Where(x => x.Id == id)
                                                .SingleOrDefaultAsync();

            if (entity == null) return null; //non ha trovato comment
            if (entity.InverseParent != null || entity.InverseParent.Count > 0) return -1; //ha trovato comment ma ci sono commenti associati

            _context.Comments.Remove(entity);
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
