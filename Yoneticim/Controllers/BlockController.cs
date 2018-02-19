using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yoneticim.Controllers
{
    public class BlockController : Controller
    {
        YoneticimDBEntities db = new YoneticimDBEntities();

        [HttpPost]
        public JsonResult Add(Mulk mulk)
        {
            mulk.BlokSayisi = 1;

            if(mulk.MulkId != null)
            {
                mulk.Mulku = db.Mulkler.Find(mulk.MulkId.Value);
            }

            try
            {
                db.Mulkler.Add(mulk);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { hasError = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { hasError = false, message = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            try
            {
                var mulk = db.Mulkler
                    .Where(x => x.Id == id)
                    .Select(x => new {x.Id, x.Adi, x.Adres })
                    .FirstOrDefault();

                return Json(mulk, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { hasError = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Edit(Mulk model)
        {
            try
            {
                Mulk mulk = db.Mulkler.Find(model.Id);
                mulk.Adi = model.Adi;
                mulk.Adres = model.Adres;

                db.SaveChanges();

                return Json(new { hasError = false, message = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { hasError = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult List()
        {
            // Sadece binaları listele..
            return View(db.Mulkler.Where(x => x.MulkId == null && x.BlokSayisi == 1).ToList());
        }

        public ActionResult Details(int id)
        {
            // daireleri listele..
            Mulk bina = db.Mulkler.FirstOrDefault(x => x.Id == id);

            ViewBag.Mulk = bina;

            return View(bina.Daireler.ToList());
        }

        [HttpPost]
        public ActionResult Delete(int blockId)
        {
            Mulk mulk = db.Mulkler.FirstOrDefault(x => x.Id == blockId);
            db.Mulkler.Remove(mulk);

            db.SaveChanges();

            return RedirectToAction("List");
        }
    }
}