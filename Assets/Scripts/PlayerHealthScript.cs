using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    public Sprite[] sprites;
    public Image healthBarImage;
    public float healthPercentage = 1;
    public BlasterPlayerScript blasterPlayerScript;

    // Update is called once per frame
    void Update()
    {
        var spritePosition = Mathf.Max(0, Mathf.FloorToInt((sprites.Length - 1) * blasterPlayerScript.playerHealthPercentage));
        healthBarImage.sprite = sprites[spritePosition];

    }
}
