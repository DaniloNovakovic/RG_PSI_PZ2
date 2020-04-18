namespace RG_PSI_PZ2.Model
{
    public class SwitchEntity : PowerEntity
    {
        public string Status { get; set; }

        public override string ToString()
        {
            return base.ToString() + ", Status: " + Status;
        }
    }
}