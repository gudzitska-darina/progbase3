using Terminal.Gui;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class ViewOrder : Window
    {
        public bool del;
        public bool edited;
        protected Order order;
        private TextField idInput;
        protected TextField isPayedInput;
        protected DateField createdAtInput;
        protected long userIdInput;
        protected TextField userInput;
        public OrderRepository orderRepo;
        public ProductRepository prodRepo;
        private ListView allProdListView;
        public bool exit ;
        public ViewOrder()
        {
            this.Title = "Preview of order";
            this.Width = Dim.Fill();
            this.Height = Dim.Fill();
            Button modify = new Button(5, 2,"Edit");
            modify.Clicked += WindowModifAct;
            Button delete = new Button(15, 2, "Delete");
            delete.Clicked += OnActDelete;
            Button exit = new Button(25, 2, "Back");
            exit.Clicked += WindowBackToMain;
            this.Add(modify, delete, exit);
            
            Label id = new Label (3, 6, "Id:");
            Label createdAt = new Label (3, 8, "Date of creation:");
            Label isPayed = new Label(3, 10, "Pay:");
            Label user = new Label (3, 12, "Customer:");

            
            this.Add(id, createdAt, user, isPayed);

            idInput = new TextField (){X = 25, Y = Pos.Top(id), Width = 5, Text = "", ReadOnly = true};
            createdAtInput = new DateField (){X = 25, Y = Pos.Top(createdAt), Width = 20, Text = "", ReadOnly = true};
            isPayedInput = new TextField (){X = 25, Y = Pos.Top(isPayed), Width = 5, Text = "", ReadOnly = true};
            userInput = new TextField (){X = 25, Y = Pos.Top(user), Width = 15, Text = "", ReadOnly = true};
            this.Add(idInput, createdAtInput, userInput, isPayedInput);

            FrameView frameViewProds = new FrameView("All ordered products:")
            {
                X = 3,
                Y = 16,
                Width = Dim.Fill() - 4,
                Height = Dim.Fill()
            };
            this.allProdListView = new ListView(new List<Product>())
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            frameViewProds.Add(this.allProdListView);

            this.Add(frameViewProds);
        }
        public void SetRepositories(ProductRepository prodRepo, OrderRepository ordRepo)
        {
            this.prodRepo = prodRepo;
            this.orderRepo = ordRepo;
        }
        public Order GetOrder()
        {
            return this.order;
        }
        public void SetOrder(Order ord)
        {
            this.order = ord;
            this.idInput.Text = ord.id.ToString();
            this.createdAtInput.Text = ord.createdAt.ToShortDateString();
            this.isPayedInput.Text = ord.isPayed.ToString();
            this.userIdInput = ord.userId;
            this.userInput.Text = ord.autor.login;
            ord.products = prodRepo.GetOrderProducts(orderRepo.GetAllOrderPrdId(ord.id));
            this.allProdListView.SetSource(ord.products);
        }

        private void WindowBackToMain()
        {
            this.exit = true;
            Application.RequestStop();
        }

        private void WindowModifAct()
        {
            ModifyOrder window = new ModifyOrder();
            window.SetOrder(this.order);
            Application.Run(window);
            if(window.status)
            {
                Order changed = window.GetOrder();
                this.edited = true;
                this.SetOrder(changed);
                
            }
            
            
        }
        private void OnActDelete()
        {
            int index = MessageBox.Query("Delete", "Are u sure?", "yes", "no");
            if(index == 0)
            {
                this.del = true;
                this.exit = true;
                Application.RequestStop();
            }
        }
    }
}