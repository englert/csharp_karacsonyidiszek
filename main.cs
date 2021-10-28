/*diszek.txt http://www.infojegyzet.hu/erettsegi/informatika-ismeretek/kozep-prog-2021maj/
1;3;0;0;0;4;0
2;4;0;5;-2;1;0
*/
using System;       // Console
using System.IO;    // StreamReader
using System.Text;  // Encoding
using System.Collections.Generic; // List<>, Dictionary<>
using System.Linq;  // from where group by orderby select

class NapiMunka{
    public static int KeszultDb { get; private set; }
    public int Nap              { get; private set; }
    public int HarangKesz       { get; private set; }
    public int HarangEladott    { get; private set; }
    public int AngyalkaKesz     { get; private set; }
    public int AngyalkaEladott  { get; private set; }
    public int FenyofaKesz      { get; private set; }
    public int FenyofaEladott   { get; private set; }

    public NapiMunka(string sor){
        string[] s = sor.Split(';');
        Nap = Convert.ToInt32(s[0]);
        HarangKesz      = Convert.ToInt32(s[1]);
        HarangEladott   = Convert.ToInt32(s[2]);
        AngyalkaKesz    = Convert.ToInt32(s[3]);
        AngyalkaEladott = Convert.ToInt32(s[4]);
        FenyofaKesz     = Convert.ToInt32(s[5]);
        FenyofaEladott  = Convert.ToInt32(s[6]);

        NapiMunka.KeszultDb += HarangKesz + AngyalkaKesz + FenyofaKesz;
    }
    public int NapiBevetel(){
        return -(HarangEladott * 1000 + AngyalkaEladott * 1350 + FenyofaEladott * 1500);
    }
}

class Program{
    public static void Main (string[] args){
        // 3.feladat 
        var f = new StreamReader("diszek.txt");
        var lista = new List<NapiMunka>();
        while(!f.EndOfStream){
            var sor  = f.ReadLine();
            var napi = new NapiMunka( sor );
            lista.Add( napi );
        }
        f.Close();
    
        // 4.feladat: Összesen {} darab dísz készült.
        Console.WriteLine ($"4.feladat: Összesen {NapiMunka.KeszultDb} darab dísz készült.");
   
        // 5. feladat: Volt olyan nap, amikor egyetlen dísz sem készült.
        bool egyetlen_disz_sem_keszult = false;
        foreach(var sor in lista){
            if( sor.HarangKesz + sor.AngyalkaKesz + sor.FenyofaKesz == 0){
                egyetlen_disz_sem_keszult = true;
                break;
            }
        }
        if(egyetlen_disz_sem_keszult){
            Console.WriteLine("5.feladat: Volt olyan nap, amikor egyetlen dísz sem készült.");
        }
        else{
            Console.WriteLine("5.feladat: Nem volt olyan nap, amikor egyetlen dísz sem készült.");
        }

        // 6. feladat "Adja meg a keresett napot [1 ... 40]:  "
        Console.WriteLine("6. feladat:");
        var nap = 0;
        do{
            Console.Write("Adja meg a keresett napot [1 ... 40]:  ");
            int.TryParse( Console.ReadLine(), out nap );
        }
        while(nap <1 || nap > 40);
        
        var harangok   = (from sor in lista where sor.Nap <= nap select (sor.HarangEladott + sor.HarangKesz));
        var angyalkak  = (from sor in lista where sor.Nap <= nap select (sor.AngyalkaEladott + sor.AngyalkaKesz));
        var fenyofak   = (from sor in lista where sor.Nap <= nap select (sor.FenyofaEladott + sor.FenyofaKesz));
         
        Console.WriteLine($"A(z) {nap}. nap végén {harangok.Sum()} harang, {angyalkak.Sum()} angyalka és {fenyofak.Sum()} fenyőfa maradt készleten.");
        
        // 7. feladat: Legtöbbet eladott dísz: {} darab
        var eladott_harang    = (from sor in lista select -sor.HarangEladott ).Sum();
        var eladott_angyalka  = (from sor in lista select -sor.AngyalkaEladott).Sum();
        var eladott_fenyofa   = (from sor in lista select -sor.FenyofaEladott).Sum();
        
        var eladasok = new List<(int, string)>(){
            (eladott_harang,  "Harang"),
            (eladott_angyalka,"Angyalka"),
            (eladott_fenyofa, "Fenyőfa")
        };
        var legtobb_darab = eladasok.Max().Item1;
        Console.WriteLine(        $"7.feladat: Legtöbbet eladott dísz: {legtobb_darab} darab");
        foreach (var i in eladasok){
            if (i.Item1 == legtobb_darab){
                Console.WriteLine($"        {i.Item2}");
            }
        }
       // 8. írja ki a bevetel.txt fájlba azokat a napi bevételeket, melyek elérték a 10 000 forintot! 
       var fw = new StreamWriter("bevetel.txt");
       int napok_szama = 0;
       foreach(var sor in lista){
           if (sor.NapiBevetel() >= 10000){
               napok_szama++;
               fw.WriteLine($"{sor.Nap}:{sor.NapiBevetel()}");
           }
       }
       fw.WriteLine($"{napok_szama} napon volt legalább 10000 Ft a bevétel.");
       fw.Close();
    } // Main

} // class Program
