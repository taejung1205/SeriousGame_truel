using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject gameStart, gameEnd;
	public GameObject modeSelect; // 모드 선택 화면
	public GameObject showRule; // 기본 규칙 설명 화면
	public GameObject showBasic; // 기본 모드 상황 설명
	public bool start, end = false;
	public bool isBasicMode = true;
	public AudioSource au;
	public GameObject player;

    void Start ()
	{
		//player.GetComponent<CharacterController>().enabled = false;
		start = false;
		modeSelect.SetActive(false);
		gameEnd.SetActive (false);	
		gameStart.SetActive (true);
		showRule.SetActive(false);
		showBasic.SetActive(false);
	}

    void Update()
	{
        if (end)
        {
			gameEnd.SetActive(true);
			//player.GetComponent<CharacterController>().enabled = false;
		}
    }


    public void OnButton(string btn)
	{
		switch (btn)
        {
			case ("start"):
				gameStart.SetActive(false);
				modeSelect.SetActive(true);
				au.Play();
				break;
			case ("basic"):
				isBasicMode = true;
				modeSelect.SetActive(false);
				showRule.SetActive(true);
				break;
			case ("basic rule next"):
				showRule.SetActive(false);
				showBasic.SetActive(true);
				break;
			case ("basic start"):
				showBasic.SetActive(false);
				start = true;
				break;
			case ("challenge"):
				isBasicMode = false;
				Debug.Log("challenge");
				modeSelect.SetActive(false);
				break;
			case ("restart"):
				StartCoroutine(GameOver());
				break;
		}
	}

	IEnumerator GameOver()
	{
		yield return new WaitForSeconds(0.5f);
		Application.LoadLevel(Application.loadedLevel);
		gameEnd.SetActive(false);
	}
}
