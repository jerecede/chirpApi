using chirpApi.Models;
using chirpApi.Services.Models.DTOs;
using chirpApi.Services.Models.Filters;
using chirpApi.Services.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Services.Interfaces
{
    public interface ICommentsService
    {
        Task<IEnumerable<CommentViewModel>> GetAllComments();
        Task<IEnumerable<CommentViewModel>> GetCommentsByFilter(CommentFilter filter);
        Task<CommentViewModel?> GetCommentById(int id);
        Task<bool> UpdateComment(int id, CommentUpdateDTO comment);
        Task<int?> CreateComment(CommentCreateDTO comment);
        Task<int?> DeleteComment(int id);
    }
}
