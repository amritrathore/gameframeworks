namespace Core.StateMachineSystem
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Tick();
    }
}
