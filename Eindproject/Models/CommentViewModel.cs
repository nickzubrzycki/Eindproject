using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class CommentViewModel
    {
        public int Comment_Id { get; set; }
        public DateTime Created_Date { get; set; }

        public string Comment_Message { get; set; }

    }
}
