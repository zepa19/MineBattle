using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creeper : MonoBehaviour {


    //int lastMoveDay;
    //float lastMoveSec;

    public int Health = 100;
    public int Strength = 8;
    public float speed = 2f;
    public float jumpSpeed = 8f;
    public float gravity = 40f;
    public float rotSpeed = 2f;
    private float defense = 1f;
    private float vSpeed = 0f;
    private float timePush = 0f;
    private bool jump = false;
    private bool isDead = false;
    private bool healthbar = false;
    private bool pushing = false;

    private Transform myTransform;
    private Transform target;
    private CharacterController Enemy;

    private void OnGUI()
    {
        if (!isDead && healthbar)
        {
            GUI.Box(new Rect(700, 10, Screen.width / 6, 20), Health + "/" + 100);
        }
    }

    public void DisableHealthBar()
    {
        healthbar = false;
    }

    private void Awake()
    {
        myTransform = transform;
        target = GameManager._Instance.Player.transform;
        Enemy = GetComponent<CharacterController>();
        //lastMoveDay = GameTime.Day;
        //lastMoveSec = GameTime.Seconds;
    }

    private void Start()
    {
        defense = 1.1f - Player.PStatus.gameDifficulty;
    }

    // Update is called once per frame
    private void Update()
    {

        if (!isDead)
        {
            Vector3 chaseDir = target.position - myTransform.position;
            chaseDir.y = 0;
            float distance = chaseDir.magnitude;
            speed = 0;

            if (!pushing)
            {
                if (distance <= 1.3f)
                {
                    myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(chaseDir), rotSpeed * Time.deltaTime);

                    if (GameManager._Instance.ModeOfTheGame == GameManager.GameMode.SURVIVAL)
                        Attack();

                }
                else if (distance < 9)
                {
                    myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(chaseDir), rotSpeed * Time.deltaTime);
                    speed = 2f;

                    if (GameManager._Instance.ModeOfTheGame != GameManager.GameMode.SURVIVAL)
                        speed = 0f;
                }
                else
                {
                    RandomMove();
                }

                if (Enemy.isGrounded)
                {
                    vSpeed = 0;
                    if (jump)
                    {
                        vSpeed = jumpSpeed;
                        jump = false;
                    }
                }

                if (speed != 0)
                {
                    gameObject.GetComponent<Animation>().Play("Creeper");
                }
                else
                {
                    gameObject.GetComponent<Animation>().Stop();
                }

                vSpeed -= gravity * Time.deltaTime;
                chaseDir = chaseDir.normalized * speed;
                chaseDir.y += vSpeed;

                Enemy.Move(chaseDir * Time.deltaTime);
            }
            else
            {

                timePush += Time.deltaTime;

                if (timePush > 0.8f)
                {
                    timePush = 0f;
                    pushing = false;
                }

                if (Enemy.isGrounded)
                {
                    vSpeed = 0;
                    if (jump)
                    {
                        vSpeed = jumpSpeed;
                        jump = false;
                    }
                }

                vSpeed -= gravity * Time.deltaTime;
                chaseDir = chaseDir.normalized * speed;
                chaseDir *= -1;
                chaseDir.y += vSpeed;

                Enemy.Move(chaseDir * Time.deltaTime);
            }

        }
        else
        {
            if (!GetComponent<Animation>().IsPlaying("CreeperDead"))
            {
                Player._Instance.Enemies.Remove(this.gameObject);
                GameObject.Destroy(this.gameObject);
            }
        }


    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Mathf.Abs(hit.normal.y) < 0.5)
        {
            jump = true;
        }
    }

    private void Attack()
    {
        Player.PStatus.TakeDamage(Strength * 2);
        gameObject.GetComponent<Animation>().Stop();
    }

    public bool TakeDamage(int d)
    {
        Health -= Mathf.FloorToInt(d * defense);
        pushing = true;
        healthbar = true;

        if (Health <= 0)
        {
            Health = 0;
            pushing = false;
            Dead();
            return true;
        }
        return false;
    }

    private void Dead()
    {
        GetComponent<Animation>().Play("CreeperDead");
        isDead = true;
    }

    private void RandomMove()
    {
        //if (isRandomMoving)
        //    return;

        //if(lastMoveDay != GameTime.Day || GameTime.Seconds-lastMoveSec > 360)
        //{
        //    isRandomMoving = true;

        //}
    }
}
