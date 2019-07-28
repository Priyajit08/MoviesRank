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
    public class TVShowsController : Controller
    {
        private MovieRankEntities db = new MovieRankEntities();

        // GET: TVShows
        public ActionResult Index(string nameToFind)
        {
            if (nameToFind == null)
            {
                return View(db.TVShows.ToList());
            }
            else if (string.Compare(nameToFind, "TopTV") == 0)
            {
                return View((from Comp in db.TVShows where (Comp.Rating >= 4) select Comp).ToList());
            }
            else if (string.Compare(nameToFind, "LatestTV") == 0)
            {
                DateTime dt = DateTime.Now;

                return View((from Comp in db.TVShows where (Comp.YearOfRelease.Month - dt.Month < 2 && Comp.YearOfRelease.Year - dt.Year == 0) select Comp).ToList());
            }
            else
            {
                return View((from Comp in db.TVShows where (Comp.Name.Contains(nameToFind)) select Comp).ToList());
            }
        }

        // GET: TVShows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVShow tVShow = db.TVShows.Find(id);
            if (tVShow == null)
            {
                return HttpNotFound();
            }
            return View(tVShow);
        }

        // GET: TVShows/Create
        public ActionResult Create()
        {
            ActorData d = new ActorData();
            TVShow TVShow = new TVShow();
            d.getActorList = db.Actors.Select(x => new ActorData { AID = x.AID, Bio = x.Bio, DOB = x.DOB, Name = x.Name, Sex = x.Sex, workedON = x.workedON }).ToList();
            TVShow.getActorList = d.getActorList;
            return View(TVShow);
        }

        // POST: TVShows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TID,Name,Plot,ImageFile,actorList,YearOfRelease,Rating")] TVShow tVShow)
        {
            string filename = Path.GetFileNameWithoutExtension(tVShow.ImageFile.FileName);
            string ext = Path.GetExtension(tVShow.ImageFile.FileName);
            filename = filename + ext;
            tVShow.Poster = "~/Posters/" + filename;
            filename = Path.Combine(Server.MapPath("~/Posters/"), filename);
            tVShow.ImageFile.SaveAs(filename);
            tVShow.CastID = string.Join(",", tVShow.actorList);
            string[] str = tVShow.actorList;
            foreach (string i in str)
            {
                var item = (from Comp in db.Actors where (Comp.Name == i) select Comp.AID).Single();
                Actor act = db.Actors.Find(Convert.ToInt32(item));
                if (act.workedON != null)
                {
                    act.workedON = act.workedON + "," + tVShow.Name;
                }
                else
                {
                    act.workedON = tVShow.Name;
                }
                db.Entry(act).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (ModelState.IsValid)
            {
                db.TVShows.Add(tVShow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tVShow);
        }

        // GET: TVShows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVShow tVShow = db.TVShows.Find(id);
            ActorData d = new ActorData();
            d.getActorList = db.Actors.Select(x => new ActorData { AID = x.AID, Bio = x.Bio, DOB = x.DOB, Name = x.Name, Sex = x.Sex, workedON = x.workedON }).ToList();
            tVShow.getActorList = d.getActorList;
            if (tVShow == null)
            {
                return HttpNotFound();
            }
            return View(tVShow);
        }

        // POST: TVShows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TID,Name,Plot,ImageFile,actorList,YearOfRelease,Rating")] TVShow tVShow)
        {
            string filename = Path.GetFileNameWithoutExtension(tVShow.ImageFile.FileName);
            string ext = Path.GetExtension(tVShow.ImageFile.FileName);
            filename = filename + ext;
            tVShow.Poster = "~/Posters/" + filename;
            filename = Path.Combine(Server.MapPath("~/Posters/"), filename);
            tVShow.ImageFile.SaveAs(filename);
            tVShow.CastID = string.Join(",", tVShow.actorList);
            string[] str = tVShow.actorList;
            foreach (string i in str)
            {
                int count = 0;
                var item = (from Comp in db.Actors where (Comp.Name == i) select Comp.AID).Single();
                Actor act = db.Actors.Find(Convert.ToInt32(item));
                if (act.workedON != null && count > 0)
                {
                    act.workedON = act.workedON + "," + tVShow.Name;
                }
                else
                {
                    act.workedON = tVShow.Name;
                }
                db.Entry(act).State = EntityState.Modified;
                db.SaveChanges();
                count++;
            }
            if (ModelState.IsValid)
            {
                db.Entry(tVShow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tVShow);
        }

        // GET: TVShows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVShow tVShow = db.TVShows.Find(id);
            if (tVShow == null)
            {
                return HttpNotFound();
            }
            return View(tVShow);
        }

        // POST: TVShows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TVShow tVShow = db.TVShows.Find(id);
            var i = (from Comp in db.TComments where (Comp.TID == id) select Comp).ToList();
            foreach (var del in i)
            {
                Comment comment = db.Comments.Find(del.CID);
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
            db.TVShows.Remove(tVShow);
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
