using Eindproject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public CommentRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public void AddComment(Comment comment)
        {
            applicationDbContext.Comments.Add(comment);
            Save();
        }

        public void DeleteComment(int CommentId)
        {
            var comment  = applicationDbContext.Comments.Where(c => c.CommentId == CommentId).SingleOrDefault();
            applicationDbContext.Comments.Remove(comment);
            Save();
        }

        public IEnumerable<Comment> GetComments(string movieTitle)
        {

            return applicationDbContext.Comments.Where(c => c.MovieOrSerie_Title == movieTitle);
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }

        public void UpdateComment(Comment comment)
        {
            applicationDbContext.Comments.Update(comment);
            Save();
        }

        public Comment GetComment(int id)
        {
            return applicationDbContext.Comments.Find(id);
        }
    }

    public interface ICommentRepository
    {
        IEnumerable<Comment> GetComments(string movieTitle);

        void DeleteComment(int CommentId);

        void UpdateComment(Comment comment);
        Comment GetComment(int id);
        void AddComment(Comment comment);
        void Save();
        
    }
}
