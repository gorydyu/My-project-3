using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ChangoTest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [YarnCommand] public void ChangeImage()
    {
        Debug.Log("Test Chango");
        spriteRenderer.color = Color.red;
    }
}
