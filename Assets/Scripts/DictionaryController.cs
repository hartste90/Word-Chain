using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class DictionaryController
{

    private static HashSet<string> dictionarySet = new HashSet<string>();

    public static void ReadExternalDictionary()
    {

        // TextAsset textFile = Resources.Load<TextAsset>("Dictionary_EN");
        TextAsset textFile = Resources.Load("Dictionary_EN") as TextAsset;
        string text = textFile.text;
        string[] lines = text.Split('\n');
        int count = 0;
        foreach (string line in lines)
        {
            string word = line.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            dictionarySet.Add(word);
            count++;
        }

    }

    public static bool ExistsInDictionary(string word)
    {
        word = word.ToUpper();
        return dictionarySet.Contains(word);
    }

}