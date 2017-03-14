using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;

public class Agent : MonoBehaviour
{
    WorldGen world;

    string matrixDebug;

    int[,] Rmatrix = new int[5,7];
    int[,] Qmatrix = new int[5, 7];

    int[,] moves = {
         {0, -1},   // North
         {1, 0},    // East
         {0, 1},    // South
         {-1, 0}};  // West

    public Vector2 position; 

    public List<Vector2> actions = new List<Vector2>();

    void Start()
    {
        world = GameObject.Find("Global_Scripts").GetComponent<WorldGen>();
        //Debug.Log(world.position[new Vector2(0,0)].transform.position);
        for (int i = 0; i < world.height; i++)
        {
            for (int j = 0; j < world.width; j++)
            {
                Rmatrix[i, j] = 0;
            }
        }

        Rmatrix[4, 1] = -10;
        Rmatrix[4, 2] = -10;
        Rmatrix[4, 3] = -10;
        Rmatrix[4, 4] = -10;
        Rmatrix[4, 5] = -10;
        Rmatrix[4, 6] = 100;

        for (int i = 0; i < world.height; i++)
        {
            for (int j = 0; j < world.width; j++)
            {
                Qmatrix[i, j] = 0;
            }
        }

        DebugMatrix(Qmatrix);
        position = new Vector2(2,2);
        

        GetLegalMoves(position);

        float stepTime = 1.0f;
        float delay = 0.0f;

        InvokeRepeating("TestMove", delay , stepTime);

    }

    void GetLegalMoves(Vector2 currentPos)
    {
        actions.Clear();
        Debug.Log(currentPos.ToString());
        for (int i = 0; i < world.width; i++)
        {
            for (int j = 0; j < world.height; j++)
            {
                if ((int)currentPos.x == i && (int)currentPos.y == j)
                {
                    // if currentpos[0] + moves[0][0] <= rows-1 and currentpos[0] + moves[0][0] >= 0 and currentpos[1] + moves[0][1] <= columns-1 and currentpos[1] + moves[0][1] >= 0:
                    if(currentPos.x + moves[0, 0] < world.width && currentPos.x + moves[0, 0] >= 0 && currentPos.y + moves[0, 1] < world.height && currentPos.y + moves[0, 1] >= 0)
                    {
                        Debug.Log("North");
                        actions.Add(new Vector2(moves[0,0] , moves[0,1]));
                    }
                    if(currentPos.x + moves[1, 0] < world.width && currentPos.x + moves[1, 0] >= 0 && currentPos.y + moves[1, 1] < world.height && currentPos.y + moves[1, 1] >= 0)
                    {
                        Debug.Log("East");
                        actions.Add(new Vector2(moves[1, 0], moves[1, 1]));
                    }
                    if(currentPos.x + moves[2, 0] < world.width && currentPos.x + moves[2, 0] >= 0 && currentPos.y + moves[2, 1] < world.height && currentPos.y + moves[2, 1] >= 0)
                    {
                        Debug.Log("South");
                        actions.Add(new Vector2(moves[2, 0], moves[2, 1]));
                    }
                    if(currentPos.x + moves[3, 0] < world.width && currentPos.x + moves[3, 0] >= 0 && currentPos.y + moves[3, 1] < world.height && currentPos.y + moves[3, 1] >= 0)
                    {
                        Debug.Log("West");
                        actions.Add(new Vector2(moves[3, 0], moves[3, 1]));
                    }

                }
            }
        }
    }

    void GetRandomAction()
    {
        Vector2 temp = actions[Random.Range(0, actions.Count)];
        position += temp;
    }

    void QLearning()
    {
        // state = square currently in.
        // action = moves possible.

        // need to get max reward action
        // do action 
        // calc reward for last state.
    }


    void Update()
    {
        Vector3 temp = new Vector3(position.x , -position.y , 0);
        transform.position = temp;
    }


    // Debug Stuff
    void TestMove()
    {
        GetLegalMoves(position);
        GetRandomAction();
    }

    void DebugMatrix(int[,] matirx)
    {
        string temp;
        for (int i = 0; i < world.height; i++)
        {
            for (int j = 0; j < world.width; j++)
            {
                temp = matirx[i, j].ToString();
                matrixDebug += temp;
                matrixDebug += ',';
            }
            matrixDebug += '\n';
        }   
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 200, 200), matrixDebug, 100);
    }
}


