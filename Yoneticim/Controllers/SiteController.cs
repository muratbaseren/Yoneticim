using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yoneticim.Controllers
{
    public class SiteController : Controller
    {
        YoneticimDBEntities db = new YoneticimDBEntities();

        [HttpPost]
        public ActionResult Add(string siteName, int blockCount, string address)
        {
            Mulk mulk = new Mulk()
            {
                Adi = siteName,
                Adres = address,
                BlokSayisi = blockCount
            };

            db.Mulkler.Add(mulk);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Edit(int siteId, string siteName, int blockCount, string address)
        {
            Mulk mulk = db.Mulkler.FirstOrDefault(x => x.Id == siteId);

            mulk.Adi = siteName;
            mulk.Adres = address;
            mulk.BlokSayisi = blockCount;

            db.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Delete(int siteId)
        {
            // TODO : Mulk detayları önce silinmeli..

            Mulk mulk = db.Mulkler.FirstOrDefault(x => x.Id == siteId);
            db.Mulkler.Remove(mulk);

            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            // Sadece siteleri listele..
            return View(db.Mulkler.Where(x => x.MulkId == null && x.BlokSayisi > 1).ToList());
        }

        public ActionResult Details(int id)
        {
            // Sitenin binaları getirilir.
            Mulk site = db.Mulkler.FirstOrDefault(x => x.Id == id);

            ViewBag.Mulk = site;

            return View(site.Mulkleri.ToList());
        }

        public JsonResult GetSiteInfo(int id)
        {
            // site bilgisini json olarak verir.
            Mulk site = db.Mulkler.FirstOrDefault(x => x.Id == id);

            return Json(site, JsonRequestBehavior.AllowGet);
        }
    }
}