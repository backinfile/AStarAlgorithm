using AStarAlgorithm.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Vector2Int Size;
    public Vector2Int From;
    public Vector2Int To;
    public float Gap = 1f;
    public bool ShowOpenList = true;
    public bool ShowCloseList = true;
    public bool AutoCalc = false;

    public GameObject PointPrefab;

    private int[,] map;
    private (Image image, InteractiveController interactiveController)[,] points;
    private AStarAlgo algorithm;

    void Start()
    {
        float size = PointPrefab.GetComponent<RectTransform>().rect.width + Gap;
        InitMap(size);
    }

    private void Update()
    { 
        if (AutoCalc)
        {
            if (Time.frameCount % 60 == 0)
            {
                Calc();
            }
        }
    }

    private void InitMap(float size)
    {
        map = new int[Size.x, Size.y];
        algorithm = new AStarAlgo(map);
        points = new (Image, InteractiveController)[Size.x, Size.y];
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var dx = i - (Size.x + 1) / 2f;
                var dy = j - (Size.y + 1) / 2f;
                Vector3 position = new Vector3(dx * size + Screen.width / 2, dy * size + Screen.height / 2);
                GameObject point = Instantiate(PointPrefab, position, Quaternion.identity, transform);
                Image image = point.GetComponent<Image>();
                InteractiveController interactiveController = point.GetComponent<InteractiveController>();

                points[i, j] = (image, interactiveController);

                var pos = new Vector2Int(i, j);
                interactiveController.OnSet = draw =>
                {
                    int value = GetValue(pos.x, pos.y);
                    if (value != 2 && value != 3)
                    {
                        if (draw)
                        {
                            SetValue(pos.x, pos.y, -1);
                        }
                        else
                        {
                            SetValue(pos.x, pos.y, 0);
                        }
                    }
                };
            }
        }
        SetValue(From.x, From.y, 2);
        SetValue(To.x, To.y, 3);
    }

    public void Calc()
    {
        //Debug.Log("calc");

        // 清空路径
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                int value = GetValue(i, j);
                if (value == 4 || value == 5 || value == 6)
                {
                    SetValue(i, j, 0);
                }
            }
        }

        // 获取路径
        var startTime = DateTime.Now;
        var path = algorithm.GetPath((From.x,From.y), (To.x, To.y), value=>value==-1);
        Debug.Log($"计算路径用时:{DateTime.Now - startTime}");

        if (path == null)
        {
            Debug.LogWarning("没有路径！");
            return;
        }

        foreach (var (x, y) in path)
        {
            if (GetValue(x,y) == 0)
            {
                SetValue(x, y, 4);
            }
            else if (GetValue(x,y) == 1)
            {
                Debug.LogWarning("路径生成在障碍上了");
            }
        }

        //while (!algorithm.OpenList.Empty)
        //{
        //    var node = algorithm.OpenList.PopTop();
        //    if (GetValue(node.x, node.y) == 0)
        //    {
        //        SetValue(node.x, node.y, 5);
        //    }
        //}
        foreach (var node in algorithm.nodes)
        {
            if (node == null) continue;
            if (ShowCloseList && node.isClosed)
            {
                if (GetValue(node.x, node.y) == 0)
                {
                    SetValue(node.x, node.y, 6);
                }
            }
            else if (ShowOpenList && node.isOpened && !node.isClosed)
            {
                if (GetValue(node.x, node.y) == 0)
                {
                    SetValue(node.x, node.y, 5);
                }
            }
        }
    }

    private int GetValue(int i, int j)
    {
        return map[i, j];
    }
    private void SetValue(int x, int y, int value)
    {
        map[x, y] = value;
        switch (value)
        {
            case 0: // 空白
                points[x, y].image.color = Color.white;
                break;
            case -1: // 墙
                points[x, y].image.color = Color.black;
                break;
            case 2: // start
                points[x, y].image.color = Color.red;
                break;
            case 3: // end
                points[x, y].image.color = Color.blue;
                break;
            case 4: // path
                points[x, y].image.color = Color.green;
                break;

            case 5: // in openList
                points[x, y].image.color = Color.gray;
                break;

            case 6: // in closeList
                points[x, y].image.color = Color.cyan;
                break;
        }
    }

    /// <summary>
    /// 清空障碍和路径
    /// </summary>
    public void Clear()
    {
        //Debug.Log("clear");
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                if (GetValue(i, j) != 2 && GetValue(i, j) != 3) SetValue(i, j, 0);
            }
        }
    }

}
