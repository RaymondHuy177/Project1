using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace WebPhoHienTravel.App_Code
{
    public class _Logo
    {
//acdefefe
        private string masterPath;
        private WebPhoHienTravel.App_Code.DATACONNECT data;
        public _Logo(string masterPath)
        {
            this.masterPath = masterPath;
        }
        public string LoadData(string eventArg)
        {
            data = new WebPhoHienTravel.App_Code.DATACONNECT();
            string tmp = data.getValue("select 'type=' + type + '&url=' + url from Logo");
            return "arg=load&" + tmp;
        }
        public string Update(string eventArg)
        {
            data = new WebPhoHienTravel.App_Code.DATACONNECT();
            if (WebPhoHienTravel.App_Code.ProcessURL.getQuery("type", eventArg) == "default")
            {
                data.Query("update Logo set type='default', url='Images/logo.jpg'");
                File.Copy(masterPath + "default_logo.jpg", masterPath + "logo.jpg", true);
                return "arg=update&ac=success&type=default&url=Images/logo.jpg";
            }
            else if (WebPhoHienTravel.App_Code.ProcessURL.getQuery("type", eventArg) == "upload")
            {
                data.Query("update Logo set type='upload', url=N'/Images/logo.jpg'");
                return "arg=update&ac=success&type=upload&url=" + "/Images/logo.jpg";
            }
            else
            {
                string url = WebPhoHienTravel.App_Code.ProcessURL.getQuery("url", eventArg);
                data.Query("update Logo set type='web', url=N'" + url + "'");
                byte[] content = GetBytesFromUrl(url);
                WriteBytesToFile(masterPath + "logo.jpg", content);
                return "arg=update&ac=success&type=web&url=" + WebPhoHienTravel.App_Code.ProcessURL.getQuery("url", eventArg);
            }
        }
        private byte[] GetBytesFromUrl(string url)
        {
            byte[] b;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResp = myReq.GetResponse();
            Stream stream = myResp.GetResponseStream();
            using (BinaryReader br = new BinaryReader(stream))
            {
                b = br.ReadBytes(500000);
                br.Close();
            }
            myResp.Close();
            return b;
        }
        private void WriteBytesToFile(string fileName, byte[] content)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            w.Write(content);
        }
    }
}
