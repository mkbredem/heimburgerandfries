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
using SWsoft.SiteBuilder.Modules.Guestbook.Data;
using SWsoft.SiteBuilder.Modules.Storage;
using SWsoft.SiteBuilder.Modules.Util;
using SWsoft.SiteBuilder.Modules.WebControls.Misc;
using SWsoft.SiteBuilder.Modules.WebControls;
using System.Collections.Generic;
using SWsoft.SiteBuilder.Modules.WebControls.Template;
using SWsoft.SiteBuilder.Modules.Guestbook;

public partial class Guestbook_Guestbook : ModulePage
{
	#region Guestbook Controls
	private DataList MessageList
	{
		get { return GuestbookPanel.FindRequiredControl<DataList>("MessageList"); }
	}

	private IEditableTextControl AuthorInput
	{
		get { return GuestbookPanel.FindRequiredControl<IEditableTextControl>("AuthorInput"); }
	}

	private IEditableTextControl MessageInput
	{
		get { return GuestbookPanel.FindRequiredControl<IEditableTextControl>("MessageInput"); }
	}

	private ITextControl AuthorCaption
	{
		get { return GuestbookPanel.FindRequiredControl<ITextControl>("AuthorCaption"); }
	}

	private ITextControl AddMessageTitle
	{
		get { return GuestbookPanel.FindRequiredControl<ITextControl>("AddMessageTitle"); }
	}

	private ITextControl MessageCaption
	{
		get { return GuestbookPanel.FindRequiredControl<ITextControl>("MessageCaption"); }
	}

	private IButtonControl AddMessage
	{
		get { return GuestbookPanel.FindRequiredControl<IButtonControl>("AddMessage"); }
	}

	private StatusBarControl StatusBar
	{
		get { return GuestbookPanel.FindRequiredControl<StatusBarControl>("StatusBar"); }
	}

	private PagerControl Pager
	{
		get { return GuestbookPanel.FindRequiredControl<PagerControl>("Pager"); }
	}

	private BaseValidator AuthorRequiredFieldValidator
	{
		get { return GuestbookPanel.FindRequiredControl<BaseValidator>("AuthorRequiredFieldValidator"); }
	}

	private BaseValidator MessageRequiredFieldValidator
	{
		get { return GuestbookPanel.FindRequiredControl<BaseValidator>("MessageRequiredFieldValidator"); }
	}
	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			InitFormValidators();

			GuestbookPanel.DataBind();
			BindMessageList();

