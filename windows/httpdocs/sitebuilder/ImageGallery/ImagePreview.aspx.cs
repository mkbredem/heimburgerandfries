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
using SWsoft.SiteBuilder.Modules.WebControls;
using SWsoft.SiteBuilder.Modules.WebControls.Paging;
using SWsoft.SiteBuilder.Modules.ImageGallery;
using SWsoft.SiteBuilder.Modules.ImageGallery.Map;
using SWsoft.SiteBuilder.Modules.Util;
using System.Collections.Generic;

public partial class ImageGallery_ImagePreview : ModulePage
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
		LinkButtonAllImages.Command += new CommandEventHandler(ImagePreview_Command);
		LinkButtonAllImages.CommandName = "All";
		LinkButtonAllImages1.Command += new CommandEventHandler(ImagePreview_Command);
		LinkButtonAllImages1.CommandName = "All";
		LinkButtonNext.Command += new CommandEventHandler(ImagePreview_Command);
		LinkButtonNext.CommandName = "Next";
		LinkButtonNext1.Command += new CommandEventHandler(ImagePreview_Command);
		LinkButtonNext1.CommandName = "Next";
		LinkButtonPrevious.Command += new CommandEventHandler(ImagePreview_Command);
		LinkButtonPrevious.CommandName = "Previous";
		LinkButtonPrevious1.Command += new CommandEventHandler(ImagePreview_Command);
		LinkButtonPrevious1.CommandName = "Previous";
	}
	#endregion

	#region DataBinding
	public override void DataBind()
	{
		ImageGalleryNode node = (ImageGalleryNode)SiteMap.CurrentNode;
		if (node == null) return;
		CategoryId = (Guid)val.CategoryID;
		ImageId = (Guid)val.ImageID;
		Guid = (Guid)node.Provider.GalleryId;
		MaxCategoryItems = node.Provider.MaxCategoryItems;
		MaxImageItems = node.Provider.MaxImageItems;

		if (ImageList == null)
		{
			ImageList = ImageGalleryData.Instance.GetImages(Guid, CategoryId);
		}
		CurrentPage = ImageGalleryData.Instance.GetImageIndex(Guid, CategoryId, ImageId);

		if (CurrentPage > 0)
		{
			LinkButtonPrevious.CommandArgument = ImageList.Rows[CurrentPage - 1][ImageGalleryData.ID_FIELD].ToString();
			LinkButtonPrevious1.CommandArgument = ImageList.Rows[CurrentPage - 1][ImageGalleryData.ID_FIELD].ToString();
		}
		if (CurrentPage < ImageList.Rows.Count - 1)
		{
			LinkButtonNext.CommandArgument = ImageList.Rows[CurrentPage + 1][ImageGalleryData.ID_FIELD].ToString();
			LinkButtonNext1.CommandArgument = ImageList.Rows[CurrentPage + 1][ImageGalleryData.ID_FIELD].ToString();
		}

		base.DataBind();
	}
	#endregion

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		LinkButtonAllImages.Text = GetLocalResourceObject("AllImages") as string;
		LinkButtonAllImages1.Text = GetLocalResourceObject("AllImages") as string;

		if (ImageList == null)
		{
			ImageCurrent.Visible = false;
			return;
		}
		LinkButtonNext.Text = GetLocalResourceObject("NextImage") as string;
		LinkButtonNext1.Text = GetLocalResourceObject("NextImage") as string;
		LinkButtonPrevious.Text = GetLocalResourceObject("PreviousImage") as string;
		LinkButtonPrevious1.Text = GetLocalResourceObject("PreviousImage") as string;
		int width = ImageGalleryData.Instance.GetImageSize(ImageGalleryData.ImageType.PreviewImage).Width;
		ImageCurrent.ImageUrl = ImageGalleryHandler.GetLink(ImageId.ToString(), width, width, "large");

		LabelName.Text = HttpUtility.HtmlEncode(ImageList.Rows[CurrentPage][ImageGalleryData.TITLE_FIELD].ToString());
		string date = string.Format(DateTimeFormat, ImageList.Rows[CurrentPage][ImageGalleryData.CREATE_DATE_FIELD]);
		LabelDate.Text = string.Format(GetLocalResourceObject("AddAt").ToString(), date);
		LabelDescription.Text = ImageList.Rows[CurrentPage][ImageGalleryData.DESCRIPTION_FIELD].ToString();

		string fullScreanLink = ImageGalleryHandler.GetLink(ImageId.ToString(), 100, 100, "fullscrean");
		aFull.Target = "_blank";
		aFull.HRef = fullScreanLink;

		if (CurrentPage == 0)
		{
			LinkButtonPrevious.Visible = false;
			LinkButtonPrevious1.Visible = false;
		}
		if (CurrentPage == ImageList.Rows.Count - 1)
		{
			LinkButtonNext.Visible = false;
			LinkButtonNext1.Visible = false;
		}
	}

	#region Properties
	DataTable _imageList = null;
	public DataTable ImageList
	{
		get { return _imageList; }
		set { _imageList = value; }
	}
	int _currentPage = 0;
	public int CurrentPage
	{
		get { return _currentPage; }
		set { _currentPage = value; }
	}
	private Guid _imageId = Guid.NewGuid();
	public Guid ImageId
	{
		get{return _imageId;}
		set{_imageId = value;}
	}
	private Guid _categoryId = Guid.NewGuid();
	public Guid CategoryId
	{

		get{return _categoryId;}
		set{_categoryId = value;}
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
	private Label LabelName
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<Label>("LabelName"); }
	}

	private Label LabelDate
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<Label>("LabelDate"); }
	}

	private Label LabelDescription
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<Label>("LabelDescription"); }
	}

	private LinkButton LinkButtonPrevious
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<LinkButton>("LinkButtonPrevious"); }
	}

	private LinkButton LinkButtonAllImages
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<LinkButton>("LinkButtonAllImages"); }
	}

	private LinkButton LinkButtonNext
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<LinkButton>("LinkButtonNext"); }
	}

	private LinkButton LinkButtonPrevious1
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<LinkButton>("LinkButtonPrevious1"); }
	}

	private LinkButton LinkButtonAllImages1
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<LinkButton>("LinkButtonAllImages1"); }
	}

	private LinkButton LinkButtonNext1
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<LinkButton>("LinkButtonNext1"); }
	}

	private Image ImageCurrent
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<Image>("ImageCurrent"); }
	}

	private HtmlAnchor aFull
	{
		get { return ImageGalleryPanel.TemplateContainer.FindRequiredControl<HtmlAnchor>("aFull"); }
	}
	#endregion

	#region Handlers
	protected void ImagePreview_Command(object sender, CommandEventArgs e)
	{
		switch (e.CommandName)
		{
			case "All":
				if (CategoryId.Equals(Guid.Empty))
				{
					Response.Redirect(Links.GetCategoryListLink(Guid));
				}
				else
				{
					Response.Redirect(Links.GetCategoryLink(CategoryId, Guid));
				}
				break;
			case "Next":
			case "Previous":
				Response.Redirect(Links.GetImageLink(CategoryId, new Guid(e.CommandArgument as string), Guid));
				break;
		}
	}
	#endregion
}
