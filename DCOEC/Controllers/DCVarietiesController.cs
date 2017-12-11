using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DCOEC.Models;
using Microsoft.AspNetCore.Http;

namespace DCOEC.Controllers
{
    public class DCVarietiesController : Controller
    {
        private readonly OECContext _context;

        public DCVarietiesController(OECContext context)
        {
            _context = context;
        }



        const string SessionKeyCropId = "_CropID";
        const string SessionKeyCropName = "_CropName";




        // GET: Varieties

        public async Task<IActionResult> Index(int? CropId, string CropName)
        {
            // Requires using Microsoft.AspNetCore.Http;


            //try
            //{
                

                if ( CropId != null || CropName != null)//Some Value in CropID or Crop Name
                {

                var oECContext = _context.Variety
                               .Include(c => c.Crop)
                               .Include(p => p.Plot)
                               .Where(v => v.CropId == CropId)
                               .OrderBy(a => a.Name);

                       

                //Set View Data
                    ViewData["CropID"] = oECContext.FirstOrDefault().CropId;
                    ViewData["CropName"] = oECContext.FirstOrDefault().Crop.Name;


                ViewData["VarietyID"] = oECContext.FirstOrDefault().VarietyId;
                ViewData["VarietyName"] = oECContext.FirstOrDefault().Name;

                ViewData["Plot"] = oECContext.FirstOrDefault().Plot;


                //Set Session

                // create session variables
                    HttpContext.Session.SetInt32(SessionKeyCropId, oECContext.FirstOrDefault().Crop.CropId);
                    HttpContext.Session.SetString(SessionKeyCropName, oECContext.FirstOrDefault().Crop.Name);

                //HttpContext.Session.SetInt32(SessionKeyVarietyId, oECContext.FirstOrDefault().Crop.CropId);
                //HttpContext.Session.SetString(SessionKeyVarietyName, oECContext.FirstOrDefault().Crop.Name);


                TempData["Message"] = "Crop ID and Crop Name Stored to session.";



                return View("Index", await oECContext.ToListAsync());
                }

                else if( (CropId == null && HttpContext.Session.GetString(SessionKeyCropId) != null) ) // CropID IS null
                {
                    TempData["Message"] = "Got Crop ID and Crop Name from session.";


                //artistId = Convert.ToInt32(HttpContext.Session.GetInt32(nameof(artistId)));
                    var cropIDFromSession = Convert.ToInt32(HttpContext.Session.GetInt32(SessionKeyCropId));
                    var cropNameFromSession = HttpContext.Session.GetString(SessionKeyCropName);

                //Set View Data
                ViewData["CropID"] = cropIDFromSession;
                ViewData["CropName"] = cropNameFromSession;


                var oECContext = _context.Variety
                               .Include(c => c.Crop)
                               .Include(p => p.Plot)
                               .Where(v => v.CropId == cropIDFromSession)
                               .OrderBy(a => a.Name);




                    return View("Index", await oECContext.ToListAsync());

                }


                else
                {
                    TempData["Message"] = "Please select a crop to see its varieties.";
                    return RedirectToAction("Index","DCCrops");
                    //return View("Index", await _context.Variety.Include(c=>c.Crop).Include(p=>p.Plot).ToListAsync());
                }

               

            //}

            //catch (Exception ex)
            //{
            //    while (ex.InnerException != null) ex = ex.InnerException;
            //    ModelState.AddModelError("", "exception getting Varieties: " + ex.Message);
            //    return View(new List<Variety>());
            //}


      
            

        }





        // GET: Varieties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety
                .Include(v => v.Crop)
                .SingleOrDefaultAsync(m => m.VarietyId == id);
            if (variety == null)
            {
                return NotFound();
            }

            return View(variety);
        }

        // GET: Varieties/Create
        public IActionResult Create()
        {
            //ViewBag.CropId = new SelectList(_context.Crop, "CropId", "CropId");

            //Get CropId from session Convert.ToInt32(HttpContext.Session.GetInt32(SessionKeyCropId)

            int cId = Convert.ToInt32(HttpContext.Session.GetInt32("CropId"));

            ViewBag.cId = cId;
            ViewData["CropName"] = HttpContext.Session.GetString(SessionKeyCropName);

            return View();
        }

        // POST: Varieties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VarietyId,CropId,Name")] Variety variety)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variety);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CropId"] = new SelectList(_context.Crop, "CropId", "CropId", variety.CropId);

            //ViewData["CropId"] = new SelectList(_context.Crop, "CropId", "CropId", HttpContext.Session.GetString(SessionKeyCropId));

            int cId = Convert.ToInt32(HttpContext.Session.GetInt32("SessionKeyCropId"));

            ViewBag.cId = cId;
            ViewData["CropName"] = HttpContext.Session.GetString(SessionKeyCropName);

            return View(variety);
        }

        // GET: Varieties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety.SingleOrDefaultAsync(m => m.VarietyId == id);
            if (variety == null)
            {
                return NotFound();
            }
            //ViewData["CropId"] = new SelectList(_context.Crop, "CropId", "CropId", variety.CropId);


            int cId = Convert.ToInt32(HttpContext.Session.GetInt32(SessionKeyCropId));

            ViewBag.cId = cId;
            ViewData["CropName"] = HttpContext.Session.GetString(SessionKeyCropName);

            return View(variety);
        }

        // POST: Varieties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VarietyId,CropId,Name")] Variety variety)
        {
            if (id != variety.VarietyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variety);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VarietyExists(variety.VarietyId))
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
            ViewData["CropId"] = new SelectList(_context.Crop, "CropId", "CropId", variety.CropId);
            ViewData["CropName"] = HttpContext.Session.GetString(SessionKeyCropName);

            return View(variety);
        }

        // GET: Varieties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety
                .Include(v => v.Crop)
                .SingleOrDefaultAsync(m => m.VarietyId == id);
            if (variety == null)
            {
                return NotFound();
            }

            return View(variety);
        }

        // POST: Varieties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variety = await _context.Variety.SingleOrDefaultAsync(m => m.VarietyId == id);
            _context.Variety.Remove(variety);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VarietyExists(int id)
        {
            return _context.Variety.Any(e => e.VarietyId == id);
        }
    }
}
