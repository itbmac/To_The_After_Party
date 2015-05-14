using UnityEngine;
using System.Collections;

public class PaintBall : MonoBehaviour {

	float startTime;
	bool released;
	Paintbrush brush;
	public GameObject BlueSplat;
	public GameObject RedSplat;

	// Use this for initialization
	void Start () {
		
	}
	
	float fullSize = .2f;
	float power = 0;
	
	// Update is called once per frame
	void Update () {
		if (brush == null)
			return;
			
		if (GameManager.Instance.CurrentState != GameManager.GameState.Running) {
			Destroy(gameObject);
			return;
		}
			
		if (Time.time - startTime > 15.0f)
			Destroy(gameObject);
		
		if (initialized && !released) {
			power = Mathf.Min (1, (Time.time - startTime) / GameManager.Instance.BlobChargeTime);
			transform.localScale = Vector3.one * fullSize * power;
			
			if (GameManager.Instance.BlobsUseGestures)
				transform.position = brush.arm.transform.position + brush.arm.transform.forward * .5f;
			else {
				Vector3 offset = brush.arm.transform.up * -1 * .25f 
					+ brush.arm.transform.forward * .5f;
				transform.position = brush.arm.transform.position 
					+ offset;
					
				transform.rotation = Quaternion.LookRotation(offset);
			}
		}	
	}
	
	bool initialized;
	void Initialize(Paintbrush brush) {
		initialized = true;
		this.brush = brush;
		startTime = Time.time;
		this.GetComponent<Renderer>().material.color = brush.MyColor;
	}
	
	void Release() {
		released = true;
		GetComponent<Rigidbody>().isKinematic = false;
		Bounds b = Painting.Instance.GetComponent<Collider>().bounds;
		
		Vector3 target = new Vector3(
			Random.Range(b.min.x, b.max.x),
			Random.Range(b.min.y, b.max.y),
			Random.Range(b.min.z, b.max.z)
		);		
		
		Vector3 direction = target - transform.position;
		direction.Normalize();
		transform.rotation = Quaternion.LookRotation(direction);
		
		GetComponent<Rigidbody>().velocity = direction * 2;
	}
	
	void OnCollisionEnter(Collision collision) {
		if (!released)
			return;
		
		Painting p = collision.collider.GetComponent<Painting>();
		Color32 a = brush.MyColor;
		Color b = Painting.Instance.OurRed;
		if (a == b)
			Instantiate(RedSplat, transform.position, Quaternion.identity);
		else
			Instantiate(BlueSplat, transform.position, Quaternion.identity);
			
		if (p != null)
			p.PaintBallHit(transform.position, collision.contacts[0].normal, brush.MyColor, power);
	
		Destroy(gameObject);
	}
}
