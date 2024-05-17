using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class AirlineController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public AirlineController(ApplicationDbContext context,IWebHostEnvironment environment) 
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var airline = context.Airlines.OrderByDescending(a=>a.Id).ToList();
            return View(airline);
        }

        public IActionResult Create() 
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Create(AirlineDto airlineDto)
        {
            if (airlineDto.imageFile== null)
            {
                ModelState.AddModelError("imageFile", "The image file is required");
            }

            if (!ModelState.IsValid) 
            { 
                return View(airlineDto);
            }

            //Save image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(airlineDto.imageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/project/" + newFileName;
            using (var stream=System.IO.File.Create(imageFullPath))
            {
                airlineDto.imageFile.CopyTo(stream);
            }

            //Save new airline inDB
            Airline airline = new Airline()
            {
                Name = airlineDto.Name,
                ShortName = airlineDto.ShortName,
                AirlineCode = airlineDto.AirlineCode,
                Location = airlineDto.Location,
                CreatedDate = DateTime.Now,
                imageFileName = newFileName,

            };

            context.Airlines.Add(airline);
            context.SaveChanges();
            
            return RedirectToAction("Index","Airline");
        }
        public IActionResult Edit(int id)
        {
            var airline = context.Airlines.Find(id);

            if (airline == null)
            {
                return RedirectToAction("Index","Airline");
            }

            //create AirlineDto
            var airlineDto = new AirlineDto()
            {
                Name = airline.Name,
                ShortName = airline.ShortName,
                AirlineCode = airline.AirlineCode,
                Location = airline.Location,
            };

            ViewData["AirlineId"]=airline.Id;
            ViewData["ImageFileName"]=airline.imageFileName;
            ViewData["CreatedDate"] = airline.CreatedDate.ToString("MM/dd/yyyy");

            return View(airlineDto);
        }
        [HttpPost]
        public IActionResult Edit(int id , AirlineDto airlineDto)
        {
            var airline = context.Airlines.Find(id);
            if (airline == null)
            {
                return RedirectToAction("Index", "Airline");
            }

            if(!ModelState.IsValid)
            {

                ViewData["AirlineId"] = airline.Id;
                ViewData["ImageFileName"] = airline.imageFileName;
                ViewData["CreatedDate"] = airline.CreatedDate.ToString("MM/dd/yyyy");

                return View(airlineDto);
            }

            //update the image file
            string newFileName = airline.imageFileName;
            if(airlineDto.imageFile !=null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(airlineDto.imageFile.FileName);

                string imgeFullpath=environment.WebRootPath + "/project/"+newFileName;
                using(var stream =System.IO.File.Create(imgeFullpath))
                {
                    airlineDto.imageFile.CopyTo(stream);
                }

                string oldImageFullpath = environment.WebRootPath + "/project/" + newFileName;
                using (var stream = System.IO.File.Create(oldImageFullpath))
                {
                    airlineDto.imageFile.CopyTo(stream);
                }
                
            }
            //update DB
            airline.Name = airlineDto.Name;
            airline.ShortName = airlineDto.ShortName;   
            airline.AirlineCode= airlineDto.AirlineCode;
            airline.imageFileName = newFileName;
            airline.Location = airlineDto.Location;

            context.SaveChanges();

            return RedirectToAction("Index", "Airline");

        }

        public IActionResult Activate(int id)
        {
            var airline = context.Airlines.Find(id);
            if (airline != null)
            {
                airline.Active = !airline.Active; // Toggle active state
                context.SaveChanges();
                return RedirectToAction("Index"); // Optional: Redirect back to the index view
            }

            return NotFound(); // Handle record not found scenario
        }

        public IActionResult Delete(int id)
        {
            var airline = context.Airlines.Find(id);
            if (airline == null)
            {
                return RedirectToAction("Index", "Airline");
            }

            string imageFullPath = environment.WebRootPath + "/project/" + airline.imageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Airlines.Remove(airline);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Airline");
        }


    }
}
