using Terminal.Gui;

namespace ConsoleApp
{
    public class CreateProduct : Window
    {
        protected TextField nameInput;
        protected TextField infoInput;
        protected TextField priceInput;
        protected CheckBox availabilityInput;

        public bool status;
        public CreateProduct()
        {
            this.Title = "Create new product";
            
            Label name = new Label (3, 4, "Name:");
            Label info = new Label (3, 6, "Information:");
            Label price = new Label (3, 8, "Price:");
            Label availability = new Label (3, 10, "Avalability:");
            
            this.Add(name, info, price, availability);

            nameInput = new TextField (){X = 15, Y = Pos.Top(name), Width = 20, Text = ""};
            infoInput = new TextField (){X = 15, Y = Pos.Top(info), Width = 100, Text = ""};
            priceInput = new TextField (){X = 15, Y = Pos.Top(price), Width = 13, Text = ""};
            availabilityInput = new CheckBox(){X =15, Y = Y = Pos.Top(availability), Text = ""};

            this.Add(nameInput, infoInput, priceInput, availabilityInput);

            Button confirm = new Button()
            {
                X = 10,
                Y = Pos.Bottom(availabilityInput) + 5,
                Text = "Confirm"
            };
            confirm.Clicked += WindowCreateAct;
            Button cancel = new Button()
            {
                X = Pos.Right(confirm) + 2,
                Y = Pos.Bottom(availabilityInput) + 5,
                Text = "Cancel"
            };
            cancel.Clicked += WindowCanceled;
            this.Add(confirm, cancel);

        }
        public Product GetProduct()
        {
            return new Product()
            {
                name = nameInput.Text.ToString(),
                info = infoInput.Text.ToString(),
                price = int.Parse(priceInput.Text.ToString()),
                availability = availabilityInput.Checked
            };
        }
        private void WindowCanceled()
        {
            this.status = false;
            Application.RequestStop();
        } 

        private void WindowCreateAct()
        {
            int parse;
            if(!int.TryParse(priceInput.Text.ToString(), out parse))
            {
                MessageBox.ErrorQuery("Incorrect input","Change value of a price", "OK");
            }
            else
            {
                this.status = true;
                Application.RequestStop();
            }
            
        }

    }
}