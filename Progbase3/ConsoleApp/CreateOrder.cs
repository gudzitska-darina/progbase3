using Terminal.Gui;
namespace ConsoleApp
{
    public class CreateOrder: Window
    {
        private TextField nameInput;
        private long userIdInput;
        private long productIdInput;
        private TextField productInput;
        protected CheckBox isPayedInput;
        protected User currentUser;
        public bool status;
        public CreateOrder()
        {
            this.Title = "Create new order";
            Button confirm = new Button(3, 25,"Confirm");
            confirm.Clicked += WindowCreateAct;
            Button cancel = new Button(15, 25, "Cancel");
            cancel.Clicked += WindowCanceled;
            this.Add(confirm, cancel);
            
            Label name = new Label (3, 4, "Client name:");
            Label product = new Label (3, 6, "Product:");
            Label isPayed = new Label (3, 8, "Pay:");
            
            this.Add(name, product, isPayed);

            nameInput = new TextField (){X = 17, Y = Pos.Top(name), Width = 20, Text = "", ReadOnly = true};
            productInput = new TextField (){X = 17, Y = Pos.Top(product), Width = 40, Text = "", ReadOnly = true};
            isPayedInput = new CheckBox(){X = 17, Y = Pos.Top(isPayed)};

            this.Add(nameInput, productInput, isPayedInput);

        }
        public Order GetOrder()
        {
            return new Order()
            {
                autor = currentUser,
                userId = userIdInput,
                isPayed = isPayedInput.Checked
            };
        }
        public void SetProduct(Product pro)
        {
            this.productInput.Text = pro.name;
            this.productIdInput = pro.id;
        }
        public void SetUser(User us)
        {
            this.currentUser = us;
            this.nameInput.Text = us.login;
            this.userIdInput = us.id;
        }

        private void WindowCanceled()
        {
            this.status = false;
            Application.RequestStop();
        } 

        private void WindowCreateAct()
        {
            this.status = true;
            Application.RequestStop();
            
        }
    }
}