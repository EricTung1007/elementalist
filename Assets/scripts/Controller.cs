using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
public enum Type { none, fire, water, grass }
public enum GameState {INGAME, WON, LOST}

public class Controller : MonoBehaviour
{
    public int fixedUpdateCount = 0;

    public GameObject _element;
    static public System.Random random = new System.Random();
    Type[] types = new Type[3] { Type.fire, Type.water, Type.grass };

    public int moveElementInterval = 50;
    public int generateElementInterval = 100;
    public int enemyUpdateInterval = 50;

    public string selectedMagic;
    public int selectedXGrid = -1;
    public int selectedYGrid = -1;

    public GameObject[] enemies;
    public int enemyCount = 3;

    public GameObject[,] elements;
    public int gridWidth = 9;
    public int gridHeight = 2;

    public GameObject _magic;
    public GameObject[] magics;


    private void Start()
    {
        // Spawn Elements
        //Debug.Log("!!");
        elements = new GameObject[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Type type = types[random.Next(3)];
                elements[i, j] = GenerateElement(type, i, j);
                //Debug.Log($"{i} {j}");
            }
        }

        // Spawn Magics
        magics = new GameObject[8]
        {
            GenerateMagic("fireArrow", 0),
            GenerateMagic("acidBall", 1),
            GenerateMagic("steamExplosion", 2),
            GenerateMagic("vinePull", 3),
            GenerateMagic("transformMud", 4),
            GenerateMagic("burningShield", 5),
            GenerateMagic("heal", 6),
            GenerateMagic("elementSurge", 7)
        };
    }

    public GameObject GenerateMagic(string id, int i)
    {
        GameObject obj = Instantiate(_magic, new Vector3(0f, 0f, 0f), Quaternion.identity, GameObject.Find("Magics").transform);
        Magic m = obj.GetComponent<Magic>();
        m.id = id;
        m.position = i;
        m.Activate();
        return obj;
    }
    public GameObject GenerateElement(Type type, int x, int y)
    {
        GameObject obj = Instantiate(_element, new Vector3(0f, 0f, 0f), Quaternion.identity, GameObject.Find("Elements").transform);
        Element e = obj.GetComponent<Element>();
        e.type = type;
        e.xGrid = x;
        e.yGrid = y;
        e.Activate();
        return obj;
    }
    public void TryMagic(int x, int y)
    {
        Magic magic = GameObject.Find(selectedMagic).GetComponent<Magic>();
        int xPattern = magic.xSize;
        int yPattern = magic.ySize;
        if ((x + xPattern > gridWidth) || (y + yPattern > gridHeight)) return;

        bool matchedPattern = true;
        for (int i = 0; i < xPattern; i++)
        {
            for (int j = 0; j < yPattern; j++)
            {
                if (magic.pattern[i, j] == Type.none) continue;
                if (elements[x + i, y + j] == null)
                {
                    matchedPattern = false;
                    return;
                }
                if (magic.pattern[i, j] != elements[x + i, y + j].GetComponent<Element>().type)
                {
                    matchedPattern = false;
                    return;
                }
            }
        }
        if (!matchedPattern) return;

        //Debug.Log("MATCHED!!!");
        for (int i = 0; i < xPattern; i++)
        {
            for (int j = 0; j < yPattern; j++)
            {
                if (magic.pattern[i, j] == Type.none) continue;
                Destroy(elements[x + i, y + j]);
            }
        }
        switch(selectedMagic)
        {
            case "fireArrow": fireArrow(); break;
            case "acidBall": acidBall(); break;
            case "steamExplosion": steamExplosion(); break;
            case "vinePull": vinePull(); break;
            case "transformMud": transformMud(); break;
            case "burningShield": burningShield(); break;
            case "heal": heal(); break;
            case "elementSurge": elementSurge(); break;
        }
    }

    public void fireArrow()
    {
        enemies[0].GetComponent<Unit>().TakeDamage(2, Type.fire);
    }
    public void acidBall()
    {
        enemies[0].GetComponent<Unit>().TakeDamage(5, Type.grass);
    }
    public void steamExplosion()
    {
        enemies[0].GetComponent<Unit>().TakeDamage(8, Type.none);
    }
    public void vinePull() 
    {
        MoveEnemy(enemies.Length - 1, 0);
    }
    public void transformMud()
    {
        enemies[0].GetComponent<Unit>().TakeDamage(7, Type.water);
    }
    public void burningShield()
    {
        enemies[0].GetComponent<Unit>().TakeDamage(7, Type.fire);
    }
    public void heal()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>().heal(5);
    }
    public void elementSurge()
    {
        generateElementInterval = 50;
    }

    private void Update()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (i > 0)
            {
                if ((enemies[i - 1] == null) && (enemies[i] != null))
                {
                    enemies[i - 1] = enemies[i];
                    enemies[i].GetComponent<Enemy>().position = i - 1;
                    enemies[i] = null;
                }
            }

            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Enemy>().SetWorldCoordinate();
            }
        }
    }/*
    public void GiveStatus(GameObject target, string id, int level, int duration)
    {
        target.GetComponent<Unit>().status.Add(new Status(id, level, duration));

        Debug.Log($"{target.name} {target.GetComponent<Unit>().status.Count}");
    }*/

    public void MoveEnemy(int start, int end)
    {
        GameObject tmp = enemies[start];
        if (start < end)
        {
            for(int i = start; i < end; i++)
            {
                enemies[i] = enemies[i + 1];
                enemies[i].GetComponent<Enemy>().position = i;
                enemies[i].GetComponent<Enemy>().SetWorldCoordinate();
            }
        }
        else
        {
            for(int i = start; i > end; i--)
            {

                enemies[i] = enemies[i - 1];
                enemies[i].GetComponent<Enemy>().position = i;
                enemies[i].GetComponent<Enemy>().SetWorldCoordinate();
            }
        }
        enemies[end] = tmp;
        enemies[end].GetComponent<Enemy>().position = end;
        enemies[end].GetComponent<Enemy>().SetWorldCoordinate();
    }

    private void FixedUpdate()
    {
        fixedUpdateCount++;

        // Elements
        if (fixedUpdateCount % moveElementInterval == 0)
        {
            for (int i = 1; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    if ((elements[i - 1, j] == null) && (elements[i, j] != null))
                    {
                        elements[i - 1, j] = elements[i, j];
                        elements[i, j] = null;

                        elements[i - 1, j].GetComponent<Element>().xGrid = i - 1;
                        elements[i - 1, j].GetComponent<Element>().SetWorldCoordinate();
                    }
                }
            }
        }

        if (fixedUpdateCount % generateElementInterval == 0)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                if (elements[gridWidth - 1, j] == null)
                {
                    Type type = types[random.Next(3)];
                    elements[gridWidth - 1, j] = GenerateElement(type, gridWidth - 1, j);
                }
            }
        }

        if (fixedUpdateCount % enemyUpdateInterval == 0)
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().attackCountDown--;
                if (enemy.GetComponent<Enemy>().attackCountDown <= 0)
                {
                    enemy.GetComponent<Enemy>().attackCountDown = enemy.GetComponent<Enemy>().attackCountDownOriginal;
                    enemy.GetComponent<Enemy>().Attack(enemy.GetComponent<Enemy>().atk);
                }
            }
        }

        // Enemy skill perform
    }
}
