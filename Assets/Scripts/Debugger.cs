using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    private List<ScriptableEnemy> _scriptableEnemies;
    //int counter = 0;
    //int cooldown = 2020;
    float spawnTime = 4;

    private void Awake()
    {
        _scriptableEnemies = Resources.LoadAll<ScriptableEnemy>("Units/Enemies").ToList();
    }
    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    //void Update()
    //{
    //    counter++;
    //    if (counter > cooldown)
    //    {
    //        counter = 0;
    //        if (cooldown - 30 > 35)
    //            cooldown -= 30;
    //        else
    //            cooldown = 35;
    //        UnitManager.Instance.SpawnEnemy();
    //    }
    //}
    private IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(spawnTime);
        if (spawnTime > 0.2)
            spawnTime -= 0.05f;
        UnitManager.Instance.SpawnEnemy();
        StartCoroutine(StartSpawning());
    }
}
