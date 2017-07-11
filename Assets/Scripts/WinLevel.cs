using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevel : MonoBehaviour {

	public RectTransform winPanel;
	bool isWin = false;

	void OnTriggerEnter(Collider other)
	{
		if (Equals(other.tag, "Player")){
			winPanel.gameObject.SetActive(!winPanel.gameObject.activeInHierarchy);
			isWin = !isWin;
		}
	}

	void /// <summary>
	/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	OnTriggerExit(Collider other)
	{
		if (Equals(other.tag, "Player")){
			winPanel.gameObject.SetActive(!winPanel.gameObject.activeInHierarchy);
			isWin = !isWin;
		}
	}

	void Update() {
		if(isWin && Input.GetKeyDown(KeyCode.Return)){
			ExitLevelWin();
		}
	}

	void ExitLevelWin() {
		SceneManager.LoadScene (1);
	}
}
