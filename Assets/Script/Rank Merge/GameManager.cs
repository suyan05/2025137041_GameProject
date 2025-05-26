using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("그리드")]
    public int gridWidth = 7;
    public int gridHeight = 7;
    public float cellSize = 0.4f;

    public GameObject cellPrefab;
    public Transform gridContainer;

    [Header("계급장")]
    public GameObject rankPrefab;
    public Sprite[] rankSprites;
    public int maxRankLevel = 7;

    [Header("저장")]
    public GridCell[,] grid;

    [Header("초반 생성 계급장")]
    public int SpawnRank = 4;

    private void InitalizeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x * cellSize - (gridWidth * cellSize / 2) + cellSize / 2, y * cellSize - (gridHeight * cellSize / 2) + cellSize / 2, 1f);

                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, gridContainer);
                GridCell cell = cellObj.AddComponent<GridCell>();

                cell.Initiailze(x, y);

                grid[x, y] = cell;
            }
        }
    }

    private void Start()
    {
        InitalizeGrid();

        for(int i = 0; i < SpawnRank; i++)
        {
            SpawnNewRank();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            SpawnNewRank();
        }
    }

    public DraggavleRank CreateRankIncell(GridCell cell, int level)
    {
        if (cell == null || !cell.IsEmpty()) return null;

        level = Mathf.Clamp(level, 1, maxRankLevel);

        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f);

        GameObject rankObj = Instantiate(rankPrefab, rankPosition, Quaternion.identity, gridContainer);
        rankObj.name = "Rank_Level" + level;

        DraggavleRank rank = rankObj.AddComponent<DraggavleRank>();
        rank.SetRankLevel(level);

        cell.SetRank(rank);

        return rank;
    }

    private GridCell FindEnptyCell()
    {
        List<GridCell> emptyCells = new List<GridCell>();

        for(int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].IsEmpty())
                {
                    emptyCells.Add(grid[x, y]);
                }
            }
        }

        if(emptyCells.Count == 0)
        {
            return null;
        }

        return emptyCells[Random.Range(0, emptyCells.Count)];
    }

    public bool SpawnNewRank()
    {
        GridCell emptyCell = FindEnptyCell();
        if (emptyCell == null)
        {
            Debug.Log("비어있는 칸이 없습니다.");
            return false;
        }
        int rankLevel = Random.Range(0, 100) < 80 ? 1 : 2;

        CreateRankIncell(emptyCell, rankLevel);

        return true;
    }

    public GridCell FindClosetCell(Vector3 position)
    {
        for(int x = 0;x<gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].containsPosition(position))
                {
                    return grid[x, y];
                }
            }
        }

        GridCell closestCell = null;
        float closestDistance = float.MaxValue;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float distance = Vector3.Distance(position, grid[x, y].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCell = grid[x, y];
                }
            }
        }

        if (closestDistance > cellSize * 2)
        {
            return null;
        }

        return closestCell;
    }

    public void RemoveRank(DraggavleRank rank)
    {
        if (rank == null) return;
        if (rank.currentCell != null)
        {
            rank.currentCell.SetRank(null);
        }
        Destroy(rank.gameObject);
    }

    public void MergeRankes(DraggavleRank darggRank,DraggavleRank targetRank)
    {
        if (darggRank == null || targetRank == null || darggRank.rankLevel != targetRank.rankLevel)
        {
            if(darggRank != null)
            {
                darggRank.ReturnToOriginalPosition();
                return;
            }
        }

        int newlevel = darggRank.rankLevel + 1;
        if(newlevel > maxRankLevel)
        {
            RemoveRank(darggRank);
            return;
        }

        targetRank.SetRankLevel(newlevel);
        RemoveRank(darggRank);
        if(Random.Range(0,100)<60)
        {
            SpawnNewRank();
        }
    }
}
