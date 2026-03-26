using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SWsoft.SiteBuilder.Modules.WebControls.Misc;
using SWsoft.SiteBuilder.Modules.WebControls.Paging;
using SWsoft.SiteBuilder.Modules.ImageGallery;
using SWsoft.SiteBuilder.Modules.ImageGallery.Map;
using SWsoft.SiteBuilder.Modules.Util;
using SWsoft.SiteBuilder.Modules.WebControls;

public partial class ImageGallery_CategoryList : ModulePage
{
	StateMachine val = new StateMachine();
	protected void Page_Load(object sender, EventArgs e)
	{
		SetInitializeParameters();
		DataBind();
	}

	private void SetInitializeParameters()
	{
		val.Load(Page.Request.QueryString);
		StateMachine.SetInstance(val);
	}

	#region OnInit
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		DataListCategories.ItemDataBound += new DataListItemEventHandler(DataListCategories_OnItemDataBound);
		DataListCategories.ItemCommand += new DataListCommandEventHandler(DataListCategories_ItemCommand);
		RepeaterImages.ItemDataBound += new RepeaterItemEventHandler(RepeaterImages_OnItemDataBound);
		RepeaterImages.ItemCommand += new RepeaterCommandEventHandler(RepeaterImages_ItemCommand);
		Pager.PageIndexChanged += new PageIndexChangedEventHandler(Pager_PageIndexChanged);
		Pager.InitBoundControl(RepeaterImages);
	}
	#endregion

	#region DataBinding
	public override void DataBind()
	{
		ImageGalleryNode node = (ImageGalleryNode)SiteMap.CurrentNode;
		if (node == null) return;
		Guid = (Guid)node.Provider.GalleryId;
		MaxCategoryItems = node.Provider.MaxCategoryItems;
		MaxImageItems = node.Provider.MaxImageItems;
		int pageSize = MaxImageItems;
		Pager.PageSize = pageSize;
		Pager.ShowNextButton = false;
		Pager.ShowPages = true;
		Pager.ShowPreviousButton = false;
		Pager.ShowPageSizeLinks = false;
		Pager.PagesToShow = 10;
		DataListCategories.RepeatColumns = 1;
		DataListCategories.DataSource = ImageGalleryData.Instance.GetCategoryList(Guid);
		RepeaterImages.DataSource = ImageGalleryData.Instance.GetUnCategorizedImages(Guid);
		base.DataBind();
		Pager.Visible = RepeaterImages.Items.Count > 0;
	}
	#endregion

	#region Properties
	private Guid _guid = Guid.NewGuid();
	public Guid Guid
	{

		get
		{
			return _guid ;
		}

		set
		{
			_guid = value;
		}
	}
	private int _maxImageItems = 10;
	private int _maxCategoryItems = 5;

	public int MaxCategoryItems
	{
		get { return _maxCategoryItems; }
		set { _maxCategoryItems = value; }
	}

	public int MaxImageItems
	{
		get { return _maxImageItems; }
		set { _maxImageItems = value; }
	}
	#endregion

	#region Controls
	private DataList DataListCategories
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<DataList>("DataListCategories"); }
	}

	private Repeater RepeaterImages
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<Repeater>("RepeaterImages"); }
	}

	private PagerControl Pager
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<PagerControl>("Pager"); }
	}
	#endregion

	#region Handlers
	protected void Pager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
	{
		Pager.CurrentPageIndex = e.NewPage;
		DataBind();
	}

	protected void DataListCategories_ItemCommand(object sender, DataListCommandEventArgs e)
	{
		Response.Redirect(Links.GetCategoryLink(new Guid(e.CommandArgument as string), Guid));
	}

	protected void DataListCategories_OnItemDataBound(object sender, DataListItemEventArgs e)
	{
		Image ImageButtonCategory = (Image)e.Item.FindControl("ImageButtonCategory");
		if (ImageButtonCategory != null)
		{
			string id = DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.ID_FIELD).ToString();
			if (!ImageGalleryData.Instance.IsBlobExists(id.ToUpper()))
			{
				DataTable image = ImageGalleryData.Instance.GetFirstImageInCategory(new Guid(id));
				if (image.Rows.Count > 0)
				{
					id = image.Rows[0][ImageGalleryData.ID_FIELD].ToString();
				}
			}
			
			ImageButtonCategory.ImageUrl = ImageGalleryHandler.GetLink(
							id,
							ImageGalleryData.Instance.GetImageSize(ImageGalleryData.ImageType.CategoryImage).Width,
							(int)DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.WIDTH_FIELD),
							"category");
			ImageButtonCategory.ToolTip = DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.NAME_FIELD).ToString().Replace('"', '\"');

			LinkButton LinkButtonImage = (LinkButton)e.Item.FindControl("LinkButtonImage");
			LinkButtonImage.CommandName = "ToImageList";
			LinkButtonImage.CommandArgument = DataBinder.Eval(LinkButtonImage.BindingContainer, "DataItem." + ImageGalleryData.ID_FIELD).ToString();

			LinkButton LinkButtonCategory = (LinkButton)e.Item.FindControl("LinkButtonCategory");
			LinkButtonCategory.Text = HttpUtility.HtmlEncode((string)DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.NAME_FIELD));
			LinkButtonCategory.ToolTip = DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.NAME_FIELD).ToString().Replace('"', '\"');
			LinkButtonCategory.CommandName = "ToImageList";
			LinkButtonCategory.CommandArgument = DataBinder.Eval(LinkButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.ID_FIELD).ToString();

			Label LabelDescription = (Label)e.Item.FindControl("LabelDescription");
			LabelDescription.Text = DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.DESCRIPTION_FIELD).ToString();

		}
	}

	protected void RepeaterImages_ItemCommand(object sender, RepeaterCommandEventArgs e)
	{
		Response.Redirect(Links.GetImageLink(Guid.Empty, new Guid(e.CommandArgument as string), Guid));
	}

	protected void RepeaterImages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		Image ImageButtonCategory = (Image)e.Item.FindControl("ImageButtonCategory");
		if (ImageButtonCategory != null)
		{
			ImageButtonCategory.ImageUrl = ImageGalleryHandler.GetLink(
							DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.ID_FIELD).ToString(),
							ImageGalleryData.Instance.GetImageSize(ImageGalleryData.ImageType.Image).Width,
							(int)DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.WIDTH_FIELD),
							"image");
			ImageButtonCategory.ToolTip = DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.TITLE_FIELD).ToString().Replace('"', '\"');
			LinkButton LinkButtonImage = (LinkButton)e.Item.FindControl("LinkButtonImage");
			LinkButtonImage.CommandName = "ToImage";
			LinkButtonImage.CommandArgument = DataBinder.Eval(LinkButtonImage.BindingContainer, "DataItem." + ImageGalleryData.ID_FIELD).ToString();

			LinkButton LinkButtonCategory = (LinkButton)e.Item.FindControl("LinkButtonCategory");
			LinkButtonCategory.Text = HttpUtility.HtmlEncode((string)DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.TITLE_FIELD));
			LinkButtonCategory.ToolTip = DataBinder.Eval(ImageButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.TITLE_FIELD).ToString().Replace('"', '\"');
			LinkButtonCategory.CommandName = "ToImage";
			LinkButtonCategory.CommandArgument = DataBinder.Eval(LinkButtonCategory.BindingContainer, "DataItem." + ImageGalleryData.ID_FIELD).ToString();

		}
	}
	#endregion
}
