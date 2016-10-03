using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MyGraph
{
    class TaskHelper
    {
        internal async Task<ObservableCollection<Task>> GetMyTasks()
        {
            try
            {
                HttpClient client = new HttpClient();
                var token = AuthenticationHelper.TokenForUser;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.GetAsync(new Uri("https://graph.microsoft.com/beta/me/tasks"));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Could not retrieve tasks: " + response.StatusCode.ToString());
                }
                dynamic jsonObj = JsonConvert.DeserializeObject(response.Content.ToString());
                var taskList = new ObservableCollection<Task>();
                foreach(var task in jsonObj.value)
                {
                    taskList.Add(new MyGraph.Task { Name = task.title, PercentComplete = task.percentComplete });
                }
                return taskList;
            }
            catch (Exception e)
            {
                throw new Exception("Could not retrieve tasks: " + e.Message);
            }
        }
    }
}
