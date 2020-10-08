using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stock.Models;

namespace Stock.Controllers
{
    [Authorize(Roles = "storeKeeper")]
    public class SalesInfosController : Controller
    {
        private readonly ApplicationContext _context;

        public SalesInfosController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: SalesInfoes
        public async Task<IActionResult> Index(int id)
        {
            var salesInvoiceId = id;
            ViewBag.SalesInvoiceId = salesInvoiceId;
            return View(await _context.SalesInfos.Where(x=>x.SalesInvoiceId == salesInvoiceId).ToListAsync());
        }

        // GET: SalesInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesInfo = await _context.SalesInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesInfo == null)
            {
                return NotFound();
            }

            return View(salesInfo);
        }

        // GET: SalesInfoes/Create
        public IActionResult Create(int id)
        {
            ViewBag.SalesInvoiceId = id;
            //ViewData["ProductId"] 
            var products = _context.Products.Select(x => new {Id= x.Id, Name = x.Name}).ToList();
            products.Add(new {
                Id = 0,
                Name = "select"
            });
            ViewData["ProductId"] = new SelectList(products, "Id", "Name");
            ((SelectList) ViewData["ProductId"]).First(x => x.Value == "0").Selected = true;

            return View();
        }

        public int GetProductCount(int productId)
        {
            var stocks = _context.Stocks.Where(x => x.ProductId == productId);
            return stocks.Sum(x=>x.Count);
        }

        public decimal GetAverageCost(int productId, int count)
        {
            var stocks = _context.Stocks.Where(x => x.ProductId == productId);
            var counter = count;
            decimal sumCost = 0;
            // validate count
            if (stocks.Sum(x => x.Count) < count)
                throw new ArgumentException("количество превышает на складе");
            foreach (var stock in stocks)
            {
                if (counter == 0)
                    break;
                if (counter > stock.Count)
                {
                    sumCost += stock.Count * stock.Cost;
                    counter -= stock.Count;
                    _context.Remove(stock);
                }
                else
                {
                    stock.Count -= counter;
                    sumCost += counter * stock.Cost;
                    break;
                }
            }
            return sumCost / count;
        }

        // POST: SalesInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Count,Cost, SalesInvoiceId")] SalesInfo salesInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "SalesInfos", new { id = salesInfo.SalesInvoiceId});
            }
            return View(salesInfo);
        }

        // GET: SalesInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesInfo = await _context.SalesInfos.FindAsync(id);
            if (salesInfo == null)
            {
                return NotFound();
            }
            return View(salesInfo);
        }

        // POST: SalesInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Count,Cost")] SalesInfo salesInfo)
        {
            if (id != salesInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesInfoExists(salesInfo.Id))
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
            return View(salesInfo);
        }

        // GET: SalesInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesInfo = await _context.SalesInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesInfo == null)
            {
                return NotFound();
            }

            return View(salesInfo);
        }

        // POST: SalesInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesInfo = await _context.SalesInfos.FindAsync(id);
            _context.SalesInfos.Remove(salesInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesInfoExists(int id)
        {
            return _context.SalesInfos.Any(e => e.Id == id);
        }
    }
}
