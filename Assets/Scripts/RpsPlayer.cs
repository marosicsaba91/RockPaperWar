using UnityEngine;
using UnityEngine.UIElements;

public class RpsPlayer : MonoBehaviour
{
	[SerializeField] RpsHand hand;     // Kő / Papír / Olló

	// Referenciák saját komponensekre:
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] new Rigidbody2D rigidbody2D;

	// Referencia a Manager objektumra:
	RpsManager _manager;

	void Awake()
	{
		// Manager objektum keresése:
		_manager = FindObjectOfType<RpsManager>();
	}

	void OnValidate()
	{
		// Saját Referenciák megtalálása
		rigidbody2D = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public RpsHand Hand      // Property, a játékos típusár
	{
		get => hand;
		set
		{
			if (value == hand) return;

			spriteRenderer.sprite = _manager.GetSprite(value);
			hand = value;
			_manager.TestGameEnd();
		}
	}

	void Update()
	{
		// Haladás iránya:
		Vector2 weightedDirection = GetWeightedDirection();

		// Ha nem mozgunk:
		if (weightedDirection == Vector2.zero)
			rigidbody2D.velocity = Vector2.zero;
		else // Ha mozgunk:
		{
			// Sebesség:
			rigidbody2D.velocity = weightedDirection * _manager.speed;

			// Elfordulás:
			float angle = Vector2.SignedAngle(Vector2.up, weightedDirection);
			rigidbody2D.rotation = angle;
		}
	}

	// Visszaadja a haladás irányát
	Vector2 GetWeightedDirection()
	{
		Vector2 weightedDirection = Vector3.zero;
		Vector2 position = transform.position;
		foreach (RpsPlayer player in _manager.Players)
		{
			if (player == this)
				continue;

			Vector2 direction = (Vector2)player.transform.position - position;
			float weight = 1 / direction.sqrMagnitude;
			float multiplier = _manager.PlayRps(hand, player.hand);

			weightedDirection += direction.normalized * (weight * multiplier);
		}
		if (weightedDirection == Vector2.zero)
			return weightedDirection;

		return weightedDirection.normalized;
	}

	// Ütközés kezeláse:
	void OnCollisionEnter2D(Collision2D other)
	{
		HandleCollision(other);
	}

	void OnCollisionStay2D(Collision2D other)
	{
		HandleCollision(other);
	}

	void HandleCollision(Collision2D other)
	{
		if (!other.gameObject.TryGetComponent<RpsPlayer>(out RpsPlayer otherPlayer))
			return;

		// Ha egy másik player-rel találkoztunk:

		if (_manager.PlayRps(hand, otherPlayer.hand) > 0)  // Ha legyőztük a másikat,
			otherPlayer.Hand = hand;  //akkor átváltoztatjuk a típusát a sajátunkra
	}
}
