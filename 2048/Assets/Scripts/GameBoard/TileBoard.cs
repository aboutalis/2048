using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TileBoard : MonoBehaviour
{
    public GM gameManager;
    public GMTimer gameManagerTimer;
    public Tile tilePrefab;
    public StateOfTile[] tileStates;

    private TileGrid grid;

    private List<Tile> tiles;
    private bool waiting;

    private static System.Random rand = new System.Random();

    private AudioManager audio;

    public int highValue = 0;

    public int nextGoalValue;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>();
    }

    private void FixedUpdate()
    {
        audio = FindObjectOfType<AudioManager>();
    }

    public void TileCreation()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);

        int randomNumber = rand.Next(0, 2); // Generates a random integer between 0 and 1
        
        if (randomNumber == 0)
        {
            tile.SetState(tileStates[0], 2);
        }
        else
        {
            tile.SetState(tileStates[1], 4);
        }

        tile.PlayAppearAnimation();

        tile.Spawn(grid.RandomEmptyCell());
        tiles.Add(tile);
    }

    private void Update()
    {
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(Vector2Int.up, 0, 1, 1, 1);
                
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
                //audio.Play("Move");
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Vector2Int.left, 1, 1, 0, 1);
                //audio.Play("Move");
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Vector2Int.right, grid.width - 2, -1, 0, 1);
                //audio.Play("Move");
            }
        }       
    }

    public void ClearingBoard()
    {
        foreach(var cell in grid.cells){
            cell.tile = null;
        }

        foreach(var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }

    private void Move(Vector2Int direction, int startX, int incrX, int startY, int incrY)
    {
        bool changed = false;
        for (int x = startX; x>=0 && x < grid.width; x+=incrX)
        {
            for (int y = startY; y>=0 && y < grid.height; y+= incrY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.notempty)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
            audio.Play("Move");
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adj = grid.GetAdjacentCell(tile.cell, direction);

        while (adj != null)
        {
            if (adj.notempty)
            {
                if (CanMerge(tile, adj.tile))
                {
                    Merge(tile, adj.tile);
                    return true;
                }
                break;
            }

            newCell = adj;
            adj = grid.GetAdjacentCell(adj, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number==b.number && !b.locked;       
    }

    public void FindMax(Tile tile)
    {
        if (tile.number > highValue)
        {            
            highValue = tile.number;
            nextGoalValue = highValue * 2;

            if(SceneManager.GetActiveScene().name == "TimerMode")
            {
                gameManagerTimer.SetNextGoal();
            }
            else
            {
                gameManager.SetNextGoal();
            }            
        }

        float best = PlayerPrefs.GetFloat("ht");

        if (highValue == 2048 && SceneManager.GetActiveScene().name == "TimerMode")
        {
            //pause time
            GameObject.Find("GM").GetComponent<Timer>().PauseTimer();
            float time = GameObject.Find("GM").GetComponent<Timer>().pausedTime;

            //check
            if (time < best)
            {
                gameManagerTimer.BestWin(time);
            }
            else
            {
                gameManagerTimer.YouWin();
            }           
        }
    }

    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length-1);
        int number = b.number * 2;
        
        b.SetState(tileStates[index], number);

        FindMax(b);

        b.PlayMergeAnimation();

        if (!(SceneManager.GetActiveScene().name == "TimerMode"))
        {
            gameManager.AddScore(number);
        }    
    }

    private int IndexOf(StateOfTile state)
    {
        for(int i=0; i<tileStates.Length; i++)
        {
            if(state == tileStates[i])
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach(var tile in tiles)
        {
            tile.locked = false;
        }

        if (tiles.Count != grid.size)
        {
            TileCreation();
        }

        if (GameOverCheck())
        {
            if (SceneManager.GetActiveScene().name == "TimerMode")
            {
                GameObject.Find("GM").GetComponent<Timer>().PauseTimer();
                gameManagerTimer.GameOver();
            }
            else
            {
                gameManager.GameOver();
            }   
        }
    }

    private bool GameOverCheck()
    {
        if(tiles.Count != grid.size)
        {
            return false;
        }
        foreach(var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if(up != null && CanMerge(tile, up.tile))
            {
                return false;
            }
            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }
            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }
            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }

        Debug.Log("Game over");
        nextGoalValue = 0;

        if (SceneManager.GetActiveScene().name == "TimerMode")
        {
            gameManagerTimer.SetNextGoal();
        }
        else
        {
            gameManager.SetNextGoal();
        }
        
        return true;
    }
}
