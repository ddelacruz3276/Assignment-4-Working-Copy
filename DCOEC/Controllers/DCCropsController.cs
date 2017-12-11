using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DCOEC.Models;

namespace DCOEC.Controllers
{
    public class DCCropsController : Controller
    {
        private readonly OECContext _context;

        public DCCropsController(OECContext context)
        {
            _context = context;
            

        }


        // GET: DCCrops
        public async Task<IActionResult> Index(string sortOrder="")
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.IDSortParm = sortOrder == "ID" ? "ID_desc" : "ID";
            var crops = from c in _context.Crop
                           select c;
            crops.Include(v => v.Variety);
            
            switch (sortOrder)
            {
                case "name_desc":
                    crops = crops.OrderByDescending(s => s.Name);
                    break;
                case "ID":
                    crops = crops.OrderBy(s => s.CropId);
                    break;
                //case "date_desc":
                //    crops = crops.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    crops = crops.OrderBy(s => s.Name);
                    break;
            }


            return View(await crops.ToListAsync());


            //return View(await _context.Crop
            //                .OrderBy(a => a.Name)
            //                .ToListAsync());
        }

        // GET: DCCrops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop
                .SingleOrDefaultAsync(m => m.CropId == id);
            if (crop == null)
            {
                return NotFound();
            }

            return View(crop);
        }

        // GET: DCCrops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DCCrops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CropId,Name,Image")] Crop crop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(crop);
        }

        // GET: DCCrops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop.SingleOrDefaultAsync(m => m.CropId == id);
            if (crop == null)
            {
                return NotFound();
            }
            return View(crop);
        }

        // POST: DCCrops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CropId,Name,Image")] Crop crop)
        {
            if (id != crop.CropId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CropExists(crop.CropId))
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
            return View(crop);
        }

        // GET: DCCrops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop
                .SingleOrDefaultAsync(m => m.CropId == id);
            if (crop == null)
            {
                return NotFound();
            }

            return View(crop);
        }

        // POST: DCCrops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crop = await _context.Crop.SingleOrDefaultAsync(m => m.CropId == id);
            _context.Crop.Remove(crop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CropExists(int id)
        {
            return _context.Crop.Any(e => e.CropId == id);
        }


        // GET: DCCrops
        public async Task<IActionResult> Test()
        {
            try
            {
                var crops = _context.Crop;
                return View(crops);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                ModelState.AddModelError("", "exception getting Crops from database: " + ex.Message);
                return View(new List<Crop>());
            }


            return View(await _context.Crop.ToListAsync());
        }
    }
}
