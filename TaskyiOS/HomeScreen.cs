using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using MonoTouch.Dialog;
using Tasky.PortableStandardLibrary;
using Tasky.ApplicationLayer;
using TaskyPortableStandardLibrary;

namespace Tasky.Screens {

	/// <summary>
	/// A UITableViewController that uses MonoTouch.Dialog - displays the list of Tasks
	/// </summary>
	public class HomeScreen : DialogViewController {
		// 
		List<TodoItem> tasks;
		
		// MonoTouch.Dialog individual TaskDetails view (uses /ApplicationLayer/TaskDialog.cs wrapper class)
		BindingContext context;
		TodoItemDialog taskDialog;
		TodoItem currentItem;
		DialogViewController detailsScreen;
        IEventHubListener eventHubListener;

		public HomeScreen () : base (UITableViewStyle.Plain, null)
		{
			Initialize ();
		}
		
		protected void Initialize()
		{
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { ShowTaskDetails(new TodoItem()); };
            eventHubListener = new EventHubListener();
            eventHubListener.MessageReceived += EventHubListener_MessageReceived;
		}

        private void EventHubListener_MessageReceived(object sender, string e)
        {
            string message = e;
            string title = "Event Hub message";
            var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            var positiveAction = UIAlertAction.Create(title,
                                                      UIAlertActionStyle.Default,
                                                      action =>
                                                      {
                                                      });
            alertController.AddAction(positiveAction);

            GetTopViewController().PresentViewControllerAsync(alertController, true);
        }

        private UIViewController GetTopViewController(UIViewController startViewController = null)
        {
            var viewController = startViewController ?? UIApplication.SharedApplication.KeyWindow.RootViewController;

            return viewController?.PresentedViewController == null
                ? viewController
                : GetTopViewController(viewController.PresentedViewController);
        }

        protected void ShowTaskDetails(TodoItem item)
		{
			currentItem = item;
			taskDialog = new TodoItemDialog (currentItem);
			context = new BindingContext (this, taskDialog, "Task Details");
			detailsScreen = new DialogViewController (context.Root, true);
			ActivateController(detailsScreen);
		}
		public void SaveTask()
		{
			context.Fetch (); // re-populates with updated values
			currentItem.Name = taskDialog.Name;
			currentItem.Notes = taskDialog.Notes;
			// TODO: show the completion status in the UI
			currentItem.Done = taskDialog.Done;
			AppDelegate.Current.TodoManager.SaveTask(currentItem);
			NavigationController.PopViewController (true);
		}
		public void DeleteTask ()
		{
			if (currentItem.ID >= 0)
				AppDelegate.Current.TodoManager.DeleteTask (currentItem.ID);
			NavigationController.PopViewController (true);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// reload/refresh
			PopulateTable();

            eventHubListener.Start();
		}

        public override void ViewDidDisappear(bool animated)
        {
            eventHubListener.Stop();
            base.ViewDidDisappear(animated);
        }

        protected void PopulateTable()
		{
			tasks = AppDelegate.Current.TodoManager.GetTasks().ToList ();
//			var rows = from t in tasks
//				select (Element)new StringElement ((t.Name == "" ? "<new task>" : t.Name), t.Notes);
			// TODO: use this element, which displays a 'tick' when item is completed
			var rows = from t in tasks
				select (Element)new CheckboxElement ((t.Name == "" ? "<new task>" : t.Name), t.Done);
			var s = new Section ();
			s.AddAll(rows);
			Root = new RootElement("Tasky") {s}; 
		}
		public override void Selected (Foundation.NSIndexPath indexPath)
		{
			var todoItem = tasks[indexPath.Row];
			ShowTaskDetails(todoItem);
		}
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new EditingSource (this);
		}
		public void DeleteTaskRow(int rowId)
		{
			AppDelegate.Current.TodoManager.DeleteTask(tasks[rowId].ID);
		}
	}
}