using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using System;

public class Agent : MonoBehaviour
{
    WorldGen world;

    string matrixDebug;

    int[,] Rmatrix = new int[7,5];
    int[,] Qmatrix = new int[7,5];

    int[,] moves = {
         {0, -1},   // North
         {1, 0},    // East
         {0, 1},    // South
         {-1, 0}};  // West

    public Vector2 position; 

    public List<Vector2> actions = new List<Vector2>();

    public List<R> Rewards = new List<R>();
    public int Qvalues;
    public float gamma = 0.8f;

    void Start()
    {
        world = GameObject.Find("Global_Scripts").GetComponent<WorldGen>();
        
        for (int i = 0; i < world.width; i++)
        {
            for (int j = 0; j < world.height; j++)
            {
                Rmatrix[i, j] = 0;
                Qmatrix[i, j] = 0;
            }
        }

        Rmatrix[1, 4] = -10;
        Rmatrix[2, 4] = -10;
        Rmatrix[3, 4] = -10;
        Rmatrix[4, 4] = -10;
        Rmatrix[5, 4] = -10;
        Rmatrix[6, 4] = 100;

        
        position = new Vector2(0,4);

        

        GetLegalMoves(position);

        float stepTime = 0.3f;
        float delay = 0.0f;

        InvokeRepeating("TestMove", delay , stepTime);

    }

    void GetLegalMoves(Vector2 currentPos)
    {
        actions.Clear();
        //Debug.Log(currentPos.ToString());
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (Mathf.Ceil(currentPos.x) == i && (Mathf.Ceil(currentPos.y) == j)) ;
                {
                    // if currentpos[0] + moves[0][0] <= rows-1 and currentpos[0] + moves[0][0] >= 0 and currentpos[1] + moves[0][1] <= columns-1 and currentpos[1] + moves[0][1] >= 0:
                    if(currentPos.x + moves[0, 0] < world.width && currentPos.x + moves[0, 0] >= 0 && currentPos.y + moves[0, 1] < world.height && currentPos.y + moves[0, 1] >= 0)
                    {
                        //Debug.Log("North");
                        actions.Add(new Vector2(moves[0,0] , moves[0,1]));
                    }
                    if(currentPos.x + moves[1, 0] < world.width && currentPos.x + moves[1, 0] >= 0 && currentPos.y + moves[1, 1] < world.height && currentPos.y + moves[1, 1] >= 0)
                    {
                        //Debug.Log("East");// + new Vector2(currentPos.x + moves[1, 0] , currentPos.y + moves[1, 1]) );
                        actions.Add(new Vector2(moves[1, 0], moves[1, 1]));
                    }
                    if(currentPos.x + moves[2, 0] < world.width && currentPos.x + moves[2, 0] >= 0 && currentPos.y + moves[2, 1] < world.height && currentPos.y + moves[2, 1] >= 0)
                    {
                        //Debug.Log("South");
                        actions.Add(new Vector2(moves[2, 0], moves[2, 1]));
                    }
                    if(currentPos.x + moves[3, 0] < world.width && currentPos.x + moves[3, 0] >= 0 && currentPos.y + moves[3, 1] < world.height && currentPos.y + moves[3, 1] >= 0)
                    {
                        //Debug.Log("West");
                        actions.Add(new Vector2(moves[3, 0], moves[3, 1]));
                    }

                }
            }
        }
    }

    void GetRandomAction()
    {
        try
        {
            Vector2 temp = actions[UnityEngine.Random.Range(0, actions.Count)];
            position += temp;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("East" + new Vector2(position.x + moves[1, 0] , position.y + moves[1, 1]) );
            throw;
            
        }
        
    }

    R getMaxACtion()
    {
        //var temp = Rewards.Select(a => a.reward).Max(a => a);

        R maxItem = Rewards[0];
        foreach (var a in Rewards)
        {
            if (a.reward > maxItem.reward)
            {
                maxItem = a;
            }
        }

        //Debug.Log("Max action: " + temp);
        return maxItem;
    }

    void QLearning()
    {
        Rewards.Clear();
        // state = square currently in.
        // action = moves possible.

        // need to get max reward action
        // do action 
        // calc reward for last state.
        Debug.Log(position.x + " , " + position.y);
        Qvalues = (Qmatrix[Convert.ToInt32(position.x), Convert.ToInt32(position.y)]);
        foreach (var a in actions)
        {
            Rewards.Add(new R(a, Rmatrix[Convert.ToInt32(position.x + a.x ), Convert.ToInt32(position.y + a.y)]));  
        }
        if (getMaxACtion().reward > 0)
        {
            Qvalues = Convert.ToInt32(getMaxACtion().reward + gamma * (Qmatrix[(int)getMaxACtion().action.x, (int)getMaxACtion().action.y]));
            Qmatrix[Convert.ToInt32(position.x + getMaxACtion().action.x), Convert.ToInt32(position.y + getMaxACtion().action.y)] = Qvalues;
            position += getMaxACtion().action;
            Debug.Log(Qmatrix[Convert.ToInt32(position.x), Convert.ToInt32(position.y)]);
            DebugMatrix(Qmatrix, "Qmatrix");
            DebugMatrix(Rmatrix, "Rmatrix");
        }
        else
        {
            GetRandomAction();
            Vector2 ghj = new Vector2(); 
        }
        actions.Clear();
        
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
        //GetRandomAction();
        QLearning();
    }

    void DebugMatrix(int[,] matirx , string name)
    {
        string temp;
        matrixDebug += name + '\n';
        for (int i = 0; i < world.width; i++)
        {
            for (int j = 0; j < world.height; j++)
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
       GUI.Label(new Rect(10, 10, 200, 500), matrixDebug);
    }
}

[System.Serializable]
public class R
{
    public Vector2 action;
    public int reward;

    public R(Vector2 a , int b)
    {
        action = a;
        reward = b;
    }
}


