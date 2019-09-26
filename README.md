# LaboratoryBookWindows
We used to store all data concerning our samples in a paper laboratory book, as every laboratory do. The chief of our laboratory came up with idea to create a database of samples wich will be accessed remotely for each user of the laboratory book. The easy and fast solution was to put the samples data in the excel file strored in a cloud. It was implemented immedeately and worked fine, however I decided to improve it because of the lack of some importaint features concerning data management and access. This inspired me to make a desktop client of the laboratory book

To protect the samples, database authorisation and authentication was implemented. Users passwords are "salted" and stored in encrypted format. Different policies are applied to users with different permissions when they access a database. User class has four subclasses: Guest, Laborant, Moderator and Administer. Moderator and Administer classes implement IAdvancedUser interface which grant an additional functionality for operation with users and laboratory books.

Data are visualised by means of DataGrid control. Columns type (TextColumn or CombobBoxColumn) are resolved during DataGrid authogeneration. Each change in DataGrid is instantly synchronised with a database.

The web client of the laboratory book was introduced later. The link is given in "Links" section below. You can use login "TestUser" and password "qwert1234" to test the application. Screenshoots of the working application are given below.

## Highlights
* Plain SQL for interaction with database.
* MVVM pattern was implemented where it was necessary.
* A lot of multithreading and asynchronous programming.
## Used frameworks and programming languages
C#(WPF), MySQL
