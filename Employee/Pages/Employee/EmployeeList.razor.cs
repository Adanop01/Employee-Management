using Newtonsoft.Json;
using System.Net.Http;
using View=Model.ViewModel;

namespace UI.Pages.Employee
{
    public partial class EmployeeList
    {
        public List<View.EmployeeModel> employeeList { get; set; }=new List<View.EmployeeModel>();
        protected override async Task OnInitializedAsync()
        {
            await PopulateData();
        }
        public async Task PopulateData()
        {
            var response = await httpClient.GetAsync(httpClient.BaseAddress+"api/employee/GetUserList");
            var contentTemp = await response.Content.ReadAsStringAsync();
            employeeList = JsonConvert.DeserializeObject<List<View.EmployeeModel>>(contentTemp);
        }
    }
}
