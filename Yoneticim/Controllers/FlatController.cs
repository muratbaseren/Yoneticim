using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yoneticim.Controllers
{
    public class FlatController : Controller
    {
        YoneticimDBEntities db = new YoneticimDBEntities();

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(int mulkid, Daire daire)
        {
            Mulk mulk = db.Mulkler.Find(mulkid);

            if (mulk != null)
            {
                mulk.Daireler.Add(daire);
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Block", new { id = mulkid });
        }

        [HttpGet]
        public JsonResult Detail(int id)
        {
            var daire = db.Daireler
                .Where(x => x.Id == id)
                .Select(x => new { x.Id, x.SahibiAdi, x.SahibiSoyadi, x.SahibiTel })
                .FirstOrDefault();

            return Json(daire, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var daire = db.Daireler
                .Where(x => x.Id == id)
                .Select(x => new { x.Id, x.SahibiAdi, x.SahibiSoyadi, x.SahibiTel, x.KiraciAdi, x.KiraciSoyadi, x.KiraciTel, x.EPosta, x.Kat, x.No })
                .FirstOrDefault();

            return Json(daire, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(Daire model)
        {
            var daire = db.Daireler
                .Where(x => x.Id == model.Id).FirstOrDefault();

            if (daire != null)
            {
                daire.KiraciAdi = model.KiraciAdi;
                daire.KiraciSoyadi = model.KiraciSoyadi;
                daire.KiraciTel = model.KiraciTel;
                daire.EPosta = model.EPosta;
                daire.SahibiAdi = model.SahibiAdi;
                daire.SahibiSoyadi = model.SahibiSoyadi;
                daire.SahibiTel = model.SahibiTel;
                daire.Kat = model.Kat;
                daire.No = model.No;

                try
                {
                    db.SaveChanges();

                    return Json(db.Daireler
                                    .Where(x => x.Id == daire.Id)
                                    .Select(x => new { x.Id, x.SahibiAdi, x.SahibiSoyadi, x.SahibiTel, x.KiraciAdi, x.KiraciSoyadi, x.KiraciTel, x.EPosta, x.Kat, x.No })
                                    .FirstOrDefault(), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { hasError = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { hasError = true, message = "Daire bulunamadı." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            Daire daire = db.Daireler.Find(id);

            if (daire != null)
            {
                try
                {
                    // TODO : Daire ile ilişkili veriler temizlensin mi? Yoksa silindi olarak işaretlensin mi? Yoksa silinen veriler bir başka yere taşınsın mı?

                    db.Daireler.Remove(daire);
                    db.SaveChanges();

                    return Json(new { hasError = false, message = string.Empty }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { hasError = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { hasError = true, message = "Daire bulunamadı." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}