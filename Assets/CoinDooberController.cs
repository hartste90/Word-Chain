using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CoinDooberController : MonoBehaviour
{
    private int coinAmt;
    private UnityAction<int> onCompleteCallback;

    private Vector3 ogScale;

    public void Init(int amtSet, Vector2 origin, UnityAction<int> onCompleteCallbackSet)
    {
        coinAmt = amtSet;
        onCompleteCallback = onCompleteCallbackSet;

        //ogScale = transform.localScale;
        //transform.localScale = Vector3.zero;
        transform.position = origin;
        FlyTo(MoneyController.GetPurseScreenPosition(), Random.Range(1f, 1.5f));


    }

    public void FlyTo(Vector3 targetPosSet, float durationSet)
    {
        Vector3[] waypointList = new Vector3[3];
        waypointList[0] = transform.position;
        waypointList[1] = GetRandomPointOutsidePositions(transform.position, targetPosSet);
        waypointList[2] = targetPosSet + Vector3.right * (UnityEngine.Random.Range(-1f, 1f));
        transform.DOPunchScale(new Vector3(.5f, .5f, 1), .5f);
        transform.DOPath(waypointList, durationSet, PathType.CatmullRom, PathMode.Sidescroller2D).SetEase(Ease.InSine).OnComplete(OnAnimationComplete);
    }

    private Vector3 GetRandomPointOutsidePositions(Vector3 pointA, Vector3 pointB)
    {
        float dist = Vector3.Distance(pointA, pointB);
        dist *= .5f;
        Vector3 pos = new Vector3(
            UnityEngine.Random.Range(pointA.x - dist / 1.5f, pointA.x + dist / 1.5f),
            UnityEngine.Random.Range(pointA.y, pointA.y + dist / 2),
            UnityEngine.Random.Range(pointA.z, pointB.z)
            );
        return pos;
    }

    private void OnAnimationComplete()
    {
        Destroy(gameObject);
        onCompleteCallback?.Invoke(coinAmt);
    }
}


//public class Doober : GemcrackBase
//{

//    public SpriteRenderer sprite;
//    public Vector3 targetPosition;
//    public Vector3 currentPosition;
//    public Vector3 startPosition;
//    public bool isAnimating = false;
//    public float duration;
//    private float startTime;

//    public Action callback;

    

//    private Vector3 GetRandomPointBetweenPositions(Vector3 pointA, Vector3 pointB)
//    {
//        Vector3 pos = new Vector3(
//            UnityEngine.Random.Range(pointA.x, pointB.x),
//            UnityEngine.Random.Range(pointA.y, pointB.y),
//            UnityEngine.Random.Range(pointA.z, pointB.z)
//            );
//        return pos;
//    }

    

    
//}
