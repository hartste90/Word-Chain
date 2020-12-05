using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBasket
{
    // public static Dictionary<string, int> availableLetters;
    // public static Dictionary<string, int> usedLetters;

    private static List<List<string>> boggleDiceList;
    // public static List<string> availableLetters;
    // public static List<string> usedLetters;
    
    public static void Initialize()
    {
        boggleDiceList = GetBoggleDiceList();
        // availableLetters = GenerateAvailableLetters();
        // usedLetters = new List<string>();
    }
    
    private static List<List<string>> GetBoggleDiceList()
    {
        List<List<string>> diceList = new List<List<string>>();

        diceList.Add(new List<string>() { "A","A","C","I","O","T" });
        diceList.Add(new List<string>() { "A","H","M","O","R","S" });
        diceList.Add(new List<string>() { "E","G","K","L","U","Y" });
        diceList.Add(new List<string>() { "A","B","I","L","T","Y" });
        diceList.Add(new List<string>() { "A","C","D","E","M","P" });
        diceList.Add(new List<string>() { "E","G","I","N","T","V" });
        diceList.Add(new List<string>() { "G","I","L","R","U","W" });
        diceList.Add(new List<string>() { "E","L","P","S","T","U" });
        diceList.Add(new List<string>() { "D","E","N","O","S","W" });
        diceList.Add(new List<string>() { "A","C","E","L","R","S" });
        diceList.Add(new List<string>() { "A","B","J","M","O","Qu" });
        diceList.Add(new List<string>() { "E","E","F","H","I","Y" });
        diceList.Add(new List<string>() { "E","H","I","N","P","S" });
        diceList.Add(new List<string>() { "D","K","N","O","T","U" });
        diceList.Add(new List<string>() { "A","D","E","N","V","Z" });
        diceList.Add(new List<string>() { "B","I","F","O","R","X" });
        
        diceList = RandomizeDiceOrder(diceList);
        return diceList;
    }

    private static List<List<string>> RandomizeDiceOrder(List<List<string>> diceList)
    {
        List<List<string>> newList = new List<List<string>>();
        int count = diceList.Count;
        for(int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, diceList.Count);
            newList.Add(diceList[idx]);
            diceList.RemoveAt(idx);
        }
        return newList;
    }

    public static string RollDiceAtIdx (int idx)
    {
        string letter = boggleDiceList[idx][Random.Range(0, boggleDiceList[idx].Count)];
        return letter;
    }

    public static List<string> GetDiceOptionsAtIdx (int idx)
    {
        return boggleDiceList[idx];
    }

    // public static string GrabLetter()
    // {
    //     int idx = Random.Range(0, availableLetters.Count);
    //     string letter = availableLetters[idx];
    //     availableLetters.RemoveAt(idx);
    //     usedLetters.Add(letter);
    //     return letter;
    // }

    // public static void ReturnLetter(string letter)
    // {
    //     usedLetters.Remove(letter);
    //     availableLetters.Add(letter);
    // }

    // private static List<string> GenerateAvailableLetters()
    // {
    //     List<string> list = new List<string>();
    //     for(int i = 0; i < 19; i++)
    //     {
    //         list.Add("E");
    //     }
    //     for(int i = 0; i < 13; i++)
    //     {
    //         list.Add("T");
    //     }
    //     for(int i = 0; i < 12; i++)
    //     {
    //         list.Add("A");
    //         list.Add("R");
    //     }
    //     for(int i = 0; i < 11; i++)
    //     {
    //         list.Add("I");
    //         list.Add("N");
    //         list.Add("O");
    //     }
    //     for(int i = 0; i < 9; i++)
    //     {
    //         list.Add("S");
    //     }
    //     for(int i = 0; i < 6; i++)
    //     {
    //         list.Add("D");
    //     }
    //     for(int i = 0; i < 5; i++)
    //     {
    //         list.Add("C");
    //         list.Add("L");
    //         list.Add("U");
    //     }
    //     for(int i = 0; i < 4; i++)
    //     {
    //         list.Add("F");
    //         list.Add("M");
    //         list.Add("P");
    //         list.Add("U");
    //     }
    //     for(int i = 0; i < 3; i++)
    //     {
    //         list.Add("G");
    //         list.Add("Y");
    //     }
    //     for(int i = 0; i < 2; i++)
    //     {
    //         list.Add("W");
    //     }
    //     for(int i = 0; i < 1; i++)
    //     {
    //         list.Add("B");
    //         list.Add("J");
    //         list.Add("K");
    //         list.Add("Q");
    //         list.Add("V");
    //         list.Add("X");
    //         list.Add("Z");
    //     }

    //     return list;
    // }


    // private static Dictionary<string, int> GenerateAvailableLetters()
    // {
    //     return new Dictionary<string, int>()
    //     {
    //         { "e", 19 },
    //         { "t", 13 },
    //         { "a", 12 },
    //         { "r", 12 },
    //         { "i", 11 },
    //         { "n", 11 },
    //         { "o", 11 },
    //         { "s", 9 },
    //         { "d", 6 },
    //         { "c", 5 },
    //         { "h", 5 },
    //         { "l", 5 },
    //         { "f", 4 },
    //         { "m", 4 },
    //         { "p", 4 },
    //         { "u", 4 },
    //         { "g", 3 },
    //         { "y", 3 },
    //         { "w", 2 },
    //         { "b", 1 },
    //         { "j", 1 },
    //         { "k", 1 },
    //         { "q", 1 },
    //         { "v", 1 },
    //         { "x", 1 },
    //         { "z", 1 }
    //     };
    // }
}
