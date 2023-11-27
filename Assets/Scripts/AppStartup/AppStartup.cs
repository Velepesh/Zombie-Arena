using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;

public class AppStartup : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Shop _shop;
    [SerializeField] private WalletSetup _walletSetup;
    [SerializeField] private LevelCounterSetup _levelCounterSetup;
    [SerializeField] private EquipmentSetup _equipmentSetup;
    [SerializeField] private SettingsSetup _settingsSetup;
    [SerializeField] private PlayerCompositeRoot _playerCompositeRoot;
    [SerializeField] private TwinsCompositeRoot _twinsCompositeRoot;
    [SerializeField] private ZombieTargetsCompositeRoot _zombieTargetsCompositeRoot;
    [SerializeField] private ZombieSpawnerCompositeRoot _zombieSpawnerCompositeRoot;
    [SerializeField] private WinReward _winReward;

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
        _levelCounterSetup.Init();
        LevelCounter levelCounter = _levelCounterSetup.LevelCounter;

        _walletSetup.Init();
        Wallet wallet = _walletSetup.Wallet;

        _equipmentSetup.Init();
        Equipment equipment = _equipmentSetup.Model;

        _settingsSetup.Init();

        _shop.Init(equipment);
        equipment.InitWeapons();

        _playerCompositeRoot.Init(levelCounter, equipment);
        _twinsCompositeRoot.Init();
        _zombieTargetsCompositeRoot.Init(_playerCompositeRoot.Player, _twinsCompositeRoot.Twins);
        _zombieSpawnerCompositeRoot.Init(levelCounter);
        _winReward.Init(wallet);
        _game.Init(levelCounter);
    }
}