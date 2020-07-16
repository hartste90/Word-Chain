using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExactQuestItem : QuestItem
{
    public override void AccountWord(string word)
    {
        if (word.Length == targetCount)
        {
            currentCount++;
        }
        base.AccountWord(word);
    }
}
