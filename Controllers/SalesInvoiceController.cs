using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stock.Models;

namespace Stock.Controllers
{
    [Authorize(Roles = "storeKeeper")]
    public class SalesInvoiceController : Controller
    {
        private ApplicationContext _context;

        public SalesInvoiceController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var invoices = _context.SalesInvoices.Include(x => x.ProductInfos);
            return View(invoices);
        }

        // GET: SalesInvoices/Create
        public IActionResult Create()
        {
            var driverRoleId = _context.Roles.First(r => r.Name == "driver").Id;
            var drivers = _context.Users.Where(u => u.RoleId == driverRoleId);

            ViewBag.Drivers = new List<SelectListItem>();
            foreach (var driver in drivers)
            {
                ViewBag.Drivers.Add(new SelectListItem(driver.Email, driver.Id.ToString()));
            }

            var storeKeeperRoleId = _context.Roles.First(r => r.Name == "storeKeeper").Id;
            var storeKeepers = _context.Users.Where(u => u.RoleId == storeKeeperRoleId);

            ViewBag.StoreKeepers = new List<SelectListItem>();
            foreach (var storeKeeper in storeKeepers)
            {
                ViewBag.StoreKeepers.Add(new SelectListItem(storeKeeper.Email, storeKeeper.Id.ToString()));
            }

            var loaderRoleId = _context.Roles.First(r => r.Name == "loader").Id;
            var loaders = _context.Users.Where(u => u.RoleId == loaderRoleId);

            ViewBag.Loaders = new List<SelectListItem>();
            foreach (var loader in loaders)
            {
                ViewBag.Loaders.Add(new SelectListItem(loader.Email, loader.Id.ToString()));
            }

            return View();
        }

        // POST: SalesInvoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BroughtId,When,ShippedId,CheckedId")] SalesInvoice salesInvoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesInvoice);
        }

        public IActionResult Apply(int id)
        {
            var invoice = _context.SalesInvoices.Include(x=>x.ProductInfos).First(x => x.Id == id);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // todo test: get from stock and change state
                    foreach (var salesInfo in invoice.ProductInfos)
                    {
                        var productId = salesInfo.ProductId;
                        var stocks = _context.Stocks.Where(x => x.ProductId == productId).OrderBy(x => x.Expiration);
                        var count = salesInfo.Count;
                        decimal sumCost = 0;
                        // validate count
                        if(stocks.Sum(x => x.Count) < count)
                            throw new ArgumentException("количество превышает на складе");
                        foreach (var stock in stocks)
                        {
                            if(count == 0)
                                break;
                            if (count > stock.Count)
                            {
                                sumCost += stock.Count * stock.Cost;
                                count -= stock.Count;
                                _context.Remove(stock);
                            }
                            else
                            {
                                stock.Count -= count;
                                sumCost += count * stock.Cost;
                                break;
                            }
                        }
                        salesInfo.Cost = sumCost / salesInfo.Count;
                        _context.SaveChanges();
                    }
                    invoice.IsOutOfStock = true;
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // transaction RollBack
                }
            }
            return RedirectToAction("Index");
        }
    }
}