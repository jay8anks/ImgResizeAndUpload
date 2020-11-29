using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Drawing;
using System.Web.Hosting;
using System.IO;
using System.Drawing.Imaging;

public class Images
{
    public string name { get; set; }   

}


public class ImagesController : ApiController
{
    // POST api/values
    public string Post()
    {
        Uri unparsedUrl = Request.RequestUri;
        string query = unparsedUrl.Query;
        var queryParams = HttpUtility.ParseQueryString(query);

        string hashId = queryParams["hid"];
        string userName = queryParams["uid"];
        string fileName = queryParams["fnm"];

        string result = Request.Content.ReadAsStringAsync().Result;
        byte[] bytes = Convert.FromBase64String(result.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)[1]);
        string ServerBaseUrl = string.Empty;
        if (!fileName.ToLower().Contains(".pdf"))
        {
            Image image;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                Bitmap bmp = new Bitmap(image);
                bmp.Save(HostingEnvironment.MapPath("~/uploads/" + hashId + "_" + fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            ServerBaseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
        }
        else
        {     
            BinaryWriter Writer = null;
            string Name = HostingEnvironment.MapPath("~/uploads/" + hashId + "_" + fileName);

            using (Writer = new BinaryWriter(File.OpenWrite(Name)))
            {
                // Writer raw data                
                Writer.Write(bytes);
                Writer.Flush();
                Writer.Close(); // https://stackoverflow.com/questions/381508/can-a-byte-array-be-written-to-a-file-in-c
            }

            ServerBaseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
        }

        fileName = hashId + "_" + fileName;
        SaveToDataBase(hashId, userName, fileName);

        // If it isn't a PDF, create a thumbnail.
        if (!fileName.ToLower().Contains(".pdf"))
        {
            string actualImagePath = "~/uploads/";
            string thumbName = "thumb_" + fileName;
            CreateThumb(actualImagePath + fileName, actualImagePath + thumbName, 300, 300);
        }

        return ServerBaseUrl + "/uploads/" + fileName;
    }

    protected void SaveToDataBase(string hashId, string userName, string fileName)
    {
        // Save to DataBase
    }

    
    // Create Thumbnail source = 
    // https://www.infinetsoft.com/Post/How-to-generate-high-Quality-thumbnail-images-and-maintain-aspect-ration-in-asp-net-c/2504#.X8PTY_xMEn4

    public static void CreateThumb(string actualImagePath, string thumbnailPath, int thumbWidth, int thumbHeight)
    {
        Image orignalImage = Image.FromFile(HostingEnvironment.MapPath(actualImagePath));

        orignalImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
        orignalImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

        int newHeight = orignalImage.Height * thumbWidth / orignalImage.Width;
        int newWidth = thumbWidth;

        if (newHeight > thumbHeight)
        {
            newWidth = orignalImage.Width * thumbHeight / orignalImage.Height;
            newHeight = thumbHeight;
        }

        //Generate a thumbnail image
        Image thumbImage = orignalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

        // Saveresized picture
        var qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
        var quality = (long)100; //Image Quality 
        var ratio = new EncoderParameter(qualityEncoder, quality);
        var codecParams = new EncoderParameters(1);
        codecParams.Param[0] = ratio;
        //Rightnow I am saving JPEG only you can choose other formats as well
        var codecInfo = GetEncoder(ImageFormat.Jpeg);


        thumbImage.Save(HostingEnvironment.MapPath(thumbnailPath), codecInfo, codecParams);

        //Dispose unnecessory objects
        orignalImage.Dispose();
        thumbImage.Dispose();
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
}