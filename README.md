# Seznam úkolů

**Autor:** Serhii Tsurkan, xtsus001@studenti.czu.cz

## Popis

Tento program umožňuje uživatelům spravovat své úkoly a ukládat je do XML souboru. Aplikace nabízí následující funkce:
- Přidání nového úkolu s prioritou a datem splnění.
- Zobrazení seznamu úkolů s možností označit úkol jako splněný nebo ho smazat.
- Ukládání dat do XML souboru a jejich načtení při spuštění.
- Aplikace využívá menu pro interakci s uživatelem, ovládání se provádí pomocí obvyklých tlačítek klávesnice (šipky, enter, mezera atd.)

## Návrh hlavních proměnných a datových struktur

- **TextFormat**: Struktura pro zobrazení barevného nebo zvýrazněného textu. Navrženo pro snížení množství opakujícího se kódu.
- **Ukol**: Třída reprezentující úkol definovaný uživatelem.
- **Uzivatel**: Třída reprezentující uživatele. Obsahuje `List<Ukol>`, který obsahuje uživatelské úkoly.
- **Aplikace**: Třída reprezentující aktuální relace aplikace a také slouží k ukládání dat do souboru. Obsahuje `List<Uzivatel>`, který obsahuje všechny uživatele. Obsahuje metody `UlozitUdaje` (pro ukládání dat), `NahratUdaje` (pro načítání dat) a `VytvoritSoubor` (pro vytvoření souboru, do kterého se budou data zapisovat).
- **hlaska**: Proměnná, která obsahuje chybovou zprávu. Pokud dojde k chybě kvůli nesprávně zadaným údajům, je v ní umístěna zpráva. Pokud je zadání úspěšné, proměnná se vymaže.

## Koncepční popis programu

1. **Správa uživatelů**: Uživatel se identifikuje pomocí e-mailu. Program kontroluje, zda je zadaný e-mail správně formátován.
    - Pokud uživatel neexistuje, je vytvořen. Pokud již existuje, obnoví se jeho úkoly ze souboru.
2. **Menu aplikace**: Program zobrazuje hlavní menu, kde si uživatel může vybrat akci, kterou chce provést.
    - Možnosti zahrnují zobrazení seznamu úkolů, přidání nového úkolu a ukončení aplikace.
3. **Přidání úkolu**: Uživatel může přidat nový úkol s textem, prioritou a termínem splnění.
    - Program kontroluje, zda text úkolu není prázdný a priorita je mezi 1 a 5.
    - Datum splnění musí být ve správném formátu (DD-MM-YYYY) a musí být v budoucnosti nebo přítomnosti.
4. **Zobrazení úkolů**: Uživatel může zobrazit seznam svých úkolů, označit úkol jako splněný nebo ho smazat.
    - Úkoly mohou být označeny jako splněné nebo nesplněné a zobrazují se s prioritou.
5. **Ukončení a uložení**: Aplikace ukládá data do XML souboru. Při ukončení program uloží změny a ukončí se.
6. **Chytání brouků**: Pokud jsou údaje zadány nesprávně, program o tom informuje uživatele zobrazením chybového textu červeně.

## Vstupní omezení a možné problémy

- E-mail musí být ve správném formátu.
- Úkol musí mít text a priorita musí být mezi 1 a 5.
- Datum splnění musí být ve formátu DD-MM-YYYY a musí být pozdější než aktuální datum.
- Program může pracovat s více uživateli, ale při ukládání zajišťuje, že se ukládají pouze unikátní uživatelé.
- Program používá soubor `data.xml` pro ukládání a načítání dat.

## Technické poznámky

- Program je napsán v jazyce C# a využívá standardní knihovny pro práci s konzolí, soubory a XML serializací.
- Program podporuje Unicode pro zobrazování speciálních znaků v konzoli.
- Ukládání dat do XML souboru je implementováno tak, aby se ukládali pouze unikátní uživatelé podle jména.
- Autor se omlouvá, pokud jsou v textu programu lexikální chyby. To neovlivňuje činnost samotného programu.
