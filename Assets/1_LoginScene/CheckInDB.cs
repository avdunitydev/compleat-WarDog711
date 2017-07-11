﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckInDB : MonoBehaviour
{
	public Button btnSIN;
	public Button btnSUP;
	public RectTransform alert;
	public Text textAlert;
	public Text consoleText;

	public enum IsValidate
	{
		login = 1,
		email = 1 << 1,
		pass = 1 << 2,
		rePass = 1 << 3
	}

	public static IsValidate isFieldOk;

	// Show Alert dialog
	void ShowAlert (RectTransform alertPanel, Text targetText, string alertText)
	{
		targetText.text = alertText;
		alertPanel.gameObject.SetActive (true);
	}
	
	// Validate fields
	bool ValidateField (InputField targetField)
	{
		if (Equals (targetField.name, "IF_Name")) {
			if (targetField.text.Length < 4) {
				ShowAlert (alert, textAlert, "Логін повинен містити не менше 4 символів");
				return false;
			} else {
				return true;
			}
		}
		if (Equals (targetField.name, "IF_Email")) {
			if (targetField.text.Length < 7 && !targetField.text.Contains ("@")) {
				ShowAlert (alert, textAlert, "Перевірте правильність написання пошти");
				return false;
			} else {
				return true;
			}
		}
		if (Equals (targetField.name, "IF_Pass") || Equals (targetField.name, "IF_PassUP")) {
			if (targetField.text.Length < 6) {
				ShowAlert (alert, textAlert, "Пароль повинен містити не менше 6 символів");
				return false;
			} else {
				return true;
			}
		}
		return false;
	}

	// Validate pass fields
	bool ValidateField (InputField passField, InputField rePassField)
	{
		if (!Equals (passField.text, rePassField.text)) {
			ShowAlert (alert, textAlert, "Повторний пароль не збігається.\nБудьте УВАЖНІ !!!\nПовторіть спробу.");
			return false;
		} else {
			return true;
		}
		return false;
	}

	// Check name, email and interactable button
	public void SINPanelActivBtn (Button targetButton)
	{
		if (isFieldOk == (IsValidate.email | IsValidate.login)) {
			targetButton.interactable = true;
		} else {
			targetButton.interactable = false;
		}
	}
	// Check name, email, pass, re_pass and interactable button
	public void SUPPanelActivBtn (Button targetButton)
	{
		if (isFieldOk == (IsValidate.email | IsValidate.login | IsValidate.pass | IsValidate.rePass)) {
			targetButton.interactable = true;
		} else {
			targetButton.interactable = false;
		}
	}

	// Check name in DB
	public void EndEditName (InputField targetField)
	{
		if (ValidateField (targetField) && Equals (targetField.tag, "fieldSUP")) {
			StartCoroutine (FindNameUP (targetField.text));
		}
		if (ValidateField (targetField) && Equals (targetField.tag, "fieldSIN")) {
			StartCoroutine (FindNameIN (targetField.text));
		}
	}

	IEnumerator FindNameIN (string nameSIN)
	{
		consoleText.text = "start find user name ...";
		WWWForm form = new WWWForm ();
		form.AddField ("name", nameSIN);
		WWW www = new WWW ("http://www.local/php_wd/findName.php", form);
		yield return www;
		if (!string.IsNullOrEmpty (www.error)) {
			consoleText.text = "Error: conect (name) ! ---------------------------------" + www.error;
			yield break;
		}
		consoleText.text = "... result >> " + www.text;
		if (www.text.Length == 1) {
			if (Equals (www.text, "1")) {
				consoleText.text = "User name OK !";
				isFieldOk |= IsValidate.login;
			} else if (Equals (www.text, "0")) {
				ShowAlert (alert, textAlert, "Логін відсутній !\nЗареєструйтесь !\nАбо перевірте написання !");
				isFieldOk &= ~IsValidate.login;
			}
			SINPanelActivBtn (btnSIN);
		}
		yield return null;
	}

	IEnumerator FindNameUP (string nameSUP)
	{
		consoleText.text = "start find user name ...";
		WWWForm form = new WWWForm ();
		form.AddField ("name", nameSUP);
		WWW www = new WWW ("http://www.local/php_wd/findName.php", form);
		yield return www;
		if (!string.IsNullOrEmpty (www.error)) {
			consoleText.text = "Error: name !---------------------------------" + www.error;
			yield break;
		}
		consoleText.text = "... result >> " + www.text;
		if (www.text.Length == 1) {
			if (Equals (www.text, "1")) {
				consoleText.text = "Такий логін зайнятий виберіть інший";
				isFieldOk &= ~IsValidate.login;
			} else if (Equals (www.text, "0")) {
				consoleText.text = "User name OK ! ";
				isFieldOk |= IsValidate.login;
			}
			SUPPanelActivBtn (btnSUP);
		}
		yield return null;
	}

	// Check email in DB
	public void EndEditEmail (InputField targetField)
	{        
		if (ValidateField (targetField) && Equals (targetField.tag, "fieldSUP")) {
			StartCoroutine (FindEmailUP (targetField.text));
		}
		if (ValidateField (targetField) && Equals (targetField.tag, "fieldSIN")) {
			StartCoroutine (FindEmailIN (targetField.text));
		}
	}

	IEnumerator FindEmailIN (string emailSIN)
	{
		consoleText.text = "start find user email ...";
		WWWForm form = new WWWForm ();
		form.AddField ("email", emailSIN);
		WWW www = new WWW ("http://www.local/php_wd/findEmail.php", form);
		yield return www;
		if (!string.IsNullOrEmpty (www.error)) {
			consoleText.text = "Error: conect (email) ! ---------------------------------" + www.error;
			yield break;
		}
		consoleText.text = "... result >> " + www.text;
		if (www.text.Length == 1) {
			if (Equals (www.text, "1")) {
				consoleText.text = "User email OK !";
				isFieldOk |= IsValidate.email;
			} else if (Equals (www.text, "0")) {
				ShowAlert (alert, textAlert, "Пошта відсутня !\nЗареєструйтесь !\nАбо перевірте написання !");
				isFieldOk &= ~IsValidate.email;
			}
			SINPanelActivBtn (btnSIN);
		}
		yield return null;
	}

	IEnumerator FindEmailUP (string emailSUP)
	{
		consoleText.text = "start find user email ...";
		WWWForm form = new WWWForm ();
		form.AddField ("email", emailSUP);
		WWW www = new WWW ("http://www.local/php_wd/findEmail.php", form);
		yield return www;
		if (!string.IsNullOrEmpty (www.error)) {
			consoleText.text = "Error: email !" + www.error;
			yield break;
		}
		consoleText.text = "... result >> " + www.text;
		if (www.text.Length == 1) {
			if (Equals (www.text, "1")) {
				consoleText.text = "Така пошта використовуєьться, виберіть іншу";
				isFieldOk &= ~IsValidate.email;
			} else if (Equals (www.text, "0")) {
				consoleText.text = "User email OK !";
				isFieldOk |= IsValidate.email;
			}
			SUPPanelActivBtn (btnSUP);
		}
		yield return null;
	}
	//Check pass
	public void EndEditPass (InputField targetField)
	{        
		if (ValidateField (targetField) && Equals (targetField.tag, "fieldSIN")) {
			consoleText.text = "pass Ok !";
			SINPanelActivBtn (btnSIN);
		} else
			isFieldOk &= ~IsValidate.pass;
		if (ValidateField (targetField) && Equals (targetField.tag, "fieldSUP")) {
			isFieldOk |= IsValidate.pass;
			SUPPanelActivBtn (btnSUP);
		} else
			isFieldOk &= ~IsValidate.pass;
	}
	//Confirm password
	public void EndEditRePass ()
	{
		InputField pass = GameObject.Find ("IF_PassUP").GetComponent<InputField> ();
		InputField rePass = GameObject.Find ("IF_RePassUP").GetComponent<InputField> ();

		if (ValidateField (pass, rePass)) {
			isFieldOk |= IsValidate.rePass;
		}
		SUPPanelActivBtn (btnSUP);
	}
}
