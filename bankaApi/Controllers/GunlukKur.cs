﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace bankaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GunlukKur : ControllerBase
    {
        [HttpPost]
        public ResponseData Run(RequestData request)
        {
            ResponseData result = new ResponseData();

            try
            {
                // link
                string tcmblink = string.Format("https://www.tcmb.gov.tr/kurlar/{0}.xml",       // xml olarak geliyor 
                    (request.isBugun) ? "today" : string.Format("{2}{1}/{0}{1}{2}"
                    , request.Gun.ToString().PadLeft(2, '0'), request.Ay.ToString().PadLeft(2, '0'), request.Yil
                    )
                    );

                result.Data = new List<ResponseDataKur>();

                XmlDocument doc = new XmlDocument();

                doc.Load(tcmblink);     // linkteki xml bilgisini load ettik

                // bilginin dolu olup olmadığı kontrolü
                if (doc.SelectNodes("Tarih_Date").Count < 1)        // Tarih_Date merkez bankası tarafından verilen anahtar
                {
                    result.Hata = "Kur Bilgisi Bulunamadi";
                    return result;
                }

                foreach(XmlNode node in doc.SelectNodes("Tarih_Date")[0].ChildNodes)
                {
                    ResponseDataKur kur = new ResponseDataKur();

                    kur.Kodu = node.Attributes["Kod"].Value;

                    kur.Adi = node["Isim"].InnerText;
                    kur.Birimi = int.Parse(node["Unit"].InnerText);

                    kur.AlisKuru = Convert.ToDecimal("0" + node["ForexBuying"].InnerText.Replace(",", "."));
                    kur.SatisKuru = Convert.ToDecimal("0" + node["ForexSelling"].InnerText.Replace(",", "."));
                    kur.EfektifAlisKuru = Convert.ToDecimal("0" + node["BanknoteBuying"].InnerText.Replace(",", "."));
                    kur.EfektifSatisKuru = Convert.ToDecimal("0" + node["BanknoteSelling"].InnerText.Replace(",", "."));
                    // bazı bilgiler boş string gelebilir o yüzden convert hatası çıkmaması için başa 0 koy
                    // convert işleminin olması için noktaları da virgüle çevir
                    // üstteki Isim , Unit , ForexBuying... anahtarları da merkez bankası tarafından veriliyor.



                    result.Data.Add(kur);
                }
            }
            catch ( Exception ex )
            {
                result.Hata = ex.Message;
            }

            return result;  
        }
    }
}
