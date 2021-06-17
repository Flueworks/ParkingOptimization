namespace Optimizer
{
    public class Customer
    {
        public string Name { get; set; }
        public string Hotel { get; set; }
        public Node ParkingSpot { get; set; }

        public int GetWalkingDistance()
        {
            return ParkingSpot.RoutingTable[Hotel];
        }
    }
}