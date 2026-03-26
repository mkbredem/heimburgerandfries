<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ImagePreview.aspx.cs" Inherits="ImageGallery_ImagePreview" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
	<SiteBuilder:PagePanel ID="ImageGalleryPanel" runat="server">
		<Template>
			<center>
			<table cellpadding="0" cellspacing="0" border="0" width="100%" style="margin: 10px 0; width: 500px;">
				<tr>
					<td colspan="3" class="mod-item-body">
						<table cellpadding="5" cellspacing="0" border="0" width="100%" style="border-collapse: collapse;">
							<tr>
								<td width="33%"><asp:LinkButton ID="LinkButtonPrevious" runat="server" CssClass="mod-item-body-a"></asp:LinkButton></td>
								<td width="33%" align="center"><asp:LinkButton ID="LinkButtonAllImages" runat="server" CssClass="mod-item-body-a"></asp:LinkButton></td>
								<td width="33%" align="right"><asp:LinkButton ID="LinkButtonNext" runat="server" CssClass="mod-item-body-a"></asp:LinkButton></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="3">
						<div style="width:0px; height:10px;"><span></span></div>
					</td>
				</tr>
				<tr><td colspan="3" style="text-align: center; vertical-align: middle; border-bottom-width: 0px; height: 272px;"  class="mod-item-body"><a id="aFull" runat="server"><asp:Image ID="ImageCurrent" runat="server" BorderWidth="0" /></a></td></tr>
				<tr>
					<td class="mod-item-body" style="border-top-width: 0px;">
						<table cellpadding="0" cellspacing="0" border="0" width="100%" class="main-font">
							<tr>
								<td style="padding: 10px; white-space: nowrap; border-width: 0px; vertical-align: top; width: 30%;" class="mod-item-body">
									<b><asp:Label ID="LabelName" runat="server"></asp:Label></b><br/>
									<asp:Label ID="LabelDate" runat="server"></asp:Label>
								</td>
								<td style="padding: 10px; border-width: 0px; vertical-align: top;" class="mod-item-body">
									<asp:Label ID="LabelDescription" runat="server"></asp:Label>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="3">
						<div style="width:0px; height:10px;"><span></span></div>
					</td>
				</tr>
				<tr>
					<td colspan="3" class="mod-item-body">
						<table cellpadding="5" cellspacing="0" border="0" width="100%">
							<tr>
								<td width="33%"><asp:LinkButton ID="LinkButtonPrevious1" runat="server" CssClass="mod-item-body-a"></asp:LinkButton></td>
								<td width="33%" align="center"><asp:LinkButton ID="LinkButtonAllImages1" runat="server" CssClass="mod-item-body-a"></asp:LinkButton></td>
								<td width="33%" align="right"><asp:LinkButton ID="LinkButtonNext1" runat="server" CssClass="mod-item-body-a"></asp:LinkButton></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			</center>
		</Template>
	</SiteBuilder:PagePanel>
</asp:Content>
