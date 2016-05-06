using UnityEngine;
using System.Collections;

public class PlayerMobility : MonoBehaviour {
  [SerializeField] private float speed;

	private string horizontalCtrl;
	private string verticalCtrl;

  public void Init(int playerId = 0) {
    if (playerId != 0) {
      horizontalCtrl = "Horizontal_P" + playerId;
      verticalCtrl = "Vertical_P" + playerId;
    }
  }

	protected void FixedUpdate() {
    float h = Input.GetAxis(horizontalCtrl);
    float v = Input.GetAxis(verticalCtrl);
    float t = 0.01f;

		if (h < -t) {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

		if (h > t) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}

		if (v > t) {
			transform.position += Vector3.up * speed * Time.deltaTime;
		}
		
    if (v < -t) {
			transform.position += Vector3.down * speed * Time.deltaTime;
		}
	}
}
