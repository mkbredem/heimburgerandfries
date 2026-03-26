<%@ Page Language="C#" AutoEventWireup="true" Inherits="SWsoft.SiteBuilder.Site.Helpers.WebControls.Error404Page"
    MasterPageFile="~/default.master" %>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <style type="text/css">
      BODY,td { font: 8pt/12pt verdana }
      H1 { font: 13pt/15pt verdana }
      H2 { font: 8pt/12pt verdana }
      A:link { color: red }
      A:visited { color: maroon }
    </style>
    <table width="500" border="0" cellspacing="10">
        <tr>
            <td>
                <asp:Label ID="LabelMessage" runat="server" EnableViewState="false" />
                <!--ERROR_PAGE-->
            </td>
        </tr>
    </table>
</asp:Content>
