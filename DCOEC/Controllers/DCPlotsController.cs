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
    public class DCPlotsController : Controller
    {
        private readonly OECContext _context;

        public DCPlotsController(OECContext context)
        {
            _context = context;
        }


        const string SessionKeyId = "_Id"; //Crop ID or Variety ID
        const string SessionKeySearchCriteria = "_SearchCriteria";




        ////GET: Plots
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int? ID, string SearchCriteria = "plot" )  //Only returns where CropId=1 dont get to see other crop ids?
        {
            //SearchCriteria = SearchCriteria ?? ""; //Null search criteria defaults to plot

            //if id and sc are passed store to session
            if (ID != null)
            {


                //Set Session

                // create session variables
                HttpContext.Session.SetInt32(SessionKeyId, Convert.ToInt32(ID));
                HttpContext.Session.SetString(SessionKeySearchCriteria, SearchCriteria);
                TempData["Message"] = "ID and SearchCriteria stored to session.";

            }

            else if ((ID == null ) && (HttpContext.Session.GetInt32(SessionKeyId)) != null)
            {
                //Read from session
                ID = Convert.ToInt32(HttpContext.Session.GetInt32(SessionKeyId));
                SearchCriteria = HttpContext.Session.GetString(SessionKeySearchCriteria);
                TempData["Message"] = "Got ID and search criteria from session.";
            }

            else //Not in Session
            {
                SearchCriteria = "";
                //TempData["Message"] = "No session was found.";
                //return RedirectToAction("Index", "DCPlots");
            }
            

            ////can be extended to fertilizer, treament and Soil likewise
            var oECContext = _context.Plot;

         
            //foreach (var item in oECContext)
            //{
            
            //item.Treatment.OrderBy(a => a.Name);
            
            //}


            switch (SearchCriteria)
            {
                case "farm":
                    return View(await _context.Plot
                                .Include(p => p.Farm)
                                .Include(p => p.Variety)
                                .Include(p => p.Treatment)
                                .Include(p => p.Variety.Crop)
                                .Where(p => p.FarmId == ID)
                                .OrderByDescending(p => p.DatePlanted)
                                 .ToListAsync());

                    ;
                case "variety":
                    return View(await _context.Plot
                                .Include(p => p.Farm)
                                .Include(p => p.Variety)
                                .Include(p => p.Treatment)
                                .Include(p => p.Variety.Crop)
                                .Where(p => p.VarietyId == ID)
                                .OrderByDescending(p => p.DatePlanted)
                                 .ToListAsync());


                    
                case "crop":
                        return View(await _context.Plot
                                .Include(p => p.Farm)
                                .Include(p => p.Variety)
                                .Include(p => p.Treatment)
                                .Include(p => p.Variety.Crop)
                                .Where(p => p.Variety.CropId == ID)
                                .OrderByDescending(p => p.DatePlanted)
                                .ToListAsync());
                case "plot":
                    return View(await _context.Plot
                                .Include(p => p.Farm)
                                .Include(p => p.Variety)
                                .Include(p => p.Treatment)
                                .Include(p => p.Variety.Crop)
                                .Where(p => p.PlotId == ID)
                                .OrderByDescending(p => p.DatePlanted)
                                 .ToListAsync());
                case "":
                    return View(await _context.Plot
                                .Include(p => p.Farm)
                                .Include(p => p.Variety)
                                .Include(p => p.Treatment)
                                .Include(p => p.Variety.Crop)
                                .OrderByDescending(p => p.DatePlanted)
                                 .ToListAsync());


                default:
                    return View(await _context.Plot.Include(p => p.Farm)
                                                    .Include(p => p.Variety)
                                                    .Include(p => p.Treatment)
                                                    .Include(p => p.Variety.Crop)
                                                    .OrderByDescending(p => p.DatePlanted)
                                                     .ToListAsync());
            }


            
        }

        


        // GET: Plots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot
                .Include(p => p.Farm)
                .Include(p => p.Variety)
                .SingleOrDefaultAsync(m => m.PlotId == id);
            if (plot == null)
            {
                return NotFound();
            }

            return View(plot);
        }

        // GET: Plots/Create
        public IActionResult Create()
        {
            
            if (HttpContext.Session.GetString(SessionKeySearchCriteria) != null)
            {
                var Id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionKeyId));
                var SearchCriteria = HttpContext.Session.GetString(SessionKeySearchCriteria);


                switch (SearchCriteria)
                {
                    case "farm":
                        break;

                    case "variety":
                        ViewData["VarietyID"] = Id;
                        ViewData["VarietyName"] = SearchCriteria;
                        break;


                    case "crop":

                        //Set View Data
                        ViewData["CropID"] = Id;
                        ViewData["CropName"] = SearchCriteria;
                        break;

                    default:
                        break;
                        
                }
                

                TempData["Message"] = "Got ID and Search Key from session.";

            }



            ViewData["FarmId"] = new SelectList(_context.Farm.OrderBy(x=>x.Name), "FarmId", "Name");
            ViewData["VarietyId"] = new SelectList(_context.Variety.OrderBy(x=>x.Name), "VarietyId", "Name");
            return View();
        }

        // POST: Plots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlotId,FarmId,VarietyId,DatePlanted,DateHarvested,PlantingRate,PlantingRateByPounds,RowWidth,PatternRepeats,OrganicMatter,BicarbP,Potassium,Magnesium,Calcium,PHsoil,PHbuffer,Cec,PercentBaseSaturationK,PercentBaseSaturationMg,PercentBaseSaturationCa,PercentBaseSaturationH,Comments")] Plot plot)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString(SessionKeySearchCriteria) != null)
                {
                    var Id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionKeyId));
                    var SearchCriteria = HttpContext.Session.GetString(SessionKeySearchCriteria);


                    switch (SearchCriteria)
                    {
                        case "farm":
                            break;

                        case "variety":
                            plot.Variety = _context.Variety
                                            .FirstOrDefault(x => x.VarietyId == plot.VarietyId);

                                //_context.Variety.SingleOrDefault(x => x.VarietyId == plot.VarietyId);
                            break;


                        case "crop":
                            plot.Variety.Crop = _context.Crop
                                                 .Include(x => x.Variety)
                                                 .FirstOrDefault (x => x.CropId == plot.Variety.Crop.CropId);

                            break;

                        default:
                            break;

                    }


                }
                
                
                _context.Add(plot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            ViewData["FarmId"] = new SelectList(_context.Farm.OrderBy(x => x.Name), "FarmId", "Name");
            ViewData["VarietyId"] = new SelectList(_context.Variety.OrderBy(x => x.Name), "VarietyId", "Name");


            //ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "ProvinceCode", plot.FarmId);
            //ViewData["VarietyId"] = new SelectList(_context.Variety, "VarietyId", "VarietyId", plot.VarietyId);
            return View(plot);
        }

        // GET: Plots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot.SingleOrDefaultAsync(m => m.PlotId == id);
            if (plot == null)
            {
                return NotFound();
            }
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "ProvinceCode", plot.FarmId);
            ViewData["VarietyId"] = new SelectList(_context.Variety, "VarietyId", "VarietyId", plot.VarietyId);
            return View(plot);
        }

        // POST: Plots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlotId,FarmId,VarietyId,DatePlanted,DateHarvested,PlantingRate,PlantingRateByPounds,RowWidth,PatternRepeats,OrganicMatter,BicarbP,Potassium,Magnesium,Calcium,PHsoil,PHbuffer,Cec,PercentBaseSaturationK,PercentBaseSaturationMg,PercentBaseSaturationCa,PercentBaseSaturationH,Comments")] Plot plot)
        {
            if (id != plot.PlotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlotExists(plot.PlotId))
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
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "ProvinceCode", plot.FarmId);
            ViewData["VarietyId"] = new SelectList(_context.Variety, "VarietyId", "VarietyId", plot.VarietyId);
            return View(plot);
        }

        // GET: Plots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot
                .Include(p => p.Farm)
                .Include(p => p.Variety)
                .SingleOrDefaultAsync(m => m.PlotId == id);
            if (plot == null)
            {
                return NotFound();
            }

            return View(plot);
        }

        // POST: Plots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plot = await _context.Plot.SingleOrDefaultAsync(m => m.PlotId == id);
            _context.Plot.Remove(plot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlotExists(int id)
        {
            return _context.Plot.Any(e => e.PlotId == id);
        }
    }
}
