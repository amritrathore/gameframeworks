namespace Core.StateMachineSystem
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public virtual void ChangeState(IState nextState)
        {
            if (CurrentState == nextState)
                return;

            CurrentState?.Exit();

            CurrentState = nextState;

            CurrentState.Enter();
        }

        public void Tick()
        {
            CurrentState?.Tick();
        }
    }
}