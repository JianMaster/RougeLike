using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMove : MonoBehaviour {
    public float speed = 5f;
    private Rigidbody2D rig;
    private Transform Player;
    private Vector2 targetPos;
    private Animator anim;
    private GameManager gameManager;


    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player").transform;
        targetPos = transform.position;
        anim = GetComponent<Animator>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        rig.MovePosition(Vector2.Lerp(rig.position, targetPos, speed));
    }


    public void Move()
    {
        Vector2 offset = Player.position - transform.position;
        if(offset .magnitude <1.1f)
        {
            anim.SetTrigger("Attack");
            gameManager.PlayerHurt();
        }
        else
        {
            float x = 0, y = 0;
            if (Mathf .Abs (offset .x )>Mathf .Abs(offset .y ))
            {
                if (offset.x > 0)
                    x = 1f;
                else
                    x = -1f;
            }
            else
            {
                if (offset.y > 0)
                    y = 1f;
                else
                    y = -1f;
            }
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(x, y), 1 << LayerMask.NameToLayer("Obstacle"));
            if (hit .transform ==null )
                targetPos += new Vector2(x, y);
            else
                if (!hit .collider .CompareTag("Obstacle"))
                    targetPos += new Vector2(x, y);
        }
    }

}
