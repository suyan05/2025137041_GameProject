using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x, y;
    public DraggavleRank currentRank;
    public SpriteRenderer cellRenderer;

    private void Awake()
    {
        cellRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initiailze(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        name = "Cell_" + x + "_" + y;
    }

    public bool IsEmpty()
    {
        return currentRank == null;
    }

    public bool containsPosition(Vector3 position)
    {
        Bounds bounds = cellRenderer.bounds;
        return bounds.Contains(position);
    }

    public void SetRank(DraggavleRank rank)
    {
        currentRank = rank;

        if(rank !=null)
        {
            rank.currentCell = this;
        }

        rank.originalPosition = new Vector3(transform.position.x, transform.position.y, 0);
        rank.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
