using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinFlipper : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear);
    }
}
