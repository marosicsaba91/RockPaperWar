using UnityEngine;

public class RpsActor : MonoBehaviour
{
	[SerializeField] RpsHand hand;     // Kő / Papír / Olló
	[SerializeField] ActorSetup setup;

	// Referenciák saját komponensekre:
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] new Rigidbody2D rigidbody2D;
	[SerializeField] Animator animator;

	// Referencia a Manager objektumra:
	RpsManager _manager;
	Stage _stage;

	Vector2 _velocity;
	Vector2 _acceleration;
	float _maxSpeed;

	void Awake()
	{
		// Manager objektum keresése:
		_manager = FindObjectOfType<RpsManager>();
		_stage = FindObjectOfType<Stage>();
		ChangeSprite();
		_maxSpeed = setup.MaxSpeed;
	}

	void OnValidate()
	{
		// Saját Referenciák megtalálása
		rigidbody2D = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	public RpsHand Hand      // Property, a játékos típusár
	{
		get => hand;
		set
		{
			if (value == hand) return;

			hand = value;
			_manager.TestGameEnd();
			animator.SetTrigger("Change");
		}
	}

	void Update()
	{
		rigidbody2D.simulated = _manager.IsGameOn;
		if (!_manager.IsGameOn) return;

		// Gyorsulás:
		_acceleration = setup.CalculateFullAcceleration(this, _manager, _stage);
		_velocity += _acceleration * Time.deltaTime;

		// Sebesség korlátozása:

		if (_velocity.magnitude > _maxSpeed)
			_velocity = _velocity.normalized * _maxSpeed;

		// Sebesség beállítása:
		rigidbody2D.velocity = _velocity;

		// Elfordulás:
		if (_velocity != Vector2.zero)
		{
			float targetAngle = Vector2.SignedAngle(Vector2.up, _velocity);
			float maxTurn = setup.AngularSpeed * Time.unscaledDeltaTime;
			rigidbody2D.rotation =Mathf.MoveTowardsAngle(rigidbody2D.rotation, targetAngle, maxTurn);
		}
	}

	// Ütközés kezelése:
	void OnCollisionEnter2D(Collision2D other) => HandleCollision(other);

	void OnCollisionStay2D(Collision2D other) => HandleCollision(other);

	void HandleCollision(Collision2D other)
	{
		if (!_manager.IsGameOn) return;
		if (!other.gameObject.TryGetComponent(out RpsActor otherPlayer))
			return;

		// Ha egy másik player-rel találkoztunk:

		if (RpsManager.GetRelation(hand, otherPlayer.hand) == RpsRelation.Pray)  // Ha legyőztük a másikat,
			otherPlayer.Hand = hand;  //akkor átváltoztatjuk a típusát a sajátunkra
	}

	void ChangeSprite()  // ANIM EVENT
	{
		spriteRenderer.sprite = _manager.GetSprite(hand);
	}

	void OnDrawGizmos()
	{
		Vector2 position = transform.position;
		Gizmos.color = Color.red;

		Gizmos.DrawLine(position, position + _acceleration/10f);
	}
}
