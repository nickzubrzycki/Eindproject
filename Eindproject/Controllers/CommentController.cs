using Eindproject.Data;
using Eindproject.Domain;
using Eindproject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{

    public class CommentController : Controller
    {
        private ICommentRepository commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }


        public IActionResult DeleteComment(Comment comment)
        {
            commentRepository.DeleteComment(comment.CommentId);
            return RedirectToAction("Movie/ViewFilmSerie");
        }

        public IActionResult EditComment(Comment comment)
        {
            commentRepository.UpdateComment(comment);
            return RedirectToAction("Movie/ViewFilmSerie");
        }
        [HttpGet]
        public IActionResult Edit(MovieCommentViewModel movieCommentViewModel)
        {
            CommentViewModel commentView = new CommentViewModel();
            return PartialView("CommentModalPartial", commentView);

        }


      

    }
}
