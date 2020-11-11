using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewFishStat", menuName ="FishStat")]
public class FishStat : ScriptableObject
{
    public float SPEED = 1f;
    public float HP = 100f;
    public float DAME = 10f;

    public float TIME_GROW1 = 50f;
    public float TIME_GROW2 = 100f;
    public float TIME_HUNGER = 10f;
    public float TIME_DIE = 20f;
    public float SIZE_SMALL = 1.8f;
    public float SIZE_MEDIUM = 2.4f;
    public float SIZE_BIG = 3f;
    public float ADD_OXYGEN = 10f;
    public float ADD_MONEY = 0f;
    public float SPAWN_OXYGEN = 0f;
    public float SPAWN_MONEY = 10f;
    public float SHOP_VALUE = 20f;

    public string EAT_TAG = "Grass";

    public Sprite[] normalSheet;
    public Sprite[] attackedSheet;
    public GameObject ghost;
}
