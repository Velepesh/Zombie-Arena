using System.Collections.Generic;
using YG;

public static class MetricaSender
{
    public static void LevelStart(int level)
    {
        var eventParams = new Dictionary<string, string>
        {
            { "level", level.ToString() }
        };

        YandexMetrica.Send("level_start", eventParams);
    }

    public static void LevelComplete(int level, int time)
    {
        var eventParams = new Dictionary<string, string>
        {
            { "level", level.ToString() },
            { "time_spent", time.ToString() }
        };

        YandexMetrica.Send("level_complete", eventParams);
    }

    public static void Fail(int level, int time)
    {
        var eventParams = new Dictionary<string, string>
        {
            { "level", level.ToString() },
            { "time_spent", time.ToString() }
        };

        YandexMetrica.Send("fail", eventParams);
    }

    public static void Restart(int level)
    {
        var eventParams = new Dictionary<string, string>
        {
            { "level", level.ToString() }
        };

        YandexMetrica.Send("restart", eventParams);
    }

    public static void Money(string type, string name, int amount)
    {
        var eventParams = new Dictionary<string, string>
        {
            { "type", type},
            { "name", name },
            { "amount", amount.ToString() }
        };

        YandexMetrica.Send("money", eventParams);
    }

    public static void Device(string type)
    {
        var eventParams = new Dictionary<string, string>
        {
            { "type", type},
        };

        YandexMetrica.Send("device", eventParams);
    }

    public static void Reward(string placement)
    {
        var eventParams = new Dictionary<string, string>
        {
            { "placement", placement},
        };

        YandexMetrica.Send("reward", eventParams);
    }
}