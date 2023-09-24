using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using View = Model.ViewModel;

namespace UI.Pages.Employee
{
    public partial class AddEmployee
    {
        [Inject]
        ISnackbar Snackbar { get; set; }
        [Parameter]
        public string Id { get; set; }
        public bool ShowSpinner { get; set; } = true;
        public View.EmployeeModel EmployeeData { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await PopulateData();
            ShowSpinner=false;
        }
        public async Task PopulateData()
        {
            if(Id !=null && !String.IsNullOrEmpty(Id))
            {
                var response =await httpClient.GetAsync(httpClient.BaseAddress+$"api/employee/GetUserById?id={Id}");
                var contentTemp = await response.Content.ReadAsStringAsync();
                 EmployeeData = JsonConvert.DeserializeObject<View.EmployeeModel>(contentTemp);
            }
        }
        public async Task Save(EditContext AD)
        {
            if (AD.Validate())
            {
				var content = JsonConvert.SerializeObject(EmployeeData);
				var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var result= await httpClient.PostAsync(httpClient.BaseAddress+"api/employee/addemployee", bodyContent);
                if(result.IsSuccessStatusCode)
                {
                    Snackbar.Add("Details Submitted Successfully!", Severity.Success);
                    Clear();
                }
                else
                {
                    Snackbar.Add("Not Able to AddDetails!", Severity.Error);
                }

			}
        }
        public void Clear()
        {
            EmployeeData=new View.EmployeeModel();
        }
    }
}
