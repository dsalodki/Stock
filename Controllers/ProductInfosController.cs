using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stock.Models;
using Stock.ViewModels;

namespace Stock.Controllers
{
    public class ProductInfosController : Controller
    {
        private readonly ApplicationContext _context;

        public ProductInfosController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ProductInfos
        public async Task<IActionResult> Index(int id)
        {
            var purchaseInvoiceId = id;
            ViewBag.PurchaseInvoiceId = purchaseInvoiceId;
            var applicationContext = _context.ProductInfos.Include(p => p.Product)
                .Where(p => p.PurchaseInvoiceId == purchaseInvoiceId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductInfo, ProductInfoViewModel>()
                .ForMember(m=>m.ProductName, m=>m.MapFrom(x=>x.Product.Name))
            );
            // Настройка AutoMapper
            var mapper = new Mapper(config);
            // сопоставление
            var viewModel = mapper.Map<List<ProductInfoViewModel>>(await applicationContext.ToListAsync());

            return View(viewModel);
        }

        // GET: ProductInfos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInfo = await _context.ProductInfos
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productInfo == null)
            {
                return NotFound();
            }

            return View(productInfo);
        }

        // GET: ProductInfos/Create
        public IActionResult Create(int id)
        {
            ViewBag.PurchaseInvoiceId = id;
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: ProductInfos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseInvoiceId,ProductId,Count,Cost, Expiration")] ProductInfo productInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = productInfo.PurchaseInvoiceId });
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productInfo.ProductId);
            return View(productInfo);
        }

        // GET: ProductInfos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInfo = await _context.ProductInfos.FindAsync(id);
            if (productInfo == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productInfo.ProductId);
            return View(productInfo);
        }

        // POST: ProductInfos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PurchaseInvoiceId,ProductId,Count,Cost")] ProductInfo productInfo)
        {
            if (id != productInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInfoExists(productInfo.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productInfo.ProductId);
            return View(productInfo);
        }

        // GET: ProductInfos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInfo = await _context.ProductInfos
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productInfo == null)
            {
                return NotFound();
            }

            return View(productInfo);
        }

        // POST: ProductInfos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInfo = await _context.ProductInfos.FindAsync(id);
            _context.ProductInfos.Remove(productInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInfoExists(int id)
        {
            return _context.ProductInfos.Any(e => e.Id == id);
        }

        public IActionResult EditExpirationDate()
        {
            throw new NotImplementedException();
        }
    }
}
