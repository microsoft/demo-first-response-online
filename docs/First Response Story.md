Microsoft

First Response Demo

Last updated: 6/5/2017

  ------------------------------ --
  **Microsoft Contact **         
                                 
  Tara Shankar Jana              
                                 
  Sr Product Marketing Manager   
                                 
  Data Platform & IoT Mktg       
                                 
  <tarasha@microsoft.com>        
  ------------------------------ --

Introduction
============

First Response Online is a communication and collaboration platform
built to support first responders. It lets police officers, fire
fighters, and paramedics share critical data with each other in near
real-time. It supports iPhone, iPad, and PC and integrates with computer
aided dispatch and GPS tracking. Units in the field can update their
status, complete traffic stops, and even query state & federal databases
all without using the radio.

First Response Online is powered by Azure. In order to build a business
that handles major life-threating incidents, they not only rely on
Azure’s proven scalability and availability but also its breadth of
features to get to market quickly. By using services like App Service,
Azure SQL, Cosmos DB & Azure Search, First Response Online can instead
focus on empowering first responders instead of maintaining
infrastructure.

In the following demo we will examine some of the technologies that
enable cloud born applications like First Response Online .

Setup
=====

Before starting the demo, follow the deployment instructions that you
could find at .\\Deploy\\Deploy.md

Once completed, you will be ready to open MSCorp.FirstResponse.sln and
run locally **MSCorp.FirstResponse.WebApiDemo**

![](media/image1.png){width="3.6666666666666665in"
height="3.024850174978128in"}

This should launch the API at <http://localhost:50002/>

Now that we have the API ready you can run the client application you
like to start the demo. In the following instructions we’ve used UWP app
as sample.

By default, the app will load Seattle data. If you are in a different
city or would like to use different data click in the City name on login
page to access the settings page:

You will be able to change city or set different API endpoint in Other
tab:

![](media/image3.png){width="3.4349595363079617in"
height="2.706639326334208in"}

Use “Mock Services” flag if you don’t want to get data from the API. The
app will then use his own datasets locally.

In this document, there are references to a CityID number. Use the one
from the following list based on your city choice:

-   24 | Chicago

-   25 | Johannesburg

-   26 | Frankfurt

-   27 | Washington, DC

-   28 | Singapore

-   29 | Bangalore

-   30 | Milan

-   31 | Amsterdam

-   32 | Birmingham

-   33 | Copenhagen

-   34 | Seoul

-   35 | Seattle

Contents
========

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------
  Demo Story Elements                                                                                                                                      Minutes
  -------------------------------------------------------------------------------------------------------------------------------------------------------- ---------
  *Easy to Build*                                                                                                                                          5
                                                                                                                                                           
  Illustrates how easy it is to build a modern SaaS application on Azure with technologies like Azure SQL database and its elastic capabilities.           

  *Evolve and grow*                                                                                                                                        5
                                                                                                                                                           
  Shows how the breadth of Azure capabilities can be used to expand application capabilities sets using technologies like Cosmos DB and Search.            

  *Innovate and differentiate*                                                                                                                             5
                                                                                                                                                           
  Turn data into actionable insight using technologies like Power BI and its ability to connect and consume from multiple different Azure data services.   
  ------------------------------------------------------------------------------------------------------------------------------------------------------------------

