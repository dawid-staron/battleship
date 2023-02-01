namespace GameComponents.Ships
{
    internal interface IShipGroup : IShip
    {
        public void Join(IShip ship);
    }
}