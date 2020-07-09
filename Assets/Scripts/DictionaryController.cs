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

        TextAsset textFile = Resources.Load<TextAsset>("Dictionary_EN");
        string text = textFile.text;
        string[] lines = text.Split(System.Environment.NewLine.ToCharArray());

        foreach (string line in lines)
        {
            dictionarySet.Add(line);
        }
    }

    public static bool ExistsInDictionary(string word)
    {
        word = word.ToUpper();
        return dictionarySet.Contains(word);
    }

}