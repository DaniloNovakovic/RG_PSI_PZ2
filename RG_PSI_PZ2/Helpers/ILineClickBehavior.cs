using RG_PSI_PZ2.Model;

namespace RG_PSI_PZ2.Helpers
{
    public interface ILineClickBehavior
    {
        void Handle(GridPoint first, GridPoint second);
    }
}