using UnityEngine;
using System.Collections;

public class Drag_Mechanic : MonoBehaviour {

	public GameObject DraggedFriendObject;
	private LineRenderer lineRenderer;
	private SpringJoint2D springJoint;

	public string PartierTag = "Partier";
	public KeyCode DragReleaseKey = KeyCode.E;
	public float MaxDistanceToGrab = 4.0f;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.sortingOrder = 9;

		springJoint = GetComponent<SpringJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (DraggedFriendObject != null) {
			springJoint.connectedBody = DraggedFriendObject.GetComponent<Rigidbody2D>();

			lineRenderer.SetPosition(0, GetComponent<Rigidbody2D>().transform.position);
			lineRenderer.SetPosition(1, DraggedFriendObject.transform.position);
			lineRenderer.enabled = true;
			springJoint.enabled = true;

			if (Input.GetKeyDown(DragReleaseKey)) {
				DraggedFriendObject = null;
			}
		}
		else {
			lineRenderer.enabled = false;
			springJoint.enabled = false;

			if (Input.GetKeyDown(DragReleaseKey)) {
				GameObject[] partiers = GameObject.FindGameObjectsWithTag(PartierTag);
				foreach(GameObject partier in partiers) {
					if (CheckIfObjectInRange(partier)) {
						DraggedFriendObject = partier;
						break;
					}
				}
			}
		}
	}

	bool CheckIfObjectInRange(GameObject target) {
		Vector3 distBetween = this.GetComponent<Rigidbody2D>().transform.position - target.GetComponent<Rigidbody2D>().transform.position;
		
		if (distBetween.magnitude < MaxDistanceToGrab) {
			
			RaycastHit2D hitInfo = Physics2D.Linecast(new Vector2(this.transform.position.x, this.transform.position.y), 
			                                          new Vector2(target.transform.position.x, target.transform.position.y), 
			                                          LayerMask.GetMask("Wall", "UserLayer8"));
			
			if (hitInfo.collider == null)
			{	
				return true;
			}
		}
		
		return false;
	}
}
