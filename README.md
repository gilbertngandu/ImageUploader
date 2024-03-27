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
