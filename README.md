![Image](Gallery/1.png?raw=true "Title")

Note:    
Approximation is done using Runge-Kutta Method   
Delta Pendulum refers to the pendulum with small change in initial angles
*   To visualize the difference in position of two pendulums depending on different initial values of the angles
*   It is colored pink   

Default Pendulum refers to the other pendulum
*   It is colored blue



Functionalities:
*   Top Left:
    *   Play/Pause Reset
    *   Enable/Disable Delta Pendulum
    *   Change Δθs of the Delta Pendulum
*   Top Right:
    *   Initial Conditions
*   Bottom Left:
    *   Total Energy
    *   Difference in Initial total energy and current total energy of default pendulum
        *   This error occurs because of the approximations
*   Center Right:
    *   Rate
        *   To adjust the speed of the simulation relative to realtime
        *   Should be strictly greater than 0 and less than equals to 2
    *   Steps
        *   Number of steps to compute in each iterations
        *   Increase to get better approximation
        *   Should be a Natural Number
