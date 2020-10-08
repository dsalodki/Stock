using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock.Models;

namespace Stock.ViewModels
{
    public class ChangeRoleViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }
    }
}
