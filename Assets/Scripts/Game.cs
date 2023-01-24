using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    public List<GameObject> ground;
    [SerializeField]
    private GameObject menu;
    public Boss boss;
    public GameObject orb, background_particles;
    int powr = 0;
    public List<stage> stages = new List<stage>();
    public List<float> hp_stage;
    public stage current_stage;
    public int phase = 0;

    [System.Serializable]
    public class stage
    {
        public float cam_fov = 5;
        public int particle_speed = 5;
        public Color bg_color;
        public float spawn_platform_speed = 3f;
    }
    public void Stop()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        current_stage = stages[0];
        StartCoroutine(Spawn());
        Stop();
    }
    public void CheckStage()
    {
        for(int i=0; i<stages.Count; i++)
        {
            if (boss.hp <= hp_stage[i])
            {
                current_stage = stages[i];
                phase = i;
                boss.GetAttackNames();
                StartCoroutine(ChangeStage());
            }
        }
    }
    IEnumerator ChangeStage()
    {
        if(Camera.main.orthographicSize<=current_stage.cam_fov)
        {
            yield return new WaitForSeconds(0.1f);
            Camera.main.orthographicSize += 0.1f;
            StartCoroutine(ChangeStage());
        }
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(current_stage.spawn_platform_speed);
        GameObject newGround = Instantiate(ground[Random.Range(0,ground.Count)]);
        newGround.transform.position = new Vector2(Random.Range(-current_stage.cam_fov, current_stage.cam_fov), Random.Range(-current_stage.cam_fov + 2, current_stage.cam_fov - 2));
        if(powr==3)
        {
            SpawnOrb();
            powr = 0;
        }
        powr++;
        StartCoroutine(Spawn());
    }
    void SpawnOrb()
    {
        GameObject newOrb = Instantiate(orb);
        newOrb.transform.position = new Vector2(Random.Range(-current_stage.cam_fov, current_stage.cam_fov), Random.Range(-current_stage.cam_fov+2, current_stage.cam_fov-2));
    }
    // Update is called once per frame
    void Update()
    {
        Camera.main.backgroundColor = current_stage.bg_color;
    }
}
