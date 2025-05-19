using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class DraggavleRank : MonoBehaviour
{
    public int rankLevel = 1;
    public float drageSpeed = 10f;
    public float snapBackSpeed = 20f;

    public bool isDragging = false;
    public Vector3 originalPosition;
    public GridCell currentCell;

    public Camera mainCamera;
    public Vector3 dragOffset;
    public SpriteRenderer SpriteRenderer;

    public GameManager gameManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + dragOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, drageSpeed * Time.deltaTime);
        }
        else if (transform.position != originalPosition && currentCell != null)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, snapBackSpeed * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        if (!isDragging)
        {
            return;
        }
        StopDragging();
    }

    private void StartDragging()
    {
        isDragging = true;
        dragOffset = transform.position - GetMouseWorldPosition();
        SpriteRenderer.sortingOrder = 10;
    }

    private void StopDragging()
    {
        isDragging = false;
        SpriteRenderer.sortingOrder = 1;
        GridCell targetCell = gameManager.FindClosetCell(transform.position);

        if(targetCell != null)
        {
            if(targetCell.currentRank == null)
            {
                MoveToCell(targetCell);
            }
            else if(targetCell.currentRank!=this && targetCell.currentRank.rankLevel == rankLevel)
            {
                MergeWithCell(targetCell);
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    public void MoveToCell(GridCell targetCell)
    {
        if(currentCell != null)
        {
            currentCell.currentRank=null;
        }

        currentCell = targetCell;
        targetCell.currentRank=this;

        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f);
        transform.position = originalPosition;

        gameManager.MergeRankes(this, targetCell.currentRank);
    }

    public void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
    }

    public void MergeWithCell(GridCell targetCell)
    {
        if(targetCell.currentRank==null || targetCell.currentRank.rankLevel != rankLevel)
        {
            ReturnToOriginalPosition();
            return;
        }

        if(currentCell!=null)
        {
            currentCell.currentRank = null;
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    public void SetRankLevel(int level)
    {
        rankLevel = level;
        if (gameManager != null && gameManager.rankSprites.Length > level - 1)
        {
            SpriteRenderer.sprite = gameManager.rankSprites[level - 1];
        }
    }
}
