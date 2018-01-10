using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Microsoft.Security.Application;

public partial class AntiXss_2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            String Name = Encoder.HtmlEncode(Request.QueryString["Name"]);
            AntiXss.HtmlEncode("test");

            String Name2 = Sanitizer.GetSafeHtmlFragment(Request.QueryString["Name"]);

            Response.Write(Name);
            Response.Write("<br/>");
            Response.Write(Name2);
            Response.Write("<br/>");
            //Response.Write(Request.QueryString["Name"]);

            txtop.Text = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "";

        }
    }
}

/*
public class MyReferer : HttpRequest
{
    public MyReferer()
    {
        
    }
    
}

 */


