using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stock.Models;
using Stock.ViewModels;

namespace Stock.Controllers
{
    [Authorize(Roles = "storeKeeper")]
    public class StockKeeperController : Controller
    {
        private readonly ApplicationContext _context;

        public StockKeeperController(ApplicationContext context)
        {
            _context = context;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Возвращает список приходных накладных которые ещё не перевели на склад </summary>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult Index()
        {
            var pi = _context.PurchaseInvoices.Where(pi => pi.IsOnStock == false).Include(x=>x.ProductInfos);
            return View(pi);
        }

        public IActionResult Edit(int id)
        {
            var purchaseInvoiceId = id;
            var invoice = _context.PurchaseInvoices.First(x => x.Id == purchaseInvoiceId && x.IsOnStock == false);

            var loaderRoleId = _context.Roles.First(x => x.Name == "loader").Id;
            ViewData["Loaders"] = _context.Users.Where(u => u.RoleId == loaderRoleId).Select(x=>new SelectListItem(x.Email, x.Id.ToString()));
                
            ViewData["StoreKeeper"] = _context.Users.First(x => x.Email == User.Identity.Name);

            return View(invoice);
        }

        [HttpPost]
        public IActionResult Edit(PurchaseInvoice model)
        {
            var invoice = _context.PurchaseInvoices.Include(x => x.ProductInfos).First(x => x.Id == model.Id);

            if (model.IsOnStock && model.ShippedId != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductInfo, Models.Stocks>()
                            .ForMember(m => m.Id, m => m.Ignore())
                        );
                        // Настройка AutoMapper
                        var mapper = new Mapper(config);
                        // сопоставление
                        var stock = mapper.Map<List<Models.Stocks>>(invoice.ProductInfos);

                        _context.AddRange(stock);
                        _context.SaveChanges();

                        invoice.IsOnStock = true;
                        invoice.ShippedId = model.ShippedId;
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // transaction RollBack
                    }
                }
            }
            else if (model.ShippedId != null)
            {
                invoice.ShippedId = model.ShippedId;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var purchaseInvoiceId = id;
            var invoice = _context.PurchaseInvoices.Include(x=>x.ProductInfos).First(x => x.Id == purchaseInvoiceId);

            return View(invoice.ProductInfos);
        }
    }
}