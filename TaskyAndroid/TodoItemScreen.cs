using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using Tasky.PortableStandardLibrary;

namespace TaskyAndroid.Screens
{
    /// <summary>
    /// View/edit a Task
    /// </summary>
    [Activity (Label = "TaskDetailsScreen")]			
	public class TodoItemScreen : Activity 
	{
		TodoItem task = new TodoItem();
		Button cancelDeleteButton;
		EditText notesTextEdit;
		EditText nameTextEdit;
		Button saveButton;
		CheckBox doneCheckbox;
        IEventHubSender eventHubSender;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			int taskID = Intent.GetIntExtra("TaskID", 0);
            string itemJson = Intent.GetStringExtra("item");
            if (taskID > 0) {
				task = TaskyApp.Current.TodoManager.GetTask(taskID);
			}
            else if(!string.IsNullOrEmpty(itemJson))
            {
                task = JsonConvert.DeserializeObject<TodoItem>(itemJson);
            }
			
			// set our layout to be the home screen
			SetContentView(Resource.Layout.TaskDetails);
			nameTextEdit = FindViewById<EditText>(Resource.Id.NameText);
			notesTextEdit = FindViewById<EditText>(Resource.Id.NotesText);
			saveButton = FindViewById<Button>(Resource.Id.SaveButton);

			// TODO: find the Checkbox control and set the value
			doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);
			doneCheckbox.Checked = task.Done;

			// find all our controls
			cancelDeleteButton = FindViewById<Button>(Resource.Id.CancelDeleteButton);
			
			// set the cancel delete based on whether or not it's an existing task
			cancelDeleteButton.Text = (task.ID == 0 ? "Cancel" : "Delete");
			
			nameTextEdit.Text = task.Name; 
			notesTextEdit.Text = task.Notes;

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };

            eventHubSender = new EventHubSender();
		}

        protected override void OnResume()
        {
            base.OnResume();

            eventHubSender.Start();
        }

        protected override void OnPause()
        {
            eventHubSender.Stop();

            base.OnPause();
        }

        void Save()
		{
			task.Name = nameTextEdit.Text;
			task.Notes = notesTextEdit.Text;
			//TODO: 
			task.Done = doneCheckbox.Checked;

			TaskyApp.Current.TodoManager.SaveTask(task);

            eventHubSender.Send(JsonConvert.SerializeObject(task));

			Finish();
		}
		
		void CancelDelete()
		{
			if (task.ID != 0) {
				TaskyApp.Current.TodoManager.DeleteTask(task.ID);
			}
			Finish();
		}
	}
}