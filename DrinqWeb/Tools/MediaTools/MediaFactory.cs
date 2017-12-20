using DrinqWeb.Models.CodeFirstModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DrinqWeb.Tools.MediaTools
{
    public class MediaFactory
    {
        String[] imgExtensions = { "ase", "art", "bmp", "blp", "cd5", "cit", "cpt", "cr2", "cut", "dds", "dib", "djvu", "egt", "exif", "gif", "gpl", "grf", "icns", "ico", "iff", "jng", "jpeg", "jpg", "jfif", "jp2", "jps", "lbm", "max", "miff", "mng", "msp", "nitf", "ota", "pbm", "pc1", "pc2", "pc3", "pcf", "pcx", "pdn", "pgm", "PI1", "PI2", "PI3", "pict", "pct", "pnm", "pns", "ppm", "psb", "psd", "pdd", "psp", "px", "pxm", "pxr", "qfx", "raw", "rle", "sct", "sgi", "rgb", "int", "bw", "tga", "tiff", "tif", "vtf", "xbm", "xcf", "xpm", "3dv", "amf", "ai", "awg", "cgm", "cdr", "cmx", "dxf", "e2d", "egt", "eps", "fs", "gbr", "odg", "svg", "stl", "vrml", "x3d", "sxd", "v2d", "vnd", "wmf", "emf", "art", "xar", "png", "webp", "jxr", "hdp", "wdp", "cur", "ecw", "iff", "lbm", "liff", "nrrd", "pam", "pcx", "pgf", "sgi", "rgb", "rgba", "bw", "int", "inta", "sid", "ras", "sun", "tga" };
        String[] videoExtensions = { "3g2", "3gp", "aaf", "asf", "avchd", "avi", "drc", "flv", "m2v", "m4p", "m4v", "mkv", "mng", "mov", "mp2", "mp4", "mpe", "mpeg", "mpg", "mpv", "mxf", "nsv", "ogg", "ogv", "qt", "rm", "rmvb", "roq", "svi", "vob", "webm", "wmv", "yuv" };
        public MediaType GetMediaType(string filename)
        {
            string ext = filename.Split('.').Last();
            if (imgExtensions.Contains(ext))
                return MediaType.Image;
            if (videoExtensions.Contains(ext))
                return MediaType.Video;
            return MediaType.Undefined;
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