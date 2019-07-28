using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieRank.Models;
using System.IO;

namespace MovieRank.Controllers
{
    public class MoviesController : Controller
    {
        private MovieRankEntities db = new MovieRankEntities();
        
        // GET: Movies
        public ActionResult Index(string nameToFind)
        {
            if(nameToFind == null)
            {
                var movies = db.Movies;
                return View(movies.ToList());
            }
            ViewBag.SearchKey = nameToFind;
            if(string.Compare(nameToFind,"TopMovies") == 0)
            {
                return View((from Comp in db.Movies where (Comp.Rating >= 4) select Comp).ToList());
            }
            if (string.Compare(nameToFind, "LatestMovies") == 0)
            {
                DateTime dt = DateTime.Now;
                
                return View((from Comp in db.Movies where (Comp.YearOfRelease.Month - dt.Month  < 2 && Comp.YearOfRelease.Year - dt.Year == 0) select Comp).ToList());
            }
            if ((from Comp in db.Movies where (Comp.Name.Contains(nameToFind)) select Comp).ToList().Count > 0)
            {
                return View((from Comp in db.Movies where (Comp.Name.Contains(nameToFind)) select Comp).ToList());
            }
            else if((from Comp in db.Actors where (Comp.Name.Contains(nameToFind)) select Comp).ToList().Count > 0)
            {
                return RedirectToAction("Index", "Actors", new { nameToFind = nameToFind });
            }
            else if ((from Comp in db.TVShows where (Comp.Name.Contains(nameToFind)) select Comp).ToList().Count > 0)
            {
                return RedirectToAction("Index", "TVShows", new { nameToFind = nameToFind });
            }
            else
            {
                TempData["Message"] = "Not Found!";
                return RedirectToAction("Index","Home");
            }
        }
        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ActorData d = new ActorData();
            Movie movie = new Movie();
            d.getActorList = db.Actors.Select(x => new ActorData { AID = x.AID, Bio = x.Bio, DOB = x.DOB, Name = x.Name, Sex = x.Sex, workedON = x.workedON }).ToList();
            movie.getActorList = d.getActorList;
            return View(movie);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MID,Name,Plot,ImageFile,actorList,YearOfRelease,Rating")] Movie movie)
        {
            string filename = Path.GetFileNameWithoutExtension(movie.ImageFile.FileName);
            string ext = Path.GetExtension(movie.ImageFile.FileName);
            filename = filename + ext;
            movie.Poster = "~/Posters/" + filename;
            filename = Path.Combine(Server.MapPath("~/Posters/"),filename);
            movie.ImageFile.SaveAs(filename);
            movie.CastID = string.Join(",", movie.actorList);
            string[] str = movie.actorList;
            foreach (string i in str)
            {
                var item = (from Comp in db.Actors where (Comp.Name == i) select Comp.AID).Single();
                Actor act = db.Actors.Find(Convert.ToInt32(item));
                if (act.workedON != null)
                {
                    act.workedON = act.workedON + "," + movie.Name;
                }
                else
                {
                    act.workedON = movie.Name;
                }
                db.Entry(act).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            ActorData data = new ActorData();
            data.getActorList = db.Actors.Select(x => new ActorData { AID = x.AID, Bio = x.Bio, DOB = x.DOB, Name = x.Name, Sex = x.Sex, workedON = x.workedON}).ToList();
            movie.getActorList = data.getActorList;
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MID,Name,Plot,ImageFile,actorList,YearOfRelease,Rating")] Movie movie)
        {
            string filename = Path.GetFileNameWithoutExtension(movie.ImageFile.FileName);
            string ext = Path.GetExtension(movie.ImageFile.FileName);
            filename = filename + ext;
            movie.Poster = "~/Posters/" + filename;
            filename = Path.Combine(Server.MapPath("~/Posters/"), filename);
            movie.ImageFile.SaveAs(filename);
            movie.CastID = string.Join(",", movie.actorList);
            string[] str = movie.actorList;
            foreach (string i in str)
            {
                int count = 0;
                var item = (from Comp in db.Actors where (Comp.Name == i) select Comp.AID).Single();
                Actor act = db.Actors.Find(Convert.ToInt32(item));
                if (act.workedON != null && count>0)
                {
                    act.workedON = act.workedON + "," + movie.Name;
                }
                else
                {
                    act.workedON = movie.Name;
                }
                db.Entry(act).State = EntityState.Modified;
                db.SaveChanges();
                count++;
            }
            if (ModelState.IsValid)
            {
               
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            var i = (from Comp in db.Comments where (Comp.MID == id) select Comp).ToList();
            foreach (var del in i)
            {
                Comment comment = db.Comments.Find(del.CID);
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
