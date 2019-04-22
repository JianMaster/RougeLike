using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int level = 1;
    public int food = 100;
    private bool enemyMove = true;
    public List<GameObject> enemies;
    private PlayerController player;
	// Use this for initialization
	void Start () {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void LoadLevel()
    {
        ++level;
        enemyMove = true;
        player.Recover();
        DestroyImmediate(GameObject.Find("map"),false );
        GetComponent<MapManager>().InitMap();
        enemies.Clear();
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    public void EnemyMove()
    {
        enemyMove = !enemyMove;
        if (enemyMove )
        {
            foreach (GameObject g in enemies)
                g.GetComponent <ZombieMove >().Move ();
        }
    }

    public void PlayerHurt()
    {
        food -= 30;
        player.TakeDamage();
    }

    public void Food(GameObject g)
    {
        food += 20;
        Destroy(g);
    }
    public void Soda(GameObject g)
    {
        food += 10;
        Destroy(g);
    }
}
