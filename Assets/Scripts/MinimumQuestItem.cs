using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimumQuestItem : QuestItem
{
    public override bool AccountWord(string word)
    {
        if (word.Length >= wordLengthTarget)
        {
            currentCount++;
            MarkProgressMade();
            return base.AccountWord(word);
        }
        return false;
        
    }

    public override void MarkProgressMade()
    {
        base.MarkProgressMade();
    }
}