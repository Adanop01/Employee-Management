using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Model.ViewModel.Authentication;
using MudBlazor;
using System.Web;

namespace UI.Pages.Authenticate
{
	public partial class Login
	{
		[Inject]
		public ILogger<Login> _logger { get; set; }
		public bool ShowSpinner { get; set; }
		public Model.ViewModel.Authentication.Authentication UserForAuthentication = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowAuthenticationErrors { get; set; }
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public ISession SessionService { get; set; }
        private ElementReference firstInput;

        protected async override Task OnInitializedAsync()
        {

			//var user = await LocalStorage.GetItemAsync<User>(Security.Local_UserDetails);
			//if (user != null)
			//{
			//	session.UserID = user.Id;
			//	if (session.UserID != null)
			//	{
			//		NavManager.NavigateTo("/");
			//	}

			//}


		}
        public async Task Loginuser()
		{
			//var absoluteUri = new Uri(NavManager.Uri);
			//var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
			//ReturnUrl = queryParam["returnUrl"];
			//this.ShowSpinner = true;
			//ShowAuthenticationErrors = false;
			//IsProcessing = true;
			//var result = await AuthenticationService.Login(UserForAuthentication);
			//if (result.IsAuthSuccessful)
			//{
			//	SessionService.Token = result.Token;
			//	SessionService.UserID = result.User.Id;
			//	SessionService.Roles = result.Roles;
			//	SessionService.Location = result.Location;
			//	SessionService.Company = result.Company;
			//	NavManager.NavigateTo("/", true);
			//	IsProcessing = false;
			//}
			//else
			//{
			//	IsProcessing = false;
			//	ErrorMessage = result.ErrorMessage;
			//	ShowAuthenticationErrors = true;
			//}
			this.ShowSpinner = false;
		}
	}
}
