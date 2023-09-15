using MJC.common.components;
using MJC.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MJC.forms.order
{
    public partial class Processing : BasicModal
    {
        private FInputBox ProcessedBy = new FInputBox("Processed By");
        private FInputBox InvoiceDesc = new FInputBox("InvoiceDesc");
        private FDateTime DateShiped = new FDateTime("DateShiped");
        private FInputBox InvoiceNumber = new FInputBox("Invoice #");

        private ModalButton MBResume = new ModalButton("F1 Resume", Keys.F1);
        private Button MBResume_button;
        private ModalButton MBCancelOrder = new ModalButton("F5 Cancel Order", Keys.F5);
        private Button MBCancelOrder_button;
        private ModalButton MBNext = new ModalButton("F2 Next", Keys.F2);
        private Button MBNext_button;

        public Processing() : base("Processing")
        {
            InitializeComponent();
            this.Size = new Size(800, 400);
            InitForms();
            InitModalButtons();
        }

        private void InitModalButtons()
        {
        }
        private void resume_button_Click(object sender, EventArgs e)
        {

        }
        private void cancelOrder_button_Click(object sender, EventArgs e)
        {

        }
        private void next_button_Click(object sender, EventArgs e)
        {

        }
        private void InitForms()
        {
            int xPos = 30;
            int yPos = 20;
            int yDistance = 40;

            ProcessedBy.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(ProcessedBy.GetLabel());
            this.Controls.Add(ProcessedBy.GetTextBox());

            yPos += yDistance;
            InvoiceDesc.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(InvoiceDesc.GetLabel());
            this.Controls.Add(InvoiceDesc.GetTextBox());

            yPos += yDistance;
            DateShiped.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(DateShiped.GetLabel());
            this.Controls.Add(DateShiped.GetDateTimePicker());

            yPos += yDistance;
            InvoiceNumber.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(InvoiceNumber.GetLabel());
            this.Controls.Add(InvoiceNumber.GetTextBox());

            yPos += yDistance;

            ModalButton_HotKeyDown(MBResume);
            MBResume_button = MBResume.GetButton();
            MBResume_button.Click += (sender, e) => resume_button_Click(sender, e);
            MBResume.SetPosition(new Point(xPos, yPos));
            this.Controls.Add(MBResume_button);

            ModalButton_HotKeyDown(MBCancelOrder);
            MBCancelOrder_button = MBCancelOrder.GetButton();
            MBCancelOrder_button.Click += (sender, e) => cancelOrder_button_Click(sender, e);
            MBCancelOrder.SetPosition(new Point(xPos + 300, yPos));
            this.Controls.Add(MBCancelOrder_button);

            ModalButton_HotKeyDown(MBNext);
            MBNext_button = MBNext.GetButton();
            MBNext_button.Click += (sender, e) => next_button_Click(sender, e);

            MBNext.SetPosition(new Point(xPos + 600, yPos));
            this.Controls.Add(MBNext_button);
        }

    }
}
