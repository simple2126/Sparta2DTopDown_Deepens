# Sparta2DTopDown_Deepens
 
Q1. 심화주차 1-3강 ( UI 만들기 )
확인 문제 : 강의를 듣고, 강의 내용을 다시 점검하는 문제를 풀어봅시다.
🧠 UI의 앵커와 피벗에 대해서 세팅하는 19강의 강의 자료를 다시 확인해보시고, 아래 퀴즈를 풀어보세요!

[🅾️❎퀴즈]

앵커와 피벗은 같은 기능을 한다. (O/X) -> X
피벗을 왼쪽 상단으로 설정하면, UI 요소는 화면의 왼쪽 상단을 기준으로 위치가 고정된다. (O/X) -> X
피벗을 UI 요소의 중심에 설정하면, 회전 시 UI 요소가 중심을 기준으로 회전한다. (O/X) -> O

🤔 생각해보기

게임의 상단바와 같이 화면에 특정 영역에 꽉 차게 구성되는 UI와 화면의 특정 영역에 특정한 크기로 등장하는 UI의 앵커 구성이 어떻게 다른 지 설명해보세요.

꽉 차게 구성하려면 앵커의 x, y 축 모두를 stretch로 설정한다.
특정 영역에 특정한 크기로 하려면 anchor를 x축은 center, y 축은 middle로 설정한다.


돌아다니는 몬스터의 HP 바와 늘 고정되어있는 플레이어의 HP바는 Canvas 컴포넌트의 어떤 설정이 달라질 지 생각해보세요.

돌아다니는 몬스터의 HP 바는 Canvas의 Render Mode를 World Space로 변경하여야 하고
플레이어의 HP 바는 Screen Space - Screen Overlay로 설정해야 한다.

확장 문제 : 강의 내용을 바탕으로 새로운 기능을 추가 구현해봅시다.

⏱️ 게임이 길어지니 힘이 듭니다. 게임을 일시정지하는 버튼을 만들어봅시다.

Resume Game이라는 텍스트가 들어있는 버튼을 만들고, 그 버튼을 누르면 게임이 재개되게 하세요.

    public void GameStop()
    {
        Time.timeScale = 0f;
        gameStopUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        gameStopUI.SetActive(false);
    }

Q2. 심화주차 1-4강 ( 게임로직 수정 )
확인 문제 : 강의를 듣고, 강의 내용을 다시 점검하는 문제를 풀어봅시다.
[🅾️❎퀴즈]

코루틴은 비동기 작업을 처리하기 위해 사용된다. (O/X) -> X
yield return new WaitForSeconds(1);는 코루틴을 1초 동안 대기시킨다. (O/X) -> O
코루틴은 void를 반환하는 메소드의 형태로 구현된다. (O/X) -> X

**[**🤔 생각해보기]

코루틴을 이미 실행중이라면 추가로 실행하지 않으려면 어떻게 처리해주면 될까요?
if(coroutine == null) StartCoroutine(coroutine);

코루틴 실행 중 게임오브젝트가 파괴되더라도 코루틴의 실행이 정상적으로 지속될까요?
SetActive(false)나 Destroy(gameObject)를 했을 경우 코루틴은 즉시 종료됩니다.

확장 문제 : 강의 내용을 바탕으로 새로운 기능을 추가 구현해봅시다.

🙃 **웨이브 10, 30, 50, …**에 부여되는 랜덤 디버프를 만들어봅시다.
랜덤 디버프는 체력을 0%~50%를 감소시키는 무시무시👹한 디버프입니다.
플레이어의 HP를 실제 감소시키는 것까지 구현해봅시다. (Debug.Log만 하는 것 아님)

    private void ChangeCharacterConditions()
    {
        if(currentWaveIndex % 20 == 10)
        {
            DebuffPlayer();
        }
    }

    private void DebuffPlayer()
    {
        int random = Random.Range(0, 50);
        float health = playerHealthSystem.CurrentHealth * random / 100;
        playerHealthSystem.ChangeHealth(-health);
    }


Q3. 심화주차 1-5, 1-6강 ( 스텟 강화 - 플레이어 강화 아이템 구현 )
확인 문제 : 강의를 듣고, 내용을 다시 점검하는 문제를 풀어봅시다.
[🅾️❎퀴즈]

추상 클래스는 new를 통해 인스턴스화(instantiation)할 수 없다. (O/X) -> X
추상 클래스는 다른 클래스처럼 일반 메서드와 속성을 포함할 수 있다. (O/X) -> O
추상 클래스를 상속받은 클래스는 추상 클래스의 모든 추상 메서드를 구현해야 한다. (O/X) -> O
C#에서 한 클래스는 여러 개의 추상 클래스를 상속받을 수 있다. (O/X) -> X

**[**🤔 생각해보기]

추상 클래스를 사용하지 않고 동일한 기능을 구현하려면 어떤 문제가 발생할 수 있는지 설명해보세요.
일반 클래스를 만들어 해당 클래스를 상속받을 때 중요한 메서드들을 구현하지 못할 수 있다.
중요 메서드가 있다면 여러 클래스에 작성해야하기 때문에 코드 중복이 발생할 수 있다.

개선 문제 : 기존의 코드를 개선해봅시다.
👀 코드리뷰 결과에 따라 코드를 개선해봅시다.

당신은 CharacterStatHandler의 코드를 풀리퀘스트했고, 당신의 동료는 아래와 같은 코드리 뷰 결과를 전달했습니다.

XX님 고생하셨습니다. 아래 내용들에 관련된 코드를 정리해보면 가독성이 높아질 것 같습니다.

Awake 메소드 내의 초기화 코드를 분리하는 것이 더 깔끔해보일 것 같습니다.
    private void Awake()
    {
        UpdateCharacterStat();
        InitialAttackSO();
    }

    private void InitialAttackSO()
    {
        if (baseStat.attackSO != null)
        {
            // 일단 복사해놓기
            baseStat.attackSO = Instantiate(baseStat.attackSO);
            CurrentStat.attackSO = Instantiate(baseStat.attackSO);
        }
    }

ApplyStatModifiers 메소드 내의 switch식의 코드를 분리하면 가독성이 높아질 것 같습니다.

        Func<float, float, float> operation;
        switch (modifier.statChangeType)
        {
            case StatsChangeType.Add:
                operation = (current, change) => current + change;
                break;
            case StatsChangeType.Multiple:
                operation = (current, change) => current * change;
                break;
            default:
                operation = (current, change) => change;
                break;
        }