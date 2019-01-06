using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CourseBehaviour : MonoBehaviour {

	public static CourseBehaviour Instance;

	[SerializeField]
	CourseSegment SegmentPrefab;
	[SerializeField]
	public Vector2Int GridSize;
    [SerializeField]
    private int CoursePadding;

	private CourseSegment[][] Grid;
    public List<CourseSegment> Segments { get; private set; } = new List<CourseSegment>();

    [SerializeField]
    private int BalloonCount;
    [SerializeField]
    private float BalloonHeight;
    [SerializeField]
    private float BalloonRange;
    [SerializeField]
    private float BalloonMaxDistance;
    [SerializeField]
    private Balloon BalloonPrefab;
    private List<Balloon> Balloons = new List<Balloon>();

    Vector3 TopLeftCorner;
    Vector3 BottomRightCorner;

    private void Start () {
        Instance = this;
		SetupGrid(GridSize.x, GridSize.y);
        SetupCorners();
        SpawnBalloons(BalloonCount);
	}

    public void RemoveRandomSegment() {
        Segments
            .Where(segment => segment.Active)
            .RandomElement()
            .Disappear();
    }

	void SetupGrid(int sizeX, int sizeZ)
	{
		//Setups the base Grid
		Grid = new CourseSegment[sizeX][];
        CourseSegment topLeft = null;
        CourseSegment bottomRight = null;

        for (int x = 0; x<sizeX; x++)
		{
			Grid[x] = new CourseSegment[sizeZ];
			for(int z = 0; z<sizeZ; z++)
			{
                var segment = Instantiate(SegmentPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                Segments.Add(segment);
                Grid[x][z] = segment;

                var xOkay = x >= CoursePadding && x < sizeX - CoursePadding;
                var zOkay = z >= CoursePadding && z < sizeZ - CoursePadding;

                if (xOkay && zOkay) {
                    segment.ReappearInstant();
                } else {
                    segment.DisappearInstant();
                }
                if (x == 0 && z == 0) {
                    topLeft = segment;
                } else {
                    bottomRight = segment;
                }
            }
		}

        transform.Translate(new Vector3(-sizeX / 2 + 0.5f, 0, -sizeZ / 2 + 0.5f));
    }

    void SetupCorners() {
        var deathZoneMesh = FindObjectOfType<DeathZone>().GetComponent<Collider>();

        TopLeftCorner = deathZoneMesh.bounds.min;
        BottomRightCorner = deathZoneMesh.bounds.max;
    }

    void SpawnBalloons(int count) {
        for (int i = 0; i < count; i++) {
            SpawnBalloon();
        }
    }

    public void SpawnBalloon() {
        Vector3 position;
        do {
            float x = Random.Range(TopLeftCorner.x, BottomRightCorner.x);
            float y = BalloonHeight;
            float z = Random.Range(TopLeftCorner.z, BottomRightCorner.z);
            position = new Vector3(x, y, z);
        } while (
            Physics.OverlapSphere(position, BalloonMaxDistance)
                .SelectMany(collider => collider.GetComponents<CourseSegment>())
                .Where(segment => segment.Active)
                .Any() == false
        );
        
        Instantiate(BalloonPrefab, position, Quaternion.identity, transform);
    }

    public void RespawnSegments(Vector3 position, Color color, int range) {
        Grid.SelectMany(segments => segments)
            .Where(segment => segment.Active == false)
            .Where(segment => Vector3.Distance(position, segment.transform.position) < (BalloonRange + range))
            .ForAll((segment) => {
                segment.SetColor(color);
                segment.ReappearInstant();
            });
    }
}
