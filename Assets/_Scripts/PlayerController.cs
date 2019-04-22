using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Vector2 target;
    private Rigidbody2D rig;
    private bool isMoving = false;
    public float speed = 5f;

    private RaycastHit2D rayInfo;
    private Animator anim;
    private GameManager gameManager;

	void Start () {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        target = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!isMoving)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (h == 1f)
                v = 0;
            if (h != 0 || v != 0)
            {
                rayInfo = Physics2D.Linecast(target, target + new Vector2(h, v),1<<LayerMask .NameToLayer ("Obstacle")|1<<LayerMask .NameToLayer ("Enemy"));

                if (rayInfo.collider != null)
                {
                    switch (rayInfo.collider.tag)
                    {
                        case "Obstacle":
                            anim.SetTrigger("Attack");
                            rayInfo.transform.SendMessage("TakeDamage");
                            break;
                        case "OutWall":
                            break;
                        case "Enemy":
                            if ((rayInfo.transform.position - transform.position).magnitude < 0.5f)
                                target += new Vector2(h, v);
                            break;
                        case "Food":
                            target += new Vector2(h, v);
                            gameManager.Food(rayInfo .transform .gameObject);
                            break;
                        case "Soda":
                            target += new Vector2(h, v);
                            gameManager.Soda(rayInfo.transform.gameObject);
                            break;
                        case "Exit":
                            gameManager.LoadLevel();
                            break;
                    }
                }
                else
                {
                    target += new Vector2(h, v);
                }
                isMoving = true;
                StartCoroutine(MoveTime());
            }
        }
        rig.MovePosition(Vector2.Lerp(transform.position, target, speed * Time.deltaTime));

	}

    IEnumerator MoveTime()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager.EnemyMove();
        isMoving = false;
    }

    public void TakeDamage()
    {
        anim.SetTrigger("Hurt");
    }

    public void Recover()
    {
        transform.position = new Vector3(-3.5f, -3.5f, 0);
        target = transform.position;
    }
}
