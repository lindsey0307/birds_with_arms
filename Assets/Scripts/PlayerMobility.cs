using UnityEngine;
using System.Collections;

public class PlayerMobility : MonoBehaviour {

	public float speed;
	public string horizontalCtrl = "Horizontal_P1";
	public string verticalCtrl = "Vertical_P1";
	public float moveForce;
	public float maxSpeed;


	// mouse controlled weirdness that works ok
//	void FixedUpdate() {
//		var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//		Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
//
//		transform.rotation = rot;
//		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
//		GetComponent<Rigidbody2D>().angularVelocity = 0;
//
//		float input = Input.GetAxis("Vertical");
//		GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * speed * input);
//	}

//	// simple keyboard, 2 player doesn't work bcuz fuck society
//	void Update() {
//		if (Input.GetKey(KeyCode.LeftArrow)) {
//			transform.position += Vector3.left * speed * Time.deltaTime;
//		}
//		if (Input.GetKey(KeyCode.RightArrow)) {
//			transform.position += Vector3.right * speed * Time.deltaTime;
//		}
//
//		if (Input.GetKey(KeyCode.UpArrow)) {
//			transform.position += Vector3.up * speed * Time.deltaTime;
//		}
//		if (Input.GetKey(KeyCode.DownArrow)) {
//			transform.position += Vector3.down * speed * Time.deltaTime;
//		}
//	}
		
	// weird ass physics based code holy shit what the fuck
	void FixedUpdate() {
		float h = Input.GetAxis(horizontalCtrl);
		float v = Input.GetAxis(verticalCtrl);


		if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

		if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed,
				GetComponent<Rigidbody2D>().velocity.y);

		if (v * GetComponent<Rigidbody2D>().velocity.y < maxSpeed)
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * v * moveForce);
		

		if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeed)
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.y) * maxSpeed,
				GetComponent<Rigidbody2D>().velocity.y);
	}



}
