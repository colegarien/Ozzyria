using Ozzyria.Game.ECS;

namespace Ozzyria.Game
{
    public class Game // TODO OZ-28 delete game and just do it in Server?
    {
        public World _world;

        public Game()
        {
            _world = new World();
        }

        public void OnPlayerJoin(int playerId)
        {
            _world.PlayerJoin(playerId);
        }

        public void OnPlayerInput(int playerId, Input input)
        {
            _world.WorldState.PlayerInputBuffer[playerId].MoveUp = input.MoveUp;
            _world.WorldState.PlayerInputBuffer[playerId].MoveDown = input.MoveDown;
            _world.WorldState.PlayerInputBuffer[playerId].MoveLeft = input.MoveLeft;
            _world.WorldState.PlayerInputBuffer[playerId].MoveRight = input.MoveRight;
            _world.WorldState.PlayerInputBuffer[playerId].TurnLeft = input.TurnLeft;
            _world.WorldState.PlayerInputBuffer[playerId].TurnRight = input.TurnRight;
            _world.WorldState.PlayerInputBuffer[playerId].Attack = input.Attack;
        }

        public void OnPlayerLeave(int playerId)
        {
            _world.PlayerLeave(playerId);
        }

        public EntityContext GetLocalContext(int playerId)
        {
            return _world.GetLocalContext(playerId);
        }

        public void Update(float deltaTime)
        {
            _world.Update(deltaTime);
        }

    }
}
