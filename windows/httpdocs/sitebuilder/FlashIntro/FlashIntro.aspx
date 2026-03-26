<%@ Page Language="C#" Title="Flash Intro" %>

<%@ Register Assembly="SiteBuilder.Modules.FlashIntro" Namespace="SWsoft.SiteBuilder.Modules.FlashIntro"
    TagPrefix="SiteBuilder" %>
<html>
<head runat="server">
</head>
<body style="margin:5px;">
    <form runat="server">
        <SiteBuilder:FlashIntro ID="FlashIntro" runat="server" ObjectId='<%# int.Parse(SiteMap.CurrentNode["ObjectId"]) %>'
            ColorSchemaId='<%# int.Parse(SiteMap.CurrentNode["ColorSchemaId"]) %>' HeaderTex='<%# SiteMap.CurrentNode["HeaderText"] %>'
            BodyText='<%# SiteMap.CurrentNode["BodyText"] %>' FlashPrefix='<%# SiteMap.CurrentNode["FlashPrefix"] %>'
            FlashName='<%# SiteMap.CurrentNode["FlashName"] %>'/>
    </form>
</body>
</html>
