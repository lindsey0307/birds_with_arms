using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {
  private Vector3 initPos;
  private System.Random random;
  private float intensity = 0f;

  void Start() {
    initPos = this.transform.position;
    random = new System.Random();
  }

	void Update () {
    intensity += 0.002f;
    this.transform.position = initPos + new Vector3(
      ((float)random.NextDouble()-.5f) * intensity, 
      ((float)random.NextDouble()-.5f) * intensity,
      0);
	}
}
