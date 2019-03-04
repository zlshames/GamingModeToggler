# Gaming Mode Toggler

This is a small windows application to toggle a customizable "gaming mode". Basically, this app is a fancy way of killing and starting specified processes. The app gives you a UI divided into 2 sides, one for turning on "gaming mode" and one for turning off "gaming mode". On each side, there is a start list and a kill list. These lists contains the paths to applications you want to kill and start when you enable/disable gaming mode. You may add or remove applications to customize your "gaming mode".

## Screenshot

![UI](./Images/UI.png)

## Notes

- By default, there are a few apps that get preloaded into the start/kill lists. However, these will only get loaded if they are installed on your machine. There very well could be no applications in your lists when you first open it up.
- When you close the application, your current lists will be saved automatically and loaded up the next time the application is run
- This app uses a simple Process.Kill() call to kill apps and a simple Process.Start() call to start apps. So if a process you need to kill requires Admin priviledges, please run this app as an Administrator
