﻿using UnityEngine;
using System.Collections;
using InControl;
using UnityStandardAssets.Characters.ThirdPerson;

public class Combat : MonoBehaviour {
    MyCharacterActions characterActions;
    public int lightAttack = 10;
    public int heavyAttack = 20;
    public float radius = 2;
    float blockRange = 0.7f;
    public float lightAttackTimer;
    public float heavyAttackTimer;
    float LATimer = 0.3f;
    float HATimer = 0.3f;
    public bool light = false;
    public bool heavy = false;
    public bool blocking = false;
    int j = 0;
    public Mana_Stamina stamina;
    public GameObject camera;
    public Animator anim;
    // Use this for initialization
    void Start ()
    {
        stamina = GetComponent<Mana_Stamina>();
        characterActions = new MyCharacterActions();
        characterActions.lightAttack.AddDefaultBinding(Mouse.LeftButton);
        characterActions.heavyAttack.AddDefaultBinding(Mouse.RightButton);
        characterActions.block.AddDefaultBinding(Key.LeftControl);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (characterActions.lightAttack.IsPressed && !light)
        {
            anim.SetBool("attacking", true);
            Invoke("ResetAttack", 1f);
            light = true;
            attackEnemies(lightAttack, 0.5f);
        }
        if (characterActions.heavyAttack.IsPressed && !heavy)
        {
            anim.SetBool("attacking", true);
            Invoke("ResetAttack", 1f);
            
            heavy = true;
            attackEnemies(heavyAttack, 0.5f);
        }
        if(light)
        {
            LATimer -= Time.deltaTime;
            if(LATimer < 0)
            {
                light = false;
                LATimer = lightAttackTimer;
                
            }
        }
        if (heavy)
        {
            HATimer -= Time.deltaTime;
            if (HATimer < 0)
            {
                heavy = false;
                HATimer = heavyAttackTimer;
                
            }
        }
        /*
        if (characterActions.block.IsPressed && (stamina.currentStamina - 10) >= 0)
        {
           //transform.rotation = new Quaternion(transform.rotation.x, camera.transform.rotation.y, transform.rotation.z, transform.rotation.w);
            //blockDamage();
            blocking = true;
            if(stamina.currentStamina < 10)
            {
                blocking = false;
            }
        }
        else if(characterActions.block.WasReleased || (stamina.currentStamina - 10) <= 0)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < enemies.Length; i++)
            {
                EnemyScript enemy = enemies[i].GetComponent<EnemyScript>();
                enemy.playerBlocking = false;
            }
            blocking = false;
        }
        */
    }

    void attackEnemies(int damage, float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        int i = 0;
        float dot = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Enemy")
            {
                Vector3 targetDir = hitColliders[i].transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);
                //dot = Vector3.Dot(hitColliders[i].transform.position, transform.TransformDirection(Vector3.forward));

                // get the distance the enemy is in front of the player
                float forwardsDist = Vector3.Dot(targetDir, transform.forward);

                // get the distance from the enemy to a line forwards of the player
                targetDir.Normalize();
                float sidewaysDist = Vector3.Dot(targetDir, transform.right);
                
                if (sidewaysDist > -range && sidewaysDist < range && forwardsDist > 0 && forwardsDist < radius)
                {
                    EnemyScript enemy = hitColliders[i].GetComponent<EnemyScript>();
                    enemy.damaged = true;
                    enemy.takeDamage(damage);
                    j++;
                    Debug.Log("hit " + j + " " + sidewaysDist);
                }
                
                //Debug.Log(angle);
            }
            i++;
        }
    }

    void blockDamage()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int i = 0;
        while(i < enemies.Length)
        {
            
            EnemyScript enemy = enemies[i].GetComponent<EnemyScript>();
            if(enemy.aggro)
            {
                /*
                Vector3 targetDir = enemies[i].transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);
                //dot = Vector3.Dot(hitColliders[i].transform.position, transform.TransformDirection(Vector3.forward));

                // get the distance the enemy is in front of the player
                float forwardsDist = Vector3.Dot(targetDir, transform.forward);

                // get the distance from the enemy to a line forwards of the player
                targetDir.Normalize();
                float sidewaysDist = Vector3.Dot(targetDir, transform.right);

                if (sidewaysDist > -blockRange && sidewaysDist < blockRange && forwardsDist > 0 && forwardsDist < radius)
                {
                    enemy.playerBlocking = true;
                }
                */
                enemy.playerBlocking = true;
            }
            
            i++;
        }
    }

    void ResetAttack()
    {
        anim.SetBool("attacking", false);
    }
}
