using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace PrimjeriLošegKoda
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public static string link;
        public static string link1;
        static string originalniJezik;

        public static string ZahtjevIOdgovor(string tekstZaPrijevod, string jezik)
        {
            string spremnik = SlanjeZahtjeva(tekstZaPrijevod, jezik);
            spremnik = Obrada_odgovora(spremnik);
            return spremnik;
        }


        static string SlanjeZahtjeva(string tekstZaPrijevod, string jezik)
        {
            tekstZaPrijevod = tekstZaPrijevod.Replace(" ", "%20");
            if (tekstZaPrijevod.Length == tekstZaPrijevod.LastIndexOf("%20")) { tekstZaPrijevod.Remove(tekstZaPrijevod.Length - 1); }
            if (tekstZaPrijevod.LastIndexOf("%20") == 0) { tekstZaPrijevod = tekstZaPrijevod.Substring(1); }
            string link1 = tekstZaPrijevod;
            int n = 0; int i = 0; int kon = 0;
            for (i = 1; i < 10; i++)
            {
                kon = link1.IndexOf("%20");
                link1 = link1.Substring(kon + 1);
                n = n + kon + 1;
            }
            link = "https://api.memory.translated.net/get?q=" + tekstZaPrijevod + "&langpair=" + originalniJezik + "|" + jezik;
            WebClient client = new WebClient();
            var link2 = client.DownloadData(link);
            link1 = Encoding.UTF8.GetString(link2);
            return link1;
        }

        static string Obrada_odgovora(string spremnik)
        {
            int rezanje = spremnik.IndexOf("ext");
            spremnik = spremnik.Substring(rezanje);
            rezanje = spremnik.IndexOf(":");
            spremnik = spremnik.Substring(rezanje);
            spremnik = spremnik.Substring(1);
            int zadnji = spremnik.IndexOf("match") - 4;
            spremnik = spremnik.Substring(1, zadnji);
            spremnik = String.Join(" ", spremnik.Split(' ').Reverse());
            if (spremnik.IndexOf("\\u") != -1)
            {
                while (link.IndexOf("\\u") != -1)
                {
                    zadnji = link.IndexOf("\\u");
                    link = link.Substring(zadnji);

                    link1 = link.Substring(0, 6);

                    link = link.Substring(6);
                    string stari = link1;
                    link1 = Regex.Replace(link1, @"\\u(?<Value>[a-zA-Z0-9]{4})", m => {
                        return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                    });
                    spremnik = spremnik.Replace(stari, link1);
                }
            }
            return spremnik;
        }
    }
}
