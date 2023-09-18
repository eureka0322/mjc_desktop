using MJC.common;
using MJC.common.components;
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

namespace MJC.forms.customer
{
    public partial class CustomerCreditCardView : GlobalLayout
    {
        private HotkeyButton hkAdds = new HotkeyButton("Ins", "Adds", Keys.Insert);
        private HotkeyButton hkDeletes = new HotkeyButton("Del", "Deletes", Keys.Delete);
        private HotkeyButton hkEdits = new HotkeyButton("Enter", "Edits", Keys.Enter);
        private HotkeyButton hkPrevScreen = new HotkeyButton("Esc", "Previous screen", Keys.Escape);

        private GridViewOrigin shipCustListGrid = new GridViewOrigin();
        private DataGridView CLGridRefer;

        int customerId = 0;

        List<CustomerCreditCard> customerCreditCardList = new List<CustomerCreditCard>();

        public CustomerCreditCardView(int cID = 0, bool readOnly = false) : base("Credit Cards Cust#", "Enter a customer ship to information")
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
                        foreach (DataGridViewRow row in CLGridRefer.SelectedRows)
                        {
                            selectedID = (int)row.Cells[0].Value;
                        }
                        this.Close();
                    }
                }
                if (e.KeyCode == Keys.Escape)
                {
                    _navigateToPrev(s, e);
                }
            };

            LoadCustomerCreditCardList();
        }

        private void LoadCustomerCreditCardList()
        {
            customerCreditCardList = Session.customerCreditCardModelObj.GetCreditCardsByCustomerId(customerId);
            CLGridRefer.DataSource = customerCreditCardList;

            CLGridRefer.Columns[0].Visible = false;
            CLGridRefer.Columns[1].Visible = false;
            CLGridRefer.Columns[2].HeaderText = "Card Number";
            CLGridRefer.Columns[2].Width = 300;
            CLGridRefer.Columns[3].HeaderText = "Card Type";
            CLGridRefer.Columns[3].Width = 300;
            CLGridRefer.Columns[4].HeaderText = "Expires";
            CLGridRefer.Columns[4].Width = 300;
//            CLGridRefer.Columns[4].HeaderText = "Security Code";
//            CLGridRefer.Columns[4].Width = 500;
            CLGridRefer.Columns[5].HeaderText = "Priority";
            CLGridRefer.Columns[5].Width = 200;
        }
    }
}
