using MJC.common.components;
using MJC.common;
using MJC.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MJC.forms
{
    public partial class ShipInformation : GlobalLayout
    {
        private HotkeyButton hkAdds = new HotkeyButton("Ins", "Adds", Keys.Insert);
        private HotkeyButton hkDeletes = new HotkeyButton("Del", "Deletes", Keys.Delete);
        private HotkeyButton hkEdits = new HotkeyButton("Enter", "Edits", Keys.Enter);
        private HotkeyButton hkPrevScreen = new HotkeyButton("Esc", "Previous screen", Keys.Escape);

        private GridViewOrigin shipCustListGrid = new GridViewOrigin();
        private DataGridView CLGridRefer;

        public string shippingAddress = "";

        int customerId = 0;

        List<CustomerShipToData> customerShipedToList = new List<CustomerShipToData>();

        public ShipInformation(int cID = 0, bool readOnly = false) : base("Ship to Cust#", "Enter a customer ship to information")
        {
            InitializeComponent();
            _initBasicSize();
            HotkeyButton[] hkButtons;
            if (readOnly) hkButtons = new HotkeyButton[1] { hkEdits };
            else hkButtons = new HotkeyButton[4] { hkAdds, hkDeletes, hkEdits, hkPrevScreen };
            if (readOnly) _initializeHKButtons(hkButtons, false);
            AddHotKeyEvents();

            this.customerId = cID;
            InitCustomerShiptoList();
            this.Load += OnLoadShipInformation;
        }

        private void OnLoadShipInformation(object sender, EventArgs e)
        {
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Hide();
                }
            };

        }

        private void AddHotKeyEvents()
        {

        }

        private void InitCustomerShiptoList()
        {
            CLGridRefer = shipCustListGrid.GetGrid();
            CLGridRefer.Location = new Point(0, 95);
            CLGridRefer.Width = this.Width;
            CLGridRefer.Height = this.Height - 295;
            CLGridRefer.VirtualMode = true;
            this.Controls.Add(CLGridRefer);
            this.CLGridRefer.CellDoubleClick += (sender, e) =>
            {

            };

            this.CLGridRefer.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int selectedID = 0;
                    if (CLGridRefer.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in CLGridRefer.Rows)
                        {
                            selectedID = (int)row.Cells[0].Value;
                            shippingAddress = row.Cells[2].Value.ToString();
                            this.Hide();
                        }
                    }
                }
            };

            LoadCustomerShipToList();
        }

        private void LoadCustomerShipToList()
        {
            string filter = "";
            customerShipedToList = Session.customerShipedModelObj.LoadCustomerShipTosNyCustomerId(filter, customerId);
            CLGridRefer.DataSource = customerShipedToList;
            if (customerShipedToList.Count > 0)
            {
                CLGridRefer.Columns[0].Visible = false;
                CLGridRefer.Columns[1].HeaderText = "Name";
                CLGridRefer.Columns[1].Width = 300;
                CLGridRefer.Columns[2].HeaderText = "Address1";
                CLGridRefer.Columns[2].Width = 300;
                CLGridRefer.Columns[3].HeaderText = "Address2";
                CLGridRefer.Columns[3].Width = 500;
                CLGridRefer.Columns[4].HeaderText = "City";
                CLGridRefer.Columns[4].Width = 200;
                CLGridRefer.Columns[5].HeaderText = "State";
                CLGridRefer.Columns[5].Width = 200;
                CLGridRefer.Columns[6].HeaderText = "Zip";
                CLGridRefer.Columns[6].Width = 200;
            }
        }
    }
}
