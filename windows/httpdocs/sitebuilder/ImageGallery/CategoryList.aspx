<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="CategoryList.aspx.cs" Inherits="ImageGallery_CategoryList" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
	<SiteBuilder:PagePanel ID="ImageGalleryPanel" runat="server">
		<Template>
			<asp:DataList ID="DataListCategories" 
				runat="server" 
				Width="100%" 
				ShowFooter="false" 
				ShowHeader="false" 
				RepeatLayout="Table"
				CellPadding="0"
				CellSpacing="0">
				<ItemTemplate>
					<table cellpadding="0" cellspacing="0" border="0" width="100%" style="border-collapse: collapse;">
						<tr>
							<td class="mod-category-body" style="text-align: center; vertical-align: middle; padding: 10px; width: 160px; height: 120px; border-right-width: 0px;">
								<asp:LinkButton ID="LinkButtonImage" runat="server" CssClass="mod-category-body-a"><asp:Image ID="ImageButtonCategory" runat="server"/></asp:LinkButton>
							</td>
							<td style="vertical-align: top; border-left-width: 0px; padding: 10px;" class="mod-category-body">
								<b><asp:LinkButton ID="LinkButtonCategory" runat="server" style="font-size: 15px;"></asp:LinkButton></b>
								<div style="width:0px; height:10px;"><span></span></div>
								<asp:Label ID="LabelDescription" runat="server"></asp:Label>
							</td>
						</tr>
					</table>
					<div style="width:0px; height:10px;"><span></span></div>
				</ItemTemplate>
				<AlternatingItemTemplate>
					<table cellpadding="0" cellspacing="0" border="0" width="100%" style="border-collapse: collapse;">
						<tr>
							<td class="mod-category-body-alter" style="text-align: center; vertical-align: middle; padding: 10px; width: 160px; height: 120px; border-right-width: 0px;">
								<asp:LinkButton ID="LinkButtonImage" runat="server"><asp:Image ID="ImageButtonCategory" runat="server"/></asp:LinkButton>
							</td>
							<td style="vertical-align: top; border-left-width: 0px; padding: 10px;" class="mod-category-body-alter">
								<b><asp:LinkButton ID="LinkButtonCategory" runat="server" style="font-size: 15px;"></asp:LinkButton></b>
								<div style="width:0px; height:10px;"><span></span></div>
								<asp:Label ID="LabelDescription" runat="server"></asp:Label>
							</td>
						</tr>
					</table>
					<div style="width:0px; height:10px;"><span></span></div>
				</AlternatingItemTemplate>
			</asp:DataList>
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
