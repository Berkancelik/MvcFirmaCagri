using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalars.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var toplamcagri = db.TblCagrilars.Where(x => x.CagriFirma == id).Count();
            var aktifcagri = db.TblCagrilars.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            var pasifcagri = db.TblCagrilars.Where(x => x.CagriFirma == id && x.Durum == false).Count();
            var yetkili = db.TblFirmalars.Where(x => x.ID == id).Select(y => y.Yetkili).FirstOrDefault();
            var sektor = db.TblFirmalars.Where(x => x.ID == id).Select(y => y.Sektor).FirstOrDefault();
            ViewBag.c1 = toplamcagri;
            ViewBag.c2 = aktifcagri;
            ViewBag.c3 = pasifcagri;
            ViewBag.c4 = yetkili;
            ViewBag.c5 = sektor;
            return View();
        }

        public PartialViewResult Partial1()
        {


            // true okunmamış mesaj - false okunmuş mesaj
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalars.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var mesajlar = db.TblMesajlars.Where(x => x.Alici == id && x.Durum ==true).ToList();
            var mesajsayisi = db.TblMesajlars.Where(x => x.Alici == id && x.Durum == true).Count();

            ViewBag.m1 = mesajsayisi;

            return PartialView(mesajlar);
        }

        public PartialViewResult Partial2()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalars.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var cagrilar = db.TblCagrilars.Where(x => x.CagriFirma == id && x.Durum == true).ToList();
            var cagrisayisi = db.TblCagrilars.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            ViewBag.m1 = cagrisayisi;
            return PartialView(cagrilar);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
       
     
    }
}