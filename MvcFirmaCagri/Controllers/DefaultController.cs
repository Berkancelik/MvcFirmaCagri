using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFirmaCagri.Models.Entity;
namespace MvcFirmaCagri.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        DbisTakipEntities db = new DbisTakipEntities();

        public ActionResult AktifCagrilar() 
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalars.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            ViewBag.m = mail;
            var cagrilar = db.TblCagrilars.Where(x=>x.Durum==true && x.CagriFirma==id).ToList();
            return View(cagrilar);
        }

        public ActionResult PasifCagrilar()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalars.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var cagrilar = db.TblCagrilars.Where(x => x.Durum == false && x.CagriFirma == id).ToList();
            return View(cagrilar);
        }

        // sayfa yüklendiği zaman  ne olsun : aşağıdaki o anlama gelmektedir.
        [HttpGet]
        public ActionResult YeniCagri()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniCagri(TblCagrilar p )
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalars.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            p.Durum = true;
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.CagriFirma = id;
            db.TblCagrilars.Add(p);
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        public ActionResult CagriDetay(int id)
        {
            var cagri = db.TblCagriDetays.Where(x => x.Cagri == id).ToList();
            return View();
        }
        public ActionResult CagriGetir(int id)
        {
            var cagri = db.TblCagrilars.Find(id);
            return View("CagriGetir", cagri);
        }

        public ActionResult CagriDuzenle(TblCagrilar p)
        {
            var cagri = db.TblCagrilars.Find(p.ID);
            cagri.Konu = p.Konu;
            cagri.Aciklama = p.Aciklama;
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        [HttpGet]
        public ActionResult ProfilDuzenle( )
        {

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalars.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var profil = db.TblFirmalars.Where(x => x.ID == id).FirstOrDefault();
            return View(profil);
        }

        public ActionResult AnaSayfa()
        {
            return View();
        }

       
     
    }
}