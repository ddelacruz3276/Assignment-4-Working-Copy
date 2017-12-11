using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DCOEC.Models;
using Microsoft.AspNetCore.Http; // required for GetString() & SetString()

namespace DCOEC.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
           

            //ViewBag.Name = "ViewBag Example Dinesh Condur <br/>";//View Bag Example
            //ViewData["Name"] = "View Data example Dinesh Condur";
            if (TempData["Message"] == null)
            {
                TempData["Message"] = "Hi there..Navigate to Contact page and sumit a message to see me change!";

            }


            //TempData["Message"] = HttpContext.Session.GetString("Name");


            return View();
        }

        public IActionResult About()
        {

            //HttpContext.Session.SetString("Name", "Dinesh session for 5 secs");
            //HttpContext.Session.SetInt32("ID", 3);

            ViewData["Message"] = "Your application description page.";

            ////From lecture 3# Page 5
           
            //    var mary = Request.Query["fred"];
            //    ViewData["Message"] = $"Your application description {mary} page.";

            //    HttpContext.Session.SetString("artistName", "mary");

            //    List<Generic2String> headers = new List<Generic2String>();
            //    foreach (var item in Request.Headers.Keys)
            //    {
            //        headers.Add(new Generic2String($"Request.Headers['{item}']", Request.Headers[item]));
            //    }
            //    headers.Add(new Generic2String("Request.Headers['Referer']", Request.Headers["Referer"]));
            //    headers.Add(new Generic2String("Request.Host", Request.Host.ToString()));
            //    headers.Add(new Generic2String("Request.HttpContext.Connection.LocalIpAddress (user IP)", Request.HttpContext.Connection.LocalIpAddress.ToString()));
            //    headers.Add(new Generic2String("Request.HttpContext.Connection.LocalPort (user TCP port)", Request.HttpContext.Connection.LocalPort.ToString()));
            //    headers.Add(new Generic2String("Request.HttpContext.Connection.RemoteIpAddress (server IP)", Request.HttpContext.Connection.RemoteIpAddress.ToString()));
            //    headers.Add(new Generic2String("Request.HttpContext.Connection.RemotePort (server TCP port)", Request.HttpContext.Connection.RemotePort.ToString()));
            //    headers.Add(new Generic2String("Request.HttpContext.Session.Id", Request.HttpContext.Session.Id.ToString()));
            //    headers.Add(new Generic2String("Request.IsHttps", Request.IsHttps.ToString()));
            //    headers.Add(new Generic2String("Request.Method", Request.Method.ToString()));
            //    headers.Add(new Generic2String("Request.Path", Request.Path.ToString()));
            //    headers.Add(new Generic2String("Request.Protocol", Request.Protocol.ToString()));
            //    headers.Add(new Generic2String("Request.QueryString", Request.QueryString.ToString()));
            //    headers.Add(new Generic2String("Request.Query", Request.Query.ToString()));
            //    foreach (var item in Request.Query)
            //    {
            //        headers.Add(new Generic2String($"Request.Query['{item.Key}']", item.Value));
            //    }

            //    return View(mary);




                return View();
        }

        public IActionResult Contact()
        {
            //ViewData["Message"] = "Your application <strong><i>description</i></strong> page.";
            //ViewBag.Message = "Viewbag message"; //Dynamic between controller and view
            ViewData["Message"] = "Send us a message.";
            //TempData["Message"] = "TempData from Contact Page";

            return View();
            //return RedirectToAction("Index");
        }




        [HttpPost]
        public IActionResult Contact(string message)
        {
            //ViewData["Message"] = "Your application <strong><i>description</i></strong> page.";
            //ViewBag.Message = "Viewbag message"; //Dynamic between controller and view
            ViewData["Message"] = "Send us a message.";
            TempData["Message"] = "Thanks we got your message";
            //ViewData["Message"] = Request.QueryString["Message"].Value;

            //return View();
            return RedirectToAction("Index");
        }





        public IActionResult Error()
        {
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public IActionResult Temp(string tempData)
        {
            //ViewData["Message"] = "Your application <strong><i>description</i></strong> page."; //injection
            var msg = TempData["Message"];







            //if (msg.ToString == "Error")
            //{
            //    TempData["Message"] = "Bingo!";
            //    return Content("Bingo!");
            //}


//Cookie


            String toCookie = "ForCookieStore.. This message is retrieved from cookie";
            String fromCookie = null;

            // create/change a cookie called "artistId"
            // - exists in memory for duration of the browser session
            //   - unless you give it an expiry date
            Response.Cookies.Append("UserMessage", toCookie.ToString(), new CookieOptions { Expires = DateTime.Today.AddDays(2) });


            // verify cookie exists before trying to access its value
            // otherwise, page dies …user hates your site …you're assigned to documentation …puppy eats shoes
            if (Request.Cookies["UserMessage"] != null)
            {
                fromCookie = Convert.ToString(Request.Cookies["UserMessage"]);
            }

            //@{ // accessing cookie in View
            //    ViewData["Title"] = $"Albums for {Context.Request.Cookies["UserMessage"]}";
            //}


 //Session
            String toSession = "This is set in session";
            String fromSession = null;
            // create or change session variables
            //HttpContext.Session.SetInt32(nameof(artistId), artistId);
            HttpContext.Session.SetString("sessionVariable", toSession);


            // check if session variables exist before converting them:
            if (HttpContext.Session.GetString("sessionVariable") != null)
            {
                //artistId = Convert.ToInt32(HttpContext.Session.GetInt32(nameof(artistId)));
                fromSession = HttpContext.Session.GetString("sessionVariable");
            }

            //@model IEnumerable<MvcMusicStore.Models.Album>
            //@using Microsoft.AspNetCore.Http
            //@{
            //    ViewData["Title"] = $"Albums for {Context.Session.GetString("artistName")}";
            //}






            //return Content(msg);
            //return PartialView("About"); //For AJAX like call
            //return View("Index");
            //return RedirectToAction("Index");
            return Json(data: new { name = "From Cookie", value = fromCookie , key = "From Session", sessionvalue= fromSession});
        }



        //slide 3 page 8

        //public void Sample()
        //{
        //    Response.WriteAsync("<b>Phys Path: </b>" + Request.PhysicalPath + "<br/>");
        //    Response.Write("<b>User Addr: </b>" + Request.UserHostAddress + "<br/>");
        //    if (Request.UrlReferrer != null)
        //        Response.Write("<b>Referrer host: </b>" + Request.UrlReferrer.Host + "<br/>");
        //    Response.Write("<b>Browser: </b>" + Request.Browser.Browser + "<br/>");
        //    Response.Write("<b>Browser vers: </b>" + Request.Browser.Version + "<br/>");
        //    Response.Write("<b>User O/S: </b>" + Request.Browser.Platform + "<br/>");
        //    Response.Write("<b>Logon User Identity.Name: </b>" + Request.LogonUserIdentity.Name + "<br/>");
        //    Response.Write("<b>Server Variable HTTP_ACCEPT_LANGUAGE: </b>" +
        //    Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"] + "<br/>");
        //    Response.Write("<b>Server Variable HTTP_USER_AGENT: </b>" +
        //                                 Request.ServerVariables["http_user_agent"] + "<br/>");
        //}




    }
}
