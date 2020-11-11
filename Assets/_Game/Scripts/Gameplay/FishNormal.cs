using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishNormal : Fish
{
    public TextMeshPro txtStatus;
    public GameObject ghost;
    public Sprite[] normalSheet;
    public Sprite[] attackedSheet;

    public GameObject eat;

    private float MAX_SPEED = 1f;
    private float TIME_GROW1 = 50f;
    private float TIME_GROW2 = 100f;
    private float TIME_HUNGER = 10f;
    private float TIME_DIE = 20f;
    private float SIZE_SMALL = 1.8f;
    private float SIZE_MEDIUM = 2.4f;
    private float SIZE_BIG = 3f;

    private bool moveLeft = true;
    private float movetime = 5f;
    private float vRange = 0.001f;
    private float movespeed = 1f;
    private string state = "normal";
    private float hp = 10f;

    private float timeCount;
    private float timeStatus;
    private float timeHunger;
    private float timeDie;

    private string size = "small";

    void Start()
    {
        GetComponent<DAnimator>().spritesheet = normalSheet;
        vRange = Random.Range(-MAX_SPEED, MAX_SPEED);
        movetime = Random.Range(4f, 12f);
        hp = 1f;

        txtStatus.text = "";
        timeCount = 0f;
        timeHunger = TIME_HUNGER;
        timeDie = TIME_DIE;

        transform.localScale = new Vector3(SIZE_SMALL, SIZE_SMALL);
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        //if (timeCount > timeStatus)
        //{
        //    timeStatus += Random.Range(5f, 10f);

        //    List<string> comments = new List<string>() { "I'm hungry", "Happy <3", "I need friend with benefit!", "I'm fish!", "I'm scared!" };
        //    txtStatus.text = comments[Random.Range(0, comments.Count)];

        //    GamePlay.Instance.SetTextDelay(txtStatus, "", 2f);
        //}

        if (timeCount > timeHunger && state != "hungry")
        {
            txtStatus.text = "hungry!";
            state = "hungry";

            if (eat == null)
                eat = GameObject.FindGameObjectWithTag("Grass");
        }

        if (timeCount > timeDie && state == "hungry")
        {
            //txtStatus.text = "die hunger!";
            //state = "die";

            //if (eat == null)
            //    eat = GameObject.FindGameObjectWithTag("Grass");
            state = "dead";
            GameObject go = Instantiate(ghost, transform.position, Quaternion.identity);
            GamePlay.Instance.DestroyDelay(go, 1f);
            Destroy(gameObject);
        }

        if (timeCount > TIME_GROW1 && size == "small")
        {
            size = "medium";
            transform.localScale = new Vector3(SIZE_MEDIUM, SIZE_MEDIUM);
        }

        if (timeCount > TIME_GROW2 && size == "medium")
        {
            size = "big";
            transform.localScale = new Vector3(SIZE_BIG, SIZE_BIG);
        }

        int order = (int)(-transform.position.y * GamePlay.SORT_ORDER_DEPTH);

        if (state == "hungry")
        {
            if (eat == null)
                eat = GameObject.FindGameObjectWithTag("Grass");

            if (eat == null)
                return;

            Vector3 direc = eat.transform.position - transform.position;

            if (direc.magnitude > 0.5f)
            {
                Vector3 cal = direc / direc.magnitude;
                transform.position += cal * Time.deltaTime * 3f;
                if (direc.x > 0)
                    GetComponent<SpriteRenderer>().flipX = true;
                else
                    GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                eat.GetComponent<Grass>().GetEaten(10f);
                eat = null;
                state = "normal";
                timeHunger = timeCount + TIME_HUNGER;
                timeDie = timeCount + TIME_DIE;
                txtStatus.text = "";
            }

            return;
        }

        movetime -= Time.deltaTime;
        if (movetime < 0)
        {
            movetime = Random.Range(4f, 12f);
            moveLeft = !moveLeft;
            vRange = Random.Range(-MAX_SPEED, MAX_SPEED);
            movespeed = Random.Range(0.001f, MAX_SPEED);
            if (!moveLeft)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }  
        }

        Vector3 currentPos = GetComponent<Transform>().position;
        if (currentPos.x < - GamePlay.SEA_WIDTH)
        {
            moveLeft = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (currentPos.x > GamePlay.SEA_WIDTH)
        {
            moveLeft = true;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (currentPos.y < - GamePlay.SEA_HEIGHT && vRange < 0) vRange = -vRange;
        if (currentPos.y > GamePlay.SEA_HEIGHT && vRange > 0) vRange = -vRange;

        if (moveLeft)
        {
            GetComponent<Transform>().position += new Vector3(-movespeed , vRange, 0) * Time.deltaTime;
        }
        else
            GetComponent<Transform>().position += new Vector3(movespeed, vRange, 0) * Time.deltaTime;

        if (state == "hurt")
        {
            hp -= Time.deltaTime;
            if (hp <= 0 && state != "dead")
            {
                state = "dead";
                GameObject go = Instantiate(ghost, transform.position, Quaternion.identity);
                GamePlay.Instance.DestroyDelay(go, 1f);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MonsterAttack"))
        {
            GetComponent<DAnimator>().spritesheet = attackedSheet;
            state = "hurt";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MonsterAttack"))
        {
            GetComponent<DAnimator>().spritesheet = normalSheet;
            state = "normal";
        }
    }
}
