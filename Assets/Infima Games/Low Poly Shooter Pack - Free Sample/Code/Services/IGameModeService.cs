//Copyright 2022, Infima Games. All Rights Reserved.

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// _game Mode Service.
    /// </summary>
    public interface IGameModeService : IGameService
    {
        /// <summary>
        /// Returns the Player Character.
        /// </summary>
        CharacterBehaviour GetPlayerCharacter();
    }
}