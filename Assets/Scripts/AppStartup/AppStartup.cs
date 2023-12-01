using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class AppStartup : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private GameModeSelector _selector;
    [SerializeField] private Shop _shop;
    [SerializeField] private WalletSetup _walletSetup;
    [SerializeField] private ScoreSetup _scoreSetup;
    [SerializeField] private LevelCounterSetup _levelCounterSetup;
    [SerializeField] private HighscoreSetup _highscoreSetup;
    [SerializeField] private EquipmentSetup _equipmentSetup;
    [SerializeField] private SettingsSetup _settingsSetup;
    [SerializeField] private PlayerCompositeRoot _playerCompositeRoot;
    [SerializeField] private TwinsCompositeRoot _twinsCompositeRoot;
    [SerializeField] private ZombieTargetsCompositeRoot _zombieTargetsCompositeRoot;
    [SerializeField] private ZombieSpawnerCompositeRoot _zombieSpawnerCompositeRoot;
    [SerializeField] private Reward _reward;
    [SerializeField] private CompositionOrder _order;
    [SerializeField] private RebornSetup _rebornSetup;

    private bool _isMobilePlatform;
    private LevelCounter _levelCounter;

    private void Start()
    {
        LoadingScreen.Instance.Load();

        Load();
    }

    public async void Load()
    {
        while (YandexGame.SDKEnabled == false)
            await UniTask.Delay(10, ignoreTimeScale: true);

        Init();
    }

    private void Init()
    {
        _isMobilePlatform = YandexGame.EnvironmentData.isDesktop == false;

        _levelCounterSetup.Init();
        _levelCounter = _levelCounterSetup.LevelCounter;

        _walletSetup.Init();
        Wallet wallet = _walletSetup.Wallet;

        _equipmentSetup.Init();
        Equipment equipment = _equipmentSetup.Model;

        _settingsSetup.Init();

        _shop.Init(equipment);
        equipment.InitWeapons();

        _playerCompositeRoot.Init(_levelCounter.Index, equipment);
        _twinsCompositeRoot.Init();
        _zombieTargetsCompositeRoot.Init(_playerCompositeRoot.Player, _twinsCompositeRoot.Twins);
        _rebornSetup.Init(_playerCompositeRoot.Player, _twinsCompositeRoot.Twins);
        
        _reward.Init(wallet);
        _highscoreSetup.Init();
        _game.Init(_levelCounter.Index);

        _selector.Selected += OnGameModeSelected;

        _game.Started += OnStarted;

        _zombieSpawnerCompositeRoot.SpawnerInited += (spawner) => _scoreSetup.Init(spawner);
        
        _zombieTargetsCompositeRoot.TargetDied += OnTargetDied;

        _zombieSpawnerCompositeRoot.ZombiesEnded += OnZombiesEnded;

        _rebornSetup.Reborned += () => _game.Reborn();
        _game.InfinityGameEnded += () => _highscoreSetup.Model.Record(_scoreSetup.TotalScore);
       
        _reward.DoubleRewarded += () => _game.NextLevel();
    }


    private void OnDestroy()
    {
        _selector.Selected -= OnGameModeSelected;

        _game.Started -= OnStarted;

        _zombieSpawnerCompositeRoot.SpawnerInited -= (spawner) => _scoreSetup.Init(spawner);

        _zombieTargetsCompositeRoot.TargetDied -= OnTargetDied;

        _zombieSpawnerCompositeRoot.ZombiesEnded -= OnZombiesEnded;

        _rebornSetup.Reborned -= () => _game.Reborn();
        _game.InfinityGameEnded -= () => _highscoreSetup.Model.Record(_scoreSetup.TotalScore);

        _reward.DoubleRewarded -= () => _game.NextLevel();
    }

    private void OnGameModeSelected(GameMode gameMode)
    {
        _playerCompositeRoot.OnGameModeSelected(gameMode);
        _game.StartLevel(gameMode);
    }

    private void OnStarted()
    {
        _zombieSpawnerCompositeRoot.Init(_selector.Mode, _levelCounter.Index, _zombieTargetsCompositeRoot, _isMobilePlatform);
        _order.Compose();
    }

    private void OnTargetDied()
    {
        _game.End();
        _reward.GiveGameOverReward(_selector.Mode, _scoreSetup.TotalScore);
    }

    private void OnZombiesEnded()
    {
        _game.Win();
        _levelCounter.IncreaseLevel();
        _reward.GiveWinReward(_scoreSetup.TotalScore);
    }
}