using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bacteria : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private int damage = 5;

    [SerializeField] private float speed = 1.5f;

    [SerializeField] private BacteriaData data;
    
    [SerializeField] private GameObject defenseOrganism;
    
    void Start()
    {
        defenseOrganism = GameObject.FindWithTag("DefenceOrganism"); //TODO: Merge with tobis managers - here a defense manager
        setEnemyValues();
    }

    // Update is called once per frame
    void Update()
    {
        BacteriaSwarm();
    }

    private void BacteriaSwarm() 
    {
        transform.position =
            Vector2.MoveTowards(transform.position, defenseOrganism.transform.position, speed * Time.deltaTime); //TODO: Use tobias' move method
    }


    private void OnTriggerEnter2D(Collider2D col) 
    {
        Collider2D collidingInstance = col;

        if (!collidingInstance.CompareTag("DefenseOrganism")) return;
        if (collidingInstance.GetComponent<Health>() == null) return;
        collidingInstance.GetComponent<Health>().Damage(damage);
        Debug.Log(collidingInstance + " Is attacking");
    }

    private void setEnemyValues()
    {
        GetComponent<Health>().SetHealth(data.hp);
        damage = data.damage;
        speed = data.speed;
    }
}
