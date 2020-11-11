using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grass : Fish
{
    public TextMeshPro txtStatus;

    private string state = "normal";
    private string size = "small";
    private float hp = 10f;

    private float timeCount;
    private float timeStatus;
    private float timeProduce;

    void Start()
    {
        hp = fishData.HP;
        txtStatus.text = "";
        timeCount = 0f;
        GamePlay.oxygen += fishData.ADD_OXYGEN;
        GamePlay.Instance.UpdateStat();
        txtStatus.text = "+ " + fishData.ADD_OXYGEN + " oxygen!";
        GamePlay.Instance.SetTextDelay(txtStatus, "", 2f);

        transform.localScale = new Vector3(fishData.SIZE_SMALL, fishData.SIZE_SMALL);
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount > timeProduce)
        {
            timeProduce += fishData.SPAWN_MONEY * Random.Range(0.75f, 1.25f);
            Produce();
        }

        if (timeCount > fishData.TIME_GROW1 && size == "small")
        {
            size = "medium";
            GamePlay.oxygen += fishData.ADD_OXYGEN;
            GamePlay.Instance.UpdateStat();
            hp += fishData.HP;

            txtStatus.text = "+ " + fishData.ADD_OXYGEN + " oxygen!";
            GamePlay.Instance.SetTextDelay(txtStatus, "", 2f);
            transform.localScale = new Vector3(fishData.SIZE_MEDIUM, fishData.SIZE_MEDIUM);
        }

        if (timeCount > fishData.TIME_GROW2 && size == "medium")
        {
            size = "big";
            GamePlay.oxygen += fishData.ADD_OXYGEN;
            GamePlay.Instance.UpdateStat();
            hp += fishData.HP;

            txtStatus.text = "+ " + fishData.ADD_OXYGEN + " oxygen!";
            GamePlay.Instance.SetTextDelay(txtStatus, "", 2f);
            transform.localScale = new Vector3(fishData.SIZE_BIG, fishData.SIZE_BIG);
        }

        int order = (int)(-transform.position.y * GamePlay.SORT_ORDER_DEPTH);

        if (state == "hurt")
        {
            hp -= Time.deltaTime;
            if (hp <= 0 && state != "dead")
            {
                state = "dead";
                GameObject go = Instantiate(fishData.ghost, transform.position, Quaternion.identity);
                GamePlay.Instance.DestroyDelay(go, 1f);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MonsterAttack"))
        {
            GetComponent<DAnimator>().spritesheet = fishData.attackedSheet;
            state = "hurt";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MonsterAttack"))
        {
            GetComponent<DAnimator>().spritesheet = fishData.normalSheet;
            state = "normal";
        }
    }

    private void Produce()
    {
        float amount = 10;
        if (size == "small")
            return;
        if (size == "medium")
            amount = fishData.SPAWN_MONEY;
        if (size == "big")
            amount = 2* fishData.SPAWN_MONEY;

        txtStatus.text = "+ " + amount + " money!";
        GamePlay.Instance.AddMoney(amount);
        GamePlay.Instance.SetTextDelay(txtStatus, "", 1f);
    }

    public void GetEaten(float dame)
    {
        txtStatus.text = "- " + dame + " HP";
        GamePlay.Instance.SetTextDelay(txtStatus, "", 2f);
        hp -= dame;
        if (hp < 0)
            Destroy(gameObject);
    }
}
