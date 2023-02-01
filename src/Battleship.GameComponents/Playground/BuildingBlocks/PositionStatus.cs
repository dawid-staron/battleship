namespace GameComponents.Playground.BuildingBlocks
{
    public class PositionStatus
    {
        public static readonly PositionStatus Untouched = new("Untouched");
        public static readonly PositionStatus Empty = new("Empty");
        public static readonly PositionStatus OnFire = new("OnFire");
        public static readonly PositionStatus Sank = new("Sank");


        private PositionStatus(string status)
        {
            Status = status;
        }

        private string Status { get; }

        public static bool operator ==(PositionStatus a, PositionStatus b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(PositionStatus a, PositionStatus b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode() => Status.GetHashCode();
    }
}