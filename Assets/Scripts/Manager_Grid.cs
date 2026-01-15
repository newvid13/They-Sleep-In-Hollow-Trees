using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Grid : MonoBehaviour
{
    public Node[,] grid = new Node[5, 5];

    public GameObject[] liveTiles;
    public Sprite[] deadTiles;

    private AudioSource sfxAud;
    public AudioClip[] sfxClips;

    public void SetupValues()
    {
        sfxAud = GetComponent<AudioSource>();

        int incrementalI = 0;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                grid[j, i] = new Node(j, i, deadTiles[incrementalI], liveTiles[incrementalI]);
                incrementalI++;
            }
        }

        foreach (Node n in grid)
        {
            n.CalcNeighbors(ref grid);
        }
    }

    public Node FindEmptyNode(int x, int y)
    {
        foreach (Node n in grid[x, y].neighbors)
        {
            if (n.myTile == null)
                return n;
        }

        return null;
    }

    public Node[] FindAliveNodes()
    {
        List<Node> nodes = new List<Node>();

        foreach (Node n in grid)
        {
            if(n.isAlive && n.myTile != null)
                { nodes.Add(n); }
        }

        return nodes.ToArray();
    }

    public void MoveTile(int x, int y, Node nodeToMoveTo)
    {
        Vector2 newPos = new Vector2(nodeToMoveTo.x * 116 + 68, nodeToMoveTo.y * 116 + 68);
        grid[x, y].myTile.GetComponent<Image>().rectTransform.anchoredPosition = newPos;
        nodeToMoveTo.myTile = grid[x, y].myTile;
        nodeToMoveTo.myTile.GetComponent<Tile>().currentX = nodeToMoveTo.x;
        nodeToMoveTo.myTile.GetComponent<Tile>().currentY = nodeToMoveTo.y;
        grid[x, y].myTile = null;
    }

    public void KillTile(int x, int y)
    {
        grid[x, y].isAlive = false;
        grid[x, y].myTile.GetComponent<Tile>().KillTile(grid[x, y].deadImg);
    }

    public void PlayAudio(int code)
    {
        sfxAud.PlayOneShot(sfxClips[code]);
    }
}

public class Node
{
    public int x;
    public int y;
    public bool isAlive = true;
    public GameObject myTile = null;
    public Sprite deadImg;
    public List<Node> neighbors = new List<Node>();

    public Node(int x, int y, Sprite deadTile, GameObject liveTile)
    {
        this.x = x;
        this.y = y;
        deadImg = deadTile;
        myTile = liveTile;
    }

    public void CalcNeighbors(ref Node[,] nodeGrid)
    {
        //LEFT
        if (x > 0)
        {
            neighbors.Add(nodeGrid[x - 1, y]);
        }
        //RIGHT
        if (x < 4)
        {
            neighbors.Add(nodeGrid[x + 1, y]);
        }
        //UP
        if (y < 4)
            neighbors.Add(nodeGrid[x, y + 1]);
        //DOWN
        if (y > 0)
            neighbors.Add(nodeGrid[x, y - 1]);
    }
}