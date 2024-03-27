**Simple Image Uploader API**

This C# .NET 6 Web API is able to receive an image and return a list of labels using the Google Cloud Vision API.
Response: 

{
  "data": {
    "id": "481b53d4-b5c9-4454-a1dd-03cdcb7aa51f",
    "caption": "Food,Plant,White,Fruit,Natural foods,Banana,Staple food,Superfood,Produce,Banana family",
    "url": "https://gilbertngandu.com/cloud_developer/aws/azure_solutions_architect",
    "createdDateTime": "2024-03-26T21:06:18.9040081-04:00"
  },
  "message": null,
  "statusCode": 200,
  "success": true
}


![MarineGEO circle logo](/ImageUploader.Tests/Resources/applesandbananassong.jpg "Image of a banana and an apple")


This project includes examples of how to integrate with the following API:
- Cloudinary
- GoogleCloud Vision(Google.Cloud.Vision.V1)
- Azure Cosmos DB NoSQL
- OpenId/OAuth to authenticate with Azure

  This project also includes a number of important concepts including:
  - Dependency injection
  - OpenApi with Swagger
  - Artificial Intelligence with Google Cloud Vision
  - Unit Test

**Dependencies**
- Azure Cosmos NoSQL DB https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/quickstart-portal
- Azure Emulator can be used locally (https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=windows%2Ccsharp&pivots=api-nosql)
- Google Credentials https://developers.google.com/workspace/guides/create-credentials
- Cloudinary: Register and get the cloudniary Url that looks like cloudinary://7474747474747gilbertngandu ([)](https://cloudinary.com/users/register_free)https://cloudinary.com/users/register_free
- Setup an application in Azure, https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-register-app

Contact me at https://www.linkedin.com/in/gilbert-ngandu-software-developer/
