using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class BingoCard
{
    public string UniqueID { get; }
    public List<List<string>> Rows { get; }

    public BingoCard(string uniqueID, List<List<string>> rows)
    {
        UniqueID = uniqueID;
        Rows = rows;
    }
}

class Program
{
    static string GetUserInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }

    static void Main()
    {
        //Lave den første plader, hvor den først sprøger efter et unikt ID – her skal Jeg selv taste Anse.
        string plade1UniqueID = GetUserInput("Hvad er dit unikke ID for din første plade: ");
        BingoCard plade1 = new BingoCard(plade1UniqueID, new List<List<string>>
        {
            new List<string> { "30", "41", "51", "61", "74" },
            new List<string> { "8", "17", "42", "54", "82" },
            new List<string> { "9", "18", "28", "68", "78" }
        });

        //Lave den anden plader, hvor den først sprøger efter et unikt ID – her skal Jeg selv taste anse.
        string plade2UniqueID = GetUserInput("Hvad er dit unikke ID for din anden plade: ");
        BingoCard plade2 = new BingoCard(plade2UniqueID, new List<List<string>>
        {
            new List<string> { "2", "10", "31", "41", "51" },
            new List<string> { "6", "37", "53", "64", "87" },
            new List<string> { "19", "27", "39", "67", "77" }
        });

        string trukketTal = "0";
        bool bingo1Achieved = false;
        bool bingo2Achieved = false;
        bool fuldPlade = false;
        int fullLinesCount = 0;

        while (!fuldPlade)
        {
            DisplayPlader(plade1, plade2);
            Console.WriteLine();
            Console.Write("Indsæt tal her: ");
            trukketTal = Console.ReadLine();

            if (!IsValidNumber(trukketTal))
            {
                Console.WriteLine("Det er et ugyldigt tal");
                Console.ReadLine();
                Console.Clear();
                continue;
            }

            UpdatePlader(plade1, plade2, trukketTal);
            CheckForBingo(plade1, plade2, ref fullLinesCount, ref fuldPlade, ref bingo1Achieved, ref bingo2Achieved);
            Thread.Sleep(1000);
            Console.Clear();
        }
    }

    static bool IsValidNumber(string tal)
    {
        int parsedNumber;
        return int.TryParse(tal, out parsedNumber) && parsedNumber >= 1 && parsedNumber <= 90;
    }

    static void UpdatePlader(BingoCard plade1, BingoCard plade2, string trukketTal)
    {
        UpdateRække(plade1.Rows);
        UpdateRække(plade2.Rows);

        void UpdateRække(List<List<string>> plade)
        {
            foreach (var række in plade)
            {
                if (række.Contains(trukketTal))
                {
                    int index = række.IndexOf(trukketTal);
                    række[index] = "X";
                    Console.WriteLine(trukketTal + " er på Plade");
                }
            }
        }
    }

    static void CheckForBingo(BingoCard plade1, BingoCard plade2, ref int fullLinesCount, ref bool fuldPlade, ref bool bingo1Achieved, ref bool bingo2Achieved)
    {
        int fullLinesInPlade1 = CountFullLines(plade1.Rows);
        int fullLinesInPlade2 = CountFullLines(plade2.Rows);
        fullLinesCount = fullLinesInPlade1 + fullLinesInPlade2;

        if (fullLinesCount == 1 && !bingo1Achieved)
        {
            Console.WriteLine("Bingo! Du har en række!");
            Console.WriteLine("Tryk på enter for at fortsætte");
            Console.ReadLine();
            Console.Clear();
            bingo1Achieved = true;
        }

        else if (fullLinesCount == 2 && !bingo2Achieved)
        {
            Console.WriteLine("Tillykke! Du har to rækker!");
            Console.WriteLine("Tryk på enter for at fortsætte");
            Console.ReadLine();
            Console.Clear();
            bingo2Achieved = true;
        }

        else if (fullLinesCount == 3)
        {
            Console.WriteLine("Banko! Din plade er fuld!");
            Console.WriteLine("Tryk på enter for at afslutte");
            Console.ReadLine();
            Console.Clear();
            fuldPlade = true; // Set fuldPlade to true to exit the loop
        }
    }

    static int CountFullLines(List<List<string>> plader)
    {
        int count = 0;
        foreach (var plade in plader)
        {
            if (plade.All(x => x == "X"))
            {
                count++;
            }
        }
        return count;
    }

    static void DisplayPlader(BingoCard plade1, BingoCard plade2)
    {
        DisplayPlade(plade1.UniqueID, plade1.Rows);
        DisplayPlade(plade2.UniqueID, plade2.Rows);

        void DisplayPlade(string pladeNavn, List<List<string>> plader)
        {
            Console.WriteLine(pladeNavn);
            foreach (var række in plader)
            {
                foreach (var nummer in række)
                {
                    Console.Write(nummer + " ");
                }
                Console.WriteLine();
            }
        }
    }
}