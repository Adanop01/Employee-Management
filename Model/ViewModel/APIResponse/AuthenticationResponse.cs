using Model.ViewModel.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.APIResponse
{
	public class AuthenticationResponse
	{
		public bool IsAuthSuccessful { get; set; }
		public string ErrorMessage { get; set; }
		public string Token { get; set; }
		public long Location { get; set; }
		public User User { get; set; }
		public string UserName { get; set; }
	}
}
