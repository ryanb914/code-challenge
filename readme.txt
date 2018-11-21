Brief walkthrough of app design / choices made:
I built the RESTful web service using ASP.NET MVC. I chose this because a very similar application could
be developed using ASP.NET Core, which I was told the company is wanting to move towards. The solution is
comprised of two projects, the actual web service project (called CodeChallenge) and the unit tests project
(called CodeChallenge.Tests). Separating these two projects keeps the code distinct from one another and
easy to manage. By importing the Autofac NuGet packages I was able to include dependency injection into the
project. I chose to utilize interfaces and abstract classes throughout the project to easily support
dependency injection and unit testing.
The CodeChallenge web service itself is broken down into four main files:
1.	Controllers/FlickrController.cs – Handles the public facing web service request. 
2.	Controllers/FlickrRestRequest.cs – Calls Flickr’s API and formats the request.
3.	Models/DataStructures.cs – Contains the different data classes used in the web service.
4.	Models/Interfaces.cs – Contains the interfaces used in the web service.

----------------------------------------------------------------------------
Briefly explain your object oriented analysis and design:
Below is a Use Case to explain my object oriented design:
•	The end user makes an GET API request to the CodeChallenge web service’s flickr/images web service
	method by providing a string value for the ‘q’ parameter in the URL.
•	Using the parameter value provided the end user, the flickr/images method call’s Flickr’s image
	search API with the parameter value.
•	Flickr’s web service then returns an XML response containing a list of image information related to
	the value the end user provided.
•	The CodeChallenge web service then formats the XML and returns the formatted data as JSON to the end
	user.

----------------------------------------------------------------------------
Local environment setup instructions to compile and run the app:
•	Clone the below repo to a local directory:
https://github.com/ryanb914/code-challenge.git
•	Open the CodeChallenge.sln in Visual Studio 2017.
•	Build the solution to import all NuGet packages.
•	Both projects should now compile and run successfully.

----------------------------------------------------------------------------
Test suite setup/execution instructions (if applicable):
To test the application on your local machine…
•	Make sure the projects have been successfully compiled.
•	Make sure CodeChallenge is set as the StartUp Project in Visual Studio.
•	Start debugging the project. Note: the project must remain running while testing.
•	Open any browser and type in the following URL:
http://localhost:63521/flickr/images?q=lucky
•	In your browser you will receive a block of JSON that contains a list that was populated by Flickr’s
	API response. In each element in this list you will find a title, image, & published date of the
	public photos for the given Flickr search keyword “lucky”.
•	If you want to change the search keyword, just change “lucky” on the tail end of the URL above to
	any keyword or words that you desire.

