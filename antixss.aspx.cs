using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Util;

public partial class antixss : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
         // var t1 = Request.Form["ctl00$MainContent$txtName"];
          //  var t2 = Request.Unvalidated.Form["ctl00$MainContent$txtEmail"];
          //  Response.Write("txtName: " + t1 + " txtEmail: " + t2);


        string aa = Sanitizer.GetSafeHtml(txtName.Text);
        string bb = Sanitizer.GetSafeHtmlFragment(txtName.Text);
        string c = StripHTML(txtName.Text);
        string d = StripHTML(txtEmail.Text);

            Response.Redirect("AntiXSS-2.aspx?Name=" + txtName.Text);
    }

    public string StripHTML(string text)
    {
        return Regex.Replace(text == null ? "" : text, @"<(.|\n)*?>", string.Empty);
    }

}

public class AntiXssEncoder : HttpEncoder
{
    public AntiXssEncoder() { }

    protected override void HtmlEncode(string value, TextWriter output)
    {
        output.Write(AntiXss.HtmlEncode(value));
    }

    protected override void HtmlAttributeEncode(string value, TextWriter output)
    {
        output.Write(AntiXss.HtmlAttributeEncode(value));
    }

    protected override void HtmlDecode(string value, TextWriter output)
    {
        base.HtmlDecode(value, output);
    }

    protected override byte[] UrlEncode(byte[] bytes, int offset, int count)
    {
        //Can't call AntiXss library because the AntiXss library works with Unicode strings.
        //This override works at a lower level with just a stream of bytes, independent of 
        //the original encoding.

        //
        //Internal ASP.NET implementation reproduced below.
        //
        int cSpaces = 0;
        int cUnsafe = 0;

        // count them first
        for (int i = 0; i < count; i++)
        {
            char ch = (char)bytes[offset + i];

            if (ch == ' ')
                cSpaces++;
            else if (!IsUrlSafeChar(ch))
                cUnsafe++;
        }

        // nothing to expand?
        if (cSpaces == 0 && cUnsafe == 0)
            return bytes;

        // expand not 'safe' characters into %XX, spaces to +s
        byte[] expandedBytes = new byte[count + cUnsafe * 2];
        int pos = 0;

        for (int i = 0; i < count; i++)
        {
            byte b = bytes[offset + i];
            char ch = (char)b;

            if (IsUrlSafeChar(ch))
            {
                expandedBytes[pos++] = b;
            }
            else if (ch == ' ')
            {
                expandedBytes[pos++] = (byte)'+';
            }
            else
            {
                expandedBytes[pos++] = (byte)'%';
                expandedBytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
                expandedBytes[pos++] = (byte)IntToHex(b & 0x0f);
            }
        }
        return expandedBytes;
    }

    protected override string UrlPathEncode(string value)
    {
        //AntiXss.UrlEncode is too "pessimistic" for how ASP.NET uses UrlPathEncode

        //ASP.NET's UrlPathEncode splits the query-string off, and then Url encodes
        //the Url path portion, encoding any parts that are non-ASCII, or that
        //are <= 0x20 or >=0x7F.

        //Additionally, it is expected that:
        //                       UrPathEncode(string) == UrlPathEncode(UrlPathEncode(string))
        //which is not the case for UrlEncode.

        //The Url needs to be separated into individual path segments, each of which
        //can then be Url encoded.
        string[] parts = value.Split("?".ToCharArray());
        string originalPath = parts[0];

        string originalQueryString = null;
        if (parts.Length == 2)
            originalQueryString = "?" + parts[1];

        string[] pathSegments = originalPath.Split("/".ToCharArray());

        for (int i = 0; i < pathSegments.Length; i++)
        {
            pathSegments[i] = AntiXss.UrlEncode(pathSegments[i]);  //this step is currently too aggressive
        }

        return String.Join("/", pathSegments) + originalQueryString;
    }

    private bool IsUrlSafeChar(char ch)
    {
        if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9')
            return true;

        switch (ch)
        {

            //These are the characters ASP.NET considers safe by default
            //case '-':
            //case '_':
            //case '.':
            //case '!':
            //case '*':
            //case '\'':
            //case '(':
            //case ')':
            //    return true;

            //Modified list based on what AntiXss library allows from the ASCII character set
            case '-':
            case '_':
            case '.':
                return true;
        }

        return false;
    }

    private char IntToHex(int n)
    {
        if (n <= 9)
            return (char)(n + (int)'0');
        else
            return (char)(n - 10 + (int)'a');
    }

}