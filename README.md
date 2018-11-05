[![Build status](https://virtuaboza.visualstudio.com/HackerNews%20Jr/_apis/build/status/HackerNews%20Jr-ASP.NET%20Core-CI)](https://virtuaboza.visualstudio.com/HackerNews%20Jr/_build/latest?definitionId=9)

# HackerNewsJr
An interview take-home code challenge.

## Challenge Instructions
Use https://github.com/HackerNews/API to generate a web-based solution using C# and .NET that displays the title and author of the newest stories on Hacker News. 
- [x] There should be tests. 
- [x] It should compile and run. 
- [x] The title should take you to the article. 
- [x] Be modern in your example. (*I'm mimicking HN stylistically, which isn't modern, but the tech underneath is nice and modern.*)
 
If you have time, try some of the bonuses:
- [x] Add a search function.
- [ ] Build caching. (*I don't know what this is.*) ¯\_(ツ)_/¯
- [x] Put it on Azure and send us a link to it working.

### Guidance
> They are to use GitHub so the others on the team can review, etc. Doesn't need to be rockstar status...just needs to work. JavaScript and C# are the most important things - making the code work, and accessible, clean, and using JavaScript (Angular if possible) on the front end.

## Design Overview
This solution was built from the Visual Studio ASP.NET Core 2.1 React template. The ASP.NET Core app has a hosted service which periodically fetches and caches new stories so that the stories can be returned much faster than by fetching directly from the Hacker News API. New requests by loading/refreshing the client app will also check to see if there are even newer stories to fetch (and cache). The search function is performed client-side by filtering the stories held in state locally.

### Client-Side
The frontend is a React SPA with a single view which is rendered directly from App.js. The css and styling is taken from Hacker News. Component logic and helper functions are tested with Jest and Enzyme.

### Server-Side
The backend is an ASP.NET Core 2.1 application tested with MSTest and Moq. The solution is split into projects to help organize dependencies:
- **HackerNewsJr.App** is at the center of the solution. It provides interfaces and the application's core model (Story). It had no dependencies.
- **HackerNewsJr.Web** contains the client app, the single API endpoint, and a few object built into the template. This project bootstraps the entire application, so it contains a reference to every other project in the solution, but beyond Startup.cs, only HackerNewsJr.App is referenced.
- **HackerNewsJr.Services** is where you'll find the core application logic. Classes here implement HackerNewsJr.App interfaces (they could have been part of the HackerNewsJr.App project, but keeping them separated ensures that the core *.App project doesn't take on any dependencies). This project's only intra-solution dependency is on HackerNewsJr.App.
- **HackerNewsJr.Infrastructure** contains concrete interface implementations which provide abstractions to infrastructure concerns. The only use in this case is abstracting the http client. Like HackerNewsJr.Services, this project's only intra-solution dependency is HackerNewsJr.App.

### DevOps/Hosting
The app is hosted on Azure with Azure App Service at https://hackernewsjr.azurewebsites.net/, and it's built and deployed to that target by Azure DevOps pipelines.

### Other Considerations
- My first pass at this challenge was accomplished with just client-side code, but this didn't demonstrate any C#/.NET knowledge. Also, given the way the HackerNews API is set up, fetching those stories was taking a long time. Using the ASP.NET Core app as a proxy, I'm able to have the server periodically fetch and cache responses from HackerNews API. Since I'm only displaying the title and author of the stories and not dynamic data like score or comments, this works just fine.
- I used React instead of Angular because I wasn't sure how much time I would be able to spend on this challenge, and I'm accustomed to using React almost every day, so I figured it was a safe choice since I wouldn't need to spend time getting up to speed.
- The Hacker News API documentation wants people to use Firebase clients to consume their API, but there wasn't a .NET client, and it didn't appear that there would be a benefit to using a Firebase client for this specific challenge.