Story 1: Easy to Build
----------------------

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  Screen                                                                               Click Steps                                                                                                                                                                         Demo Script
  ------------------------------------------------------------------------------------ ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  ![](media/image4.png){width="2.3805555555555555in" height="3.296527777777778in"}     1.  Open the **First Response** app from the Start menu.                                                                                                                            First Response is a multi platform app built with Xamarin.Forms. It supports three platforms – from Universal Windows Platform, to iOS and Android tablets – and provides first responders with key features like offline, data security, GPS, first-class touch support, and more.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Enter Username : jclarkson                                                                                                                                                      Let’s login as one of the officers on duty.
                                                                                                                                                                                                                                                                           
                                                                                       3.  Enter Password : jclarkson                                                                                                                                                      
                                                                                                                                                                                                                                                                           
                                                                                       4.  Click Right arrow                                                                                                                                                               
                                                                                                                                                                                                                                                                           

  ![](media/image5.png){width="3.7083048993875765in" height="2.3731878827646544in"}    1.  Wait for the **dashboard** to load.                                                                                                                                             Police officers in the city of Sammamish can see a visual representation of active incidents (Incidents tab). They can also see their current location (centered blue icon).
                                                                                                                                                                                                                                                                           
                                                                                       2.  Click on the **Responders** tab.                                                                                                                                                They can also see other units in the field (Responders tab). Police data is isolated; however, they can opt to securely share with neighboring agencies and counties enabling better co-operation.
                                                                                                                                                                                                                                                                           
                                                                                       3.  Click back to the **Incidents** tab.                                                                                                                                            
                                                                                                                                                                                                                                                                           

                                                                                       1.  Click on the **speeding report** (red icon in the lower right corner or the red incident on the left hand panel).                                                               A speeding car has been reported in Sammamish. We can see the details of the call alongside the map and see that sensitive information has been masked from officers (like John Clarkson). Full visibility is restricted to supervisors.
                                                                                                                                                                                                                                                                           

  ![](media/image6.png){width="3.590579615048119in" height="2.2978477690288712in"}     1.  Click **navigate** button that appears in a popup window.                                                                                                                       Let’s respond to the call using the app and mark ourselves on route. GPS directions are provided from our current location and the map updates our progress in real-time (for both us and other users).
                                                                                                                                                                                                                                                                           
                                                                                       2.  In the left panel you will see the latest incident information while our position is moving to the incident location.                                                           Updates from dispatchers are also available immediately. It looks like the speeding driver has collided with the curb and potentially sustained an injury.
                                                                                                                                                                                                                                                                           

                                                                                       1.  Switch to the **Azure Portal** in Edge.                                                                                                                                         At this point, it’s time to talk about how Azure makes building apps like this one easy.
                                                                                                                                                                                                                                                                           

  ![](media/image7.png){width="3.375896762904637in" height="1.75in"}                   1.  Open the **Elastic database pool**.                                                                                                                                             By using an elastic database pool, SaaS services like First Response can provide burstable performance and reduce costs all while ensuring data isolation. The throughput units and storage are shared across all their customer databases rather than being tied to individual customers.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Point out the **eDTU** and **GB** tiles.                                                                                                                                        
                                                                                                                                                                                                                                                                           

                                                                                       1.  Click on **Configure pool** in the **Settings** blade.                                                                                                                          As First Response grows – or even just for busy periods of the year – it can easily allocate additional capacity on demand.
                                                                                                                                                                                                                                                                           

  ![](media/image8.png){width="3.35in" height="1.7102198162729658in"}                  1.  Open the **list** of databases in the pool.                                                                                                                                     Here we can see separate, isolated databases for the fire, police, and ambulance services. While they are all in the pool, we can manage them like normal databases.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Click on the **police** database.                                                                                                                                               
                                                                                                                                                                                                                                                                           

                                                                                       1.  Click on **Pricing tier (scale DTUs)** under the **Settings** blade.                                                                                                            If needed, we can allocate additional dedicated capacity to individual databases and scale them vertically on demand. In the Frist Response Online we may want to upscale the Police DB proactively to allow for increased traffic during an event.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Click **View all** to see the full set of pricing options.                                                                                                                      
                                                                                                                                                                                                                                                                           

  ![](media/image9.png){width="3.333033683289589in" height="1.7916666666666667in"}     1.  Switch to **SSMS**.                                                                                                                                                             Azure SQL also allows us to protect data at the database level through features like dynamic data masking. Let’s first see how easy it is to set up. We simply provide a mask to the column in the table and then specify which roles can unmask the data.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Open **PoliceDynamicDataMasking.sql** (read only; don’t run)                                                                                                                    
                                                                                                                                                                                                                                                                           
                                                                                       **Note:** These SQL scripts can be found in the deployment folder.                                                                                                                  

                                                                                       1.  Open **PoliceDynamicDataMaskingQuery.sql**                                                                                                                                      Next, let’s see it in action. This script runs a query to retrieve incident data twice: once as an officer, and again as a supervisor. Look how the database returns masked data for users without supervisor permissions.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Select the **police** database.                                                                                                                                                 
                                                                                                                                                                                                                                                                           
                                                                                       3.  Press **Execute**.                                                                                                                                                              
                                                                                                                                                                                                                                                                           
                                                                                       "UserName" =\[YourPrefixName\]-admin                                                                                                                                                
                                                                                                                                                                                                                                                                           
                                                                                       "Password" =f1r4tR3sp0Ns1                                                                                                                                                           

                                                                                       1.  Open **PoliceRowLevelSecurity.sql** (read only; don’t run)                                                                                                                      Azure SQL also allows us enforce security at a row level based on user permissions.
                                                                                                                                                                                                                                                                           
                                                                                                                                                                                                                                                                           To use RLS, we need to set up a predicate function. This will be automatically appended to the WHERE clause of any queries run on the table. It allows users in the supervisor role to see all data and non-supervisor users to see only the data in the regions they are assigned to patrol.
                                                                                                                                                                                                                                                                           
                                                                                                                                                                                                                                                                           The security policy then connects the predicate to the table we want to protect.

  ![](media/image10.png){width="3.911111111111111in" height="1.3080850831146107in"}    1.  Open **PoliceRowLevelSecurityQuery.sql**                                                                                                                                        Let’s look at John’s assigned regions. He can access information about the zip codes where he works. In this case, it’s just two.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Select the **police** database.                                                                                                                                                 When a query for all incident data is run as John, you can see only information from those zip codes is returned.
                                                                                                                                                                                                                                                                           
                                                                                       3.  Run the **first** query.                                                                                                                                                        
                                                                                                                                                                                                                                                                           
                                                                                       4.  Run the **second** query.                                                                                                                                                       
                                                                                                                                                                                                                                                                           

  ![](media/image11.png){width="3.388888888888889in" height="1.1953751093613298in"}    1.  Run the **third** query.                                                                                                                                                        Now let’s look at Ben’s assigned regions. He works in a different area to John. When he queries the incidents table, he can only see data from is region.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Run the **fourth** query.                                                                                                                                                       
                                                                                                                                                                                                                                                                           

  ![](media/image12.png){width="3.611111111111111in" height="1.4793908573928258in"}    1.  Run the **fifth** query.                                                                                                                                                        Now let’s look at Evan’s assigned regions. But we can see through the next query that he’s in a supervisor role. When he queries for incidents, he can see all the data.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Run the **sixth** query.                                                                                                                                                        
                                                                                                                                                                                                                                                                           
                                                                                       3.  Run the **seventh** query.                                                                                                                                                      
                                                                                                                                                                                                                                                                           

  ![](media/image13.PNG){width="3.45in" height="0.38333333333333336in"}                1.  Run the **eighth** query.                                                                                                                                                       Finally, let’s run the query as ourselves (active directory user). Our user hasn’t been granted any rights to this table so it is not available to us.
                                                                                                                                                                                                                                                                           

                                                                                       1.  Switch to **PowerShell**.                                                                                                                                                       While keeping customer data in separate databases – combined with the security features discussed here – ensures a high level of data protection and isolation, it obviously increases complexity. We’ve already seen how Elastic Pools help manage cost & performance; now we’ll look at how Elastic Database helps us manage and query *N* databases as if they were one.
                                                                                                                                                                                                                                                                           

  ![](media/image14.png){width="3.330084208223972in" height="2.3037390638670168in"}    1.  Open **ShardMapManager.ps1**                                                                                                                                                    Let’s connect to the master database and retrieve the list of shards. The ‘Value’ property is a piece of business information that determines which shard should be used to store the information. This mapping can evolve over time as clients come and go.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Run steps \#1 and \#2.                                                                                                                                                          For First Response this ‘Value’ is the key which represents the service / departments type. We have one for Police, Ambulance and Fire.
                                                                                                                                                                                                                                                                           
                                                                                       **Note**: Replace \[YOUR\_PREFIX\] with the demo prefix used at database creation time. These can be also be retrieved from Azure portal or the demo application web.config file.   We are using a range shard map here, because of this in the future we could easily have two services stored within one database.
                                                                                                                                                                                                                                                                           
                                                                                       **Note:** These scripts can be found in the deployment folder.                                                                                                                      

  ![](media/image15.png){width="3.2897189413823273in" height="1.0178313648293964in"}   1.  Let’s stay in the same ps1 file and execute Step \#3                                                                                                                            Here are some of the other commands available (note: add & remove). Additionally, Azure services like Elastic Jobs (to run scripts across all databases) and Elastic Query (to return a single result set for a query over all shards) also help make management simple.
                                                                                                                                                                                                                                                                           

                                                                                       1.  Switch to **Visual Studio**.                                                                                                                                                    Now that we’ve reviewed the management tools, let’s look at what elastic databases mean for application developers. Today we’ll be looking at raw ADO.NET; however, it works just as well with Entity Framework.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Open **MSCorp.FirstResponse.WebApiDemo.sln** which is in the demo folder                                                                                                        
                                                                                                                                                                                                                                                                           

  ![](media/image16.png){width="3.2269925634295715in" height="3.216666666666667in"}    1.  Open **IncidentController.cs** and place a breakpoint on the first line of **GetAllIncidents**.                                                                                 Let’s run through a simple query against an elastic database. We’ll spin up the service locally and debug through it. As you can see the query is pretty simple; but how much more complicated is it to run this one query across our police, fire, and ambulance databases?
                                                                                                                                                                                                                                                                           
                                                                                       2.  Scroll the end of the file and show the **IncidentQuery**.                                                                                                                      
                                                                                                                                                                                                                                                                           
                                                                                       3.  Press **F5**.                                                                                                                                                                   
                                                                                                                                                                                                                                                                           

  ![](media/image17.png){width="3.420561023622047in" height="0.9229166666666667in"}    1.  Let’s go back to **PowerShell** and open **ShardMapManager.ps1**.                                                                                                               We’ll call the service using PowerShell.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Run step \#4                                                                                                                                                                    The port (i.e. XX) will be shown in the system tray under IIS Express.
                                                                                                                                                                                                                                                                           
                                                                                       3.  Wait for the breakpoint to be hit and switch back to Visual Studio.                                                                                                             
                                                                                                                                                                                                                                                                           

  ![](media/image18.png){width="3.373832020997375in" height="2.192230971128609in"}     1.  Step over the **connection string**.                                                                                                                                            First, we get the connection string. It’s just like a normal SQL connection string except without the database details (as we don’t know which shards we’re working with yet).
                                                                                                                                                                                                                                                                           
                                                                                       > (F10, or Debug menu, Step Over option)                                                                                                                                            Next, we’ll get the shard list. This is retrieved from the master shard database and helps the application understand where data list for each customer. In this case, we want to query all shards (but you can see us querying individual shards elsewhere).
                                                                                                                                                                                                                                                                           
                                                                                       1.  Step over the **shards**. (F10)                                                                                                                                                 
                                                                                                                                                                                                                                                                           
                                                                                       2.  Step into **QueryHelper**.                                                                                                                                                      
                                                                                                                                                                                                                                                                           
                                                                                       > (F11, or Debug menu, Step Into option)                                                                                                                                            

  ![](media/image19.png){width="3.137548118985127in" height="0.9626159230096238in"}    1.  Step over until you reach **MultiShardConnection**. (F10)                                                                                                                       As you can see, it’s just like querying using a SqlConnection, SqlCommand, and a SqlReader. They inherit from the same interfaces and share the same query pattern. The queries to individual databases – and the aggregation of results – is hidden from us entirely.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Press **F5**.                                                                                                                                                                   Let’s have a look at how we would query an individual database.
                                                                                                                                                                                                                                                                           
                                                                                       3.  Switch back to the powershell window and view results (notice Ambulance and Fire results being returned)                                                                        
                                                                                                                                                                                                                                                                           
                                                                                       **Note** : Police are not being returned as we have RLS enabled and we are connecting as a dbo service account in this project.                                                     

                                                                                       1.  Open **IncidentController.cs** and place a breakpoint on the first line of **GetAmbulanceIncidents**.                                                                           Now lets call the ambulance end point to load only Ambulance incidents.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Run Step \#5 from file **ShardMapManager.ps1**                                                                                                                                  
                                                                                                                                                                                                                                                                           
                                                                                       3.  Wait for the breakpoint to be hit and switch back to Visual Studio.                                                                                                             
                                                                                                                                                                                                                                                                           

  ![](media/image20.png){width="3.378018372703412in" height="1.3301202974628172in"}    1.  Step over the **connection string**.                                                                                                                                            Notice that the setup of the method is the same, we are re-using the same query and connection string to the master, I.e we are not adding an explicit where clause.
                                                                                                                                                                                                                                                                           
                                                                                       > (F10, or Debug menu, Step Over option)                                                                                                                                            Have a look at how we load the shard, previously we executed the query across all shards, now we use a single shard based on a Key.
                                                                                                                                                                                                                                                                           
                                                                                       1.  Step over the **shard**. (F10)                                                                                                                                                  In First Response, we have each shard setup to store incidents based on the department type / service as seen previously. In this case we are achieving the filtering based on the shard we issue the query too.
                                                                                                                                                                                                                                                                           
                                                                                       2.  Expand out the **Shard.Location** show the database we are hitting is called **XXX-ambo**                                                                                       At this point, we’ve seen how you can quickly create modern applications using the latest SaaS, security, and scalability features available with Azure SQL. But what’s next for First Response?
                                                                                                                                                                                                                                                                           
                                                                                       3.  Press **F5**                                                                                                                                                                    
                                                                                                                                                                                                                                                                           
  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Story 2: Evolve and grow
