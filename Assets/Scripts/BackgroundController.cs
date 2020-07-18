using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public BackgroundBlock blockPrefab;
    public Transform blockParent;
    public Transform blockSpawnMin;
    public Transform blockSpawnMax;
    public float maxDistance = .25f;
    List<BackgroundBlock> blockList = new List<BackgroundBlock>();

    //spawns a word with the letters spread out
    public void SpawnWord(string word)
    {
        float width = blockSpawnMax.position.x - blockSpawnMin.position.x;
        float distance = Mathf.Min(maxDistance, width/word.Length); 
        float offset;
        if (word.Length % 2 != 0)
            offset = word.Length/2 * distance;
        else
            offset = (word.Length/2 - .5f ) * distance;
        for (int i = 0; i < word.Length; i++)
        {
            string letter = word.Substring(i, 1);
            float xPos = (i*distance) - offset;
            float yPos = 0f;
            if (word.Length >= 10)
            {
                yPos = i % 2 == 0 ? .35f : 0f;
            }  
            SpawnLetter(letter, xPos, yPos);
        }
    }

    private void SpawnLetter(string letter, float xPos, float yPos)
    {
        BackgroundBlock block = Instantiate<BackgroundBlock>(blockPrefab, blockParent);
        block.transform.position = new Vector3(xPos, blockSpawnMin.position.y, blockSpawnMin.position.z);
        block.transform.localPosition = block.transform.localPosition + Vector3.up * yPos;
        block.Initialize(letter);
        blockList.Add(block);
    }

    public void DestroyAllBlocks()
    {
        for(int i = 0; i < blockList.Count; i++)
        {
            Destroy(blockList[i].gameObject);
        }
        blockList.Clear();
    }
}
