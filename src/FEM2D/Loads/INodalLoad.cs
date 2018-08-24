namespace FEM2D.Loads
{
    public interface INodalLoad
    {
        NodalLoad[] NodalLoads { get; }
    }
}