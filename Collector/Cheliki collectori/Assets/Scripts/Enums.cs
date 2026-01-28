using UnityEngine;

public enum ChelixState { Idle, MovingToGoal, OnGoalInteraction, Sleeping, Spawning }

public enum Currencies { Dollars, Fails }

public enum ItemName
{
    NewCoinBronze, NewCoinSilver, NewCoinGold,
    NewChelix,
    UpgradeCoinBronzeValue, UpgradeCoinSilverValue, UpgradeCoinGoldValue,
    UpgradeChelixSpeed,
    UpgradeCoinMoveDurationBronze, UpgradeCoinMoveDurationSilver, UpgradeCoinMoveDurationGold,
    FeatureMouseHover,
    FeatureUnlockCoinSilverForChelix, FeatureUnlockCoinGoldForChelix
}