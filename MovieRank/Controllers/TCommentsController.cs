using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieRank.Models;

namespace MovieRank.Controllers
{
    public class TCommentsController : Controller
    {
        private MovieRankEntities db = new MovieRankEntities();

        // GET: TComments
        public ActionResult Index(int? id)
        {
            Session["id"] = id;
            Session["Name"] = (from Comp in db.TVShows where (Comp.TID == id) select Comp.Name).Single();
            Session["Ratings"] = (from Comp in db.TVShows where (Comp.TID == id) select Comp.Rating).Single();
            return View((from Comp in db.TComments where (Comp.TID == id) select Comp).ToList());
        }


        // GET: TComments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CID,TID,Comments")] TComment tComment)
        {
            int id = Convert.ToInt32(Session["id"]);
            tComment.TID = id;
            if (ModelState.IsValid)
            {
                db.TComments.Add(tComment);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }
            return View((from Comp in db.TComments where (Comp.TID == id) select Comp).ToList());
        }

        // GET: TComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TComment tComment = db.TComments.Find(id);
            if (tComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TID = new SelectList(db.TVShows, "TID", "Name", tComment.TID);
            return View(tComment);
        }

        // POST: TComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CID,TID,Comments")] TComment tComment)
        {
            int id = Convert.ToInt32(Session["id"]);
            tComment.TID = id;
            if (ModelState.IsValid)
            {
                db.Entry(tComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }
            return View((from Comp in db.TComments where (Comp.TID == id) select Comp).ToList());
        }

        // GET: TComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TComment tComment = db.TComments.Find(id);
            if (tComment == null)
            {
                return HttpNotFound();
            }
            return View(tComment);
        }

        // POST: TComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int ids = Convert.ToInt32(Session["id"]);
            TComment tComment = db.TComments.Find(id);
            db.TComments.Remove(tComment);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = ids });
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
