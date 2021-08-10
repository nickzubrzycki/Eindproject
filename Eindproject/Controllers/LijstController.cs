using Eindproject.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    public class LijstController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LijstController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext; 
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Voeg een nieuwe Film of Serie toe
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View(); 
        }

        /// <summary>
        /// Update gegevens van film of serie
        /// </summary>
        /// <returns></returns>
        public IActionResult Update()
        {
            return View(); 
        }

        /// <summary>
        /// Nakijken gegevens film en serie
        /// </summary>
        /// <returns></returns>
        public IActionResult ViewFilmSerie()
        {
            return View(); 
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
