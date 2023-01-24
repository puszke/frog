using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    private GameObject boom, crack_particles;
    Rigidbody2D rb2d;
    bool moving;
    public bool moveable = true, stone=false, cracked=false,show_particles=true;
    Vector2 mov;
    bool dead = false;

    void Start()
    {
        mov = new Vector2(Random.Range(-1,1),Random.Range(-1,1));
        moving = Random.Range(1, 5)==1;
        rb2d = GetComponent<Rigidbody2D>();
        DestroyGround();
    }

    private void Update()
    {
        if (moving && !dead && moveable)
            rb2d.velocity = mov;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stone && !cracked)
            StartCoroutine(Crack());
    }
    IEnumerator Crack()
    {
        cracked = true;
        GameObject newCrack = Instantiate(crack_particles);
        newCrack.transform.position = transform.position;
        Destroy(newCrack, 2);
        yield return new WaitForSeconds(2);
        GameObject newCrack2 = Instantiate(crack_particles);
        newCrack2.transform.position = transform.position;
        Destroy(newCrack2, 2);
        Destroy(this.gameObject);
    }
    IEnumerator ded()
    {
        yield return new WaitForSeconds(9);
        rb2d.isKinematic = false;
        dead = true;
        rb2d.AddTorque(3036*Time.deltaTime);
        rb2d.gravityScale = 1;
        Destroy(this.gameObject, 10);
    }
    public void DestroyGround()
    {
        if (show_particles)
        {
            GameObject newBoom = Instantiate(boom);
            newBoom.transform.position = transform.position;
            Destroy(newBoom, 2);
        }
        StartCoroutine(ded());
        
    }
}
