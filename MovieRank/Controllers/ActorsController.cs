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
    public class ActorsController : Controller
    {
        private MovieRankEntities db = new MovieRankEntities();

        // GET: Actors

        public ActionResult Index(string nameToFind)
        {
            if (nameToFind == null)
            {
                return View(db.Actors.ToList());
            }
            else
            {
                return View((from Comp in db.Actors where (Comp.Name.Contains(nameToFind)) select Comp).ToList());
            }
        }
        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            MovieData d = new MovieData();
            Actor act = new Actor();
            d.getMovieList = db.Movies.Select(x => new MovieData { MID = x.MID , Name = x.Name , Plot = x.Plot}).ToList();
            act.getMovieList = d.getMovieList;
            return View(act);
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AID,Name,Sex,DOB,Bio,MovieList,ImageFileActor")] Actor actor)
        {
            string filename = Path.GetFileNameWithoutExtension(actor.ImageFileActor.FileName);
            string ext = Path.GetExtension(actor.ImageFileActor.FileName);
            filename = filename + ext;
            actor.Image = "~/ActorImg/" + filename;
            filename = Path.Combine(Server.MapPath("~/ActorImg/"), filename);
            actor.ImageFileActor.SaveAs(filename);
            if (actor.workedON != null)
            {
                actor.workedON = string.Join(",", actor.MovieList);
                string[] str = actor.MovieList;
                foreach (string i in str)
                {
                    var item = (from Comp in db.Movies where (Comp.Name == i) select Comp.MID).Single();
                    Movie MOV = db.Movies.Find(Convert.ToInt32(item));
                    if (MOV.CastID != null)
                    {
                        MOV.CastID = MOV.CastID + "," + actor.Name;
                    }
                    else
                    {
                        MOV.CastID = actor.Name;
                    }
                    db.Entry(MOV).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
                if (ModelState.IsValid)
            {
                db.Actors.Add(actor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actor);
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieData d = new MovieData();
            Actor actor = db.Actors.Find(id);
            d.getMovieList = db.Movies.Select(x => new MovieData { MID = x.MID, Name = x.Name, Plot = x.Plot , Rating = x.Rating , YearOfRelease = x.YearOfRelease}).ToList();
            actor.getMovieList = d.getMovieList;
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AID,Name,Sex,DOB,Bio,MovieList,ImageFileActor")] Actor actor)
        {
            string filename = Path.GetFileNameWithoutExtension(actor.ImageFileActor.FileName);
            string ext = Path.GetExtension(actor.ImageFileActor.FileName);
            filename = filename + ext;
            actor.Image = "~/ActorImg/" + filename;
            filename = Path.Combine(Server.MapPath("~/ActorImg/"), filename);
            actor.ImageFileActor.SaveAs(filename);
            if (actor.workedON != null)
            {
                actor.workedON = string.Join(",", actor.MovieList);
                string[] str = actor.MovieList;
                foreach (string i in str)
                {
                    var item = (from Comp in db.Movies where (Comp.Name == i) select Comp.MID).Single();
                    Movie MOV = db.Movies.Find(Convert.ToInt32(item));
                    if (MOV.CastID != null)
                    {
                        MOV.CastID = MOV.CastID + "," + actor.Name;
                    }
                    else
                    {
                        MOV.CastID = actor.Name;
                    }
                    db.Entry(MOV).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        // GET: Actors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actor actor = db.Actors.Find(id);
            db.Actors.Remove(actor);
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
