using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseBehaviour : MonoBehaviour {

	public static CourseBehaviour Main;

	[SerializeField]
	CourseSegment SegmentPrefab;
	[SerializeField]
	public Vector2Int GridSize;
	[SerializeField]
	public int CourseWidth;

	public List<CourseSegment> ActiveSegments = new List<CourseSegment>();
	public List<CourseSegment> NecessarySegments = new List<CourseSegment>();

	CourseSegment[][] StartFinishPairs;
	public CourseSegment[][] Segments;

	// Use this for initialization
	void Start () {
		GridSize = EvenOut(GridSize);
		CourseWidth = MaxWidth(CourseWidth);
		SetupGrid(GridSize.x, GridSize.y);
		//RemoveHole(CourseWidth);
		SetupConnections(GridSize.x/2, CourseWidth);
		SetupStartFinishPairs(GridSize.x / 2, CourseWidth);
		Main = this;
	}
	
	/// <summary>
	/// removes a seg if possible
	/// </summary>
	/// <param name="seg"></param>
	/// <returns></returns>
	public bool RemoveSegment(CourseSegment seg)
	{
		if (seg.Active)
		{
			seg.Active = false;
			foreach (CourseSegment[] pair in StartFinishPairs)
			{
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
				Segments[x][z].Active = true;
				ActiveSegments.Add(Segments[x][z]);
			}
		}
		transform.position = new Vector3(-CourseWidth / 2, 0, -GridSize.y / 2);
	}

	/// <summary>
	/// Removes a hole in the center of the Course depending on the Width
	/// </summary>
	/// <param name="courseWidth"></param>
	void RemoveHole(int courseWidth)
	{
		for (int x = courseWidth; x < Segments.Length-courseWidth; x++)
		{
			for (int z = courseWidth; z < Segments[x].Length-courseWidth; z++)
			{
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
}
