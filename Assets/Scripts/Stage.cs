using UnityEngine;

[ExecuteAlways]
public class Stage : MonoBehaviour
{
	[SerializeField] Camera cam;
	[SerializeField] Transform rightWall;
	[SerializeField] Transform leftWall;
	[SerializeField] Transform topWall;
	[SerializeField] Transform bottomWall;

	[SerializeField] float marginRight = 0;
	[SerializeField] float marginLeft = 0;
	[SerializeField] float marginTop = 5;
	[SerializeField] float marginBottom = 0;
	[SerializeField] float wallThickness = 1f;

	[SerializeField] AnimationCurve wallEffectByDistance;
	[SerializeField] float wallEffectMaxDistance;
	[SerializeField] float wallMaxEffect;

	void OnValidate()
	{
		if (cam == null)
			cam = Camera.main;

		UpdateWalls();
	}

	Vector2Int lastResolution;

	void Awake()
	{
		UpdateWalls();
	}

	Rect stageRect;
	public Rect StageRect => stageRect;

	void Update()
	{
		Vector2Int resolution = new(Screen.width, Screen.height);
		if (resolution != lastResolution)
			UpdateWalls();
	}

	void UpdateWalls()
	{
		if (!cam || !rightWall || !leftWall || !topWall || !bottomWall) return;

		Vector2 cameraPos = cam.transform.position;
		float y = cameraPos.y - cam.orthographicSize + marginBottom;
		float x = cameraPos.x - cam.orthographicSize * cam.aspect + marginLeft;
		float width = cam.orthographicSize * cam.aspect * 2 - marginRight - marginLeft;
		float height = cam.orthographicSize * 2 - marginTop - marginBottom;
		stageRect = new Rect(x, y, width, height);

		lastResolution = new(Screen.width, Screen.height); 

		rightWall.position = new Vector3(stageRect.xMax + wallThickness / 2f, stageRect.center.y, 0f);
		leftWall.position = new Vector3(stageRect.xMin - wallThickness / 2f, stageRect.center.y, 0f);
		topWall.position = new Vector3(stageRect.center.x, stageRect.yMax + wallThickness / 2f, 0f);
		bottomWall.position = new Vector3(stageRect.center.x, stageRect.yMin - wallThickness / 2f, 0f);

		rightWall.localScale = new Vector3(wallThickness, (wallThickness + stageRect.height / 2f) * 2 , 1f);
		leftWall.localScale = new Vector3(wallThickness, (wallThickness + stageRect.height / 2f) * 2, 1f);
		topWall.localScale = new Vector3((wallThickness + stageRect.width / 2f) * 2, wallThickness, 1f);
		bottomWall.localScale = new Vector3((wallThickness + stageRect.width / 2f) * 2, wallThickness, 1f);
	}


	public Vector2 GetWallAcceleration(Vector2 position)
	{
		float rightDistance = stageRect.xMax - position.x;
		float leftDistance = position.x - stageRect.xMin; 
		float topDistance = stageRect.yMax - position.y; 
		float bottomDistance = position.y - stageRect.yMin;


		Vector2 wallForceDirection = Vector2.zero;
		if (rightDistance < wallEffectMaxDistance)
			wallForceDirection += (1 - (rightDistance / wallEffectMaxDistance)) * Vector2.left;
		if (leftDistance < wallEffectMaxDistance)
			wallForceDirection += (1 - (leftDistance / wallEffectMaxDistance)) *  Vector2.right;
		if (topDistance < wallEffectMaxDistance)
			wallForceDirection += (1 - (topDistance / wallEffectMaxDistance)) * Vector2.down;
		if (bottomDistance < wallEffectMaxDistance)
			wallForceDirection += (1 - (bottomDistance / wallEffectMaxDistance)) * Vector2.up;


		if(wallForceDirection == Vector2.zero)
			return wallForceDirection;

		wallForceDirection.Normalize();
		float minDistance = Mathf.Min(rightDistance, leftDistance, topDistance, bottomDistance); 
		return wallForceDirection * (wallEffectByDistance.Evaluate(minDistance / wallEffectMaxDistance) * wallMaxEffect);
	}
}