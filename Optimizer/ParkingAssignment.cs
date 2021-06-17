namespace Optimizer
{
    public class ParkingAssignment
    {
        public Customer Customer { get; }
        public Node ParkingSpot { get; }

        public ParkingAssignment(Customer customer, Node parkingSpot)
        {
            Customer = customer;
            ParkingSpot = parkingSpot;
        }
        
        public int GetWalkingDistance()
        {
            return ParkingSpot.RoutingTable[Customer.Hotel];
        }
    }
}