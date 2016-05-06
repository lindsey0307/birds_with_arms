using UnityEngine;
using System.Collections;
using System;

public class PlayerMobility : MonoBehaviour {
  [SerializeField] private float speed;

	private string horizontalCtrl;
	private string verticalCtrl;
  private bool isAi;

  // stoopid ai birb movement tingz
  private int xMove = 0;
  private int yMove = 0;
  private DateTime nextDirectionSwtichTime;
  private System.Random random;

  public void Init(int playerId, System.Random random) {
    this.random = random;
    isAi = playerId == 0;
    if (isAi) {
      nextDirectionSwtichTime = DateTime.Now;
    } else {
      horizontalCtrl = "Horizontal_P" + playerId;
      verticalCtrl = "Vertical_P" + playerId;
    }
  }

	protected void FixedUpdate() {
    if (isAi) {
      DoAiMovement();
    } else {
      DoPlayerMovement();
    }
	}

  private void DoAiMovement() {
    if (DateTime.Now >= nextDirectionSwtichTime) {
      xMove = random.Next(-1, 2);
      yMove = random.Next(-1, 2);
      nextDirectionSwtichTime = DateTime.Now.AddSeconds(5);
    }

    transform.position += Vector3.right * xMove + Vector3.up * yMove;
  }

  private void DoPlayerMovement() {
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
