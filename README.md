# BattleshipsSimulation

BattleshipsSimulation is a simple web app made in Blazor pages which generates two boards and simulate gameplay between 2 players.

# Thoughts / Assumptions / Decisions
- since this application is a simulation, I wanted the gameplay to run continuously without pauses / turns until the end. In order to make this, I had to use Dynamic Interface which brings me to choose between Blazor and JavaScript. I chose Blazor because I am much better at using C# than JS.

- To create boards I chose tables over svg rects. 

- To generate boards, I added GenerateBoard task in GameMechanics.DataGenerator 9 line

- To generate and add ships to boards I added GenerateShips task in GameMechanics.DataGenerator 25 line

- When boards and ships are generated you only need to click Start button to begin simulation, Index.razor 148 line

- Shot mechanic part one explained Index.razor 197 line
- part two GameMechanics.DataGenerator 84 line

- When simulation ends it increment win count of the winner and you can start another simulation if you want to.

- I did not add database because I did not consider its a good idea for this project, so this app use local storage which deletes values with every page refresh.


