using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public Texture foregroundTexture;
    public Texture backgroundTexture;
    public Texture2D damageTexture;

    // Start is called before the first frame update


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        Rect rect = new Rect(30, 200, Screen.width / 2 - 20, backgroundTexture.height * 2);

        GUI.DrawTexture(rect, backgroundTexture);

        float health = PuxxeStudio.PlayerControllDemo.health;
        rect.width *= health;

        GUI.color = damageTexture.GetPixelBilinear(health, 1f);
        GUI.DrawTexture(rect, foregroundTexture);

        // GUI.color = Color.white;
    }
}
