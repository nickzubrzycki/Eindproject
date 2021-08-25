using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class RoleCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string UserId { get; set; }
        public string UserRoleId { get; set; }

        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> UserRoles { get; set; }
    }
}
