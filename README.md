## Sand 2D Navigation

Sand 2D Navigation is a simple and performatic grid navigation solution for 2D Unity games.

## Install

As long as Sand 2D Navigation is a UPM, all you need to do is to add a package to your project with this repo URL (don't forget the .git extension).

## Quick Setup

Follow the next steps to set up your grid navigation system

- Add the NavigationGrid component to a new GameObject.
- Add the NavigationNode component to the tiles in your scene.
- Add the NavigationAgent component to the agents in your scene.
- Set the world size of your nods in the property "Node Size" in the NavigationGrid inspector.- Assign the NavigationGrid to the property "Grid" of your agents and nodes in the inspector.
- Set the property "Walkable" of your nodes in the inspector.
- Set the property "Velocity" of your agent in the inspector.

Now your setup is done and you can start using all the Sand Grid features!

In case you want to try a straightforward implementation of this system, consider taking a look at our [examples](#examples) :)

## Examples

- [Examples repository](https://github.com/ccadori/sand-2d-navigation-examples)

## Documentation

#### Navigation Grid

NavigationGrid is the main component that will control the other navigation components.

Inspector Properties:

- Node Size: The world size of your grid nodes.
- Allow Diagonal Move: Turn on this property to allow agents to move diagonally
- Limit Agents Per Node: Turn on this property to limit the number of agents in the same target node.
- Agents Per Node: The maximum number of agents in the same node (the above property should be enabled).

#### Navigation Node

NavigationNode is the node/tile component, you must add it to every node you want the pathfinder to consider when calculating agents path.

Inspector Properties:

- Grid: A reference to the grid.
- Move Cost: A cost number to be considered when pathfinder is calculating the agent path.
- Walkable: Turn on this property to allow agents to walk over the node.

#### Navigation Agent

NavigationAgent is the agent component, it controls all the interactions with the grid. You must add it to every agent in the scene.

Inspector Properties:

- Grid: A reference to the grid.
- Velocity: The agent velocity when walking over the grid.

## Roadmap

- Create a low-level API, so the logic can be used independently of Unity components.
- Add unit tests.
- Add more flexible ways to calculate paths.
- Add different movement velocities considering the node movement cost.
