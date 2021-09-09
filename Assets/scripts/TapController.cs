using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

	public delegate void PlayerDelegate();
	public static event PlayerDelegate OnPlayerDied;
	public static event PlayerDelegate OnPlayerScored;

	public float tapForce = 10;
	public float tiltSmooth = 5;
	public Vector3 startPos;
	public AudioSource tapSound;
	public AudioSource scoreSound;
	public AudioSource dieSound;

	Rigidbody2D rigidBody;
	Quaternion downRotation;
	Quaternion forwardRotation;

	GameManager game;
	TrailRenderer trail;

	void Start() {
		rigidBody = GetComponent<Rigidbody2D>();
		downRotation = Quaternion.Euler(0, 0 ,-100);
		forwardRotation = Quaternion.Euler(0, 0, 40);
		game = GameManager.Instance;
		rigidBody.simulated = false;
		//trail = GetComponent<TrailRenderer>();
		//trail.sortingOrder = 20; 
	}

	void OnEnable() {
		GameManager.OnGameStarted += OnGameStarted;
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	void OnDisable() {
		GameManager.OnGameStarted -= OnGameStarted;
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	void OnGameStarted() {
		rigidBody.velocity = Vector3.zero;
		rigidBody.simulated = true;
	}

	void OnGameOverConfirmed() {
		transform.localPosition = startPos;
		transform.rotation = Quaternion.identity;
		showAd();
	}

	void showAd() {
		#if UNITY_ANDROID
		AndroidJavaClass InterstitialPlacementClass = new AndroidJavaClass("com.adpumb.ads.display.InterstitialPlacement");
		AndroidJavaClass DisplayManagerClass = new AndroidJavaClass("com.adpumb.ads.display.DisplayManager");
		AndroidJavaObject InterstitialPlacementObject =  new AndroidJavaObject("com.adpumb.ads.display.InterstitialPlacementBuilder")
											.Call<AndroidJavaObject>("name", new object[] { "unity"} )
											.Call<AndroidJavaObject>("showLoaderTillAdIsReady", new object[] { true } )
											// .Call<AndroidJavaObject>("loaderTimeOutInSeconds", new object[] { 5 } )
											// .Call<AndroidJavaObject>("frequencyCapInSeconds", new object[] { 1 } )
											.Call<AndroidJavaObject>("build");
		AndroidJavaObject DisplayManagerObject = DisplayManagerClass.CallStatic<AndroidJavaObject>("getInstance");
		DisplayManagerObject.Call("showAd",InterstitialPlacementObject);
		#endif
	}

	void Update() {
		if (game.GameOver) return;

		if (Input.GetMouseButtonDown(0)) {
			rigidBody.velocity = Vector2.zero;
			transform.rotation = forwardRotation;
			rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
			tapSound.Play();
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "ScoreZone") {
			OnPlayerScored();
			scoreSound.Play();
		}
		if (col.gameObject.tag == "DeadZone") {
			rigidBody.simulated = false;
			OnPlayerDied();
			dieSound.Play();
		}
	}

}
