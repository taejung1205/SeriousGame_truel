using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject StartPanel, EndPanel;
	public GameObject ModeSelectPanel; // 모드 선택 화면
	public GameObject RulePanel; // 기본 규칙 설명 화면
	public GameObject BasicRulePanel; // 기본 모드 상황 설명
	public GameObject ChallengeAccuracyPanel; //도전 모드에서 명중률 조정 화면
	public GameObject TruelUI; // 게임 도중의 UI

	public Text AimingTarget; //현재 조준 중인 대상의 이름
	public Text ShootText; //"을/를 사격" 텍스트
	public Text TargetStat; //현재 조준 중인 대상의 명중률
	public Text TurnText; //턴이 바뀌었을 때 누구의 턴인지 1초간 표시용
	public Text PlayerAccuracyText; //플레이어의 명중률
	public Text WinLose; // 게임 종료 후 승리 및 패배 여부
	public Text BestStrategy; //게임 종료 후 최적의 수
	public Text BestPossibility; //게임 종료 후 최적의 수 확률

	public Text ChallengeAccuracyText;

	public AudioSource Gunshot; //총소리

	public bool start = false;
	public bool isBasicMode = true;

	public AudioSource au;
	public GameObject Player;

	private int turnIndex = 0; 

	public int[] accuracy = { 30, 70, 100 };
	public int[] shootingOrder = { 0, 1, 2 };
	public bool[] isAlive = { true, true, true };

	public GameObject White;
	public GameObject Black;

	WinRateCalculator rateCalculator;

    void Start ()
	{
		//player.GetComponent<CharacterController>().enabled = false;
		start = false;
		ModeSelectPanel.SetActive(false);
		EndPanel.SetActive (false);	
		StartPanel.SetActive (true);
		RulePanel.SetActive(false);
		BasicRulePanel.SetActive(false);
		ChallengeAccuracyPanel.SetActive(false);
		TruelUI.SetActive(false);
		TurnText.text = "";
		TargetStat.text = "";
		PlayerAccuracyText.text = "";
		rateCalculator = GetComponent<WinRateCalculator>();
	}

    void Update()
	{
        if (start)
        {
			if(CurrentShooter() == 0)
            {
				Transform playerTransform = Player.transform;
				float lookingDirection = playerTransform.localRotation.eulerAngles.y;
				if (lookingDirection > 200 && lookingDirection < 260 && isAlive[1])
				{
					AimingTarget.text = "하양";
					AimingTarget.color = Color.white;
					ShootText.text = "을 사격";
					TargetStat.text = "대상의 명중률: " + accuracy[1] + "%";
					if (Input.GetMouseButton(0))
					{
						Debug.Log("Shot B");
						Shoot(1);
                        if (start)
                        {
							ChangeTurn();
						}
						
					}
				}
				else if (lookingDirection > 280 && lookingDirection < 340 && isAlive[2])
				{
					AimingTarget.text = "검정";
					AimingTarget.color = Color.black;
					ShootText.text = "을 사격";
					TargetStat.text = "대상의 명중률: " + accuracy[2] + "%";
					if (Input.GetMouseButton(0))
					{
						Debug.Log("Shot C");
						Shoot(2);
                        if (start)
                        {
							ChangeTurn();
						}
					}
				}
				else
				{
					AimingTarget.text = "허공";
					AimingTarget.color = Color.blue;
					ShootText.text = "을 사격";
					TargetStat.text = "";
					if (Input.GetMouseButton(0))
					{
						Debug.Log("Shot Air");
						Shoot(-1);
                        if (start)
                        {
							ChangeTurn();
						}
					}
				}
			}
			else
            {
				ShootText.text = "의 차례";
				if(CurrentShooter() == 1)
                {
					AimingTarget.text = "하양";
					AimingTarget.color = Color.white;
					TargetStat.text = "";
				}
				else
                {
					AimingTarget.text = "검정";
					AimingTarget.color = Color.black;
					TargetStat.text = "";
				}

            }

            if (!isAlive[1] && White.transform.localRotation.eulerAngles.z < 90)
            {
				White.transform.Rotate(0, 0, 3, Space.Self);
            }

			if (!isAlive[2] && Black.transform.localRotation.eulerAngles.z < 90)
			{
				Black.transform.Rotate(0, 0, 3, Space.Self);
			}

		}
    }


    public void OnButton(string btn)
	{
		switch (btn)
        {
			case ("start"):
				StartPanel.SetActive(false);
				ModeSelectPanel.SetActive(true);
				au.Play();
				break;
			case ("basic"):
				isBasicMode = true;
				ModeSelectPanel.SetActive(false);
				RulePanel.SetActive(true);
				break;
			case ("basic rule next"):
				RulePanel.SetActive(false);
				BasicRulePanel.SetActive(true);
				break;
			case ("basic start"):
				BasicRulePanel.SetActive(false);
				TruelUI.SetActive(true);
				accuracy[0] = 30;
				accuracy[1] = 70;
				accuracy[2] = 100;
				StartTruel();
				break;
			case ("challenge"):
				ModeSelectPanel.SetActive(false);
				ChallengeAccuracyText.text = accuracy[0] + "%";
				ChallengeAccuracyPanel.SetActive(true);
				break;
			case ("increase"):
				if(accuracy[0] < 100)
                {
					accuracy[0] += 5;
                }
				ChallengeAccuracyText.text = accuracy[0] + "%";
				break;
			case ("decrease"):
				if (accuracy[0] > 5)
				{
					accuracy[0] -= 5;
				}
				ChallengeAccuracyText.text = accuracy[0] + "%";
				break;
			case ("challenge start"):
				ChallengeAccuracyPanel.SetActive(false);
				TruelUI.SetActive(true);
				do
				{
					accuracy[1] = Random.Range(1, 21) * 5;
					accuracy[2] = Random.Range(1, 21) * 5;
				}
				while (accuracy[1] == accuracy[0] || accuracy[2] == accuracy[1] || accuracy[0] == accuracy[2]);
				StartTruel();
				break;
			case ("back to title"):
				EndPanel.SetActive(false);
				StartPanel.SetActive(true);
				break;
		}
	}

	void StartTruel()
    {
		for(int i = 0; i < 3; i++)
        {
			isAlive[i] = true;
        }
		Cursor.visible = false;
		start = true;
		White.SetActive(true);
		Black.SetActive(true);
		White.transform.rotation = Quaternion.Euler(0, 30, 0);
		Black.transform.rotation = Quaternion.Euler(0, 150, 0);
		White.GetComponent<Animator>().enabled = true;
		Black.GetComponent<Animator>().enabled = true;
		SetShootingOrder();
		turnIndex = 0;
		StartCoroutine(ShowTurn());
		if(CurrentShooter() != 0)
        {
			StartCoroutine(ComputerTurn());
        }
		PlayerAccuracyText.text = "당신의 명중률: " + accuracy[0] + "%";
    }

	void SetShootingOrder()
    {
		for(int i = 0; i < accuracy.Length  - 1; i++)
        {
			for(int j = i + 1; j < accuracy.Length; j++)
            {
				if (accuracy[shootingOrder[i]] > accuracy[shootingOrder[j]])
                {
					int temp = shootingOrder[i];
					shootingOrder[i] = shootingOrder[j];
					shootingOrder[j] = temp;
                }
			}
        }
    }

	void Shoot(int target)
    {
		Gunshot.Play();
		if (target < 0) 
		{
			// 허공에 쐈을 경우
		}
		else
		{
			float rand = Random.Range(0, 100);
			if (rand < accuracy[CurrentShooter()]) //명중함
			{
				//TODO
				isAlive[target] = false;
				Debug.Log(target + " Is Dead");
				switch (target)
				{
					case 1:
						White.GetComponent<Animator>().enabled = false;
						break;
					case 2:
						Black.GetComponent<Animator>().enabled = false;
						break;
				}
			}
			if (!isAlive[1] && !isAlive[2])
			{
				//TODO: 승리
				StartCoroutine(Victory());
				TruelUI.SetActive(false);
			}
            if (!isAlive[0])
            {
				StartCoroutine(GameOver());
				TruelUI.SetActive(false);
            }
		}
	}

	void ComputerShoot()
    {
		if(!isAlive[1] || !isAlive[2]) // 한 명이 이미 죽었으므로 무조건 플레이어 사격
        {
			PlayerShootAnimation();
			Shoot(0);
        } else
        {
			switch (turnIndex)
			{
				case 0: 
					double whenShootNone = rateCalculator.WhenAShootNone(accuracy[shootingOrder[0]], accuracy[shootingOrder[1]], accuracy[shootingOrder[2]]);
					double whenShootC = rateCalculator.WhenAShootC(accuracy[shootingOrder[0]], accuracy[shootingOrder[1]], accuracy[shootingOrder[2]]);
					// Debug.Log("None: " + whenShootNone + " C: " + whenShootC);
					if(whenShootNone > whenShootC) //허공쏘기
					{
						AirShootAnimation();
						Shoot(-1);
					} else //3순위 쏘기
                    {
						if (shootingOrder[2] == 0)
						{
							PlayerShootAnimation();
							Shoot(0); //플레이어가 3순위이므로 플레이어 사격
						}
						else
						{
							ComputerShootAnimation();
							Shoot(3 - CurrentShooter()); //다른 컴퓨터가 3순위이므로 다른 컴퓨터 사격
						}
					}
					break;
				case 1: //3순위 쏘기
					if(shootingOrder[2] == 0)
                    {
						PlayerShootAnimation();
						Shoot(0); //플레이어가 3순위이므로 플레이어 사격
                    } else
                    {
						ComputerShootAnimation();
						Shoot(3 - CurrentShooter()); //다른 컴퓨터가 3순위이므로 다른 컴퓨터 사격
                    }
					break;
				case 2: // 2순위 쏘기
					if (shootingOrder[1] == 0)
					{
						PlayerShootAnimation();
						Shoot(0); //플레이어가 3순위이므로 플레이어 사격
					}
					else
					{
						ComputerShootAnimation();
						Shoot(3 - CurrentShooter()); //다른 컴퓨터가 3순위이므로 다른 컴퓨터 사격
					}
					break;
			}
		}
    }

	//플레이어가 조준당했을 시의 애니메이션
	void PlayerShootAnimation()
    {
		switch (CurrentShooter())
		{
			case 1:
				White.transform.rotation = Quaternion.Euler(0, 60, 0);
				White.GetComponent<Animator>().SetTrigger("Shoot");
				break;
			case 2:
				Black.transform.rotation = Quaternion.Euler(0, 120, 0);
				Black.GetComponent<Animator>().SetTrigger("Shoot");
				break;
		}
	}

	//컴퓨터가 서로를 조준할 때의 애니메이션
	void ComputerShootAnimation()
    {
		switch (CurrentShooter())
		{
			case 1:
				White.transform.rotation = Quaternion.Euler(0, 0, 0);
				White.GetComponent<Animator>().SetTrigger("Shoot");
				break;
			case 2:
				Black.transform.rotation = Quaternion.Euler(0, 180, 0);
				Black.GetComponent<Animator>().SetTrigger("Shoot");
				break;
		}
	}

	//컴퓨터가 허공을 조준할 때의 애니메이션
	void AirShootAnimation()
    {
		switch (CurrentShooter())
		{
			case 1:
				White.transform.rotation = Quaternion.Euler(0, 30, 0);
				White.GetComponent<Animator>().SetTrigger("Shoot");
				break;
			case 2:
				Black.transform.rotation = Quaternion.Euler(0, 150, 0);
				Black.GetComponent<Animator>().SetTrigger("Shoot");
				break;
		}
	}

	void ChangeTurn()
    {
		turnIndex = (turnIndex + 1) % 3;
        if (isAlive[shootingOrder[turnIndex]])
        {
			StartCoroutine(ShowTurn());
			if(CurrentShooter() != 0)
            {
				StartCoroutine(ComputerTurn());
            }
        } else
        {
			ChangeTurn();
        }
	}

	void ShowResult()
    {
		if(shootingOrder[0] == 0)
        {
			double whenShootNone = rateCalculator.WhenAShootNone(accuracy[shootingOrder[0]], accuracy[shootingOrder[1]], accuracy[shootingOrder[2]]);
			double whenShootC = rateCalculator.WhenAShootC(accuracy[shootingOrder[0]], accuracy[shootingOrder[1]], accuracy[shootingOrder[2]]);
			if(whenShootNone < whenShootC)
            {
				if(accuracy[1] > accuracy[2])
                {
					BestStrategy.text = "최적의 수: 하양 제거 후 검정 제거";
				} else
                {
					BestStrategy.text = "최적의 수: 검정 제거 후 하양 제거";
                }

				BestPossibility.text = "최적의 수로 이길 확률: " + (whenShootC * 100).ToString("F") + "%";
			} else
            {
				BestStrategy.text = "최적의 수: 누군가 죽을 때까지 허공을 쏜 후 나머지 제거";
				BestPossibility.text = "최적의 수로 이길 확률: " + (whenShootNone * 100).ToString("F") + "%";
			}
        } else
        {
			if (accuracy[1] > accuracy[2])
			{
				BestStrategy.text = "최적의 수: 하양 제거 후 검정 제거";
			}
			else
			{
				BestStrategy.text = "최적의 수: 검정 제거 후 하양 제거";
			}
			if (shootingOrder[1] == 0)
			{
				BestPossibility.text = "최적의 수로 이길 확률: " + (rateCalculator.BRate(accuracy[shootingOrder[0]], accuracy[shootingOrder[1]], accuracy[shootingOrder[2]]) * 100).ToString("F") + "%";
			}
			else {
				BestPossibility.text = "최적의 수로 이길 확률: " + (rateCalculator.CRate(accuracy[shootingOrder[0]], accuracy[shootingOrder[1]], accuracy[shootingOrder[2]]) * 100).ToString("F") + "%";
			}
			
		}
    }

	int CurrentShooter()
    {
		return shootingOrder[turnIndex];
    }

	IEnumerator Victory()
	{
		yield return new WaitForSeconds(0.5f);
		//Application.LoadLevel(Application.loadedLevel);
		WinLose.text = "승리하였습니다.";
		WinLose.color = Color.white;
		EndPanel.SetActive(true);
		ShowResult();
		start = false;
		Cursor.visible = true;
	}

	IEnumerator GameOver()
	{
		yield return new WaitForSeconds(0.5f);
		//Application.LoadLevel(Application.loadedLevel);
		WinLose.text = "패배하였습니다.";
		WinLose.color = Color.grey;
		EndPanel.SetActive(true);
		ShowResult();
		start = false;
		Cursor.visible = true;
	}

	IEnumerator ComputerTurn()
    {
		Debug.Log(CurrentShooter() + "'s Turn");
		yield return new WaitForSeconds(2.0f);
		ComputerShoot();
		yield return new WaitForSeconds(2.0f);
		Debug.Log(CurrentShooter() + "'s Turn Ended");
        if (start)
        {
			ChangeTurn();
		}
	}

	IEnumerator ShowTurn()
    {
        switch (shootingOrder[turnIndex])
        {
			case 0:
				TurnText.text = "플레이어의 차례";
				TurnText.color = Color.blue;
				break;
			case 1:
				TurnText.text = "하양의 차례";
				TurnText.color = Color.white;
				break;
			case 2:
				TurnText.text = "검정의 차례";
				TurnText.color = Color.black;
				break;
		}
		yield return new WaitForSeconds(1.0f);
		TurnText.text = "";
    }
}
