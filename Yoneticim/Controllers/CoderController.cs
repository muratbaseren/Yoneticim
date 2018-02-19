using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yoneticim.Controllers
{
    public class CoderController : Controller
    {
        public string GenerateSampleData()
        {
            try
            {
                YoneticimDBEntities db = new YoneticimDBEntities();
                InsertSampleYoneticiler(db);
                InsertSampleGorevliTurleri(db);
                InsertSampleSiteAndAparts(db);
                InsertSampleBlocks(db);
                InsertSampleFlats(db);
                InsertSampleStaff(db);
                InsertSampleFinancialItems(db);

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace;
            }
        }

        private void InsertSampleGorevliTurleri(YoneticimDBEntities db)
        {
            GorevliTuru kapici = new GorevliTuru()
            {
                Adi = "Kapıcı"
            };

            GorevliTuru guvenlikci = new GorevliTuru()
            {
                Adi = "Güvenlikçi"
            };

            GorevliTuru temizlikci = new GorevliTuru()
            {
                Adi = "Temizlikçi"
            };

            db.GorevliTurleri.Add(kapici);
            db.GorevliTurleri.Add(temizlikci);
            db.GorevliTurleri.Add(guvenlikci);

            db.SaveChanges();
        }

        private void InsertSampleFlats(YoneticimDBEntities db)
        {
            List<Mulk> apartsBlokcs = db.Mulkler.Where(x => x.BlokSayisi == 1).ToList();

            foreach (Mulk apartBlock in apartsBlokcs)
            {
                for (int k = 0; k < 8; k++)
                {
                    Daire daire = new Daire()
                    {
                        EPosta = FakeData.NetworkData.GetEmail(),
                        Kat = (byte)(k + 1),
                        KiraciAdi = FakeData.NameData.GetFirstName(),
                        KiraciSoyadi = FakeData.NameData.GetSurname(),
                        KiraciTel = FakeData.PhoneNumberData.GetPhoneNumber(),
                        No = (k * 100) + 1,
                        SahibiAdi = FakeData.NameData.GetFirstName(),
                        SahibiSoyadi = FakeData.NameData.GetSurname(),
                        SahibiTel = FakeData.PhoneNumberData.GetPhoneNumber(),
                        Sifre = "123"
                    };

                    apartBlock.Daireler.Add(daire);
                }
            }

            db.SaveChanges();
        }

        private void InsertSampleFinancialItems(YoneticimDBEntities db)
        {
            foreach (Mulk mulk in db.Mulkler)
            {
                // Sadece Mülk e ait olan kalemler..
                for (int i = 0; i < FakeData.NumberData.GetNumber(15, 30); i++)
                {
                    Kalem kalem = new Kalem()
                    {
                        Ay = FakeData.NumberData.GetNumber(1, 12),
                        Yıl = FakeData.NumberData.GetNumber(2014, 2015),
                        AidatMi = false,
                        GelirMi = FakeData.BooleanData.GetBoolean(),
                        Tutar = (decimal)FakeData.NumberData.GetDouble(),
                        Açıklama = "Açıklama : " + FakeData.TextData.GetAlphabetical(10)
                    };

                    kalem.Tutar = (kalem.GelirMi) ? kalem.Tutar : kalem.Tutar * (-1);

                    mulk.Kalemler.Add(kalem);
                }

                // Daire Aidat kalemleri
                foreach (Daire daire in mulk.Daireler)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        Kalem kalem1 = new Kalem()
                        {
                            Ay = i,
                            Yıl = 2014,
                            AidatMi = true,
                            GelirMi = true,
                            Tutar = 150,
                            Açıklama = $"Aidat {i}/2014 : {daire.Kat}-{daire.No}",
                            Daireler = daire,
                            Mulkler = mulk
                        };

                        db.Kalemler.Add(kalem1);

                        Kalem kalem2 = new Kalem()
                        {
                            Ay = i,
                            Yıl = 2015,
                            AidatMi = true,
                            GelirMi = true,
                            Tutar = 200,
                            Açıklama = $"Aidat {i}/2015 : {daire.Kat}-{daire.No}",
                            Daireler = daire,
                            Mulkler = mulk
                        };

                        db.Kalemler.Add(kalem2);
                    }
                }

                foreach (Gorevli gorevli in mulk.Gorevliler)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        Kalem kalem1 = new Kalem()
                        {
                            Ay = i,
                            Yıl = 2014,
                            AidatMi = true,
                            GelirMi = false,
                            Tutar = 1500,
                            Açıklama = $"Maaş {i}/2014 : {gorevli.TcNo}",
                            Mulkler = mulk,
                            Gorevliler = gorevli
                        };

                        db.Kalemler.Add(kalem1);

                        Kalem kalem2 = new Kalem()
                        {
                            Ay = i,
                            Yıl = 2015,
                            AidatMi = true,
                            GelirMi = false,
                            Tutar = 1750,
                            Açıklama = $"Maaş {i}/2015 : {gorevli.TcNo}",
                            Mulkler = mulk,
                            Gorevliler = gorevli
                        };

                        db.Kalemler.Add(kalem2);
                    }
                }

            }

            db.SaveChanges();
        }

        private void InsertSampleStaff(YoneticimDBEntities db)
        {
            List<GorevliTuru> turler = db.GorevliTurleri.ToList();

            foreach (Mulk mulk in db.Mulkler)
            {
                for (int i = 0; i < FakeData.NumberData.GetNumber(1,3); i++)
                {
                    Gorevli gorevli = new Gorevli()
                    {
                        Adi = FakeData.NameData.GetFirstName(),
                        Soyadi = FakeData.NameData.GetSurname(),
                        GorevliTurleri = turler[FakeData.NumberData.GetNumber(0, turler.Count - 1)],
                        IsTanimi = FakeData.TextData.GetSentence(),
                        Maasi = FakeData.NumberData.GetNumber(1000, 2000),
                        SigortaNo = "S" + FakeData.NumberData.GetNumber(10000, 30000),
                        TcNo = FakeData.NumberData.GetNumber(20000, 30000).ToString() + FakeData.NumberData.GetNumber(20000, 30000).ToString(),
                    };

                    mulk.Gorevliler.Add(gorevli);
                }
            }

            db.SaveChanges();
        }

        private void InsertSampleSiteAndAparts(YoneticimDBEntities db)
        {
            // Site ve apt olan kayıtlar atıldı.
            for (int i = 0; i < FakeData.NumberData.GetNumber(4, 6); i++)
            {
                Mulk mulk = new Mulk()
                {
                    Adres = FakeData.PlaceData.GetAddress(),
                    BlokSayisi = (byte)FakeData.NumberData.GetNumber(1, 5)
                };

                if (mulk.BlokSayisi > 1)
                    mulk.Adi = $"Site-{i}";
                else
                    mulk.Adi = $"Apt-{i}";

                db.Mulkler.Add(mulk);
            }

            db.SaveChanges();
        }

        private void InsertSampleBlocks(YoneticimDBEntities db)
        {
            List<Mulk> siteler = db.Mulkler.Where(x => x.BlokSayisi > 1).ToList();

            foreach (Mulk site in siteler)
            {
                for (int i = 0; i < site.BlokSayisi; i++)
                {
                    Mulk mulk = new Mulk()
                    {
                        Adi = $"Blok-{i}",
                        BlokSayisi = 1,
                        Mulku = site
                    };

                    db.Mulkler.Add(mulk);
                }
            }

            db.SaveChanges();
        }

        private void InsertSampleYoneticiler(YoneticimDBEntities db)
        {
            Yonetici yonetici1 = new Yonetici()
            {
                Ad = "Egehan",
                Soyad = "Gür",
                EPosta = "egehan@mail.com",
                Sifre = "123"
            };

            Yonetici yonetici2 = new Yonetici()
            {
                Ad = "Murat",
                Soyad = "Başeren",
                EPosta = "murat@mail.com",
                Sifre = "123"
            };

            db.Yoneticiler.Add(yonetici1);
            db.Yoneticiler.Add(yonetici2);

            db.SaveChanges();
        }
    }
}