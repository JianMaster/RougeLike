using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    //
    //map parameters
    //
    public GameObject[] outWallArray;
    public GameObject[] floorArray;
    public int rowIndex = 10;
    public int columnIndex = 10;
    public float xOffset = 0;
    public float yOffset = 0;

    //
    //foods,obstacles,enemies
    //
    private List<Vector2> position = new List<Vector2>();
    private GameManager gameManager;
    public GameObject[] foods;
    public GameObject[] enemies;
    public GameObject[] obstacles;
    public int minObstaclesNum = 2;
    public int maxObstaclesNum = 8;

    //
    //exit
    //
    public GameObject exit;

    void Awake () {
        gameManager = GetComponent<GameManager>();
        InitMap();
	}
	
    //The method of create map
    public void InitMap()
    {
        GameObject map = new GameObject("map");
        for (int row = 0; row < rowIndex; ++row)
        {
            for (int column = 0; column < columnIndex; ++column)
            {
                if (row ==0||column==0 ||row==rowIndex -1||column ==columnIndex -1)
                {
                    GameObject tmp = Instantiate(outWallArray[Random.Range(0, outWallArray.Length)], new Vector3(xOffset + column, yOffset + row, 0f), Quaternion.identity);
                    tmp.transform.parent = map.transform;
                }
                else
                {
                    GameObject tmp = Instantiate(floorArray[Random.Range(0, floorArray.Length)], new Vector3(xOffset + column, yOffset + row, 0f), Quaternion.identity);
                    tmp.transform.parent = map.transform;
                }
            }
        }

        //
        //Save the position
        //
        for (int row=2;row <rowIndex -2;++row)
        {
            for (int column=2;column <columnIndex -2;++column )
            {
                position.Add(new Vector2(xOffset + column, yOffset + row));
            }
        }

        //
        //Create obastacles
        //
        int obstalcesCount = Random.Range(minObstaclesNum, maxObstaclesNum+1);
        InitItems(obstacles, obstalcesCount, map.transform);

        //
        //Craete foods by level
        //
        int foodCount = Random.Range(2, 2+gameManager.level / 2);
        InitItems(foods, foodCount, map.transform);

        //
        //Create enemies by level
        //
        int enemyCount = Random.Range(1, 2 + gameManager.level / 4);
        InitItems(enemies, enemyCount, map.transform );

        //
        //Create exit
        //
        GameObject g = Instantiate(exit, new Vector3(xOffset + columnIndex - 2, yOffset + rowIndex - 2), Quaternion.identity);
        g.transform.parent = map.transform;
        position.Clear();
    }

    //The method of create items
    private void InitItems(GameObject [] items,int count,Transform parent)
    {
        for (int i = 0; i < count; ++i)
        {
            int positionIndex = Random.Range(0, position.Count);
            Vector2 pos = position[positionIndex];
            position.RemoveAt(positionIndex);
            GameObject go = Instantiate(items[Random.Range(0, items.Length)], new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
            go.transform.SetParent(parent);
        }
    }
}
