using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace SeznamUkolu
{

    public class Aplikace // Třída pro zobrazení aktuální relace aplikace pro pozdější uložení dat
    {
        public List<Uzivatel> Uzivatele { get; set; } // Seznam uživatelů stažených ze souboru "data.xml"

        public Aplikace()
        {
            Uzivatele = new List<Uzivatel>();
        }

        public void VytvoritSoubor() // Metoda pro vytvoření XML souboru, pokud ještě neexistuje
        {
            if (!File.Exists("data.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Uzivatel>));

                using (FileStream stream = new FileStream("data.xml", FileMode.Create))
                {
                    serializer.Serialize(stream, Uzivatele);
                }
            }
        }

        public void UlozitUdaje(string filePath) // Metoda pro uložení dat do XML souboru
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Uzivatel>));

            // Vytvoření nového seznamu pro ukládání pouze unikátních uživatelů podle jména
            List<Uzivatel> unikatniUzivatele = new List<Uzivatel>();

            foreach (var u in Uzivatele)
            {
                // Kontrola, zda již není uživatel se stejným jménem v seznamu unikátních uživatelů
                if (!unikatniUzivatele.Any(uz => uz.Jmeno == u.Jmeno))
                {
                    // Pokud uživatel se stejným jménem neexistuje, přidáme ho do seznamu unikátních uživatelů
                    unikatniUzivatele.Add(u);
                }
            }

            // Uložení pouze unikátních uživatelů do XML souboru
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                serializer.Serialize(writer, unikatniUzivatele);
            }
        }

        // Metoda pro načtení dat ze souboru do aplikace
        public void NahratUdaje(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Uzivatel>));

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                Uzivatele = (List<Uzivatel>)serializer.Deserialize(stream);
            }
        }
    }

    public class Ukol // Třída pro reprezentaci úkolu
    {
        public bool vyplneni { get; set; } // Splnění úkolu
        public string text; // Text úkolu
        public int priorita { get; set; } = 0; // Priorita úkolu
        public DateTime datumSplneni { get; set; } = DateTime.Now; // Datum splnění úkolu

        public Ukol()
        {
            vyplneni = false;
        }

        public void VyplnitUkol() // Metoda pro pohodlnu změnu proměnne vyplneni
        {
            vyplneni = !vyplneni;
        }

        public string GetText() // Metoda pro získání formátovaného textu na základě atributů úkolu 
        {
            string text = this.text;

            if (vyplneni)
            {
                text = "\x2705 " + text;
            }
            else
                text = "\x2610 " + text;

            string stars = "";

            for (int i = 0; i < priorita; i++)
            {
                stars += "\x2605 ";
            }

            return $"{text} | {datumSplneni.ToShortDateString()} | {stars}";
        }

    }

    public class Uzivatel // Třída pro reprezentaci uživatele
    {
        public string Jmeno { get; set; }
        public List<Ukol> Ukoly { get; set; } // Seznam úkolů uživatele

        public Uzivatel()
        {
            Ukoly = new List<Ukol>();
        }
    }


    class Program
    {
        public class TextFormat // Struktura pro snadné zobrazení barevného textu
        {
            // Metoda pro zobrazení textu v žluté barvě
            public void Barevny(string text)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            // Metoda pro zobrazení chybového textu červeně
            public void Hlaska(string text)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            // Metoda pro zvýraznění textu žlutým pozadím a černou barvou písma
            public void Zvyrazneny(string text)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        static void Main(string[] args)
        {


            static bool ZdaDatumSpravny(string input) // Metoda kontroluje, zda je zadané datum ve správném formátu a zda je reálné
            {
                if (input.Length != 10)
                {
                    return false;
                }

                if (input[2] != '-' || input[5] != '-')
                {
                    return false;
                }

                string[] parts = input.Split('-');

                foreach (string part in parts)
                {
                    if (!int.TryParse(part, out _))
                    {
                        return false;
                    }
                }

                string format = "dd-MM-yyyy";
                DateTime date;

                if (DateTime.TryParseExact(input, format, null, System.Globalization.DateTimeStyles.None, out date)) // Zkontrolujeme, že proměnná input odpovídá formátu proměnné format
                {
                    return date.ToString(format) == input;
                }
                else
                {
                    return false;
                }
            }

            static bool ZdaUzivatelskeJmenoSpravne(string email) // Metoda kontroluje, zda je zadané uživatelske jmeno e-mailem
            {
                var trimEmail = email.Trim();

                if (trimEmail.EndsWith("."))
                {
                    return false;
                }
                try
                {
                    var mail = new System.Net.Mail.MailAddress(email); // Pokud je e-mail napsán správně, metoda System.Net.Mail.MailAddress bude úspěšná.
                    return mail.Address == trimEmail;
                }
                catch
                {
                    return false;
                }
            }


            Console.OutputEncoding = System.Text.Encoding.Unicode; // Umožňuje zobrazit znaky Unicode v konzole

            TextFormat textFormat = new TextFormat();

            Aplikace app = new Aplikace();


            app.VytvoritSoubor();  // Vytvořime soubor "data.xml", pokud neexistuje
            app.NahratUdaje("data.xml"); // Nahravame data o uživatelích ze souboru "data.xml"


            string uzivatelskeJmeno = ""; // Proměnná, která obsahuje zadané uživatelské jméno
            string hlaska = ""; // Proměnná, do které bude uložena zpráva o chybně zadaných údajích

            while (true) // Zadání uživatelského jména
            {
                Console.Clear();

                textFormat.Barevny("Zadejte uživatelské jméno (E-mail)\n");

                if (hlaska.Length != 0)
                {
                    textFormat.Hlaska(hlaska);
                    Console.WriteLine();
                }

                string input = Console.ReadLine().ToLower().Trim();

                if (!ZdaUzivatelskeJmenoSpravne(input))
                {
                    hlaska = "Uživatelské jméno musí být ve formátu e-mailu!";
                    continue;
                }

                hlaska = "";
                uzivatelskeJmeno = input;
                break;
            }

            Uzivatel uzivatel = new() { Jmeno = uzivatelskeJmeno };

            if (app.Uzivatele.Count != 0) // Hledání uživatele v seznamu podle jména. Pokud zadaný uživatel již existuje, bude nahrazen poznámkami
            {
                foreach (Uzivatel u in app.Uzivatele)
                {
                    if (u.Jmeno == uzivatelskeJmeno)
                    {
                        uzivatel = u;
                    }
                }
            }

            string[] polozky = { "Moje úkoly", "Přidat úkol", "Uložit změny a ukončit aplikaci" }; // Seznam možností menu

            int obranaPolozka = 0; // Proměnná pro zvýraznění aktivné položky menu, aby uživatel viděl, co vybírá

            while (true)
            {
                Console.Clear();

                int pocetPolozek = polozky.Length - 1;

                if (obranaPolozka > pocetPolozek) { obranaPolozka = 0; }; // Pokud index vybraného prvku nesouhlasí s počtem prvků, resetuje se
                if (obranaPolozka < 0) { obranaPolozka = pocetPolozek; }; // Pokud index klesá do mínusu, nastavíme ho na poslední prvek

                textFormat.Barevny($"Jste přihlášen jako \"{uzivatelskeJmeno}\"\n");
                textFormat.Barevny("Změnit položku > \x2191 \x2193\nVybrat položku > Enter\n");

                
                for (int i = 0; i < polozky.Length; i++) // Výčet dostupných možností menu
                {
                    Console.WriteLine();

                    if (obranaPolozka == i) // Pokud možnost odpovídá indexu vybrané možnosti, je zvýrazněna
                    {
                        textFormat.Zvyrazneny($" {polozky[i]} ");
                    }
                    else if (i == pocetPolozek) // Poslední prvek menu je východ
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(polozky[i]);
                        Console.ResetColor();
                    } 
                    else
                    {
                        Console.WriteLine(polozky[i]);
                    }

                };

                switch (Console.ReadKey().Key) // Switch hlavního menu. Ptáme se, kterou možnost zvolí uživatel
                {
                    case ConsoleKey.DownArrow: // Změna indexu vybraného prvku při stisku tlačítek
                        obranaPolozka++;
                        break;

                    case ConsoleKey.UpArrow:
                        obranaPolozka--;
                        break;

                    case ConsoleKey.Enter:
                        switch (obranaPolozka)
                        {
                            // Přehled poznámek
                            case 0:

                                int obranyUkol = 0;
                                bool navratDoMenu = false;

                                while (!navratDoMenu) // Vyplní se pokud navratDoMenu není pravdou
                                {
                                    Console.Clear();

                                    int PocetUkolu = uzivatel.Ukoly.Count;

                                    textFormat.Barevny("Moje úkoly\n");
                                    textFormat.Barevny("Návrat do menu > Backspace\nVyplnít úkol > Enter\nVymazat úkol > Delete\n");

                                    if (uzivatel.Ukoly.Count == 0) // Uživatel nemá úkoly
                                    {
                                        textFormat.Barevny("Seznam úkolů je prázdný :/");
                                    }

                                    if (obranyUkol >= PocetUkolu) { obranyUkol = 0; }; // Pokud index vybraného prvku nesouhlasí s počtem prvků, resetuje se
                                    if (obranyUkol < 0) { obranyUkol = PocetUkolu - 1; }; // Pokud index klesá do mínusu, nastavíme ho na poslední prvek

                                    for (int i = 0; i < PocetUkolu; i++)
                                    {
                                        Console.WriteLine();
                                        if (i == obranyUkol)
                                        {
                                            textFormat.Zvyrazneny(uzivatel.Ukoly[i].GetText()); // Pokud možnost odpovídá indexu vybrané možnosti, je zvýrazněna
                                        }
                                        else
                                        {
                                            Console.WriteLine(uzivatel.Ukoly[i].GetText());
                                        }
                                    };

                                    switch (Console.ReadKey().Key) // Switch "úkolového" oddělení
                                    {
                                        case ConsoleKey.Backspace: // Zpět na menu
                                            navratDoMenu = true;
                                            break;

                                        case ConsoleKey.DownArrow: // Změna indexu vybraného prvku při stisku tlačítek
                                            obranyUkol++;
                                            continue;

                                        case ConsoleKey.UpArrow:
                                            obranyUkol--;
                                            continue;

                                        case ConsoleKey.Enter: // Vyplní úkol
                                            if (uzivatel.Ukoly.Count != 0)
                                            {
                                                uzivatel.Ukoly[obranyUkol].VyplnitUkol();
                                            }
                                            continue;

                                        case ConsoleKey.Delete: // Smaže úkol
                                            if (uzivatel.Ukoly.Count > 0)
                                            {
                                                uzivatel.Ukoly.RemoveAt(obranyUkol);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                            continue;

                                        default:
                                            break;
                                    }
                                }
                                break;

                            // Přidaní poznámky
                            case 1:


                                string text = ""; // Obsahuje zavedený text úkolu

                                while (true)
                                {
                                    Console.Clear();

                                    textFormat.Barevny("Napište svůj úkol\n");

                                    if (hlaska.Length != 0)
                                    {
                                        textFormat.Hlaska(hlaska);
                                        Console.WriteLine();
                                    }

                                    text = Console.ReadLine();

                                    if (text.Length == 0) // Ptá se text úkolu. Ošetření vstupu: úkol nemůže být prázdný
                                    {
                                        hlaska = "Úkol nemůže být prázdný!";
                                        continue;
                                    }
                                    break;
                                }

                                Console.Clear();

                                hlaska = "";

                                int priorita = 0;

                                while (priorita == 0) // Ptá se prioritu úlohy. Ošetření vstupu
                                {
                                    textFormat.Barevny("Zadějte prioritu úkolu ( 1-5 )\n");

                                    if (hlaska.Length != 0)
                                    {
                                        textFormat.Hlaska(hlaska);
                                        Console.WriteLine();
                                    }

                                    bool spravnyFormat = int.TryParse(Console.ReadLine(), out int parsedPriorita);

                                    if (spravnyFormat && parsedPriorita <= 5 && parsedPriorita > 0) // Pokud byl parsing úspěšný a číslo je mezi 1 a 5, uložte zadanou prioritu
                                    {
                                        priorita = parsedPriorita;
                                    }
                                    else
                                    {
                                        if (!spravnyFormat)
                                        {
                                            hlaska = "Priorita musí být číslo";
                                        }
                                        else
                                        {
                                            hlaska = "Priorita musí být od 1 do 5";
                                        }
                                        Console.Clear();
                                        continue;
                                    }
                                }

                                DateTime date = DateTime.Now; // Obsahuje zavedený datum úkolu

                                hlaska = "";

                                while (true) // Ptá se datům splnení úlohy. Ošetření vstupu
                                {
                                    Console.Clear();

                                    textFormat.Barevny("Zadejte požadovaný termín splnění poznamky ve formě DD-MM-YYYY\n");
                                    if (hlaska.Length != 0)
                                    {
                                        textFormat.Hlaska(hlaska);
                                        Console.WriteLine();
                                    }
                                    string input = Console.ReadLine();

                                    if (!ZdaDatumSpravny(input)) {
                                        hlaska = "Zadané datum je ve špatném formátu";
                                        continue; 
                                    }; // Zopakuje dokud není datům ve spravnem formatu

                                    string[] parts = input.Split('-');

                                    int dd = int.Parse(parts[0]);
                                    int mm = int.Parse(parts[1]);
                                    int yyyy = int.Parse(parts[2]);

                                    date = new DateTime(yyyy, mm, dd);

                                    if (DateTime.Compare(DateTime.Now.Date, date) > 0)
                                    {
                                        hlaska = "Datum splnění nemůže být dříve než dnes";
                                        continue;
                                    }

                                    hlaska = "";

                                    break;
                                }


                                Ukol novyUkol = new();

                                novyUkol.text = text;
                                novyUkol.priorita = priorita;
                                novyUkol.datumSplneni = date;

                                uzivatel.Ukoly.Add(novyUkol); // Přidaní úkolu do uživatele

                                break;

                            // Ukončení aplikace a ukládání uživatelských dat
                            case 2:

                                app.Uzivatele.Add(uzivatel);
                                app.UlozitUdaje("data.xml");
                                textFormat.Barevny("\nDěkujeme, že používáte Seznam úkolů :)");

                                return;
                        }
                        break;
                }

                Console.Clear();
                continue;
            }
        }
    }
}
