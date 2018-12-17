using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseBehaviour : MonoBehaviour {

	public static CourseBehaviour main;

	[SerializeField]
	CourseSegment segmentPrefab;
	[SerializeField]
	public Vector2Int gridSize;
	[SerializeField]
	public int courseWidth;

	public List<CourseSegment> activeSegments = new List<CourseSegment>();
	public List<CourseSegment> necessarySegments = new List<CourseSegment>();

	CourseSegment[][] startFinishPairs;
	public CourseSegment[][] segments;

	// Use this for initialization
	void Start () {
		gridSize = EvenOut(gridSize);
		courseWidth = MaxWidth(courseWidth);
		SetupGrid(gridSize.x, gridSize.y);
		RemoveHole(courseWidth);
		SetupConnections(gridSize.x/2, courseWidth);
		SetupStartFinishPairs(gridSize.x / 2, courseWidth);
		main = this;
	}
	
	/// <summary>
	/// removes a seg if possible
	/// </summary>
	/// <param name="seg"></param>
	/// <returns></returns>
	public bool RemoveSegment(CourseSegment seg)
	{
		if (seg.active)
		{
			seg.active = false;
			foreach (CourseSegment[] pair in startFinishPairs)
			{
				if (IsConnected(pair[0], pair[1]))
				{ 
					seg.Disappear();
					activeSegments.Remove(seg);
					//RemoveUnreachable();
					return true;
				}
			}
			seg.active = true;
			necessarySegments.Add(seg);
		}
		return false;
	}

	/// <summary>
	/// Removes unreachableSegments
	/// </summary>
	public void RemoveUnreachable()
	{
		foreach(CourseSegment seg in activeSegments)
		{
			seg.reachable = false;
		}
		foreach (CourseSegment[] pair in startFinishPairs)
		{
			if (IsConnected(pair[0], pair[1]))
			{
				List<CourseSegment> checkedSegments = new List<CourseSegment>();
				List<CourseSegment> exploredSegments = new List<CourseSegment>();
				exploredSegments.Add(pair[0]);
				while (exploredSegments.Count > 0)
				{
					CourseSegment[] sx = exploredSegments.ToArray();
					for (int i = 0; i < sx.Length; i++)
					{
						checkedSegments.Add(sx[i]);
						sx[i].reachable = true;
						exploredSegments.Remove(sx[i]);
						for (int j = 0; j < sx[i].neighbors.Count; j++)
						{
							if (sx[i].neighbors[j].active && !checkedSegments.Contains(sx[i].neighbors[j]) && !exploredSegments.Contains(sx[i].neighbors[j]))
							{
								exploredSegments.Add(sx[i].neighbors[j]);
							}
						}
					}
				}
				break;
			}
		}
		CourseSegment[] sa = activeSegments.ToArray();
		for(int i = 0; i < sa.Length; i++)
		{
			if (!sa[i].reachable)
			{
				sa[i].active = false;
				sa[i].Disappear();
				activeSegments.Remove(sa[i]);
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
		segments = new CourseSegment[sizeX][];
		for (int x = 0; x<sizeX; x++)
		{
			segments[x] = new CourseSegment[sizeZ];
			for(int z = 0; z<sizeZ; z++)
			{
				segments[x][z] = Instantiate(segmentPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
				segments[x][z].active = true;
				activeSegments.Add(segments[x][z]);
			}
		}
		transform.position = new Vector3(-courseWidth / 2, 0, -gridSize.y / 2);
	}

	/// <summary>
	/// Removes a hole in the center of the Course depending on the Width
	/// </summary>
	/// <param name="courseWidth"></param>
	void RemoveHole(int courseWidth)
	{
		for (int x = courseWidth; x < segments.Length-courseWidth; x++)
		{
			for (int z = courseWidth; z < segments[x].Length-courseWidth; z++)
			{
				activeSegments.Remove(segments[x][z]);
				segments[x][z].DisappearInstant();
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
		for (int x = 0; x < segments.Length; x++)
		{
			for (int z = 0; z < segments[x].Length; z++)
			{
				if (segments[x][z].active)
				{
					if (x - 1 >= 0)
					{
						segments[x][z].AddNeighbor(segments[x - 1][z]);
					}
					if (x + 1 < segments.Length)
					{
						segments[x][z].AddNeighbor(segments[x + 1][z]);
					}
					if (z - 1 >= 0 && (z != startLinePosZ || x >= startLineLengthX))
					{
						segments[x][z].AddNeighbor(segments[x][z - 1]);
					}
					if (z + 1 < segments[x].Length && (z != startLinePosZ - 1 || x >= startLineLengthX))
					{
						segments[x][z].AddNeighbor(segments[x][z + 1]);
					}
					segments[x][z].DrawLines();
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
		startFinishPairs = new CourseSegment[startLineLengthX][];
		for(int i=0; i<startLineLengthX; i++)
		{
			startFinishPairs[i] = new CourseSegment[] {segments[i][startLinePosZ], segments[i][startLinePosZ-1]};
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
		if (input >= gridSize.x / 2)
		{
			input = gridSize.x / 2 - 1;
		}
		if (input >= gridSize.y / 2)
		{
			input = gridSize.y / 2 - 1;
		}
		return input;
	}

	bool IsConnected(CourseSegment start, CourseSegment finish)
	{
		List<CourseSegment> checkedSegments = new List<CourseSegment>();
		List<CourseSegment> exploredSegments = new List<CourseSegment>();
		if (start.active)
		{
			exploredSegments.Add(start);
		}
		while (exploredSegments.Count > 0)
		{
			for (int i = 0; i<exploredSegments.Count; i++)
			{
				for (int j = 0; j<exploredSegments[i].neighbors.Count; j++)
				{
					if (exploredSegments[i].neighbors[j].active && !checkedSegments.Contains(exploredSegments[i].neighbors[j]) && !exploredSegments.Contains(exploredSegments[i].neighbors[j]))
					{
						if (ReferenceEquals(exploredSegments[i].neighbors[j], finish))
						{
							return true;
						}
						else
						{
							exploredSegments.Add(exploredSegments[i].neighbors[j]);
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
