using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project05Demo.Controllers
{
    public class CinemaController : Controller
    {
        // GET: Cinema
        DataClasses1DataContext _context = new DataClasses1DataContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(file.FileName);
                string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
                file.SaveAs(destFile);
            }
            return View();
        }

        public ActionResult ListMovies()
        {
            var movies = _context.CinemaMovies.ToList();
            return Json(movies, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] CinemaMovie movie)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(movie.ImagePath);

                movie.ImagePath = "images/" + fileName;
                _context.CinemaMovies.InsertOnSubmit(movie);
                _context.SubmitChanges();
            }
            return Json(movie, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var movie = _context.CinemaMovies.ToList().Find(m => m.Id == id);
            if (movie != null)
            {
                _context.CinemaMovies.DeleteOnSubmit(movie);
                _context.SubmitChanges();
            }
            return Json(movie, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int id)
        {
            var movie = _context.CinemaMovies.ToList().Find(m => m.Id == id);

            string rootPath = Server.MapPath("~/");
            string fileName = System.IO.Path.GetFileName(movie.ImagePath);
            string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
            movie.ImagePath = destFile;

            return Json(movie, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CinemaMovie movie)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(movie.ImagePath);
                movie.ImagePath = "images/" + fileName;

                CinemaMovie mv = (from p in _context.CinemaMovies
                                  where p.Id == movie.Id
                                  select p).SingleOrDefault();

                mv.MovieName = movie.MovieName;
                mv.ImagePath = movie.ImagePath;
                mv.Type = movie.Type;
                _context.SubmitChanges();

            }
            return Json(movie, JsonRequestBehavior.AllowGet);
        }
    }
}
