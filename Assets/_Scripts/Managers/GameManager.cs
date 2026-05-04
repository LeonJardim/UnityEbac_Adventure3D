using Leon.Singleton;
using Leon.StateMachine;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameStates
    {
        INTRO,
        GAMEPLAY,
        PAUSE,
        WIN,
        LOSE
    }
    public StateMachine<GameStates> stateMachine;

    [Header("References")]
    public Player playerPFB;
    public UIUpdater uiGunReload;
    public UIUpdater uiLife;
    public Transform actors;
    public Transform defaultSpawnPoint;

    public CinemachineCamera mainCamera;

    [HideInInspector] public Player player;

    private void Start()
    {
        //Init();
        if (playerPFB != null)
        {
            Invoke(nameof(SpawnPlayer), 0.2f);
        }
    }

    public void Init()
    {
        stateMachine = new StateMachine<GameStates>();
        stateMachine.Init();
        stateMachine.RegisterStates(GameStates.INTRO, new GMStateIntro());
        stateMachine.RegisterStates(GameStates.GAMEPLAY, new StateBase());
        stateMachine.RegisterStates(GameStates.PAUSE, new StateBase());
        stateMachine.RegisterStates(GameStates.WIN, new StateBase());
        stateMachine.RegisterStates(GameStates.LOSE, new StateBase());

        stateMachine.SwitchState(GameStates.INTRO);
    }


    public void SpawnPlayer()
    {
        var obj = Instantiate(playerPFB);
        if (CheckPointManager.Instance.lastCheckPointKey > 0)
        {
            obj.transform.position = CheckPointManager.Instance.GetPositionFromLastCheckpoint();
        }
        else
        {
            obj.transform.position = defaultSpawnPoint.position;
        }
        obj.transform.SetParent(actors);

        var r = obj.GetComponent<PlayerAbilityShoot>();
        if (r != null)
        {
            r.uIUpdaters.Add(uiGunReload);
        }
        var h = obj.GetComponent<HealthBase>();
        if (h != null)
        {
            h.uiUpdaters.Add(uiLife);
        }

        mainCamera.Target.TrackingTarget = obj.transform;
        player = obj;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
