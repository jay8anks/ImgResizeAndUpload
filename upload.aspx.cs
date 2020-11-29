using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // The original URL:
        Uri unparsedUrl = new Uri(Request.Url.ToString());
        // Grabs the query string from the URL:
        string query = unparsedUrl.Query;
        // Parses the query string as a NameValueCollection:
        var queryParams = HttpUtility.ParseQueryString(query);

        ParseUrl(queryParams);

        // https://stackoverflow.com/questions/15713542/elegant-way-parsing-url
    }

    protected void ParseUrl(System.Collections.Specialized.NameValueCollection queryParams)
    {
        string hashId = queryParams["hid"];
        string userName = queryParams["uid"];   
        
        if (!String.IsNullOrEmpty(hashId))
        {
            ViewState["HashId"] = hashId;
            HiddenField1.Value = hashId;
        }
        else
        {
            // throw warning
        }

        if (!String.IsNullOrEmpty(userName))
        {
            ViewState["UserName"] = userName;
            HiddenField2.Value = userName;
        }
        else
        {
            // throw warning
        }

    }
}