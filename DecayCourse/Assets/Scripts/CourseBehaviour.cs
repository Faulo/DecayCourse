using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CourseBehaviour : MonoBehaviour {

	public static CourseBehaviour Main;

	[SerializeField]
	CourseSegment SegmentPrefab;
	[SerializeField]
	public Vector2Int GridSize;
    [SerializeField]
    public int CoursePadding;


    public List<CourseSegment> ActiveSegments = new List<CourseSegment>();
	public List<CourseSegment> NecessarySegments = new List<CourseSegment>();

	CourseSegment[][] StartFinishPairs;
	CourseSegment[][] Segments;

    [SerializeField]
    public int BalloonCount;
    [SerializeField]
    public float BalloonHeight;
    [SerializeField]
    public float BalloonRange;
    [SerializeField]
    Balloon BalloonPrefab;
    List<Balloon> Balloons = new List<Balloon>();

    Vector3 TopLeftCorner;
    Vector3 BottomRightCorner;

    // Use this for initialization
    void Start () {
        Main = this;
        GridSize = EvenOut(GridSize);
		SetupGrid(GridSize.x, GridSize.y);
        SetupConnections(GridSize.x/2, CoursePadding);
		SetupStartFinishPairs(GridSize.x / 2, CoursePadding);
        SpawnBalloons(BalloonCount);
	}
	
	/// <summary>
	/// removes a seg if possible
	/// </summary>
	/// <param name="seg"></param>
	/// <returns></returns>
	public bool RemoveSegment(CourseSegment seg) {
        if (seg.Active) {
            seg.Disappear();
            ActiveSegments.Remove(seg);
            return true;
            /*
            seg.Active = false;
			foreach (CourseSegment[] pair in StartFinishPairs) {
                if (IsConnected(pair[0], pair[1]))
				{
					seg.Disappear();
					ActiveSegments.Remove(seg);
					//RemoveUnreachable();
					return true;
                }
            }
            seg.Active = true;
			NecessarySegments.Add(seg);
            //*/
		}
		return false;
	}

	/// <summary>
	/// Removes unreachableSegments
	/// </summary>
	public void RemoveUnreachable()
	{
		foreach(CourseSegment seg in ActiveSegments)
		{
			seg.Reachable = false;
		}
		foreach (CourseSegment[] pair in StartFinishPairs)
		{ 
			if (IsConnected(pair[0], pair[1]))
			{
				List<CourseSegment> checkedSegments = new List<CourseSegment>();
                List<CourseSegment> exploredSegments = new List<CourseSegment> {
                    pair[0]
                };
                while (exploredSegments.Count > 0)
				{
					CourseSegment[] sx = exploredSegments.ToArray();
					for (int i = 0; i < sx.Length; i++)
					{
						checkedSegments.Add(sx[i]);
						sx[i].Reachable = true;
						exploredSegments.Remove(sx[i]);
						for (int j = 0; j < sx[i].Neighbors.Count; j++)
						{
							if (sx[i].Neighbors[j].Active && !checkedSegments.Contains(sx[i].Neighbors[j]) && !exploredSegments.Contains(sx[i].Neighbors[j]))
							{
								exploredSegments.Add(sx[i].Neighbors[j]);
							}
						}
					}
				}
				break;
			}
		}
		CourseSegment[] sa = ActiveSegments.ToArray();
		for(int i = 0; i < sa.Length; i++)
		{
			if (!sa[i].Reachable)
			{
				sa[i].Active = false;
				sa[i].Disappear();
				ActiveSegments.Remove(sa[i]);
			}else
			{
				sa[i].DrawLines();
			}
		}
	}

	/// <summary>
	/// Sets up a Grid of Cubes
	/// </summary>
	/// <param name="sizeX"></param>
	/// <param name="sizeZ"></param>
	void SetupGrid(int sizeX, int sizeZ)
	{
		//Setups the base Grid
		Segments = new CourseSegment[sizeX][];
		for (int x = 0; x<sizeX; x++)
		{
			Segments[x] = new CourseSegment[sizeZ];
			for(int z = 0; z<sizeZ; z++)
			{
				Segments[x][z] = Instantiate(SegmentPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                var xOkay = x >= CoursePadding && x < sizeX - CoursePadding;
                var zOkay = z >= CoursePadding && z < sizeZ - CoursePadding;

                if (xOkay && zOkay) {
                    Segments[x][z].ReappearInstant();
                } else {
                    Segments[x][z].DisappearInstant();
                }
                if (x == 0 && z == 0) {
                    TopLeftCorner = Segments[x][z].transform.position;
                } else {
                    BottomRightCorner = Segments[x][z].transform.position;
                }
            }
		}
		//transform.position = new Vector3(-GridSize.x/2, 0, -GridSize.y/2);
	}

    /// <summary>
    /// Removes a hole in the center of the Course depending on the Width
    /// </summary>
    /// <param name="courseWidth"></param>
    void RemoveCenter(int courseWidth) {
        for (int x = courseWidth; x < Segments.Length - courseWidth; x++) {
            for (int z = courseWidth; z < Segments[x].Length - courseWidth; z++) {
                ActiveSegments.Remove(Segments[x][z]);
                Segments[x][z].DisappearInstant();
            }
        }
    }

    /// <summary>
    /// Creates a graph by connecting nearby cubes
    /// </summary>
    /// <param name="startLinePosZ"></param>
    /// <param name="startLineLengthX"></param>
    void SetupConnections(int startLinePosZ, int startLineLengthX)
	{
		for (int x = 0; x < Segments.Length; x++)
		{
			for (int z = 0; z < Segments[x].Length; z++)
			{
				if (Segments[x][z].Active)
				{
					if (x - 1 >= 0)
					{
						Segments[x][z].AddNeighbor(Segments[x - 1][z]);
					}
					if (x + 1 < Segments.Length)
					{
						Segments[x][z].AddNeighbor(Segments[x + 1][z]);
					}
					if (z - 1 >= 0 && (z != startLinePosZ || x >= startLineLengthX))
					{
						Segments[x][z].AddNeighbor(Segments[x][z - 1]);
					}
					if (z + 1 < Segments[x].Length && (z != startLinePosZ - 1 || x >= startLineLengthX))
					{
						Segments[x][z].AddNeighbor(Segments[x][z + 1]);
					}
					Segments[x][z].DrawLines();
				}
			}
		}
	}

	/// <summary>
	/// Sets up possible start and finish blocks
	/// </summary>
	/// <param name="startLinePosZ"></param>
	/// <param name="startLineLengthX"></param>
	void SetupStartFinishPairs(int startLinePosZ, int startLineLengthX)
	{
		StartFinishPairs = new CourseSegment[startLineLengthX][];
		for(int i=0; i<startLineLengthX; i++)
		{
			StartFinishPairs[i] = new CourseSegment[] {Segments[i][startLinePosZ], Segments[i][startLinePosZ-1]};
		}
	}

    void SpawnBalloons(int count) {
        for (int i = 0; i < count; i++) {
            SpawnBalloon();
        }
    }
    public void SpawnBalloon() {
        float x = Random.Range(TopLeftCorner.y, BottomRightCorner.x);
        float y = BalloonHeight;
        float z = Random.Range(TopLeftCorner.z, BottomRightCorner.z);

        Instantiate(BalloonPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
    }


    Vector2Int EvenOut(Vector2Int input)
	{
		if(input.x %2 != 0)
		{
			input.x++;
		}
		if(input.y %2 != 0)
		{
			input.y++;
		}
		return input;
	}

	int MaxWidth(int input)
	{
		if (input >= GridSize.x / 2)
		{
			input = GridSize.x / 2 - 1;
		}
		if (input >= GridSize.y / 2)
		{
			input = GridSize.y / 2 - 1;
		}
		return input;
	}

	bool IsConnected(CourseSegment start, CourseSegment finish)
	{
		List<CourseSegment> checkedSegments = new List<CourseSegment>();
		List<CourseSegment> exploredSegments = new List<CourseSegment>();
		if (start.Active)
		{
			exploredSegments.Add(start);
		}
		while (exploredSegments.Count > 0)
		{
			for (int i = 0; i<exploredSegments.Count; i++)
			{
				for (int j = 0; j<exploredSegments[i].Neighbors.Count; j++)
				{
					if (exploredSegments[i].Neighbors[j].Active && !checkedSegments.Contains(exploredSegments[i].Neighbors[j]) && !exploredSegments.Contains(exploredSegments[i].Neighbors[j]))
					{
						if (ReferenceEquals(exploredSegments[i].Neighbors[j], finish))
						{
							return true;
						}
						else
						{
							exploredSegments.Add(exploredSegments[i].Neighbors[j]);
						}
					}
				}
				checkedSegments.Add(exploredSegments[i]);
				exploredSegments.Remove(exploredSegments[i]);
			}
		}
		return false;
	}

    public void RespawnSegments(Vector3 position, Color color) {
        Segments.SelectMany(segments => segments)
            .Where(segment => segment.Active == false)
            .Where(segment => Vector3.Distance(position, segment.transform.position) < BalloonRange)
            .ForAll((segment) => {
                segment.SetColor(color);
                segment.ReappearInstant();
            });
    }
}
