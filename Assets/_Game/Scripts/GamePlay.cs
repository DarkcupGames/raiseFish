using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    public static GamePlay Instance;

    public static int SORT_ORDER_DEPTH = 1000;
    public static float SEA_WIDTH = 20f;
    public static float SEA_HEIGHT = 10f;
    public static float TIME_MONSTER_WAKEUP1 = 120;
    public static float TIME_MONSTER_WAKEUP2 = 240;

    public Slider slider;
    public TextMeshProUGUI txtMoney;
    public PopupConfirm popupConfirm;

    public static List<FishNormal> listFish;

    public GameObject grassObj;
    public GameObject fishObj;

    public static float money;
    public static float grass;
    public static float oxygen;
    public static float fishCount;

    float timeCount = 0f;
    string status = "sleep";

    void Awake()
    {
        if (Instance == null)
        {
            listFish = new List<FishNormal>();
            Instance = this;
            DontDestroyOnLoad(gameObject);
            money = 10f;
            Debug.Log("playerid is: " + ServerSystem.playerid);
            ServerSystem.sendRequest = true;
            Debug.Log("turn send request on");
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount > TIME_MONSTER_WAKEUP1 && status == "sleep")
        {
            status = "wake1";
            popupConfirm.Show("QUÁI VẬT THỨC GIẤC", "Quái vật đã thức giấc, bạn có muốn feed nó 200 food??",
                () =>
                {
                    if (!GamePlay.Instance.SpendMoney(200))
                    {
                        popupConfirm.Show("GAME OVER!", "Bạn không đủ food!! Cá của bạn sẽ bị ăn!!!");
                    }
                    else
                    {
                        popupConfirm.Hide();
                    }
                    
                },
                () =>
                {
                    popupConfirm.Show("Cá đã bị ăn","Một số cá của bạn đã chết vì quái vật ăn");
                    if (listFish.Count > 0)
                    {
                        int index = UnityEngine.Random.Range(0, listFish.Count);
                        Destroy(listFish[index]);
                        listFish.RemoveAt(index);
                    }
                });
        }

        if (timeCount > TIME_MONSTER_WAKEUP2 && status == "wake1")
        {
            status = "wake2";
            popupConfirm.Show("QUÁI VẬT THỨC GIẤC", "Quái vật đã thức giấc, bạn có muốn feed nó 1000 food??",
                () =>
                {
                    popupConfirm.Hide();
                },
                () =>
                {
                    popupConfirm.Show("Cá đã bị ăn", "Một số cá của bạn đã chết vì quái vật ăn");
                    if (listFish.Count > 0)
                    {
                        int index = UnityEngine.Random.Range(0, listFish.Count);
                        Destroy(listFish[index]);
                        listFish.RemoveAt(index);
                    }
                });
        }

        money += Time.deltaTime;
    }

    public void AddMoney(float amount) {
        money += amount;
        UpdateStat();
    }

    public bool SpendMoney(float amount) {
        if (money < amount)
            return false;
        money -= amount;
        UpdateStat();
        return true;
    }

    public void UpdateStat()
    {
        string content = "Money: " + ((int)money).ToString();
        content += "\nGrass: " + ((int)grass).ToString();
        content += "\nOxygen: " + ((int)oxygen).ToString();
        txtMoney.text = content;
    }

    public void DestroyDelay(GameObject go, float sec)
    {
        StartCoroutine(DestroyDelayCoroutine(go, sec));
    }

    private IEnumerator DestroyDelayCoroutine(GameObject go, float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(go);
    }

    public void SetTextDelay(TextMeshPro txt, string content, float sec)
    {
        StartCoroutine(SetTextDelayCoroutine(txt,content, sec));
    }

    private IEnumerator SetTextDelayCoroutine(TextMeshPro txt, string content, float sec)
    {
        yield return new WaitForSeconds(sec);
        txt.text = content;
    }

    public void DoFunctionLoop(float sec, Action action)
    {
        StartCoroutine(DoFunctionLoopCoroutine(sec, action));
    }

    public IEnumerator DoFunctionLoopCoroutine(float sec, Action action)
    {
        yield return new WaitForSeconds(sec);
        action.Invoke();
        StartCoroutine(DoFunctionLoopCoroutine(sec, action));
    }
}
