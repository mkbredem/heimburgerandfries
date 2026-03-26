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
using SWsoft.SiteBuilder.Modules.WebControls;
using SWsoft.SiteBuilder.Modules.ImageGallery;
using SWsoft.SiteBuilder.Modules.ImageGallery.Map;
using SWsoft.SiteBuilder.Modules.Util;

public partial class ImageGallery_ImageList : ModulePage
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
		RepeaterImages.ItemDataBound += new RepeaterItemEventHandler(RepeaterImages_OnItemDataBound);
		RepeaterImages.ItemCommand += new RepeaterCommandEventHandler(RepeaterImages_ItemCommand);
		Pager.PageIndexChanged += new PageIndexChangedEventHandler(Pager_PageIndexChanged);
		LinkButtonToCategoryList.Click += new EventHandler(LinkButtonToCategoryList_OnClick);
		Pager.InitBoundControl(RepeaterImages);
	}
	#endregion

	#region DataBinding
	public override void DataBind()
	{
		ImageGalleryNode node = (ImageGalleryNode)SiteMap.CurrentNode;
		if (node == null) return;
		CategoryId = (Guid)val.CategoryID;
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
		RepeaterImages.DataSource = ImageGalleryData.Instance.GetImages(CategoryId);

		DataRow cat = ImageGalleryData.Instance.GetCategory(CategoryId);
		if (cat != null)
		{
			LabelCategoryDescription.Text = cat[ImageGalleryData.DESCRIPTION_FIELD].ToString();
			if (string.IsNullOrEmpty(LabelCategoryDescription.Text))
			{
				DivTrminator.Visible = false;
			}
		}

		base.DataBind();
	}
	#endregion

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		LinkButtonToCategoryList.Text = GetLocalResourceObject("AllCategories") as string;
	}

	#region Properties
	private Guid _categoryId = Guid.NewGuid();
	public Guid CategoryId
	{

		get
		{
			return _categoryId;
		}

		set
		{
			_categoryId = value;
		}
	}

	private Guid _guid = Guid.NewGuid();
	public Guid Guid
	{

		get
		{
			return _guid;
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
	private Repeater RepeaterImages
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<Repeater>("RepeaterImages"); }
	}

	private HtmlGenericControl DivTrminator
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<HtmlGenericControl>("DivTrminator"); }
	}

	private Label LabelCategoryDescription
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<Label>("LabelCategoryDescription"); }
	}

	private LinkButton LinkButtonToCategoryList
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<LinkButton>("LinkButtonToCategoryList"); }
	}

	private PagerControl Pager
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<PagerControl>("Pager"); }
	}
	#endregion

	#region Handlers
	protected void LinkButtonToCategoryList_OnClick(object sender, EventArgs e)
	{
		Response.Redirect(Links.GetCategoryListLink(Guid));
	}

	protected void Pager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
	{
		Pager.CurrentPageIndex = e.NewPage;
		DataBind();
	}

	protected void RepeaterImages_ItemCommand(object sender, RepeaterCommandEventArgs e)
	{
		Response.Redirect(Links.GetImageLink(CategoryId, new Guid(e.CommandArgument as string), Guid));
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
