using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Flags]
public enum LevelElements
{
    None = 0,
    Platform = 1,
    Ground = 2,
    Edge = 4,
    Jump = 8,
    TwoWayPass = 16,
    Gap = 32,
    TotalBlock = 64,
}

[Flags]
public enum TileOptions
{
    None = 0,
    OpenMinus = 1,
    OpenPlus = 2,
    JumpPlus = 4,
    JumpMinus = 8,
    WallPlus = 16,
    WallMinus = 32
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
    public float lowestGroundY = 9999;
    public TileOptions options;
}

public class LevelGrid : MonoBehaviour
{
    [Header("Input values")]
    [SerializeField] private Vector2 cubeSize;
    [SerializeField] private Vector2 numberCubes;
    [Range(0, 1),SerializeField] private float delayUpdate;
    [SerializeField] private LevelMemory memory;
    [SerializeField] private Tilemap tilemap;
    
    private List<List<CubeFacts>> _gridList = new();
    private Vector2 _min;
    private Vector2 _max;
    private int _layerMask;

    private float _traceTime = 20f;

    private List<RaycastHit2D> _hitList = new();

    private float _maxJumpDistance = 6f;

    private void Awake()
    {
        _layerMask = 1 << 6 | 1 << 10 | 1 << 11;
        SetNewList();
    }

    void Start()
    {
      //  ScanAll();
      AdvancedTrace(new Vector2(0,0));
    }

    private void ScanAll()
    {
        for (var i = 0; i < numberCubes.y; i++)
        {
            for (var j = 0; j < numberCubes.x; j++)
            {
                var cube = _gridList[i][j];
                var start = cube.location + new Vector2(-1,-1f) * new Vector2(cubeSize.x / 2, cubeSize.y / 2);
                AdvancedTrace(new Vector2(i,j));
                //ScanABox(new Vector2(i,j), start, new Vector2(0,1));
                //CheckUnder(cube);
                //SetOptions(cube);

                if ((int) cube.lowestGroundY != 9999)
                {
                   // ScanABox(new Vector2(i,j), new Vector2(-1,-1), new Vector2(0,1));
                }
            }
        }
    }

    private void CheckTop(CubeFacts cube)
    {
        if ((int) cube.lowestGroundY != 9999)
        {
            
        }
    }

    private void SetOptions(CubeFacts cube)
    {
        if (cube.pointsList.SingleOrDefault(x => x.pointType == LevelElements.TwoWayPass) != default)
        {
            cube.options |= TileOptions.OpenMinus | TileOptions.OpenPlus;
        }
    }

    private void CheckForGround(CubeFacts cube)
    {
        
    }
    
    private void CheckUnder(CubeFacts cube)
    {
        var hitCount = 0;
        var missCount = 0;

        for (var i = 0; i < _hitList.Count; i++)
        {
            if (_hitList[i])
            {
                hitCount++;
            }
            
            if (i <= 0 || i >= _hitList.Count - 1) continue;
            
            var past = _hitList[i - 1];
            var present = _hitList[i];
            var future = _hitList[i + 1];

            if (!past && present)
            {
                cube.pointsList.Add(new PointsOfInterest()
                {
                    location = present.point + new Vector2(0, 2),
                    pointType = LevelElements.Edge
                });
            }
            if (!future && present)
            {
                cube.pointsList.Add(new PointsOfInterest()
                {
                    location = present.point+ new Vector2(0, 2),
                    pointType = LevelElements.Edge
                });
            }
        }

        if (cube.pointsList.Count > 0)
        {
            cube.lowestGroundY = cube.pointsList
                .OrderBy(x => x.location.y).ToList()[0].location.y;
        }
        

        if (hitCount == _hitList.Count)
        {
            cube.pointsList.Add(new PointsOfInterest()
            {
                location = cube.location,
                pointType = LevelElements.TwoWayPass
            });
        }
        else if(hitCount == 0)
        {
            cube.pointsList.Add(new PointsOfInterest()
            {
                location = cube.location,
                pointType = LevelElements.Gap
            });
        }
    }
    
    private bool CheckIfLookingAtTarget(Transform enemyTrans, Vector3 destination)
    {
        var dirFromAtoB = (enemyTrans.position - destination).normalized;
        var dotProd = Vector2.Dot(dirFromAtoB, enemyTrans.right);
        return dotProd > 0.9f;
    }

    public void GetOptions(Vector2 point)
    {
        var current = GetSquareFromPoint(point);

        foreach (var p in current.pointsList)
        {
            
        }
    }
    
    public CubeFacts GetSquareFromPoint(Vector2 thePoint)
    {
        var mag = thePoint - _min;
        var indexX = (mag.x - (mag.x % cubeSize.x)) / cubeSize.x;
        var indexY = (mag.y - (mag.y % cubeSize.y)) / cubeSize.y;

        var square = _gridList[(int)indexY][(int)indexX];

        return square;
    }

