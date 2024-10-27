using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "GameScene";
    public void Play(){
         SceneManager.LoadScene(gameScene);
    }

    public void Quit(){
        Application.Quit();
    }
}
