using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using notes.Hubi;
using notes.Models;

namespace notes.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<TextHub> _hub;

        public HomeController(IHubContext<TextHub> hub){
            _hub = hub;
        }

        public IActionResult Index(string node = "default")
        {
            if(node == null || node == "") node = "default";

            string contentString = "";

            TextHolder.content.TryGetValue(node, out contentString);
            if(contentString == null) contentString = "";

            ViewBag.TextHolderContent = contentString;
            ViewBag.node = node;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost()]
        public async Task<JsonResult> textReceiver(string container, string content)
        {  
            try
            {        
                if(TextHolder.content.ContainsKey(container))
                {
                    TextHolder.content[container] = content;
                }
                else
                {
                    TextHolder.content.Add(container, content);
                }   

            await _hub.Clients.All.SendAsync("ReceiveMessage", container, content);

            return Json(new {result = "ok"});

            }
            catch(Exception ex){
                //log the errors...
            }
            
            return Json(new {result = "error: something went wrong :o"});
        }
    

        [HttpGet()]
        public JsonResult textReader(string container = "default")
        {
            if(container == null) container = "default";

            string contentString = "";

            TextHolder.content.TryGetValue(container, out contentString);
            if(contentString == null) contentString = "";

            return Json(new {container = container, content = contentString,  result = "ok"});
        }
 
    }
}
