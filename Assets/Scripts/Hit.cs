using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private Transform boss;
    bool mov = false;
    // Start is called before the first frame update
    void Start()
    {
        //boss = transform.Find("BOSS").transform;
        StartCoroutine(hit());
    }
    IEnumerator hit()
    {
        yield return new WaitForSeconds(0.01f);
        mov = true;
    }
    IEnumerator dstr()
    {
        transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(dstr());
    }
    // Update is called once per frame
    void Update()
    {
        boss = GameObject.FindGameObjectWithTag("BossPos").transform;
        if (mov)
            transform.position = Vector3.MoveTowards(transform.position, boss.position, 0.07f);
        if (Vector2.Distance(transform.position, boss.position) < 0.1f && mov)
        {
            mov = false;
            GameObject.Find("BOSS").GetComponent<Boss>().DealHp();
            StartCoroutine(dstr());
            Destroy(this.gameObject, 2);
        }

    }
}
