using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ImageandAni : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer; // Handles static images
    [SerializeField] private Animator animator; // Handles animations

    [SerializeField] private Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>(); // Store sprites
    [SerializeField] private string defaultAnimation; // Default animation fallback

    void Start()
    {
        LoadSprites(); // Load sprites at runtime
    }

    // Load sprites from Resources/Sprites folder
    private void LoadSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");
        foreach (var sprite in sprites)
        {
            spriteDictionary[sprite.name] = sprite;
        }
    }

    [YarnCommand]
    public void ChangeImage(string spriteName)
    {
        if (spriteDictionary.ContainsKey(spriteName))
        {
            Debug.Log($"Changing image to {spriteName}");
            spriteRenderer.sprite = spriteDictionary[spriteName];
            spriteRenderer.enabled = true;
            animator.enabled = false; // Disable animator to show static sprite
        }
        else
        {
            Debug.LogWarning($"Sprite '{spriteName}' not found.");
        }
    }

    [YarnCommand]
    public void PlayAnimation(string animationName)
    {
        Debug.Log($"Playing animation: {animationName}");
        animator.enabled = true;
        spriteRenderer.sprite = null; // Clear sprite
        animator.Play(animationName, -1, 0f);
    }

    [YarnCommand]
    public void ClearVisual()
    {
        Debug.Log("Clearing visuals.");
        spriteRenderer.sprite = null; // Clear sprite
        spriteRenderer.enabled = false;
        animator.enabled = false; // Stop animation
    }

    [YarnCommand]
    public void ResetToDefault()
    {
        Debug.Log("Resetting to default visual.");
        if (!string.IsNullOrEmpty(defaultAnimation))
        {
            PlayAnimation(defaultAnimation);
        }
        else
        {
            ClearVisual();
        }
    }
}
