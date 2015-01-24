using UnityEngine;

//example
[RequireComponent(typeof(PolyNavAgent))]
public class FollowPlayer : MonoBehaviour{

	public GameObject player;
	public float distanceFromPlayerGoal = 4.5f;
	public float distanceFromPlayerInterval = 1.0f;
	public float playerPositionDifferenceToRecalcuate = 2f;
	public Vector3 oldPlayerPosition;
	Transform playerTransform;

	public enum FOLLOW_MODE {CONSTANT, CLOSEONLOW, PROPORTIONAL};
	public FOLLOW_MODE followMode;

	public float followCloseness;
	public float followClosenessAttractedLevel;
	public float followClosenessMin = 0.0f;
	public float followClosenessMax = 1.0f;

	public float DrunkWalkAmount = 0.01f;
	public float DrunkWalkOffset = 0.0f;

	private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}

	void Start() {
		playerTransform = player.GetComponent<Transform>();
		followCloseness = 0.0f;
	}

	void Update() {
		SetDestination();
	}

	//Message from Agent
	void OnDestinationReached(){
		//do something here...
	}

	//Message from Agent
	void OnDestinationInvalid(){
		//do something here...
	}

	void SetDestination() {
		if ((oldPlayerPosition == null) || 
		    (Mathf.Abs(oldPlayerPosition.x - playerTransform.position.x) > playerPositionDifferenceToRecalcuate) || 
		    (Mathf.Abs(oldPlayerPosition.y - playerTransform.position.y) > playerPositionDifferenceToRecalcuate) ||
		    (Mathf.Abs(transform.position.y - playerTransform.position.y) > distanceFromPlayerGoal) ||
		    (Mathf.Abs(transform.position.x - playerTransform.position.x) > distanceFromPlayerGoal)) {

			Vector3 target = (playerTransform.position - transform.position).normalized * distanceFromPlayerInterval + transform.position;

			if (followMode == FOLLOW_MODE.CLOSEONLOW) {
				if ((followCloseness <= followClosenessAttractedLevel) || (followCloseness <= followClosenessAttractedLevel))
					target = (transform.position - playerTransform.position).normalized * (distanceFromPlayerInterval/2f) + transform.position;
			}
			else if (followMode == FOLLOW_MODE.PROPORTIONAL) {
				float healthPerc = followCloseness/(followClosenessMax - followClosenessMin);
				target = (transform.position - playerTransform.position).normalized * (distanceFromPlayerInterval * healthPerc) + transform.position;
			}

			target.x += DrunkWalkAmount * (Mathf.Cos(Time.time + DrunkWalkOffset) + Mathf.Sin(Time.time + DrunkWalkOffset));
			target.y += DrunkWalkAmount * (Mathf.Sin(Time.time + DrunkWalkOffset) + Mathf.Cos(Time.time + DrunkWalkOffset));

			Vector2 newPoint = new Vector2(target.x, target.y);
			agent.SetDestination(newPoint);

			oldPlayerPosition = playerTransform.position;
		}
	}
}
