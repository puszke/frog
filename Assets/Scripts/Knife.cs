using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    bool look_at = false;
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1);
        look_at = false;
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, 20), ForceMode2D.Impulse);
        Destroy(this.gameObject, 10);
    }
    public void Find()
    {
        look_at = true;
        StartCoroutine(Shoot());
        Destroy(GetComponent<Animator>());
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(look_at)
        {
            Vector3 diff = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }
}
