using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yoneticim.Controllers
{
    public class StaffController : Controller
    {
        private YoneticimDBEntities db = new YoneticimDBEntities();

        public ActionResult List(int mulkid, int? gorevliTurId)
        {
            List<Gorevli> gorevliler = null;

            if (gorevliTurId == null)
                gorevliler = db.Mulkler.Find(mulkid).Gorevliler.ToList();
            else
                gorevliler = db.Mulkler.Find(mulkid).Gorevliler.Where(x => x.GorevliTurleriId == gorevliTurId).ToList();

            return View(gorevliler);
        }

        public ActionResult Add()
        {
            ViewBag.GorevliTurList = new SelectList(db.GorevliTurleri.ToList(), "Id", "Adi");

            return View();
        }

        [HttpPost]
        public ActionResult Add(int mulkid, Gorevli gorevli)
        {
            if (ModelState.IsValid)
            {
                Mulk mulk = db.Mulkler.Find(mulkid);

                if (mulk != null)
                {
                    mulk.Gorevliler.Add(gorevli);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", "Block", new { id = mulkid }); 
            }

            ViewBag.GorevliTurList = new SelectList(db.GorevliTurleri.ToList(), "Id", "Adi");
            return View(gorevli);
        }

        public ActionResult Edit(int id)
        {
            Gorevli gorevli = db.Gorevliler.Find(id);

            ViewBag.GorevliTurList = new SelectList(db.GorevliTurleri.ToList(), "Id", "Adi");
            return View(gorevli);
        }

        [HttpPost]
        public ActionResult Edit(Gorevli model)
        {
            if(ModelState.IsValid)
            {
                Gorevli dbGorevli = db.Gorevliler.Find(model.Id);

                dbGorevli.Adi = model.Adi;
                dbGorevli.IsTanimi = model.IsTanimi;
                dbGorevli.Maasi = model.Maasi;
                dbGorevli.SigortaNo = model.SigortaNo;
                dbGorevli.TcNo = model.TcNo;
                dbGorevli.GorevliTurleri = db.GorevliTurleri.Find(model.GorevliTurleriId);

                db.SaveChanges();
                return RedirectToAction("List", "Staff", new { mulkid = dbGorevli.MulkId });
            }

            ViewBag.GorevliTurList = new SelectList(db.GorevliTurleri.ToList(), "Id", "Adi");
            return View(model);
        }

        public ActionResult Delete(int staffId)
        {
            Gorevli gorevli = db.Gorevliler.Find(staffId);
            int mulkId = gorevli.MulkId;

            if (gorevli != null)
            {
                // TODO : gorevliye bağlı nesneler silinmeli..

                db.Gorevliler.Remove(gorevli);
                db.SaveChanges();
            }

            return RedirectToAction("List", "Staff", new { mulkid = mulkId });
        }
    }
}