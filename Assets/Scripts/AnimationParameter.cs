using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParameter : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void AnimationFinisher()
    {
        animator.SetBool("PlayedAlready", true);
    }
    public void AnimationRestarter()
    {
        animator.SetBool("PlayedAlready", false);
    }
    public void TriggerRestarter()
    {
        animator.ResetTrigger("AttackT");
    }
}
