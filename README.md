# MarsRover
This system is being constructed with 3 primary classes:

1. Mars Rover Ground Control
   -The Main Console of the system, used for converting and translate integrated command sequences to separate commands and send to the rover, and combine received telemetry data into telemetry report string.
2. Mars Rover
   -The rover receives navigation commands from the ground control and action to its own attitude and have its telemetry data available for sending back.
3. Navigation System
4. Used as a navigation map and rover tracking system that is on board of the rover. It holds the range of the plateau for the exploration mission and allows the ground control to update and review the spectrum of the Mars plateau, i.e., the boundary of the grid.

The operation procedures will be carried out by the operation commander who batch the commands at the ground control on per-operation basis, in order to sending these commands to the Mars Rover. Up on the Mars, the rover will be deployed according to the commands received and then navigate to the pre-defined location and heading in the specified plateau range according to the plan, and send back its telemetry data back to ground control, that will subsequently be sequenced to data that meets the required format.

Commands are described as below:
- When a coordination is being entered without a heading character, the command will be treated as defining a new plateau range, e.g.:
```
5 5
```
- When a coordination is being entered with a heading character, the command will be treated as deploying a new Mars Rover, e.g.:
```
1 2 N
```
- When a movement string is being entered, the command  will be treated as moving the current Mars Rover, e.g.:
```
LMLMLMLLM
```
- When an empty command is entered, the current Mars Rover will be retreated, or, if all Mars Rovers within that plateau are retreated, the plateau will be let alone.

The development process is following TDD practice, which is first build a set of classes and methods that provides the simplest data for basic testing requirements, then build some critical testing criteria following AAA practice and setup expected results. With these first few test codes a practice called Red/Green/Refactor (RGR) will be carried out to keep writing and testing codes and loop, until the system is reasonably complete.