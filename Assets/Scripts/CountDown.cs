using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
	public GameObject controller;
	public Text instructions;

	public Text countDown;
	public float timeLeft = 60f;
	public bool onBlack = false;

	private GameObject something;
	public GameObject prefab;

	private void Start()
	{
		countDown.text = "";
		instructions.text = "";
	}

	public void Update()
	{
		if (controller.GetComponent<GameController>().start)
		{
			countDown.text = "남은 시간: " + Mathf.Round(timeLeft);

			if (!info)
			{
				StartCoroutine(Instructions());
				info = true;
			}

			if (!onBlack)
			{
				timeLeft -= Time.deltaTime;
			}

			if (timeLeft <= 1)
			{
				countDown.text = "";
				controller.GetComponent<GameController>().start = true;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "black")
		{
			onBlack = true;
			something = Instantiate(prefab, new Vector3(0f, 4f, -10f), Quaternion.identity);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "black")
		{
			onBlack = false;
		}
	}

	private bool info = false;
	IEnumerator Instructions()
	{
		yield return new WaitForSecondsRealtime(2);
		instructions.text = "<color=#1dff00>목표를 찾으세요</color>";
		yield return new WaitForSecondsRealtime(5);
		instructions.text = "어떻게 하면 이 게임을 끝낼 수 있을까요?";
	}
}
