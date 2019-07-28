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
    public class CommentsController : Controller
    {
        private MovieRankEntities db = new MovieRankEntities();

        // GET: Comments
        public ActionResult Index(int? id)
        {
                Session["id"] = id;
                Session["Name"] = (from Comp in db.Movies where (Comp.MID == id) select Comp.Name).Single();
                Session["Ratings"] = (from Comp in db.Movies where (Comp.MID == id) select Comp.Rating).Single();
                return View((from Comp in db.Comments where (Comp.MID == id) select Comp).ToList());
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CID,MID,Comments")] Comment comment)
        {
            int id = Convert.ToInt32(Session["id"]);
            comment.MID = id;
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id});
            }
            return View((from Comp in db.Comments where (Comp.MID == id) select Comp).ToList());
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CID,MID,Comments")] Comment comment)
        {
            int id = Convert.ToInt32(Session["id"]);
            comment.MID = id;
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }
            ViewBag.MID = new SelectList(db.Movies, "MID", "Name", comment.MID);
            return View((from Comp in db.Comments where (Comp.MID == id) select Comp).ToList());
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int ids = Convert.ToInt32(Session["id"]);
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
