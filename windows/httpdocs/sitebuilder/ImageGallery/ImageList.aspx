<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ImageList.aspx.cs" Inherits="ImageGallery_ImageList" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
	<SiteBuilder:PagePanel ID="ImageGalleryPanel" runat="server">
		<Template>
			<table cellpadding="10" cellspacing="0" border="0" width="100%" style="border-collapse: collapse;">
				<tr>
					<td class="mod-item-body">
						<asp:Label ID="LabelCategoryDescription" runat="server"></asp:Label>
						<div id="DivTrminator" runat="server" style="height: 10px;"><span></span></div>
						<asp:LinkButton ID="LinkButtonToCategoryList" runat="server" CssClass="mod-item-body-a"></asp:LinkButton>
					</td>
				</tr>
			</table>
			<div style="height: 10px;"><span></span></div>
			<asp:Repeater ID="RepeaterImages" runat="server">
				<ItemTemplate>
					<div style="float: left; width: 130px; height: 150px;">
						<table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;" width="120">
							<tr>
								<td colspan="2" class="mod-item-body" style="vertical-align: middle; text-align: center; padding: 10px; width: 100px; height: 95px; border-bottom-width: 0px;"><asp:LinkButton ID="LinkButtonImage" runat="server"><asp:Image ID="ImageButtonCategory" runat="server" /></asp:LinkButton></td>
							</tr>
							<tr>
								<td class="mod-item-body" style="width: 0px; overflow: hidden; padding: 0px 5px 5px 5px; border-right: none; border-top: none;"><div style="width: 0px; overflow: hidden;"><br /><br />.</div></td>
								<td align="center" class="mod-item-body" style="width: 120px; padding : 0px 5px 5px 0px; vertical-align: top; border-top: none; border-left:none;">
									<asp:LinkButton ID="LinkButtonCategory" runat="server" CssClass="mod-item-body-a"></asp:LinkButton>
								</td>
							</tr>
						</table>
						<div style="width:0px; height:10px;"><span></span></div>
					</div>
				</ItemTemplate>
			</asp:Repeater>
			<SiteBuilder:PagerControl runat="server" ID="Pager"></SiteBuilder:PagerControl>
		</Template>
	</SiteBuilder:PagePanel>
</asp:Content>