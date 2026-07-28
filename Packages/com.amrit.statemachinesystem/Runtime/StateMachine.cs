using Game.Events;

namespace Amrit.StateMachineSystem
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public void ChangeState(IState nextState)
        {
            if (CurrentState == nextState)
                return;

            CurrentState?.Exit();

            CurrentState = nextState;

            CurrentState.Enter();

            EventDispatcher.Dispatch<GameStateChangedEvent>(new GameStateChangedEvent(CurrentState));
        }

        public void Tick()
        {
            CurrentState?.Tick();
        }
    }
}