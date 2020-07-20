using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExactQuestItem : QuestItem
{
    public override void AccountWord(string word)
    {
        if (word.Length == wordLengthTarget)
        {
            currentCount++;
            MarkProgressMade();
            base.AccountWord(word);
        }
        
    }

    public override void MarkProgressMade()
    {
        base.MarkProgressMade();
    }
}
