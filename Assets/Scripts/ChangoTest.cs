using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ChangoTest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [YarnCommand] public void ChangeImage()
    {
        Debug.Log("Test Chango");
        spriteRenderer.color = Color.red;
        animator.Play(AnimationTags.CRYING_ANIMATION, -1, 0f);
    }
}
