using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public partial class Asp_NetControls1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetCaptchaText();
            ShowSelectedCategory("industry", "mychild", "INDI12345");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    protected void btnupload_Click(object sender, EventArgs e)
    {
        string inputContent;
        using (StreamReader inputStreamReader = new StreamReader(fileUpload1.PostedFile.InputStream))
        {
            inputContent = inputStreamReader.ReadToEnd();
        }
    }

    /// <summary>
    /// for alphanumeic only "^(?=.*?[0-9])(?=.*?[A-Za-z])[a-zA-Z0-9_]+$"
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btncheckregex_Click(object sender, EventArgs e)
    {
        Regex regExpress = new Regex(txtRegex.Text.Trim(), RegexOptions.IgnoreCase);

        //if (regExpress.IsMatch(txtinput.Text.Trim()))
        //{
        //    ltrop.Text = "Matched";
        //    ltrop.ForeColor = System.Drawing.Color.Green;
        //}
        //else
        //{
        //    ltrop.Text = "Not Matched";
        //    ltrop.ForeColor = System.Drawing.Color.Red;
        //}



        if (Regex.IsMatch(txtinput.Text.Trim(), txtRegex.Text.Trim(), RegexOptions.IgnoreCase))
        {
            ltrop.Text = "Matched";
            ltrop.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            ltrop.Text = "Not Matched";
            ltrop.ForeColor = System.Drawing.Color.Red;
        }



    }

    private void ShowSelectedCategory(string parentCategory, string childCategory, string code)
    {
        StringBuilder sb = new StringBuilder();

        //sb.Append("<span style='margin-top: 3px; float:right; display:block;' id='mclose" + parentCategory + "-" + code + "'>");
        //sb.Append("<a onclick=\"DeleteMcat('" + parentCategory + "|" + code + "');\" ");

        //sb.Append("style='display:block;'");
        //sb.Append(" href='javascript:void(0);'>");
        //sb.Append("<img border='0' src='localhost:35131/sapphirenow/en/App_Themes/Default/images/btn_close.png'/></a></span>");

        sb.Append("<span id=\"mclose" + parentCategory + "_" + code + "\" style=\"margin-top: 3px; float:right;\"><a href=\"javascript:void(0);\" onclick=\"DeleteMcat('" + parentCategory + "|" + code.ToLower() + "') \"><img src=\"App_Themes/Default/images/btn_close.png\" border=\"0\" /></a></span>");

        ltrrender.Text = sb.ToString();
        // <span style="margin-top: 3px; float:right;" id="mcloseIndustry_inda000014"><a onclick="DeleteMcat('Industry|inda000014') " href="javascript:void(0);">
        //<img border="0" src="App_Themes/Default/images/btn_close.png"/></a></span>


    }

    private void SetCaptchaText()
    {
        Random oRandom = new Random();
        int iNumber = oRandom.Next(100000, 999999);
        Session["Captcha"] = iNumber.ToString();
    }

    protected void btncheckcaptcha_Click(object sender, EventArgs e)
    {
        if (Session["Captcha"].ToString() != txtCaptcha.Text.Trim())
        {
            lblmsg.Text = "does not match";
        }
        else
        {
            lblmsg.Text = "it is matching";
        }
    }

}