    private void AdvancedTrace(Vector2 startIndex)
    {
        _hitList.Clear();
        var cube = _gridList[(int) startIndex.x][(int) startIndex.y];
        var topCorner = cube.location + new Vector2(-cubeSize.x / 2, cubeSize.y / 2);
        var numberTraces = 15 ;
        var increment = cubeSize.x  / numberTraces;
        
        for (var i = 0; i < numberTraces; i++)
        {
            var aHit = SingleTrace(topCorner, new Vector2(0,-1), cubeSize.y * numberCubes.y);

            if (aHit)
            {
                if (_hitList.SingleOrDefault(x => (int) x.point.y == (int) aHit.point.y) == default)
                {
                    _hitList.Add(aHit);
                    cube.pointsList.Add(new PointsOfInterest()
                    {
                        location = aHit.point,
                        pointType = LevelElements.Edge
                    });
                }
            }
            topCorner += new Vector2(increment, 0);
        }

        var breakLoop = false;
        List<RaycastHit2D> newHits = new();

        foreach (var h in _hitList)
        {
            newHits.Clear();
            var start = h.point + new Vector2(0, 0.1f);
            while (!breakLoop)
            {
                var aHit = SingleTrace(start, new Vector2(0,-1), 0.2f);
                
                if (!aHit) breakLoop = true;
                newHits.Add(aHit);
                start += new Vector2(increment, 0);
            }

            cube.pointsList.Add(new PointsOfInterest()
            {
                
                pointType = LevelElements.Gap
            });
        }
    }
    
    private RaycastHit2D SingleTrace(Vector2 startPos, Vector2 traceDir, float traceLength)
    {
        var hit = Physics2D.Raycast(startPos, traceDir, traceLength, _layerMask);
        
        if (hit)
        {
            Debug.DrawLine(startPos, hit.point, Color.green, _traceTime);
        }
        else
        {
            Debug.DrawLine(startPos, startPos + traceDir *cubeSize.y, Color.red, _traceTime);
        }
        
        return hit;
    }


    private void ScanInDirection(Vector2 scanDir, Vector2 moveDir, Vector2 index, Vector2 startPos)
    {
        _hitList.Clear();
        var cube = _gridList[(int) index.x][(int) index.y];
        var numberTraces = 15;
        var increment = cubeSize.x / numberTraces;
        
        for (var i = 0; i < numberTraces; i++)
        {

            startPos += new Vector2(increment, 0);
        }
    }
    
    private void ScanABox(Vector2 index, Vector2 startPos, Vector2 traceDir)
    {
        _hitList.Clear();
        var cube = _gridList[(int) index.x][(int) index.y];
  
        var numberTraces = 15;
        var increment = cubeSize.x / numberTraces;
        var start = cube.location + startPos * new Vector2(cubeSize.x / 2, cubeSize.y / 2);
        
        for (var i = 0; i < numberTraces; i++)
        {
            var hit = Physics2D.Raycast(startPos, traceDir, cubeSize.y, _layerMask);
            _hitList.Add(hit);
            if (hit)
            {
                Debug.DrawLine(startPos, hit.point, Color.green, _traceTime);
            }
            else
            {
                Debug.DrawLine(startPos, startPos + traceDir *cubeSize.y, Color.red, _traceTime);
            }
            
            startPos += new Vector2(increment, 0);
        }
    }

    private void AnalyzeHits()
    {
           var numberHits = 0;
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

                if (current) numberHits++;
                
                if (!past || !future)
                {
                    if (!past && current)
                    {
                        hitLocation = _hitList[i].point;
                        pointType = LevelElements.Ground | LevelElements.Edge;
                        storePoint = true;
                    }
                    
                    if (!future && current)
                    {
                        hitLocation = _hitList[i].point;
                        pointType = LevelElements.Ground | LevelElements.Edge;
                        storePoint = true;
                    }
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
                
                else if (current && future)
                {
                    if (current.point.y > future.point.y)
                    {
                        if (Vector2.Distance(current.point, future.point) > 1)
                        {
                            pointType = LevelElements.Platform | LevelElements.Edge;
                            storePoint = true;
                        }
                    }
                }

                if (storePoint)
                {
                }
            }
          
        }
    }
    
    private void SetNewList()
    {
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
        var min = location - new Vector2(size.x  / 2, size.y / 2);
        var max = location + new Vector2(size.x  / 2, size.y / 2);

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
                    if (p.pointType.HasFlag(LevelElements.Edge))
                    {
                        Gizmos.color = Color.yellow;
                    }

                    else if(p.pointType.HasFlag(LevelElements.Platform))
                    {
                        Gizmos.color = Color.magenta;
                    }
                    else if (p.pointType.HasFlag(LevelElements.TwoWayPass))
                    {
                        Gizmos.color = Color.green;
                    }
                    else if (p.pointType.HasFlag(LevelElements.Gap))
                    {
                        Gizmos.color = Color.red;
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
