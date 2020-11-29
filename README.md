# ImgResizeAndUpload
Client-side image resize before uploading.

Working example of compressing/resizing an image client side and then uploading it.
The vast majority of this code is provided by OneZeroEight and is located at:
https://onezeroeight.co/code/web/compress-image-before-after-upload/

A big thanks to these guys!

The big additions added here are:

1) The ability to handle a PDF.
2) The ability to pass file names and other parameters to the WebAPI.
3) Creates an image thumbnail now.
4) Sets up the ability to save everything to a database.

The thumbnail code was found at infinetsoft.com:
https://www.infinetsoft.com/Post/How-to-generate-high-Quality-thumbnail-images-and-maintain-aspect-ration-in-asp-net-c/2504#.X8PTY_xMEn4

Without the client-side-image resizing before upload, this is similar to a code I have used for several years to attach images to items that needed images attached.

While this example is slightly different the main points are:

For every table row, add a unique hash code. Add this to the image name when saving it to the database. Save the image path and the filename, either together or in separate columns. The key things I save in the database are:

path + file name, just the file name, path + thumbnail name, the hash code, login of the person uploading the image, the DateTime it was uploaded.

Of course, you could save the path and file name in separate columns and combine them in the SQL query.

Also, I add one more variable which is "type." So every image isn't saved in the same directory, I add a type of image being saved. This is used to pull the correct path for the image. This allows you to use the same code to save images to different directories. Using this method, you can group images into their proper directories.


To display an image gallery for an item, search all images with the hash code in the filename (using wildcards).

Note: You might want to add a check to make sure the hash codes are unique. However, between the hash and the file name, I've never had a hash duplicate or create a duplicate file name.

After uploading the images, I bind the thumbs to an image gallery. When you click on the thumbnail, it loads up the image using colorbox (https://www.jacklmoore.com/colorbox/).

The demo doesn't require a login. For the WebAPI security, you can add [Authorize] to the controller and this will check that calls are coming from someone logged into the site. Since this demo doesn't require a login, it would be kind of pointless to turn this on. See:

-=-=-=-

   Using the [Authorize] Attribute

   Web API provides a built-in authorization filter, AuthorizeAttribute. This filter checks
   whether the user is authenticated. If not, it returns HTTP status code 401 (Unauthorized),
   without invoking the action.

   See: https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/
   
-=-=-=-   

The absolute hardest things in this project was dealing with the following errors:

https://stackoverflow.com/questions/40727898/system-web-routing-routecollection-does-not-contain-a-definition-for-maphttpr
https://stackoverflow.com/questions/27250581/could-not-load-file-or-assembly-system-web-http-version-5-2-2-0

At some point my web.config was showing Asp.Net 4.0 as its target. Nuget won't install all the needed dll files for a WebAPI unless the project is using 4.5 or greater. This left the bin missing a crucial dll file and threw odd intellisense and runtime errors.

Also, on deploying to a production server, there was mixed results with working on this section of the web.config file:

<runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="DllName" publicKeyToken="f94615aa0424f9eb" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-1.31.1789.0" newVersion="1.31.1789.0" />
      </dependentAssembly>
</runtime>

Adding some similar code to the web.config of a production server seemed to fix a complie error, but this demo project worked fine with or without this section of the code. Stackoverflow has numerous answers where either adding or removing this section to the web.config fixed or caused the problems.

One other thing with moving to a production server: MAKE SURE TO COPY ALL THE NEEDED .DLL FILES.

Using The Code:

This is a straight Asp.Net website. Fire it up in IIS or open it using Visual Studio and view the default.aspx page in a browser.

Using IIS, browse to the site and open up the default.aspx page. Click on the "Attch" button in the GridView.
On the page that opens up, upload an image or PDF.

Of course, this requires users in the user table. If you click "Delete All," you are going to have to add some users back before this demo works.

Todo:

In production, I actually encrypt and decrypt all URL variables. Though not hard, That was a little beyond the scope of this project.

Thumbnails of PDFs are also created. This was pretty involved and is also beyond the scope of this project.

If you open up the upload page in a new tab, you can close the tab using javascript and not have to worry with back buttons. This also makes it easy to use the page with different types of uploads. For example, user image attachments and product images can be uploaded from the same upload page. You just have to tell it what type of page called it, and code accordingly to pass images the correct paths to the appropriate directory.

See the OneZeroEight link for more info on CORS. If you have the WebAPI located at a diffrent domain than the domain that is sending the data to it, you have to turn on CORS. Since that isn't the case with this demo, it is not turned on.




