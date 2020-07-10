using System.Collections.Generic;
using NUnit.Framework;
using Sand.Navigation;
using Sand.Navigation.Api;
using Sand.Navigation.Utils;
using UnityEngine;

namespace Tests
{
    public class MockedAgent : Sand.Navigation.IAgent
    {
        public INode CurrentNode { get; set; }
        public INode OccupiedNode { get; set; }
    }

    public class MockedNode : INode
    {
        public Int2 Index { get; set; }

        public Vector2 WorldPosition { get; set; }

        public int MoveCost { get; set; }
        public bool Walkable { get; set; }
        public float GCost { get; set; }
        public float HCost { get; set; }

        public float FCost { get; set; }

        public INode ParentNodeInPath { get; set; }
        public List<INode> CachedNeighbors { get; set; }
        public float LastNeighborUpdate { get; set; }
    }

    public class ApiGridTest
    {
        [Test]
        public void AddAgent()
        {
            var grid = new ApiGrid(Vector2.one, true, false, 0);
            var agent = new MockedAgent();

            grid.AddAgent(agent);

            Assert.AreEqual(grid.Agents.Count, 1);
            Assert.AreEqual(grid.Agents[0], agent);

            grid.AddAgent(agent);

            Assert.AreEqual(grid.Agents.Count, 1);

            var agent2 = new MockedAgent();
            grid.AddAgent(agent2);

            Assert.AreEqual(grid.Agents.Count, 2);
        }

        [Test]
        public void RemoveAgent()
        {
            var grid = new ApiGrid(Vector2.one, true, false, 0);
            var agent = new MockedAgent();
            var agent2 = new MockedAgent();

            grid.AddAgent(agent);
            grid.AddAgent(agent2);
            grid.RemoveAgent(agent);

            Assert.AreEqual(grid.Agents.Count, 1);
            Assert.AreEqual(grid.Agents[0], agent2);
        }

        [Test]
        public void AddNode()
        {
            var grid = new ApiGrid(Vector2.one, true, false, 0);
            var node = new MockedNode();
            node.Index = new Int2(1, 2);

            grid.AddNode(node);

            Assert.AreEqual(grid.Nodes.Count, 1);
            Assert.IsTrue(grid.Nodes.ContainsKey(node.Index));
        }

        [Test]
        public void RemoveNode()
        {
            var grid = new ApiGrid(Vector2.one, true, false, 0);
            var node = new MockedNode();

            grid.AddNode(node);
            grid.RemoveNode(node);

            Assert.AreEqual(grid.Nodes.Count, 0);
        }

        [Test]
        public void GetAgentsOccupyingNode()
        {
            var grid = new ApiGrid(Vector2.one, true, false, 0);
            var node1 = new MockedNode();
            var agent = new MockedAgent();

            grid.AddNode(node1);
            grid.AddAgent(agent);

            agent.OccupiedNode = node1;

            Assert.AreEqual(grid.GetAgentsOccupyingNode(node1, agent), 0);

            var agent2 = new MockedAgent();
            agent2.OccupiedNode = node1;
            grid.AddAgent(agent2);

            Assert.AreEqual(grid.GetAgentsOccupyingNode(node1, agent), 1);
        }

        [Test]
        public void GetAgentsWalkingOnNode()
        {
            var grid = new ApiGrid(Vector2.one, true, false, 0);
            var node1 = new MockedNode();
            var agent = new MockedAgent();

            grid.AddNode(node1);
            grid.AddAgent(agent);

            agent.CurrentNode = node1;

            Assert.AreEqual(grid.GetAgentsWalkingOnNode(node1, agent), 0);

            var agent2 = new MockedAgent();
            agent2.CurrentNode = node1;
            grid.AddAgent(agent2);

            Assert.AreEqual(grid.GetAgentsWalkingOnNode(node1, agent), 1);
        }

        [Test]
        public void CanOccupy()
        {
            var grid = new ApiGrid(Vector2.one, true, true, 1);
            var node1 = new MockedNode();
            var agent = new MockedAgent();
            var agent2 = new MockedAgent();

            grid.AddNode(node1);
            grid.AddAgent(agent);
            grid.AddAgent(agent2);

            agent.OccupiedNode = node1;

            Assert.AreEqual(grid.CanOccupy(node1, agent), true);

            agent2.OccupiedNode = node1;

            Assert.AreEqual(grid.CanOccupy(node1, agent), false);
        }

        [Test]
        public void GetNeighbors()
        {
            var grid = new ApiGrid(Vector2.one, false, true, 1);
            var node1 = new MockedNode();
            var node2 = new MockedNode();
            var node3 = new MockedNode();
            
            node1.WorldPosition = new Vector2(0, 0);
            node2.WorldPosition = new Vector2(0, 1);
            node3.WorldPosition = new Vector2(1, 1);

            grid.AddNode(node1);
            grid.AddNode(node2);
            grid.AddNode(node3);

            Assert.AreEqual(grid.GetNeighbors(node1).Count, 1);

            grid.AllowDiagonalMove = true;

            Assert.AreEqual(grid.GetNeighbors(node1).Count, 2);
        }
    }
}
