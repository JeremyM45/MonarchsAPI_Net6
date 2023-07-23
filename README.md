# MonarchsAPI
MonarchsAPI is a web API for a web app where users can rate and leave comments on their favorite Monarchs throughout history

### Technologies Used
-----
* C#
* ASP .NET Core 6.0
* SQL Server
* Entity Framework
* SwaggerUI
* OpenAPI
* JwT Authentication
* AutoMapper
----
### Description
MonarchsAPI allows for the fetching, creation, editing, and deleting of Monarchs, Dynasties, Users, and Ratings. A Monarch belongs to a Dynasty, a Rating belongs to a Monarch, and a User leaves a Rating for the Monarch. For example: The Monarch ***Frederick II*** belongs to the ***Von Hohenzollern*** Dynasty and the User ***TotallyUniqueUsername*** left a Rating of ***Value: 5, Comment: The Great*** for ***Frederick II Von Hohenzollern***. JwT Authentication is used to verify Users and Admins. Only Admins may create, edit, or remove a Monarch or Dynasty. Only an authenticated User can leave a Rating and they can only create one Rating per Monarch. A User may only create or edit a Rating that belongs to that User and may only edit or delete themselves. An Admin can edit, create, and delete any Rating or User.

---
### Setup
After cloning the repo add an appsettings.json file in the /MonarchsAPI_Net6 directory and paste the following code in the appsettings file 
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": [YOUR CONNECTION STRING HERE] 
  },
  "AppSettings": {
    "Token": [YOUR JWT SECRET HERE]
  }
}
```
And replace the [YOUR CONNECTION STRING HERE] with the connection string to your SQL database and replace [YOUR JWT SECRET HERE] with your JwT Secret

An example of a connection string connecting to a local SQL database is 
```
"Server=.\\SQLExpress;database=MonarchsDb;Trusted_Connection=true;TrustServerCertificate=true;"
```

An example of a JwT secret is
```
"BvPbKGLk9MWmdPv9QcCA0mBQap3J4ZtZgMZBLLucsu8Hs3xpJ3y2VQRzZ8GTkklA1xu1xVr15d7hUkUF"
```

### **FOR SECURITY DO NOT USE THESE EXAMPLES IN YOUR PRODUCTION ENVIROMENT**

Once your appsettings.json is configured you can run
```
dotnet ef database update
```
in your VS package manger to apply the Entity migrations 

---
### Using The API
---
#### User End Points

|Action| End Point|
|:---------|:-------|
|GET|api/User|
|POST| api/User|
|DELETE| api/User|
|PUT| api/User|
|GET| api/User/{id}|
|GET| api/User/LoginUser|

To get all users simply run hostedurl/api/User as a GET method
**Example from Swagger:**
 ![](https://i.gyazo.com/472b3b0135f1cf5e021384723037b030.png)


To Get a User by their id run the hostedurl/api/User/{id} as a GET method
**Example from Swagger: **
![](https://i.gyazo.com/4752d1f888d1f07cbf58603b3d3c843a.png)

To Register a new user run hostedurl/api/User as a POST method with a header containing a **userName**, **password**, and **email**
**Example from Swagger:**
![](https://i.gyazo.com/cfa980208427d5e3037165d2312fc62e.png)

To Login a User run the hostedurl/api/User/LoginUser endpoint as a GET method with the **UserName** and **Password** in the in the url.
**Example from Swagger:**
![](https://i.gyazo.com/fadafd4b7106f0a8e4b82b2bd6d9e3ef.png)

To Edit a User  run the hostedurl/api/User endpoint as PUT method and with the **username**, **newUsername**, **password**, **newPassword**, **email**, and **newEmail** values in the header along with **bearer** then the User's Jwt or any Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/8d27e41debce4ba0b9075505b4d6ad42.png)


To Delete a User run the hostedurl/api/User endpoint as a DELETE method and the **userId** and **password** in the header along with **bearer** then the User's Jwt or any Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/f6aa0057b338aafa3153e40b0886c84c.png)

---

#### Admin Endpoints

|Action| End Point|
|:---------|:-------|
|GET|api/Admin/all|
|GET| api/Admin/from-id|
|DELETE| api/Admin|
|POST| api/Admin/register|
|POST| api/Admin/login|

### To access any Admin endpoint besides api/Admin/login you must have a valid Admin Jwt in the Authorization field. To Create your first Admin you can either manually seed the data in your database or temporarily remove the Authorize(Roles = "Admin") from the RegisterAdmin function in the AdminController.cs file 
![](https://i.gyazo.com/41761de55577e654ed6d33f969ae4f66.png)

To Login as an Admin run the hostedurl/api/Admin/login endpoint as a POST method with the Admin's **username** and **password** in the header.
**Example from Swagger:** 
![](https://i.gyazo.com/77f6bcd51f0f8c577b25706bd2348c10.png)

To Register a new Admin run the hostedurl/api/Admin/register endpoint as a POST method with the new Admin's **username** and **password** with an existing Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/79231db51a057a1af63d1b46b9a9ea53.png)

To Get all Admins run the hostedurl/api/Admin/all endpoint as a GET method and a valid Admin's Jwt in the Authorization field
**Example from Swagger:**
![](https://i.gyazo.com/78851f49138f8b92e201b95e5f7ec532.png)

To Get an Admin by their id run the hostedurl/api/Admin/from-id endpoint followed by the id of the Admin as a GET method and a valid Admin's Jwt in the Authorization field
**Example from Swagger:**
![](https://i.gyazo.com/c5db8f9d6271a3d1c1ddc6161a018dab.png)

To Delete an Admin run the hostedurl/api/Admin endpoint as a DELETE method, with the id of the Admin to delete in the header, and a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/4802aba8e7e2048b70b92a1fcaae4754.png)

---

#### Monarch Endpoints

|Action| End Point|
|:---------|:-------|
|GET|api/Monarch|
|POST| api/Monarch|
|PUT| api/Monarch|
|DELETE| api/Monarch|
|GET| api/Monarch/GetAllMonarchsDashboard|
|GET| api/Monarch/{id}|

To Get all Monarchs run the hostedurl/api/Monarch endpoint as a GET method.
**Example from Swagger:**
![](https://i.gyazo.com/109e88c524b3b8ca7a79d5690637edf0.png)

To create a new Monarch run the hostedurl/api/Monarch endpoint as a POST method with **name**, **description**, **wikiLink**, **reign**, **dynastyId**, and **countryIds** in the header with a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/166bd25b2781ccaf4ad2d7c673001b00.png)

To Edit a Monarch run the hostedurl/api/Monarch endpoint as a PUT method with the **id**, **name**, **description**, **wikiLink**, **reign**, **dynastyId**, and **countryIds** in the header with a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/ce0e348fa258b0937f38417bc2e882d1.png)

To Delete a Monarch run the hostedurl/api/Monarch endpoint as a DELETE method with the Monarch's id in the url and a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/77666de7d4218d3544b26574f950d8ed.png)

To Get a all Monarchs without their ratings, dynasty, or country and still with their averageRating, dynastyId, and countryIds run the hostedurl/api/Monarch/GetAll/MonrachsDashboard as a GET method and with a valid Admin Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/3f22a9f93cec93b14f18290bf39be1c2.png)

To Get a Monarch by their id run the hostedurl/api/Monarch/{id} endpoint as a GET method with id in the url.
**Example from Swagger:**
![](https://i.gyazo.com/6b33bd20ba31545403dc6e06c536ca61.png)

---
#### Dynasty Endpoints

|Action| End Point|
|:---------|:-------|
|GET|api/Dynasty|
|POST| api/Dynasty|
|PUT| api/Dynasty|
|DELETE| api/Dynasty|
|GET| api/Dynasty/min|

To Get all Dynasties run the hostedurl/api/Dynasty endpoint as a GET method.
**Example from Swagger:**
![](https://i.gyazo.com/8b61add70805524dc192055083d302c4.png)

To add a new Dynasty run the hostedurl/api/Dynasty endpoint as a POST method with the **name** in the header and a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/1bfd9bdf163f05461dfaab9173dabeca.png)

To edit a Dynasty run the hostedulr/api/Dynasty endpoint as a PUT method with the **id** and **name** in the header and a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/b08eb1014fe57ecb841e3da055b4dc23.png)

To Delete a Dynasty run the hostedurl/api/Dynasty endpoint as a DELETE method with the Dynasty's id in the url and a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/d745dcebaaf23d66fae4b6547bf6d63d.png)

To Get all Dynasties with only their id's and name's run the hostedurl/api/Dynasty/min endpoint as a GET method.
**Example from Swagger:**
![](https://i.gyazo.com/cc23bf33d27652a3971d3e032bb6e3a0.png)

---
#### Country Endpoints

|Action| End Point|
|:---------|:-------|
|GET|api/Country|
|POST| api/Country|
|PUT| api/Country|
|DELETE| api/Country|
|GET| api/Country/min|

To Get all Countries run the hostedurl/api/Country endpoint as a GET method.
**Example from Swagger:**
![](https://i.gyazo.com/6904375ad3f2284b14ee0814574a9458.png)

To add a new Country run the hostedurl/api/Country endpoint as a POST method with the **name** in the header and a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/303d3cf633e4191f715e987bcac6577d.png)

To Edit a Country run the hostedurl/api/Country endpoint as a PUT method with the **id** and **name** in the header and a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/07a5c00ea0bd6e6f6089109d72b99747.png)

To Delete a Country run the hostedurl/api/Country endpoint as a DELETE method with the Country's id in the url and a valid Admin's Jwt in the Authorization field.
**Example from Swagger:**
![](https://i.gyazo.com/81a3635cebcc9f621612b0317718dc5b.png)

To Get all Countries only with their id's and name's run the hostedurl/api/Country/min endpoint as a GET method.
**Example from Swagger:**
![](https://i.gyazo.com/4da53bbfc3fcec93c3069f06f196db9a.png)

---
#### Rating Endpoints

|Action| End Point|
|:---------|:-------|
|GET|api/Rating|
|POST| api/Rating|
|PUT| api/Rating|
|DELETE| api/Rating|

To Get all Ratings run the hostedurl/api/Rating endpoint as a GET method.
**Example from Swagger:**
![](https://i.gyazo.com/7e4d3d3571bd24168d35e3d577a4b7a6.png)

To add a Rating run the hostedurl/api/Rating endpoint as a POST method with **ratingValue**, **comment**, **userId**, and **monarchId** with either a valid Admin's Jwt or the User's Jwt that corresponds to the userId.
**Example from Swagger:**
![](https://i.gyazo.com/6f9300b7ccd988207208717dc1ab8124.png)

To edit a Rating run the hostedurl/api/Rating endpoint as a PUT method with the **id**, **ratingValue**, and **comment** in the header and with either a valid Admin's Jwt or the User's Jwt that corresponds to the userId.
**Example from Swagger:**
![](https://i.gyazo.com/0f4edfe587847776c7a721e71262af1c.png)

To Delete a Rating run the hostedurl/api/Rating endpoint as a DELETE method with the **id** in the url and with either a valid Admin's Jwt or the User's Jwt that corresponds to the userId.
**Example from Swagger:**
![](https://i.gyazo.com/03d8fbf01aed7ce71836ba8cb8c5d86c.png)

---
### License

[MIT](https://opensource.org/licenses/MIT)

Copyright (c) _2023_  _Jeremy Martin_

