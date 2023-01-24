using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonBeh : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "SampleScene";
    [SerializeField]
    private bool exitButton = false;
    [SerializeField]
    private LoadNewLevel new_lvl;
    
    public void Use()
    {
        if (exitButton)
            Application.Quit();
        else
        {
            new_lvl.gameObject.SetActive(true);
            new_lvl.NewLevel(sceneName);
        }
    }
}