			AuthorCaption.Text = (string)GetLocalResourceObject("AuthorCaption");
			MessageCaption.Text = (string)GetLocalResourceObject("MessageCaption");
			AddMessage.Text = (string)GetLocalResourceObject("AddMessage");
			AddMessageTitle.Text = (string)GetLocalResourceObject("AddMessageTitle");
		}
	}

	private void InitFormValidators()
	{
		string validationGroup = "GuestbookAdding";
		string asterisk = "*";
		AuthorRequiredFieldValidator.ControlToValidate = ((Control)AuthorInput).ID;
		AuthorRequiredFieldValidator.ValidationGroup = validationGroup;
		AuthorRequiredFieldValidator.Text = asterisk;
		MessageRequiredFieldValidator.ControlToValidate = ((Control)MessageInput).ID;
		MessageRequiredFieldValidator.ValidationGroup = validationGroup;
		MessageRequiredFieldValidator.Text = asterisk;
		AddMessage.ValidationGroup = validationGroup;
	}

	private void BindMessageList()
	{
		MessageList.DataSource = GuestbookData.Instance.GetMessages(DateTime.Now - Settings.Instance.MessageLifeTime);
		MessageList.DataBind();
	}

	protected override void OnInit(EventArgs e)
	{
		#region Init settings
		if (!Settings.DesignMode)
			Settings.SetInstance(new Settings());

		string tmp;
		tmp = SiteMap.CurrentNode["MessageLifeTime"];
		if (!String.IsNullOrEmpty(tmp))
			Settings.Instance.MessageLifeTime = TimeSpan.Parse(tmp);

		tmp = SiteMap.CurrentNode["PageSize"];
		if (!String.IsNullOrEmpty(tmp))
			Settings.Instance.PageSize = Int32.Parse(tmp);

		tmp = SiteMap.CurrentNode["ShowAuthorsEmail"];
		if (!String.IsNullOrEmpty(tmp))
			Settings.Instance.ShowAuthorsEmail = Boolean.Parse(tmp);
		#endregion

		Pager.PageSize = Settings.Instance.PageSize;
		Pager.InitBoundControl(MessageList);

		MessageList.ItemDataBound += new DataListItemEventHandler(MessageList_ItemDataBound);
		AddMessage.Click += new EventHandler(AddMessage_Click);
		Pager.PageIndexChanged += new PageIndexChangedEventHandler(Pager_PageIndexChanged);

		base.OnInit(e);
	}

	void Pager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
	{
		Pager.CurrentPageIndex = e.NewPage;
		BindMessageList();
	}

	void AddMessage_Click(object sender, EventArgs e)
	{
		if (!Page.IsValid)
			return;

		try
		{
			Message msg = GuestbookData.Instance.NewMessage();
			msg.Comment = MessageInput.Text.Trim();
			MembershipUser user = Membership.GetUser();
			if (user == null)
			{
				user = MembershipEx.GetAnonymousUser(AuthorInput.Text.Trim());
			}
			msg.AuthorID = (Guid)user.ProviderUserKey;
			GuestbookData.Instance.UpdateMessage(msg);
			MessageInput.Text = string.Empty;
		}
		catch (UserManagementException ex)
		{
			if (ex.Kind != UserManagementException.ExceptionKind.InvalidUserName)
				throw;
			StatusBar.MessageStatus = Status.Error;
			StatusBar.Message = (string)GetLocalResourceObject("InvalidUserName");
		}

		BindMessageList();
	}

	void MessageList_ItemDataBound(object sender, DataListItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			string guestbookImage = "SWsoft.SiteBuilder.Modules.Guestbook.Resources.{0}";
			Dictionary<string, string> variables = new Dictionary<string, string>();
			variables.Add("DefaultMail", 
				this.ClientScript.GetWebResourceUrl(typeof(EditMessageControl), string.Format(guestbookImage, "EmailIcon.gif")));
			variables.Add("DefaultWWW",
				this.ClientScript.GetWebResourceUrl(typeof(EditMessageControl), string.Format(guestbookImage, "UrlIcon.gif")));

			Label authorLabel = (Label)e.Item.FindControl("Author");
			Label isAnonymousLabel = (Label)e.Item.FindControl("IsAnonymous");
			Link mailTo = (Link)e.Item.FindControl("MailTo");
			Label time = (Label)e.Item.FindControl("Time");
			Label message = (Label)e.Item.FindControl("Message");
			HyperLink homePageUrl = (HyperLink)e.Item.FindControl("HomePage");

			Message data = (Message)e.Item.DataItem;

			time.Text = string.Format(this.DateTimeFormat, data.PostedOn);
			message.Text = data.Comment.Replace("\n", "<br/>");

			MembershipUserEx author = MembershipEx.GetUser(data.AuthorID);
			string authorName = author.UserName;
			authorLabel.Text = authorName;

			if (author.IsAnonymous && isAnonymousLabel != null)
				isAnonymousLabel.Text = (string)GetLocalResourceObject("Anonymous");

			mailTo.Visible = homePageUrl.Visible = false;

			if (!author.IsAnonymous)
			{
				if (Settings.Instance.ShowAuthorsEmail && !MembershipEx.IsAdmin(author))
				{
					mailTo.Text = author.Email;
					mailTo.NavigateUrl = "mailto:" + author.Email;
					mailTo.Visible = true;
				}

				ProfileEx profile = ProfileEx.Create(author.UserName);
				string urlText = (string)profile.GetPropertyValue("Guestbook.HomepageUrl");
				if (!String.IsNullOrEmpty(urlText))
				{
					homePageUrl.NavigateUrl = urlText;
					homePageUrl.Text = HttpUtility.HtmlEncode(urlText);
					homePageUrl.Visible = true;
				}
			}

			Initializer.AssingVariables(e.Item, variables);
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		TextBox author = GuestbookPanel.FindRequiredControl<TextBox>("AuthorInput");
		MembershipUser user = Settings.GetCurrentUser();
		if (user != null)
		{
			author.ReadOnly = true;
			author.Text = user.UserName;
		}
	}
}