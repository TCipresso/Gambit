using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShake : MonoBehaviour
{
    private Animator animator;
    private string animationStateName = "Cup_Roll";

    void Start()
    {
       
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        
        if (animator != null)
        {
            animator.Play(animationStateName, -1, 0f);
        }
    }
}
