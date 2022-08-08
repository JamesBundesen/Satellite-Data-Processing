namespace Satellite_Data_Processing
{
    using Galileo;
    using System.Diagnostics;

    public partial class SatDataProcessing : Form
    {
        public SatDataProcessing()
        {
            InitializeComponent();
            listView.Columns.Add("Sensor A", -2, HorizontalAlignment.Left);
            listView.Columns.Add("Sensor B", -2, HorizontalAlignment.Left);
        }

        #region Global Methods

        // 4.1 LinkedLists for each sensor's data
        private LinkedList<double> aData = new LinkedList<double>();
        private LinkedList<double> bData = new LinkedList<double>();

        // 4.2 Loads sensor data (400 entries) using Galileo library
        private void LoadData()
        {
            double mu = (double)numericUpDownMu.Value;
            double sig = (double)numericUpDownSig.Value;
            Galileo.ReadData rd = new Galileo.ReadData();

            aData.Clear();
            bData.Clear();

            for (int i = 0; i < 400 ; i++) {
                aData.AddLast(rd.SensorA(mu, sig));
                bData.AddLast(rd.SensorB(mu, sig));
            }
        }
        
        // 4.3 displays sensor data in 2-column listview
        private void ShowAllSensorData()
        {
            listView.Items.Clear();

            //Create empty listviewItem Array
            ListViewItem[] items = new ListViewItem[aData.Count];
            int count = 0;

            // For each item in aData, create a new listViewItem with item as text in items array
            foreach (var a in aData) {
                items[count] = new ListViewItem(a.ToString());
                count++;
            }

            // For each item in dData, add as listViewItem subitem
            count = 0;
            foreach (var b in bData)
            {
                items[count].SubItems.Add(b.ToString());
                count++;
            }

            // Add items to listView
            listView.Items.AddRange(items);
        }

        // 4.4 Button event for Loading and Displaying Sensor Data
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            ShowAllSensorData();
        }

        #endregion

        #region Utility Methods

        // 4.5 Returns the number of nodes in a LinkedList
        private int NumberOfNodes<T>(LinkedList<T> linkedlist)
        {
            return linkedlist.Count;
        }

        // 4.6 Displays passed LinkedList in passed Listbox
        private void DisplayListboxData<T>(LinkedList<T> linkedlist, ListBox lb)
        {
            lb.Items.Clear();
            foreach (var item in linkedlist) {
                try
                {
                    lb.Items.Add(item.ToString());
                } catch (Exception t)
                {
                    Console.WriteLine(t.GetType() + ".toString() threw " + t.Message);
                    return;
                }
            }
        }

        #endregion

        #region Sort and Search methods

        // 4.7 Sorts linked list using selection sort algorithm
        private bool SelectionSort(LinkedList<double> linkedlist)
        {
            int min = 0;
            int max = NumberOfNodes(linkedlist);
            for (int i = 0; i < max; i++)
            {
                min = i;
                for (int j = i + 1; j < max; j++)
                    if (linkedlist.ElementAt(j) < linkedlist.ElementAt(min))
                        min = j;
                LinkedListNode<double> currentMin = linkedlist.Find(linkedlist.ElementAt(min));
                LinkedListNode<double> currentI = linkedlist.Find(linkedlist.ElementAt(i));

                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }
            return true;
        }

        // 4.8 Sorts linked list using Insertion sort algorithm
        private bool InsertionSort(LinkedList<double> linkedlist)
        {
            int max = NumberOfNodes(linkedlist);
            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (linkedlist.ElementAt(j - 1) > linkedlist.ElementAt(j))
                    {
                        LinkedListNode<double> current = linkedlist.Find(linkedlist.ElementAt(j));
                        LinkedListNode<double> currentPrevious = linkedlist.Find(linkedlist.ElementAt(j-1));

                        var temp = current.Value;
                        current.Value = currentPrevious.Value;
                        currentPrevious.Value = temp;
                    }
                }
            }
            return true;
        }

        // 4.9 returns index of element if found, else returns nearest neighbour. Uses binary search iterative algorithm
        private int BinarySearchIterative(LinkedList<double> linkedlist, double searchValue, int min, int max)
        {
            while (min <= max - 1)
            {
                int mid = (min + max) / 2;
                if (searchValue == linkedlist.ElementAt(mid))
                    return ++mid;
                else if (searchValue < linkedlist.ElementAt(mid))
                    max = mid - 1;
                else
                    min = mid + 1;
            }
            return min;
        }

        // 4.10 returns index of element if found, else returns nearest neighbour. Uses binary search recursive algorithm
        private int BinarySearchRecursive(LinkedList<double> linkedlist, double searchValue, int min, int max)
        { 
            if(min <= max -1)
            {
                int mid = (min + max) / 2;
                if (searchValue == linkedlist.ElementAt(mid))
                    return mid;
                else if (searchValue < linkedlist.ElementAt(mid))
                    return BinarySearchRecursive(linkedlist, searchValue, min, mid - 1);
                else
                    return BinarySearchRecursive(linkedlist, searchValue, mid + 1, max);
            }
            return min;
        }
            #endregion

            private void removeMe()
        {
            Debug.WriteLine(NumberOfNodes(aData));
            InsertionSort(aData);
            DisplayListboxData(aData, listBoxA);
            DisplayListboxData(bData, listBoxB);

        }

        // 4.11
        #region Sensor A Buttons
        private void buttonBSI_A_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textBoxBSI_A.Text))
            {
                int index = BinarySearchIterative(aData, Double.Parse(textBoxBSI_A.Text), 0, NumberOfNodes(aData));
                listBoxA.SelectedIndex = index;
            }
            
        }

        private void buttonSS_A_Click(object sender, EventArgs e)
        {
            SelectionSort(aData);
            DisplayListboxData(aData, listBoxA);
        }

        private void buttonSS_B_Click(object sender, EventArgs e)
        {
            SelectionSort(bData);
            DisplayListboxData(bData, listBoxB);
        }

        #endregion
    }
}