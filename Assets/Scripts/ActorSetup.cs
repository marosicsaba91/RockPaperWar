using UnityEngine;

[CreateAssetMenu(fileName = "ActorSetup", menuName = "ActorSetup")]
public class ActorSetup : ScriptableObject
{
	// Játékosok sebessége 
	[SerializeField] float maxSpeed1 = 2;
	[SerializeField] float maxSpeed2 = 2;
	[SerializeField] float maxActorAcceleration = 20;
	[SerializeField] float angularSpeed = 360f;


	[Space] // Többi játékos hatása
	[SerializeField] float predatorEffectMaxDistance = 10f;
	[SerializeField] float predatorMaxEffect = 1;

	[SerializeField] float prayEffectMaxDistance = 10f;
	[SerializeField] float prayMaxEffect = -1;

	[SerializeField] float neutralEffectMaxDistance = 1.5f;
	[SerializeField] float neutralMaxEffect = 0.3f;

	public float MaxSpeed => Random.Range(maxSpeed1, maxSpeed2);
	public float MaxAcceleration => maxActorAcceleration;
	public float AngularSpeed => angularSpeed;

	public Vector2 CalculateFullAcceleration(RpsActor actor, RpsManager rpsManager, Stage stage)
	{
		Vector2 acceleration = Vector3.zero;
		Vector2 position = actor.transform.position;
		RpsHand hand = actor.Hand;

		foreach (RpsActor other in rpsManager.Players)
			acceleration += CalculateOtherActorAcceleration(position, hand, other);


		if (acceleration.magnitude > maxActorAcceleration)
			acceleration = acceleration.normalized * maxActorAcceleration;

		Vector2 wallEffect = stage.GetWallAcceleration(position);
		acceleration += wallEffect;




		return acceleration;
	}


	Vector2 CalculateOtherActorAcceleration(Vector2 actorPosition, RpsHand actorHand, RpsActor other)
	{
		Vector2 otherPosition = other.transform.position;
		if (otherPosition == actorPosition)
			return Vector2.zero;

		RpsRelation relation = RpsManager.GetRelation(actorHand, other.Hand);

		Vector2 effect;

		if (relation == RpsRelation.Pray)
			effect = CalculateOtherActorAcceleration(actorPosition, otherPosition, prayEffectMaxDistance, prayMaxEffect);
		else if (relation == RpsRelation.Predator)
			effect = CalculateOtherActorAcceleration(actorPosition, otherPosition, predatorEffectMaxDistance, predatorMaxEffect);
		else 
			effect = CalculateOtherActorAcceleration(actorPosition, otherPosition, neutralEffectMaxDistance, neutralMaxEffect);

		return effect;
	}

	Vector2 CalculateOtherActorAcceleration(Vector2 actorPosition, Vector2 otherPosition, float maxDistance, float maxEffect) 
	{ 
		Vector2 distanceVector = otherPosition - actorPosition;
		float distance = distanceVector.magnitude; 

		if (distance > maxDistance)
			return Vector2.zero;

		Vector2 normalised = distanceVector / distance;
		float multiplier = 1 - (distance / maxDistance);

		return maxEffect * multiplier * normalised;
	}
}
