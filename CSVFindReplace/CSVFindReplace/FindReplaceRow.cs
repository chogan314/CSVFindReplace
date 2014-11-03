using System.Windows.Forms;
using System.Collections.Generic;

namespace CSVFindReplace
{
    public class FindReplaceRow
    {
        private static readonly List<FindReplaceRow> container = new List<FindReplaceRow>();
        public static List<FindReplaceRow> Container { get { return container; } }

        private FlowLayoutPanel parent;

        private TextBox findBox;
        private TextBox replaceBox;
        private ComboBox formatBox;
        private TextBox columnsBox;
        private Button deleteButton;

        private bool IsLast { 
            get { return container[Container.Count - 1].Equals(this); }
        }

        public TextBox FindBox { get { return findBox; } }
        public TextBox ReplaceBox { get { return replaceBox; } }
        public ComboBox FormatBox { get { return formatBox; } }
        public TextBox ColumnsBox { get { return columnsBox; } }
        public Button DeleteButton { get { return deleteButton; } }


        public FindReplaceRow(FlowLayoutPanel parent)
        {
            this.parent = parent;
            container.Add(this);

            findBox = initTextBox();
            replaceBox = initTextBox();
            formatBox = initFormatBox();
            columnsBox = initTextBox();
            deleteButton = initDeleteButton();
        }


        private TextBox initTextBox()
        {
            TextBox box = new TextBox();
            box.Width = 120;
            return box;
        }


        private ComboBox initFormatBox()
        {
            ComboBox box = new ComboBox();
            box.Width = 125;
            box.DropDownStyle = ComboBoxStyle.DropDownList;
            box.Items.Add(eCellFormat.Text);
            box.Items.Add(eCellFormat.Number);
            box.Items.Add(eCellFormat.Currency);
            box.Items.Add(eCellFormat.Date);
            box.Items.Add(eCellFormat.Percentage);
            box.Items.Add(eCellFormat.Time);
            box.Items.Add(eCellFormat.Default);
            box.SelectedIndex = 6;
            return box;
        }


        private Button initDeleteButton()
        {
            Button button = new Button();
            button.Width = 25;
            button.Text = "X";
            button.Click += new System.EventHandler(delete_click);
            return button;
        }


        private void delete_click(object sender, System.EventArgs e)
        {
            selfDestruct();
        }


        public void selfDestruct()
        {
            if (Container.Count > 1)
            {
                parent.Controls.Remove(FindBox);
                parent.Controls.Remove(ReplaceBox);
                parent.Controls.Remove(FormatBox);
                parent.Controls.Remove(ColumnsBox);
                parent.Controls.Remove(DeleteButton);
                container.Remove(this);
            }
        }


        public void addToParent()
        {
            parent.Controls.Add(FindBox);
            parent.Controls.Add(ReplaceBox);
            parent.Controls.Add(FormatBox);
            parent.Controls.Add(ColumnsBox);
            parent.Controls.Add(DeleteButton);
        }
    }
}