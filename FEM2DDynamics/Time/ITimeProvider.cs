namespace FEM2DDynamics.Time
{
    internal interface ITimeProvider
    {
        double CurrentTime { get; }
        double DeltaTime { get; }
        double EndTime { get; }

        bool IsWorking();

        void Tick();
    }
}