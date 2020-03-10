using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCPanel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MCPanel.Controllers
{
    
    [Route("/")]
    public class MinecraftController : Controller
    {
        readonly IMinecraftService minecraftService;
        readonly UserManager<ApplicationUser> _userManager;
        
        public MinecraftController(IMinecraftService m, UserManager<ApplicationUser> um)
        {
            minecraftService = m;      
            _userManager = um;
        }
        
        [Route("/")]
        [ActionName("index")]
        public async Task<IActionResult> Index()
        {            
            return View();           
        }

        [Route("console")]
        public async Task<IActionResult> Console()
        {
            List<string> list = new List<string>();
            if (System.IO.File.Exists("minecraft/logs/latest.log"))
            {
                using (var fs = new FileStream("minecraft/logs/latest.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            list.Add(sr.ReadLine());
                        }
                    }

                }
            }

            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> Start()
        {
            minecraftService.StartServer();
            return Redirect("index");
        }

        [HttpPost]
        [Route("stop")]
        public async Task<IActionResult> Stop()
        {
            minecraftService.StopServer();
            return Redirect("index");
        }

        [HttpPost]
        [Route("Restart")]
        public async Task<IActionResult> Restart()
        {
            minecraftService.RestartServer();
            return Redirect("index");
        }        
    }
}