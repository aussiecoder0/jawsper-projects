using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TabManager.TabFiles;

namespace TabManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            LoadDir(TabFile.TestFileFolder);

        }

        private void LoadDir(string dir)
        {
            var tabs = new TabList();

            foreach (var file in new DirectoryInfo(dir).GetFiles())
            {
                var tab = TabFile.OpenTab(file);
                if (tab != null)
                {
                    tabs.Add(tab);
                }
            }

            dataGridView1.DataSource = tabs;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.Width = col.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCellsExceptHeader, false);
            }
        }
    }


    public class TabList : BindingList<TabFile>
    {
        public TabList() : base()
        {
            this.AllowNew = this.AllowEdit = this.AllowRemove = false;
        }
        /*protected override bool SupportsSearchingCore
        {
            get { return true; }
        }
        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            // Ignore the prop value and search by family name.
            for (int i = 0; i < Count; ++i)
            {
                if (Items[i].FontFamily.Name.ToLower() == ((string)key).ToLower())
                    return i;

            }
            return -1;
        }*/
    }
}
