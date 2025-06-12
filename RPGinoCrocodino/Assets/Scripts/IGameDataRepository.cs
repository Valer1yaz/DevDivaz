public interface IGameDataRepository
{
    void Save(GameSaveData data);
    GameSaveData Load();
}