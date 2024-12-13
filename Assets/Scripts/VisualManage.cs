using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class VisualManage : MonoBehaviour
{
    [SerializeField] private Image visualDisplay; // Reference to the UI Image
    [SerializeField] private Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

    void Start()
    {
        LoadSprites();
        DebugSavedSprites();
    }
    

    // Load sprites from the Resources/Sprites folder
    private void LoadSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");
        Debug.Log($"Number of Sprites Loaded: {sprites.Length}");

        foreach (var sprite in sprites)
        {
            spriteDictionary[sprite.name] = sprite;
            Debug.Log($"Loaded Sprite: {sprite.name}");
        }
    }

    [YarnCommand]
    public void ChangeSprite(string spriteName)
    {
        if (visualDisplay == null)
        {
            Debug.LogError("Visual Display is not assigned!");
            return;
        }
        
        if (spriteDictionary.ContainsKey(spriteName))
        {
            visualDisplay.sprite = spriteDictionary[spriteName];
            Debug.Log($"Changed sprite to: {spriteName}");
        }
        else
        {
            Debug.LogWarning($"Sprite '{spriteName}' not found.");
        }
    }
        private void DebugSavedSprites()
    {
        Debug.Log("Saved Sprites in Dictionary:");
        foreach (var spriteName in spriteDictionary.Keys)
        {
            Debug.Log($"Sprite Name: {spriteName}");
        }
    }

    [YarnCommand]
    public void ClearSprite()
    {
        visualDisplay.sprite = null;
        Debug.Log("Cleared sprite display.");
    }
}