------------------------

+-----------------------+-----------------------+-----------------------+
| Screen                | Click Steps           | Demo Script           |
+=======================+=======================+=======================+
|                       | 1.  **View** the      | Police Officer John   |
|                       |     zoomed in         | Clarkson arrives at   |
|                       |     incident map      | the scene. The        |
|                       |     view.             | application provides  |
|                       |                       | a zoomed in           |
|                       |                       | incident-level view   |
|                       |                       | of the surrounding    |
|                       |                       | area.                 |
+-----------------------+-----------------------+-----------------------+
| ![](media/image21.png | 1.  **Notice** the    | The First Response    |
| ){width="3.4097222222 |     highlighted       | solution used to stop |
| 222223in"             |     polygon within    | here. Now, with the   |
| height="2.18194444444 |     the application   | power of Azure Cosmos |
| 44445in"}             |     showing previous  | DB, they have evolved |
|                       |     incidents within. | it to support         |
|                       |                       | e-ticketing.          |
|                       |                       |                       |
|                       |                       | Prior to exiting the  |
|                       |                       | vehicle, it’s         |
|                       |                       | important for John to |
|                       |                       | get some context of   |
|                       |                       | previous tickets      |
|                       |                       | within the area, this |
|                       |                       | will assist in        |
|                       |                       | determining whether a |
|                       |                       | ticket will be        |
|                       |                       | issued.               |
|                       |                       |                       |
|                       |                       | Cosmos DB is used as  |
|                       |                       | the data store for    |
|                       |                       | tickets issued by     |
|                       |                       | first responders. It  |
|                       |                       | supports a range of   |
|                       |                       | powerful query        |
|                       |                       | operations including  |
|                       |                       | geospatial indexing   |
|                       |                       | and querying, We can  |
|                       |                       | use this capability   |
|                       |                       | to retrieve previous  |
|                       |                       | tickets within a      |
|                       |                       | bounded area. It’s as |
|                       |                       | simple as issuing a   |
|                       |                       | query to Cosmos DB.   |
|                       |                       | Let’s take a look.    |
+-----------------------+-----------------------+-----------------------+
| ![](media/image22.png | 1.  Switch to the     | We issue a geospatial |
| ){width="2.2429899387 |     **Azure Portal**  | query from within the |
| 576553in"             |     and navigate to   | application to        |
| height="3.09168853893 |     the Cosmos DB     | display the           |
| 26336in"}             |     blade.            | incidents. Let’s look |
|                       |                       | at how to write one.  |
|                       | 2.  Click on **Query  |                       |
|                       |     Explorer** icon   | Using the Query       |
|                       |     in the header.    | Explorer you can      |
|                       |                       | easily work on your   |
|                       |                       | query and validate    |
|                       |                       | the results.          |
+-----------------------+-----------------------+-----------------------+
| ![](media/image23.png | 1.  Run the default   | As you can see these  |
| ){width="2.0280369641 |     query: **SELECT   | queries are           |
| 29484in"              |     TOP 100 \* FROM   | constructed using a   |
| height="2.27668635170 |     c**               | familiar SQL like     |
| 6037in"}              |                       | syntax. You can also  |
|                       |                       | issue commands to     |
|                       |                       | filter, order and     |
|                       |                       | simple TOP commands.  |
|                       |                       |                       |
|                       |                       | Each ticket has a     |
|                       |                       | Location object       |
|                       |                       | within its document.  |
|                       |                       | This is specified     |
|                       |                       | using the             |
|                       |                       | GeoJSON format.       |
|                       |                       |                       |
|                       |                       | ![](media/image24.png |
|                       |                       | ){width="2.1662182852 |
|                       |                       | 143483in"             |
|                       |                       | height="0.97479877515 |
|                       |                       | 31059in"}             |
|                       |                       |                       |
|                       |                       | We will use this      |
|                       |                       | information to run    |
|                       |                       | our geospatial query. |
+-----------------------+-----------------------+-----------------------+
| ![](media/image25.png | 1.  Copy the below    | Using the geospatial  |
| ){width="2.8747484689 |     query into        | function ST\_WITHIN   |
| 413825in"             |     **Query           | we can easily         |
| height="3.13814195100 |     Explorer** and    | determine if the      |
| 6124in"}              |     **Run query**.    | Location of the       |
|                       |                       | incident is within    |
|                       | SELECT x.Id, x.Type,  | the polygon defined   |
|                       | x.Location FROM x     | by a set of           |
|                       |                       | coordinates.          |
|                       | WHERE                 |                       |
|                       |                       | But what if we wanted |
|                       | ST\_WITHIN(x.Location | to find the closest   |
|                       | ,                     | incidents (rather     |
|                       | { "type": "Polygon",  | then look at          |
|                       | "coordinates": \[\[   | incidents within a    |
|                       | \[-122.022430,        | polygon)?             |
|                       | 47.584540\],          |                       |
|                       | \[-122.022217,        |                       |
|                       | 47.580847\],          |                       |
|                       | \[-122.014425,        |                       |
|                       | 47.577210\],          |                       |
|                       | \[-122.003700,        |                       |
|                       | 47.577954\],          |                       |
|                       | \[-122.003914,        |                       |
|                       | 47.584410\],          |                       |
|                       | \[-122.022430,        |                       |
|                       | 47.584540\] \]\]})    |                       |
+-----------------------+-----------------------+-----------------------+
|                       | 1.  Copy the below    | Using the geospatial  |
|                       |     query into        | function ST\_DISTANCE |
|                       |     **Query           | we can filter         |
|                       |     Explorer** and    | incidents which have  |
|                       |     **Run query**.    | occurred close to a   |
|                       |                       | location.             |
|                       | SELECT x.Id, x.Type,  |                       |
|                       | x.Location FROM x     | There are two other   |
|                       |                       | geospatial methods    |
|                       | WHERE                 | currently available:  |
|                       | ST\_DISTANCE(x.Locati | ST\_ISVALID and       |
|                       | on,                   | ST\_ISVALIDDETAILED.  |
|                       | {'type': 'Point',     | These return a        |
|                       | 'coordinates':\[-122. | validation result of  |
|                       | 022430,               | your GeoJSON points / |
|                       | 47.584540\]}) &gt; 10 | polygon.              |
+-----------------------+-----------------------+-----------------------+
|                       | 1.  Switch back to    | Cosmos DB has allowed |
|                       |     the **First       | the First Response    |
|                       |     Response**        | app to quickly        |
|                       |     application       | provide important     |
|                       |                       | context for the       |
|                       |                       | responding officer.   |
|                       |                       | They can identify     |
|                       |                       | whether this is a     |
|                       |                       | high risk area – e.g. |
|                       |                       | for accidents – as    |
|                       |                       | part of deciding      |
|                       |                       | whether to write a    |
|                       |                       | ticket.               |
+-----------------------+-----------------------+-----------------------+
| ![](media/image26.png | 1.  Click             | The officer now exits |
| ){width="3.1359995625 |     **Identify**.     | the vehicle and       |
| 546806in"             |                       | approaches vehicle    |
| height="4.73976596675 |                       | and driver. Based on  |
| 4156in"}              |                       | the prior toast       |
|                       |                       | notification, we know |
|                       |                       | there has been a      |
|                       |                       | minor accident and    |
|                       |                       | that vehicle driver   |
|                       |                       | is in shock.          |
|                       |                       |                       |
|                       |                       | Responding officer    |
|                       |                       | asks: “Are you OK     |
|                       |                       | sir? Can I please     |
|                       |                       | have your name?”      |
+-----------------------+-----------------------+-----------------------+
| ![](media/image27.png | 1.  Type in “Joe” to  | Driver responds with  |
| ){width="2.9421172353 |     **Identity        | what the officer      |
| 45582in"              |     Search** textbox  | hears as “Joe.”       |
| height="2.57600065616 |     and click         |                       |
| 7979in"}              |     **Search**.       | We can make use of    |
|                       |                       | the power of Azure    |
|                       |                       | Search to assist in   |
|                       |                       | identifying the       |
|                       |                       | driver of the vehicle |
|                       |                       | with a similar        |
|                       |                       | sounding name.        |
+-----------------------+-----------------------+-----------------------+
|                       | 1.  Select **Joao     | The officer see that  |
|                       |     Casqueiro**.      | the driver in the     |
|                       |                       | image is the same as  |
|                       | 2.  Click **Done**.   | seen in the           |
|                       |                       | application, this     |
|                       |                       | assisting in          |
|                       |                       | confirmation with the |
|                       |                       | name of the person    |
|                       |                       | being pulled over is  |
|                       |                       | Joao.                 |
|                       |                       |                       |
|                       |                       | Let’s see how this is |
|                       |                       | achieved using Azure  |
|                       |                       | search.               |
+-----------------------+-----------------------+-----------------------+
|                       | 1.  Switch across to  | Now that we have seen |
|                       |     **Visual          | how this feature      |
|                       |     Studio**.         | works, let’s look at  |
|                       |                       | how it’s implemented. |
|                       | 2.  Open              | Let’s run through a   |
|                       |     **MSCorp.FirstRes | simple query against  |
|                       | ponse.WebApiDemo.sln* | an Azure Search       |
|                       | *.                    | index. We’ll spin up  |
|                       |                       | the service locally   |
|                       |                       | and debug through it. |
+-----------------------+-----------------------+-----------------------+
|                       | 1.  Open              | We are using the      |
|                       |     **PersonControlle | Azure Search client   |
|                       | r.cs**                | library delivered     |
|                       |     and place a       | through NuGet.        |
|                       |     breakpoint on the | Alternatively, we     |
|                       |     line of           | could make use of the |
|                       |     **Search** which  | Rest API.             |
|                       |     sets the          |                       |
|                       |     **scoringParam**  | As you can see with   |
|                       |     variable.         | just a few lines of   |
|                       |                       | code we can issue a   |
|                       | 2.  Press **F5**.     | query to return       |
|                       |                       | person results.       |
+-----------------------+-----------------------+-----------------------+
|                       | 1.  Open              | Let’s trigger our     |
|                       |     **PowerShell**.   | service using         |
|                       |                       | PowerShell.           |
|                       | <!-- -->              |                       |
|                       |                       |                       |
|                       | 1.  Run Step \#6 from |                       |
|                       |     file              |                       |
|                       |     **ShardMapManager |                       |
|                       | .ps1**                |                       |
|                       |                       |                       |
|                       | <!-- -->              |                       |
|                       |                       |                       |
|                       | 1.  Wait for the      |                       |
|                       |     breakpoint to be  |                       |
|                       |     hit and switch    |                       |
|                       |     back to Visual    |                       |
|                       |     Studio.           |                       |
+-----------------------+-----------------------+-----------------------+
| ![](media/image28.png | 1.  Switch back to    | Within our search     |
| ){width="3.4097222222 |     **PersonControlle | results, we want to   |
| 222223in"             | r.cs**.               | return people whose   |
| height="1.72916666666 |                       | home location is      |
| 66667in"}             |                       | closest to the        |
|                       |                       | incident higher up    |
|                       |                       | the list.             |
|                       |                       |                       |
|                       |                       | This ordering is      |
|                       |                       | achieved using a      |
|                       |                       | scoring profile.      |
+-----------------------+-----------------------+-----------------------+
| ![](media/image29.png | 1.  Open              | Using scoring         |
| ){width="3.2056069553 |     **Deploy\\AzureSe | profiles we can       |
| 805773in"             | arch\\PersonIndex.jso | quickly weight        |
| height="2.49588801399 | n**.                  | particular documents  |
| 825in"}               |                       | based attributes of   |
|                       | 2.  Scroll to         | the document to be    |
|                       |     **“scoringProfile | returned above        |
|                       | s”**.                 | others.               |
|                       |                       |                       |
|                       |                       | In this case, we are  |
|                       |                       | using a               |
|                       |                       | referencePoint        |
|                       |                       | (Officer’s location)  |
|                       |                       | compared to the       |
|                       |                       | fieldname             |
|                       |                       | (HomeLocation) and    |
|                       |                       | boosting the score by |
|                       |                       | a factor of 35.       |
+-----------------------+-----------------------+-----------------------+
| ![](media/image30.png | 1.  Switch back to    | Here we are passing   |
| ){width="3.4097222222 |     debugging code    | in the officer’s      |
| 222223in"             |                       | location as a scoring |
| height="1.55069444444 | 2.  Press F5          | profile parameter.    |
| 44444in"}             |                       | This is what Azure    |
|                       |                       | Search uses to        |
|                       |                       | compare against the   |
|                       |                       | HomeLocation of the   |
|                       |                       | person.               |
+-----------------------+-----------------------+-----------------------+
| ![](media/image31.png | 1.  Switch back to    | This determines the   |
| ){width="3.375in"     |     PowerShell        | score. By default,    |
| height="0.97013888888 |     window.           | results are ordered   |
| 88889in"}             |                       | with the highest      |
|                       | 2.  Run the following | score first as you    |
|                       |     command           | can see in the        |
|                       |     **\$searchResults | results list.         |
|                       | .Results**            |                       |
|                       |                       | Also note that there  |
|                       |                       | are people who have   |
|                       |                       | been returned who     |
|                       |                       | have a name other     |
|                       |                       | than “Joe”. Using the |
|                       |                       | power of Azure Search |
|                       |                       | we are able to        |
|                       |                       | trigger a Phonetic    |
|                       |                       | Search across the     |
|                       |                       | name.                 |
|                       |                       |                       |
|                       |                       | Phonetic searching    |
|                       |                       | enables searching     |
|                       |                       | based on how a word   |
|                       |                       | sounds, not how it’s  |
|                       |                       | spelled. This is      |
|                       |                       | configured as part of |
|                       |                       | the index. Let’s have |
|                       |                       | a look at the index   |
|                       |                       | structure.            |
+-----------------------+-----------------------+-----------------------+
| ![](media/image32.png | 1.  Open file         | Enabling phonetic     |
| ){width="3.375in"     |     **Deploy\\AzureSe | search is a simple    |
| height="0.92777777777 | arch\\PeopleIndex.jso | two-step process.     |
| 77778in"}             | n**                   |                       |
|                       |     and scroll to     | First: activate a     |
|                       |     analyzers.        | custom analyzer (in   |
|                       |                       | preview) which lets   |
|                       |                       | the index know how    |
|                       |                       | about phonetic search |
|                       |                       | capability. Note: see |
|                       |                       | “phonetic” token      |
|                       |                       | filter.               |
+-----------------------+-----------------------+-----------------------+
| ![](media/image33.png | 1.  Scroll to         | Second: link the      |
| ){width="2.3978499562 |     **FirstName** /   | fields which you want |
| 55468in"              |     **LastName**      | to apply phonetic     |
| height="2.32368985126 |     columns in        | search across. See    |
| 85915in"}             |     **Deploy\\AzureSe | how we define the\    |
|                       | arch                  | “analyzer” as         |
|                       |     \\PeopleIndex.jso | “phonetic” on the     |
|                       | n**.                  | FirstName and         |
|                       |                       | LastName columns.     |
|                       |                       |                       |
|                       |                       | Having completed      |
|                       |                       | these two steps, we   |
|                       |                       | have now enabled      |
|                       |                       | phonetic search       |
|                       |                       | across a search       |
|                       |                       | index.                |
+-----------------------+-----------------------+-----------------------+
| ![](media/image34.png | 1.  Scroll to         | As we don’t know the  |
| ){width="2.9230019685 |     **HairColor** /   | user’s full name and  |
| 03937in"              |     **Sex** columns   | phonetic search may   |
| height="2.59882545931 |     in                | return a large range  |
| 75855in"}             |     **Deploy\\AzureSe | of results, we need a |
|                       | arch\\PeopleIndex.jso | way to show           |
|                       | n**.                  | categories to help    |
|                       |                       | the user identify the |
|                       |                       | driver of the         |
|                       |                       | vehicle.              |
|                       |                       |                       |
|                       |                       | This is easily        |
|                       |                       | achieved using        |
|                       |                       | another out of the    |
|                       |                       | box feature: facets.  |
|                       |                       | Facets will help the  |
|                       |                       | responding officers   |
|                       |                       | of the First Response |
|                       |                       | application to        |
|                       |                       | identify the user     |
|                       |                       | when they are unable  |
|                       |                       | to effectively        |
|                       |                       | communicate.          |
|                       |                       |                       |
|                       |                       | To use facets, we     |
|                       |                       | need to first tell    |
|                       |                       | Azure Search that it  |
|                       |                       | is a column which can |
|                       |                       | be “facetable”.       |
+-----------------------+-----------------------+-----------------------+
| ![](media/image35.png | 1.  Switch back to    | Also, when the search |
| ){width="3.375in"     |     **PersonControlle | is issued we need to  |
| height="1.00208333333 | r.cs                  | tell Azure Search we  |
| 33334in"}             |     in** Visual       | want the results      |
|                       |     Studio.           | returned with facets. |
|                       |                       |                       |
|                       |                       | We can do this by     |
|                       |                       | sending the columns   |
|                       |                       | to be faceted with    |
|                       |                       | the search in the     |
|                       |                       | SearchParameters.     |
+-----------------------+-----------------------+-----------------------+
| ![](media/image36.png | 1.  Switch back to    | Notice that we are    |
| ){width="3.4938910761 |     PowerShell        | returning only        |
| 154855in"             |     window.           | results being faceted |
| height="0.37383202099 |                       | by “Sex”.             |
| 737534in"}            | 2.  Run the following |                       |
|                       |     command           |                       |
|                       |     **\$searchResults |                       |
|                       | .Facets**             |                       |
+-----------------------+-----------------------+-----------------------+
|                       | 1.  Switch back to    | We’ve seen how we can |
|                       |     the **First       | use Azure Search to   |
|                       |     Response**        | identify the driver   |
|                       |     application.      | of the vehicle. Next, |
|                       |                       | let’s look at writing |
|                       | 2.  Click over to     | a ticket.             |
|                       |     **Tickets** tab.  |                       |
|                       |                       | First, we need to     |
|                       | 3.  Click on          | determine the ticket  |
|                       |     “**Select         | type. As you can see  |
|                       |     Ticket**” drop    | for each ticket type, |
|                       |     down.             | we have several       |
|                       |                       | different ticket      |
|                       | 4.  Scroll through    | categories and        |
|                       |     the **Ticket      | related attributes.   |
|                       |     types** showing   |                       |
|                       |     the different     |                       |
|                       |     ticket entry      |                       |
|                       |     forms.            |                       |
+-----------------------+-----------------------+-----------------------+
|                       | 1.  Select            | Cosmos DB’s           |
|                       |     **Traffic**.      | schema-less design    |
|                       |                       | allows us to store a  |
|                       | 2.  Driver: Joao      | range of different    |
|                       |     Casqueiro.        | tickets types in one  |
|                       |                       | database. For First   |
|                       | 3.  LicenseNumber:    | Response, this allows |
|                       |     ACK406.           | new ticket types &    |
|                       |                       | structures to be      |
|                       | 4.  Details: Reckless | added or even         |
|                       |     Driving.          | extended for certain  |
|                       |                       | regions without       |
|                       | 5.  Click **Submit**. | having to modify the  |
|                       |                       | structure of the      |
|                       |                       | database. This        |
|                       |                       | empowers First        |
|                       |                       | Response to rapidly   |
|                       |                       | iterate as            |
|                       |                       | requirements and      |
|                       |                       | customers change.     |
|                       |                       |                       |
|                       |                       | It also automatically |
|                       |                       | indexes the documents |
|                       |                       | and supports powerful |
|                       |                       | query operations like |
|                       |                       | geospatial (as        |
|                       |                       | previously covered).  |
+-----------------------+-----------------------+-----------------------+

Story 3: Innovate and differentiate
-----------------------------------

Supervising police officers require ‘up to the minute’ information about
incidents occurring across a city. Based on this information he or she
is able to take intelligent actions. Hence it is important that the
information is current, complete and presented in a way that is easy to
understand and interact with.

With Azure SQL in-memory OLTP, using a Rest API connection to Power BI,
the information presented on dashboards is current, enabling officers to
react in real-time.

So the supervisor has all of the required information available to them
in one place, it’s important to be able to combine data from multiple
sources. PowerBI does this, with a range of native connectors.

With Power BI, anyone can create rich and compelling stories that
perfectly visualize data and share insights in real time. It is a key
component of Microsoft’s market leading BI and Analytics platform, as
acknowledged in this year’s Gartner magic quadrant.

Evan Dodds uses the Supervisor Dashboard, built using Power BI, to gain
an understanding of the current situation across the city. He sees
summary information about incidents and their location.

He will observe that the current average response time (an indicator of
the police forces ability to respond) and the number of incidents have
been increasing over the past 3 hours.

To better understand what is happening he will drill into each of the
last 3 hours to see where incidents are occurring, what types of
incident these are and the themes that are emerging about these
incidents. Based on this information, and a list of current events, he
is able to determine that the Spring Jazz concert at Pine Lake is the
likely cause.

He decides to take action by deploying more officers to this location.
He reviews who is available, and has not exceeded their limit of hours,
and deploys these officers.

  -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  Screen                                                                                                                                                                        Click Steps                                                                                                                         Demo Script
  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ----------------------------------------------------------------------------------------------------------------------------------- -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                                                                                                                                                                1.  Click on **John Clarkson**                                                                                                      Let’s log out as the officer and see what extra functionality another type of user has.
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                2.  Click **Logout**                                                                                                                
                                                                                                                                                                                                                                                                                                                    

  ![](media/image4.png){width="2.3805555555555555in" height="3.296527777777778in"}                                                                                              1.  Enter Username : edodds                                                                                                         We are now logged in as Evan Dodds who is the supervisor.
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                2.  Enter Password : edodds                                                                                                         
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                3.  Click Right arrow                                                                                                               
                                                                                                                                                                                                                                                                                                                    

  ![](media/image38.png){width="2.078472222222222in" height="1.0159722222222223in"}                                                                                             1.  Click on link to Power BI dashboard                                                                                             Now as the supervisor, we have unlocked extra functionality to look at the incident and ticket reports.
                                                                                                                                                                                                                                                                                                                    

  ![C:\\Users\\gerard\\AppData\\Local\\Microsoft\\Windows\\INetCacheContent.Word\\powerbi.png](media/image39.png){width="3.7650820209973754in" height="2.7604166666666665in"}   1.  Explain that Power BI dashboards can combine data from multiple sources.                                                        Power BI dashboards can combine data from multiple sources.
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                2.  Observe that the dashboard combines incident information, stored in an Azure SQL db, and Ticket information stored in Doc db.   This dashboard demonstrates how you can combine incident information, stored in an Azure SQL db, and Ticket information stored in Doc db.
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                                                                                                                                                    A key metric for this police force is the ‘Current average response time’ as this is an indicator of their ability to respond.
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                                                                                                                                                    Also, the Average Incident Response Time has increased over the last 3 hours
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                                                                                                                                                    In memory OLTP ensures that the average response time data is ‘up to the minute’ and by passing data using Rest API the data in the dashboard reflects what is current in the database.
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                                                                                                                                                    *Note: There will need to be approx. 30 seconds of talking at this stage to allow time for the cards consuming Rest API data to update,* *demonstrating how real time OLTP works.*
                                                                                                                                                                                                                                                                                                                    
                                                                                                                                                                                                                                                                                                                    *Note: This dashboard is connected to csv extracts to allow re-use at a later date.*
  -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
