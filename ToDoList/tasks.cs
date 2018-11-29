using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ToDoList.Helper;
namespace ToDoList
{
    class tasks: AppCompatActivity
    {
        EditText edtTask;
        DbHelper dbHelper;
        CustomAdapter adapter;
        ListView lstTask;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_item, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                    edtTask = new EditText(this);
                    Android.Support.V7.App.AlertDialog alertDialog =
                        new Android.Support.V7.App.AlertDialog.Builder(this)
                        .SetTitle("Add new task")
                        .SetMessage("What is your next task?")
                        .SetView(edtTask)
                        .SetPositiveButton("Add", OkAction)
                        .SetNegativeButton("Cancel", CancelAction)
                        .Create();
                    alertDialog.Show();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void CancelAction(object sender, DialogClickEventArgs e)
        {

        }
        private void OkAction(object sender, DialogClickEventArgs e)
        {
            string task = edtTask.Text;
            dbHelper.insertNewTask(task);
            LoadTaskList();
        }

        public void LoadTaskList()
        {
            List<string> taskList = dbHelper.getTaskList();
            adapter = new CustomAdapter(this, taskList, dbHelper);
            lstTask.Adapter = adapter;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            dbHelper = new DbHelper(this);
            lstTask = FindViewById<ListView>(Resource.Id.lstTask);

            LoadTaskList();

        }


    }
}
