using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickExit : MonoBehaviour
{
	public RectTransform crosshairImage;
	public RectTransform mainPanel;
	bool pause = false;

	// Update is called once per frame
	void Update ()
	{
		if (pause) {
			Time.timeScale = 0;
		} else
			Time.timeScale = 1;

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if(Application.loadedLevel == 2){
				crosshairImage.gameObject.SetActive (!crosshairImage.gameObject.activeInHierarchy);
				mainPanel.gameObject.SetActive (!mainPanel.gameObject.activeInHierarchy);
				pause = !pause;
				Cursor.visible = !Cursor.visible;	
			}else{
				pause = !pause;
				Cursor.visible = !Cursor.visible;
			}
		}
	}

	public void CloseGame() {
		Application.Quit();
	}

}
