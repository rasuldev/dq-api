using DrinqWeb.Tools.Images;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DrinqWeb.Tools.Image
{
    public class ImageFactory
    {
        public ImageFactory()
        {

        }

        public void SaveImage(HttpPostedFile imageFile, ImageKind kind)
        {
            string fileName = GetPath(kind) + GenerateUniqueId() + imageFile.FileName;
            imageFile.SaveAs(fileName);
        }

        public string GetPath(ImageKind kind)
        {
            string path = HttpContext.Current.Server.MapPath("~/images");
            switch (kind)
            {
                case ImageKind.VerificationItem:
                    return $"{path}/items/";
                default:
                    return $"{path}/";
            }
        }
        public static string GenerateUniqueId()
        {
            return $"image_{DateTime.Now:yyyy-MM-ddTHH-mm-ss-ff}_{Path.GetRandomFileName().Replace(".", "")}";
        }
    }
}