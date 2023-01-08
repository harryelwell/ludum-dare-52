using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    
    public void PlayTimeTrial()
    {
        Image loadingScreen = GameObject.FindGameObjectWithTag("LoadingScreen").GetComponent<Image>();
        loadingScreen.color = new Color(1f,1f,1f,1f);
        loadingScreen.raycastTarget = true;
        SceneManager.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
