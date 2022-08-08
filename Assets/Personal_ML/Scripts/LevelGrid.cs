using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum LevelElements
{
    None = 0,
    Platform = 1,
    Ground = 2,
    Edge = 4
}

[Serializable]
public class PointsOfInterest
{
    public Vector2 location;
    public LevelElements pointType;
}

[Serializable]
public class CubeFacts
{
    public Vector2 location;
    public Color color;
    public Vector2 min;
    public Vector2 max;
    public List<PointsOfInterest> pointsList = new();
}

public class LevelGrid : MonoBehaviour
{
    [Header("Input values")]
    [SerializeField] private Vector2 cubeSize;
    [SerializeField] private Vector2 numberCubes;
    [Range(0, 1),SerializeField] private float delayUpdate;
    [SerializeField] private LevelMemory memory;
    
    private List<List<CubeFacts>> _gridList = new();
    private Vector3 _min;
    private Vector3 _max;
    private int _layerMask;

    private List<RaycastHit2D> _hitList = new();

    private void Awake()
    {
        _layerMask = 1 << 6;
        SetNewList();
    }

    void Start()
    {
        ScanABox(new Vector2(0,0), true);
        ScanABox(new Vector2(0,0), false);
    }
    
    private void ScanABox(Vector2 index, bool upOrDown)
    {
        _hitList.Clear();
        var cube = _gridList[(int) index.x][(int) index.y];
        var basePos = cube.location;
        var traceDir = new Vector2();
        var numberTraces = 15;
        var increment = cubeSize.x / numberTraces;

        if (upOrDown)
        {
            traceDir += new Vector2(0, 1);
            basePos += new Vector2(-cubeSize.x / 2, -cubeSize.y / 2);
        }
        else
        {
            traceDir -= new Vector2(0, 1);
            basePos += new Vector2(-cubeSize.x / 2, cubeSize.y / 2);
        }

        for (var i = 0; i < numberTraces; i++)
        {
            var hit = Physics2D.Raycast(basePos, traceDir, cubeSize.y, _layerMask);
            _hitList.Add(hit);
            if (hit)
            {
                Debug.DrawLine(basePos, hit.point, Color.green, 90);
            }
            else
            {
                Debug.DrawLine(basePos, basePos + traceDir *cubeSize.y, Color.red, 90);
            }
            
            basePos += new Vector2(increment, 0);
        }
        
        var previousHit = false;
        
        
        for (var i = 0; i < _hitList.Count; i++)
        {
            var hitLocation = _hitList[i].point;
            var pointType = LevelElements.None;
            var storePoint = false;
            var lowest = hitLocation;
            
            if (i > 0 && i < _hitList.Count - 1)
            {
                var past = _hitList[i - 1];
                var current = _hitList[i];
                var future = _hitList[i + 1];
                
                if (!past && current)
                {
                    hitLocation = _hitList[i].point;
                    pointType = LevelElements.Ground | LevelElements.Edge;
                    storePoint = true;
                }

                else if (past && current)
                {
                    if (past.point.y < current.point.y)
                    {
                        if (Vector2.Distance(past.point, current.point) > 1)
                        {
                            pointType = LevelElements.Platform | LevelElements.Edge;
                            storePoint = true;
                        }
                    }
                }

                if (storePoint)
                {
                    cube.pointsList.Add(new PointsOfInterest()
                    {
                        location = hitLocation,
                        pointType = pointType
                    });
                }
            }
          
        }
    } 
    
    private void SetNewList()
    {
        _gridList = new List<List<CubeFacts>>((int)numberCubes.y);

        for (var i = 0; i < numberCubes.y; i++)
        {
            var newList = new List<CubeFacts>((int)numberCubes.x);
            for (var j = 0; j < numberCubes.x; j++)
            {
                var next = transform.position + new Vector3(j * cubeSize.x ,i * cubeSize.y);
                var minMax = GetMinMax(next, cubeSize);
                newList.Add( new CubeFacts()
                {
                    color = Color.red,
                    location = next,
                    min = minMax.Item1,
                    max = minMax.Item2,
                });
            }
            _gridList.Add(newList);
        }
 
        _max = _gridList[^1][^1].location + cubeSize / 2;
        _min = _gridList[0][0].location - cubeSize / 2;
    }
    
    private Tuple<Vector2, Vector2> GetMinMax(Vector2 location, Vector2 size)
    {
        var min = location - new Vector2(size.x  / 2, 0);
        var max = location + new Vector2(size.x  / 2, size.y);

        return new Tuple<Vector2, Vector2>(min, max);
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (var i = 0; i < numberCubes.y; i++)
        {
            for (var j = 0; j < numberCubes.x; j++)
            {
                var next = transform.position + new Vector3(j * cubeSize.x ,i * cubeSize.y);
                Gizmos.DrawWireCube(next, new Vector3(cubeSize.x ,cubeSize.y));
            }
        }

        if (!Application.isPlaying) return;
        
        for (var i = 0; i < numberCubes.y; i++)
        {
            for (var j = 0; j < numberCubes.x; j++)
            {
                var cube = _gridList[i][j];

                foreach (var p in cube.pointsList)
                {
                    if (p.pointType.HasFlag(LevelElements.Ground))
                    {
                        Gizmos.color = Color.yellow;
                    }

                    else if(p.pointType.HasFlag(LevelElements.Platform))
                    {
                        Gizmos.color = Color.magenta;
                    }

                    else
                    {
                        Gizmos.color = Color.black;
                    }

                    Gizmos.DrawWireSphere(p.location, 0.3f);
                }
                
            }
        }
        
    }
    #endif
}
