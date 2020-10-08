using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stock.Models;

namespace Stock.Controllers
{
    public class PurchaseInvoicesController : Controller
    {
        private readonly ApplicationContext _context;

        public PurchaseInvoicesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: PurchaseInvoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.PurchaseInvoices.Where(x=>x.IsOnStock == false).ToListAsync());
        }

        // GET: PurchaseInvoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }

            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Create
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

        // POST: PurchaseInvoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BroughtId,When,ShippedId,CheckedId")] PurchaseInvoice purchaseInvoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices.FindAsync(id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }
            return View(purchaseInvoice);
        }

        // POST: PurchaseInvoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BroughtId,When,ShippedId,CheckedId")] PurchaseInvoice purchaseInvoice)
        {
            if (id != purchaseInvoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseInvoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseInvoiceExists(purchaseInvoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }

            return View(purchaseInvoice);
        }

        // POST: PurchaseInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseInvoice = await _context.PurchaseInvoices.FindAsync(id);
            _context.PurchaseInvoices.Remove(purchaseInvoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseInvoiceExists(int id)
        {
            return _context.PurchaseInvoices.Any(e => e.Id == id);
        }
    }
}
