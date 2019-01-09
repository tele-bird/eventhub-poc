using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Tasky.PortableStandardLibrary;
using TaskyAndroid.ApplicationLayer;

namespace TaskyAndroid.Screens
{
    /// <summary>
    /// Main ListView screen displays a list of tasks, plus an [Add] button
    /// </summary>
    [Activity (Label = "Tasky", MainLauncher = true, Icon="@drawable/icon")]			
	public class HomeScreen : Activity 
	{
		TodoItemListAdapter taskList;
		IList<TodoItem> tasks;
		Button addTaskButton;
		ListView taskListView;
        //IEventHubListener eventHubListener;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// set our layout to be the home screen
			SetContentView(Resource.Layout.HomeScreen);

			//Find our controls
			taskListView = FindViewById<ListView> (Resource.Id.TaskList);
			addTaskButton = FindViewById<Button> (Resource.Id.AddButton);

			// wire up add task button handler
			if(addTaskButton != null) {
				addTaskButton.Click += (sender, e) => {
                    StartActivity(typeof(TodoItemScreen));
                };
			}
			
			// wire up task click handler
			if(taskListView != null) {
				taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TodoItemScreen));
					taskDetails.PutExtra ("TaskID", tasks[e.Position].ID);
					StartActivity (taskDetails);
				};
			}

            //eventHubListener = new EventHubListener();
            //eventHubListener.MessageReceived += EventHubListener_MessageReceived;
		}

        private void EventHubListener_MessageReceived(object sender, string e)
        {
            //var taskDetails = new Intent(this, typeof(TodoItemScreen));
            //taskDetails.PutExtra("item", e);
            //StartActivity(taskDetails);
        }

        protected override void OnResume ()
		{
			base.OnResume ();

            RefreshTaskList();

            //eventHubListener.Start();
		}

        private void RefreshTaskList()
        {
            tasks = TaskyApp.Current.TodoManager.GetTasks();

            // create our adapter
            taskList = new TodoItemListAdapter(this, tasks);

            //Hook up our adapter to our ListView
            taskListView.Adapter = taskList;
        }

        protected override void OnPause()
        {
            //eventHubListener.Stop();

            base.OnPause();
        }
    }
}