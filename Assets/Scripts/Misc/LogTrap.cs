using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogTrap : MonoBehaviour
{
    [SerializeField] float desiredRotation;

    private void Start()
    {
        transform.DORotate(new Vector3(desiredRotation, 0,0), 1f, RotateMode.Fast).SetLoops(-1, LoopType.Yoyo).SetRelative().SetEase(Ease.Linear);
    }
}
