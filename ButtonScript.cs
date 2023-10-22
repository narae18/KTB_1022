using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void LevelScene()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void GameScene()
    {
        SceneManager.LoadScene("InGame");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("GameMenu");
    }
}

