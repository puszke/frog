using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadNewLevel : MonoBehaviour
{
    public void NewLevel(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(Load(sceneName));
    }
    IEnumerator Load(string sceneName)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
    }
}
