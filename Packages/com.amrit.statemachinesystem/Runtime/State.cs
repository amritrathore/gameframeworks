namespace Core.StateMachineSystem
{
    public abstract class State<T> : IState
    {
        protected readonly T Registry;

        protected State(T registry)
        {
            Registry = registry;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Tick();
    }
}