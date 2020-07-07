[![openupm](https://img.shields.io/npm/v/com.ccadori.sand2dnavigation?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.ccadori.sand2dnavigation/)

## Sand 2D Navigation

Sand 2D Navigation is a simple and performatic grid navigation solution for 2D Unity games.

## Install

There's two simple ways to install Sand 2D navigation, you can use the [OpenUPM](https://openupm.com/packages/com.ccadori.sand2dnavigation/) 
or just add the repo git url in the Unity package window.

## Quick Setup

Follow the next steps to set up your grid navigation system

- Add the NavigationGrid component to a new GameObject.
- Add the NavigationNode component to the tiles in your scene.
- Add the NavigationAgent component to the agents in your scene.
- Set the world size of your nods in the property "Node Size" in the NavigationGrid inspector
- Assign the NavigationGrid to the property "Grid" of your agents and nodes in the inspector.
- Set the property "Walkable" of your nodes in the inspector.
- Set the property "Velocity" of your agent in the inspector.

Now your setup is done and you can start using all the Sand 2D Navigation features!

Also, consider taking a look at our [examples repo](#examples) :)

## Documentation

### Navigation Grid

NavigationGrid is the main component that will control the other navigation components.

Inspector Properties:

- Node Size: The world size of your grid nodes.
- Allow Diagonal Move: Turn on this property to allow agents to move diagonally
- Limit Agents Per Node: Turn on this property to limit the number of agents in the same target node.
- Agents Per Node: The maximum number of agents in the same node (the above property should be enabled).

### Navigation Node

NavigationNode is the node/tile component and must be added to to every node GameObject you want the pathfinder to consider when calculating paths.

Inspector Properties:

- Grid: A reference to the grid.
- Move Cost: A cost number to be considered when pathfinder is calculating the agent path.
- Walkable: Turn on this property to allow agents to walk over the node.

### Navigation Agent

NavigationAgent is the agent component, it controls all the interactions with the grid. You must add it to every agent in the scene.

Inspector Properties:

- Grid: A reference to the grid.
- Velocity: The agent velocity when walking over the grid.

## Examples

- [Examples repository](https://github.com/ccadori/sand-2d-navigation-examples)

## Roadmap

- Create a low-level API, separating the logic from the Unity components.
- Add unit tests.
- Add more flexible ways to calculate paths.
- Add different agent movement velocities considering the node movement cost.
