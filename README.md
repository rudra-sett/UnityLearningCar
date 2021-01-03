# UnityLearningCar
An extremely simple evolutionary neural network that features little cars learning how to drive

Note that a folder called "Library", found in Unity projects, isn't in this repository. If you choose to use project, you'll need to copy that folder from another Unity project. 

Interesting things to note:

1. I needs to be run in the editor; for whatever reason, the cars do not move at all in a compiled version of this program
2. They take a very long time to learn anything - I've never actually seen them complete the track
3. They often spin around in circles - the reason for this is that this is technically a way to increase the distance traveled
      In order to mitigate that, I changed the scoring method to be as follows: distance * displacement from the starting point, because a spinning car will have a very small displacement. It only helps a little bit. 
4. No, this is not a proper NEAT algorithm at all. All learning is purely random luck based on copying the weights of the car with the highest score and then shifting them a little. 
