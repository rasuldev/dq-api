using System;
using System.IO;
using System.Linq;
using System.Web;

namespace DrinqWeb.Tools.VerificationItem
{
    public class VerificationItemFactory
    {
        public VerificationItemFactory()
        {

        }

        public void SaveFile(HttpPostedFile verificationFile, FileKind kind)
        {
            string fileName = GetPath(kind) + GenerateUniqueId() + verificationFile.FileName;
            verificationFile.SaveAs(fileName);
        }

        public string GetPath(FileKind kind)
        {
            string path = HttpContext.Current.Server.MapPath("~/files");
            switch (kind)
            {
                case FileKind.VerificationItem:
                    return $"{path}/items/";
                default:
                    return $"{path}/";
            }
        }
        public static string GenerateUniqueId()
        {
            return $"item_{DateTime.Now:yyyy-MM-ddTHH-mm-ss-ff}_{Path.GetRandomFileName().Replace(".", "")}";
        }
    }
}