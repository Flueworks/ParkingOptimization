using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimizer
{
    public class ParkingGarage
    {
        public List<Node> Nodes { get; } = new List<Node>();
        public List<Source> Hotels { get; } = new List<Source>();

        public ParkingGarage()
        {
            
        }

        public void AddSection(ParkingGarageSection section)
        {
            Nodes.AddRange(section.Nodes);
        }

        public void AddHotel(string hotelName, string parkingId, int angle = 0)
        {
            var hotel = new Source(hotelName);

            var parkingSpot = Nodes.First(x => x.Id == parkingId);
            Edge.Connect(parkingSpot, hotel, angle);
            
            Hotels.Add(hotel);
        }

        public void Connect(string id1, string id2, int angle)
        {
            var parkingSpot1 = Nodes.First(x => x.Id == id1);
            var parkingSpot2 = Nodes.First(x => x.Id == id2);
            Edge.Connect(parkingSpot1, parkingSpot2, angle);
        }

        public void PropagateRouting()
        {
            foreach (var hotel in Hotels)
            {
                hotel.PropagateRoutingTable(hotel.Id, 0);
            }
        }
    }

    public class ParkingGarageSection
    {
        public ParkingGarageSection(string names)
        {
            var rows = names.Split(new []{"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            Node previousNode = null;
            
            foreach (var row in rows)
            {
                var column = row.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                var node = new Node(column[0]);
                if (column.Length > 1)
                {
                    var connectingNode = node;
                    var sideParkings = column.Skip(1).ToList();
                    for (var index = 0; index < sideParkings.Count; index++)
                    {
                        var col = sideParkings[index];
                        var node2 = new Node(col);
                        Edge.Connect(connectingNode, node2, index == 0 ? 90 : 0); // side parking
                        Nodes.Add(node2);
                        connectingNode = node2;
                    }
                }
                Edge.Connect(previousNode, node, 0);
                previousNode = node;
                Nodes.Add(node);
            }
        }

        public List<Node> Nodes { get; } = new List<Node>();

    }
}