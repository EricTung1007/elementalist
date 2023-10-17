using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject element;

    static public int fixedUpdateCounter = 0;

    private GameObject[,] positions;

    // Start is called before the first frame update
    void Start()
    {
        var elements = new string[3] { "fire", "water", "grass" };
        positions = new GameObject[9, 2];

        var rand = new System.Random();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int elementIndex = rand.Next(elements.Length);
                positions[i, j] = Create(elements[elementIndex], i, j);
            }
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(element, new Vector3(0, 0, -1), Quaternion.identity, this.transform.parent);
        Element e = obj.GetComponent<Element>();
        e.name = name;
        e.SetXGrid(x);
        e.SetYGrid(y);
        e.Activate();
        return obj;
    }

    public void FixedUpdate()
    {
        fixedUpdateCounter++;
        if (Game.fixedUpdateCount % 50 == 0) 
        {
            for(int i = 1; i < 9;i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    if ((positions[i - 1, j] == null) && (positions[i, j] != null))
                    {
                        positions[i - 1, j] = positions[i, j];
                        positions[i - 1, j].GetComponent<Element>().SetXGrid(i - 1);
                        positions[i - 1, j].GetComponent<Element>().SetCoordinate();
                        positions[i, j] = null;
                    }
                }
            }
            
        }
        var elements = new string[3] { "fire", "water", "grass" };
        var rand = new System.Random();
        if (Game.fixedUpdateCount % 100 == 0)
        {
            for (int j = 0; j < 2; j++)
            {
                if ((positions[8, j] == null))
                {
                    int elementIndex = rand.Next(elements.Length);
                    positions[8, j] = Create(elements[elementIndex], 8, j);
                }
            }
        }
    }

}
