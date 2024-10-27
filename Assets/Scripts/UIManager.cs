using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set;}
   public TextMeshProUGUI message;
   public TextMeshProUGUI notifs;
   private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
        ClosePanel();

   }
   public void OpenPanel(){
    Time.timeScale = 0;
    transform.localScale = Vector3.one;
   }
   public void ClosePanel(){
    Time.timeScale = 1;
    transform.localScale = Vector3.zero;
   }
   public void BeanWon(){
    message.text = "Congrats! You defeated the crusher!";
   }
   public void CrusherWon(){
    message.text = "Oh No! You've been crushed!";
   }
   public void Burnt(){
    message.text = "Oh No! You're burnt!";
   }

    public void RePlay(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ClosePanel();
    }

    public void Quit(){
        SceneManager.LoadScene("MainMenu");
        ClosePanel();
    }
}
