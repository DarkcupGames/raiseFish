using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private float speed = 3f;

    public Joystick joystick;
    public TextMeshPro txtStatus;
    public Sprite[] anim_createFish;
    public Sprite[] anim_normal;
    public Sprite[] anim_attack;

    public GameObject newFish;
    public GameObject sailFish;
    public GameObject grass;
    public GameObject createFishEffect;
    public string state = "normal";

    private float timeCount;
    private float timeChangeText;

    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount > timeChangeText)
        {
            List<string> comments = new List<string>() { "I'm queen of the sea", "Be prepared" };
            txtStatus.text = comments[Random.Range(0,comments.Count)] ;
            timeChangeText += 10f;
            GamePlay.Instance.SetTextDelay(txtStatus, "", 2f);
        }

        if (state == "create_fish")
            return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            CreateFish(grass);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateFish(newFish);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            CreateFish(sailFish);
        }

        transform.parent.position += new Vector3(joystick.Horizontal, joystick.Vertical) * speed * Time.deltaTime;
        if (joystick.Horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (joystick.Horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void CreateFish(GameObject fish)
    {
        float money = fish.GetComponent<Fish>().fishData.SHOP_VALUE;

        if (GamePlay.Instance.SpendMoney(money))
            StartCoroutine(CreateFishCoroutine(fish));
    }

    public IEnumerator CreateFishCoroutine(GameObject fish)
    {
        state = "create_fish";
        GetComponent<DAnimator>().spritesheet = anim_createFish;
        GameObject effect = Instantiate(createFishEffect, transform.position + new Vector3(0, 1f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(effect);
        GetComponent<DAnimator>().spritesheet = anim_normal;
        GameObject go = Instantiate(fish, transform.position + new Vector3(0, 1f), Quaternion.identity);
        state = "normal";
    }

    public void Attack()
    {
        if (state != "normal")
            return;
        StartCoroutine(AttackCoroutine());
    }

    public IEnumerator AttackCoroutine() {
        state = "attack";
        GetComponent<DAnimator>().spritesheet = anim_attack;
        GetComponent<DAnimator>().frameshow = 0;
        yield return new WaitForSeconds(0.5f);
        GetComponent<DAnimator>().spritesheet = anim_normal;
        state = "normal";
    }
}
