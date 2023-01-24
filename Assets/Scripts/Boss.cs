using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    [SerializeField]
    private Image hpBar,effectBar;
    private AudioSource audioSource;
    private Animator animator;
    public float hp = 25;
    public List<int> attackChances;
    public List<string> attacks;
    [SerializeField] string[] current_attacks;
    [SerializeField]
    GameObject scratch_prop,move_show, knife_prop;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveEffectBar());
        audioSource=GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        GetAttackNames();
        StartCoroutine(ChooseAttack(5));
    }

    public void GetAttackNames()
    {
        current_attacks = attacks[Game.instance.phase].Split(char.Parse(" "));
    }

    IEnumerator ChooseAttack(float sec)
    {
        yield return new WaitForSeconds(sec);
        string choosen = current_attacks[Random.Range(0, current_attacks.Length)];
        StartCoroutine(choosen);
    }
    IEnumerator SimpleMove()
    {
        for(int i=0; i<5; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4),0);
            GameObject newMoveShow = Instantiate(move_show, pos, Quaternion.identity);
            yield return new WaitForSeconds(2);
            transform.position = pos;
            Destroy(newMoveShow);
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(ChooseAttack(1));
    }
    IEnumerator Scratch()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject newScratch = Instantiate(scratch_prop);
                newScratch.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                newScratch.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(2);
        }
        StartCoroutine(ChooseAttack(1));
    }
    IEnumerator Knife()
    {
        for(int i=0; i<3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 pos = transform.position = new Vector3(Random.Range(-1, 1),Random.Range(-1, 1));
            GameObject newKnife = Instantiate(knife_prop, pos, Quaternion.identity);
            Destroy(newKnife, 10);
        }
        StartCoroutine(ChooseAttack(5));
    }

    IEnumerator MoveEffectBar()
    {
        if (effectBar.fillAmount > hp / 100)
            effectBar.fillAmount -= 0.01f;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveEffectBar());
    }
    public void DealHp()
    {
        hp -= 4;
        Game.instance.CheckStage();
        audioSource.PlayOneShot(audioSource.clip);
    }
    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = hp/100;

    }
}
