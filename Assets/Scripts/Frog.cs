using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Frog : MonoBehaviour
{
    private bool is_dead = false;
    public Sprite frogJump, normalFrog, deadFrog;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    public ParticleSystem particle;
    private Rigidbody2D rb2d;
    public bool jumping = true, double_jump=true, can_jump=false, low_jump=false;
    public GameObject hit, arrow, deadScreen, deadCircle, jumpChangeUI, frogExplode;
    private bool displayParticle = false;
    ParticleSystem.EmissionModule em;

    // Start is called before the first frame update
    void Start()
    {
        em = particle.emission;
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalFrog = spriteRenderer.sprite;
        StartCoroutine(WakeUp());
    }
    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(0.1f);
        can_jump = true;
    }
    IEnumerator Die()
    {
        is_dead = true;
        GameObject newExplode = Instantiate(frogExplode);
        newExplode.transform.position = transform.position;
        spriteRenderer.sprite = deadFrog;
        spriteRenderer.sortingOrder = 99;
        deadCircle.transform.position = transform.position;
        rb2d.isKinematic = true;
        rb2d.velocity = Vector2.zero;
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        deadScreen.GetComponent<Animator>().SetTrigger("die");
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene("SampleScene");
    }

    void Jump(float force)
    {
        if(can_jump)
            rb2d.AddForce(new Vector2(0, force/50), ForceMode2D.Impulse);
    }

    IEnumerator particleDelay()
    {
        //rb2d.gravityScale = -2;
        yield return new WaitForSeconds(0.5f);
        displayParticle = false;
        rb2d.gravityScale = 2;
    }
    // Update is called once per frame
    void Update()
    {
        if(is_dead && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("SampleScene");
        low_jump = Input.GetMouseButton(1);

        if (!is_dead)
        {
            jumpChangeUI.GetComponent<Animator>().SetBool("LowJump", low_jump);
            if(low_jump)
            {
                rb2d.gravityScale = 5;
            }
            else
            {
                rb2d.gravityScale = 2;
            }

            em.enabled = displayParticle;
            GetComponent<Animator>().SetBool("noJump", !double_jump);

            if (double_jump && Input.GetMouseButtonDown(0) && can_jump)
            {
                rb2d.velocity = Vector2.zero;
                Jump(1000);
                double_jump = false;
                displayParticle = true;
                StartCoroutine(particleDelay());
            }

            boxCollider2D.isTrigger = rb2d.velocity.y > 0.2f;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (can_jump)
                transform.position = new Vector3(Vector3.MoveTowards(transform.position, mousePos, 1).x, transform.position.y);

            arrow.SetActive(transform.position.y > Game.instance.current_stage.cam_fov+1);
            arrow.transform.position = new Vector2(transform.position.x, arrow.transform.position.y);

            if (rb2d.velocity.y > 0.2f)
                spriteRenderer.sprite = frogJump;
            else
                spriteRenderer.sprite = normalFrog;

            if (transform.position.y < -Game.instance.current_stage.cam_fov-1)
            {
                rb2d.velocity = Vector2.zero;
                transform.position = new Vector2(transform.position.x, Game.instance.current_stage.cam_fov+1);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Island"&&jumping)
        {
            rb2d.velocity = Vector2.zero;
            Jump(500);
            collision.transform.GetComponent<Animator>().SetTrigger("bonk");
        }
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Island" && jumping && rb2d.velocity.y == 0)
        {
            Jump(500);
        }
        if (collision.transform.tag == "Hurt" || collision.transform.tag == "BossPos" && !is_dead)
        {
            StartCoroutine(Die());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Hurt" || collision.transform.tag == "BossPos")
        {
            if(!collision.GetComponent<BoxCollider2D>().isTrigger && !is_dead)
                StartCoroutine(Die());
        }
        if (collision.transform.tag == "Fly" || collision.transform.tag == "Orb" && jumping)
        {
            rb2d.velocity = Vector2.zero;
            if (collision.transform.tag == "Orb")
            {
                GameObject newHit = Instantiate(hit);
                newHit.transform.position = collision.transform.position;
            }
            double_jump = true;
            Jump(500);
            Destroy(collision.transform.gameObject);
        }
        
    }
}
