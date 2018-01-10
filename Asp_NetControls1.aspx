<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Asp_NetControls1.aspx.cs" Inherits="Asp_NetControls1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table align="center">
        <tr>
            <td>
                <b>Validation to File Upload Control</b>
                <br />
                <asp:FileUpload ID="fileUploadVideo" runat="server" />
            </td>
            <td>
                <asp:RegularExpressionValidator ID="rexp" runat="server" ControlToValidate="fileUploadVideo"
                    ErrorMessage="Only .vtt" ValidationExpression="(.*\.([Vv][Tt][Tt])$)" ValidationGroup="a"></asp:RegularExpressionValidator>
                <asp:Button ID="Button1" runat="server" Text="Save" OnClick="Button1_Click" ValidationGroup="a" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload ID="fileUpload1" runat="server" />
            </td>
            <td>
                <asp:Button ID="btnupload" runat="server" Text="Read File Content" OnClick="btnupload_Click" />
            </td>
        </tr>
    </table>
    <p>
        <asp:Literal ID="ltrrender" runat="server"></asp:Literal>
    </p>
    <script type="text/javascript">

        function Validate() {
            alert('in');
            if (!$('#MainContent_fileUploadVideo').hasExtension(['.vtt', '.VTT'])) {
                alert('valid');
                return true;
            }
            else {
                alert('in valid ');
                return false;
            }
        }
    </script>
    <asp:FileUpload ID="fpImages" runat="server" title="maximum file size 1 MB or less"
        onChange="return validateFileExtension(this, args)" />
    <br />
    <table>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="ltrop" runat="server" Font-Bold="true"></asp:Label></b>
            </td>
        </tr>
        <tr>
            <td>
                <b>Enter Input :</b>
            </td>
            <td>
                <asp:TextBox ID="txtinput" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <b>Enter Regex :</b>
            </td>
            <td>
                <asp:TextBox ID="txtRegex" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btncheckregex" runat="server" Font-Bold="true" Text="Click To Check"
                    OnClick="btncheckregex_Click" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label></td>
        </tr>
        <tr>
            <td><asp:Image ID="imgCaptcha" ImageUrl="Captcha.ashx" runat="server" /> &nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txtCaptcha" runat="server"></asp:TextBox>

            </td>
            <td><asp:Button ID="btncheckcaptcha" runat="server"  Text="Compare Captcha" OnClick="btncheckcaptcha_Click"  /></td>

        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        function DeleteMcat(input) {
            alert(input);
        }
        function validateFileExtension(Source, args) {

            var fuData = document.getElementById('<%= fpImages.ClientID %>');
            var FileUploadPath = fuData.value;

            if (FileUploadPath == '') {
                // There is no file selected 
                args.IsValid = false;
            }
            else {
                var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();
                if (Extension == "vtt" || Extension == "VTT" || Extension == "txt") {
                    args.IsValid = true; // Valid file type
                    FileUploadPath == '';
                }
                else {
                    args.IsValid = false; // Not valid file type
                }
            }
        }
    </script>
</asp:Content>
