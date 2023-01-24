using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipGenerator : MonoBehaviour
{
    [SerializeField]
    public List<string> tips;
    private Text txt;
    void Start()
    {
        txt = GetComponent<Text>();
        txt.text = "Tip: "+tips[Random.Range(0, tips.Count)];
    }
}
