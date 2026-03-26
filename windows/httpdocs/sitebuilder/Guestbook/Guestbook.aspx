<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Guestbook.aspx.cs" Inherits="Guestbook_Guestbook" EnableEventValidation="false" %>
<asp:content contentplaceholderid="content" runat="Server">
	<SiteBuilder:PagePanel ID="GuestbookPanel" runat="server">
		<Template>
			<SiteBuilder:StatusBarControl ID="StatusBar" runat="server" />
			<asp:DataList ID="MessageList" SkinID="MessageList" runat="server" Width="100%">
				<ItemTemplate>
					<table cellpadding="0" cellspacing="0" border="0" width="100%" style="border-collapse: collapse; margin-top: 10px;">
						<tr>
							<td class="mod-item-header" style="padding: 5px 10px;">
								<div style="float:left;">
									<b><asp:Label ID="Author" runat="server" Font-Bold="true"></asp:Label></b>
									<SiteBuilder:Link ID="MailTo" runat="server" Type="Image" ImageUrl="$DefaultMail$" ImageAlign="AbsMiddle" style="margin-left: 10px;" />
									<SiteBuilder:Link ID="HomePage" runat="server" Type="Image" ImageUrl="$DefaultWWW$" ImageAlign="AbsMiddle" style="margin-left: 10px;" />
								</div>
								<div style="float:right;">
									<asp:Label ID="Time" runat="server"></asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td class="mod-item-body" style="padding: 5px 10px;">
								<asp:Label ID="Message" runat="server"></asp:Label>
							</td>
						</tr>
					</table>
				</ItemTemplate>
			</asp:DataList>
			<SiteBuilder:PagerControl runat="server" ID="Pager" />
			<table cellpadding="10" cellspacing="0" border="0" width="100%" class="mod-form" style="margin: 10px 0;">
				<tr>
					<td class="mod-form-title"><b><asp:Label ID="AddMessageTitle" runat="server"/></b></td>
				</tr>
				<tr>
					<td style="padding: 0 10px;">
						<div class="mod-form-hr" style="height:1px;"><span></span></div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="AuthorCaption" runat="server" />
						<asp:RequiredFieldValidator Display="Static" ID="AuthorRequiredFieldValidator" runat="server" /><br/>
						<asp:TextBox ID="AuthorInput" CssClass="mod-input" runat="server" style="width: 100%" />
					</td>
				</tr>
				<tr>
					<td style="padding: 0 10px;">
						<asp:Label ID="MessageCaption" runat="server" />
						<asp:RequiredFieldValidator Display="Static" ID="MessageRequiredFieldValidator" runat="server" /><br />
						<asp:TextBox ID="MessageInput" runat="server" TextMode="Multiline" style="width: 100%; height: 87px;" />
					</td>
				</tr>
				<tr>
					<td align="right">
						<asp:Button ID="AddMessage" CssClass="mod-form-button" runat="server" />
					</td>
				</tr>
			</table>
		</Template>
	</SiteBuilder:PagePanel>
</asp:content>