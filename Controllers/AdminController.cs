using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Models;
using Stock.ViewModels;

namespace Stock.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationContext _context;

        const int AdminUserId = 1;

        public AdminController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult ChangeRole()
        {
            var users = _context.Users.Where(u => u.Id != AdminUserId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, ChangeRoleViewModel>()
                //.ForMember(m=>m.RoleId, m=>m.MapFrom(x=>x.RoleId))
                );
            // Настройка AutoMapper
            var mapper = new Mapper(config);
            // сопоставление
            var viewModel = mapper.Map<List<ChangeRoleViewModel>>(users);
            ViewBag.Roles = _context.Roles.Where(r => r.Name != "admin").ToList();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ChangeRole([FromBody]List<ChangeRoleViewModel> model)
        {
            foreach (var user in _context.Users)
            {
                var updatedUser = model.FirstOrDefault(x => x.Id == user.Id);
                if (updatedUser != null)
                    user.RoleId = updatedUser.RoleId;
            }
            _context.SaveChanges();

            return Ok();
        }


    }
}